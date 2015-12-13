using System.Collections;

// Should have just used some kind of built-in Pair class...
// Utility class for storing ints
public class Int2 {
    public int x;
    public int z;

    public Int2(int x, int z) {
        this.x = x;
        this.z = z;
    }

    public static Int2 left = new Int2(-1, 0);
    public static Int2 right = new Int2(1, 0);
    public static Int2 front = new Int2(0, 1);
    public static Int2 back = new Int2(0, -1);

    public static Int2 operator +(Int2 i1, Int2 i2) {
        return new Int2(i1.x + i2.x, i1.z + i2.z);
    }

    public override int GetHashCode() {
        return z * 31 + x;
    }

    public override bool Equals(object obj) {
        Int2 other = obj as Int2;
        return other != null && other.x == this.x && other.z == this.z;
    }

    public override string ToString() {
        return string.Format("x: {0}, z: {1}", x, z);
    }
}
