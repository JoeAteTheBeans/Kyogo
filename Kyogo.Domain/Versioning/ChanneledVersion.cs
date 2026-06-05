namespace Kyogo.Domain.Versioning;

public readonly struct ChanneledVersion(char channel = 'r', int major = 1, int minor = 0, int patch = 0) : IEquatable<ChanneledVersion>
{
    //Constructors
    public ChanneledVersion(char channel, Version version) :
        this(channel, version.Major, version.Minor, version.Build >= 0 ? version.Build : 0) {}

    public ChanneledVersion(char channel, string versionString) :
        this(channel, Version.Parse(versionString)) {}
    
    //Properties
    public char Channel { get; } = channel;
    
    public int Major { get; } = major;
    
    public int Minor { get; } = minor;
    
    public int Patch { get; } = patch;
    
    //Methods
    public bool CompatibleWith(ChanneledVersion other)
    {
        return Channel == other.Channel
               && Major == other.Major
               && Minor == other.Minor;
    }

    public int CompareTo(ChanneledVersion other)
    {
        int major = Major.CompareTo(other.Major);
        if (major != 0) 
            return major;

        int minor = Minor.CompareTo(other.Minor);
        if (minor != 0) 
            return minor;

        return Patch.CompareTo(other.Patch);
    }
    
    //Equality
    public bool Equals(ChanneledVersion other)
    {
        return Channel == other.Channel
               && Major == other.Major
               && Minor == other.Minor
               && Patch == other.Patch;
    }

    public override bool Equals(object? obj)
        => obj is ChanneledVersion other && Equals(other);

    //Representation
    public override int GetHashCode()
        => HashCode.Combine(Channel, Major, Minor, Patch);

    public override string ToString()
        => $"{Channel}{Major}.{Minor}.{Patch}";
    
    //Parsing
    public static ChanneledVersion Parse(string value)
        => TryParse(value, out ChanneledVersion version)
            ? version
            : throw new FormatException($"Invalid version format: '{value}'");
    
    public static bool TryParse(string value, out ChanneledVersion channeledVersion)
    {
        channeledVersion = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        char channel = Convert.ToChar(value[..1]);
        string[] components = value[1..].Split('.');
        int patch = 0;
        
        if (components.Length is not (2 or 3))
            return false;

        if (!int.TryParse(components[0], out int major) 
            || !int.TryParse(components[1], out int minor)) 
            return false;
        
        if (components.Length == 3)
            if (!int.TryParse(components[2], out patch))
                return false;

        channeledVersion = new ChanneledVersion(channel, major, minor, patch);
        return true;
    }
    
    //Operators
    public static bool operator ==(ChanneledVersion cv1, ChanneledVersion cv2)
        => cv1.Equals(cv2);
    
    public static bool operator !=(ChanneledVersion cv1, ChanneledVersion cv2)
        => !cv1.Equals(cv2);

    public static bool operator <(ChanneledVersion cv1, ChanneledVersion cv2)
        => cv1.CompareTo(cv2) < 0;
    
    public static bool operator >(ChanneledVersion cv1, ChanneledVersion cv2)
        => cv1.CompareTo(cv2) > 0;
    
    public static bool operator <=(ChanneledVersion cv1, ChanneledVersion cv2)
        => cv1.CompareTo(cv2) <= 0;
    
    public static bool operator >=(ChanneledVersion cv1, ChanneledVersion cv2)
        => cv1.CompareTo(cv2) >= 0;
}