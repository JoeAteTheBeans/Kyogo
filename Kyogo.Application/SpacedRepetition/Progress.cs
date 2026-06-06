using Kyogo.Domain.Users;

namespace Kyogo.Application.SpacedRepetition;

public sealed class Progress
{
    public required ProgressId Id { get; init; }
    
    public required UserId OwnerId { get; init; }
    
    public bool Fluent { get; private set; }

    public ProgressStage Stage { get; private set; }
    
    public DateTimeOffset DueDate { get; private set; }

    public TimeSpan Interval { get; private set; }
    
    public int? Step { get; private set; }
    
    public TimeSpan? RetainedInterval { get; private set; }
    
    private static readonly TimeSpan[] LearningSteps = [
        TimeSpan.FromMinutes(3),
        TimeSpan.FromMinutes(5),
        TimeSpan.FromMinutes(10),
        TimeSpan.FromHours(1),
        TimeSpan.FromHours(2),
        TimeSpan.FromHours(6),
        TimeSpan.FromHours(12),
    ];
    
    private static readonly TimeSpan[] RelearningSteps = [
        TimeSpan.FromMinutes(10),
        TimeSpan.FromHours(1),
        TimeSpan.FromHours(2),
        TimeSpan.FromHours(3),
    ];
    
    private static readonly TimeSpan VeryHardRedoLowerLimit = TimeSpan.FromDays(3); 
    
    private static readonly TimeSpan VeryHardRedoInterval = TimeSpan.FromDays(1);
    
    private static readonly TimeSpan LearningGoodBreakoutInterval = TimeSpan.FromDays(1);
    
    private static readonly TimeSpan LearningEasyBreakoutInterval = TimeSpan.FromDays(2);

    private const double RelearningBreakoutModifier = 0.4;

    public void TransitionStage(ProgressStage stage)
    {
        switch (stage)
        {
            case ProgressStage.Learning:
                Step = 0;
                Interval = LearningSteps[Step.Value];
                break;
            case ProgressStage.Reviewing:
                if (RetainedInterval is not null)
                {
                    Interval = RetainedInterval.Value;
                    RetainedInterval = null;
                }
                Step = null;
                break;
            case ProgressStage.Relearning:
                RetainedInterval = Interval;
                Step = 0;
                Interval = RelearningSteps[Step.Value];
                break;
            default: throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
        }
        Stage = stage;
    }
    
    public void ApplyGrade(AnswerGrade grade)
    {
        if (Stage == ProgressStage.Reviewing)
        {
            switch (grade)
            {
                case AnswerGrade.Nothing or AnswerGrade.Something:
                    TransitionStage(ProgressStage.Relearning);
                    break;
                case AnswerGrade.VeryHard when Interval > VeryHardRedoLowerLimit:
                    RetainedInterval = Interval;
                    Interval = VeryHardRedoInterval;
                    break;
                default:
                    Interval *= GetIntervalModifier(grade);
                    break;
            }
            DueDate = DateTime.UtcNow + Interval;
            return;
        }
        TimeSpan[] steps = Stage == ProgressStage.Learning ? LearningSteps : RelearningSteps;
        Step ??= 0;
        Step = Math.Clamp(Step.Value + GetStepChange(grade), 0, steps.Length - 1);
        Interval = steps[Step.Value];
        if (grade is not (AnswerGrade.Good or AnswerGrade.Easy) || Step < steps.Length - 1)
        {
            DueDate = DateTime.UtcNow + Interval;
            return;
        }
        if (Stage == ProgressStage.Learning || RetainedInterval is null)
            Interval = grade == AnswerGrade.Good ? LearningGoodBreakoutInterval : LearningEasyBreakoutInterval;
        else
        {
            Interval = RetainedInterval.Value * RelearningBreakoutModifier;
            RetainedInterval = null;
        }
        TransitionStage(ProgressStage.Reviewing);
        Step = null;
        DueDate = DateTime.UtcNow + Interval;
    }

    private static int GetStepChange(AnswerGrade grade)
        => grade switch
        {
            AnswerGrade.Nothing => -2,
            AnswerGrade.Something => -1,
            AnswerGrade.VeryHard => 0,
            AnswerGrade.Hard => 1,
            AnswerGrade.Good => 2,
            AnswerGrade.Easy => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(grade), grade, null)
        };
    
    private static double GetIntervalModifier(AnswerGrade grade)
        => grade switch
        {
            AnswerGrade.Nothing => 0.3,
            AnswerGrade.Something => 0.6,
            AnswerGrade.VeryHard => 0.8,
            AnswerGrade.Hard => 1.25,
            AnswerGrade.Good => 1.8,
            AnswerGrade.Easy => 2.75,
            _ => throw new ArgumentOutOfRangeException(nameof(grade), grade, null)
        };
}