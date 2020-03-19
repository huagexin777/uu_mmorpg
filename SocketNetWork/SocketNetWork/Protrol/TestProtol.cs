using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TestProtol : IProto
{
    public ushort ProtoCode { get { return 1001; } }
    public string Name;
    public int Sex;
    public int Age;
    public string Des;

    public byte[] ToArray() 
    {
        using (MMO_MemoryStream mmo = new MMO_MemoryStream())
        {
            //mmo.WriteUShort(ProtoCode);
            mmo.WriteUTF8String(Name);
            mmo.WriteInt(Sex);
            mmo.WriteInt(Age);
            mmo.WriteUTF8String(Des);
            return mmo.ToArray();
        }
    }

    public override string ToString()
    {
        return "Name:" + Name + "--Sex:" + Sex + "--Age:" + Age + "--Des:" + Des;
    }

    public static TestProtol GetProtol(byte[] buffer) 
    {
        TestProtol test = new TestProtol();

        using (MMO_MemoryStream mmo = new MMO_MemoryStream(buffer))
        {
            test.Name = mmo.ReadUTF8String();
            test.Sex = mmo.ReadInt();
            test.Age = mmo.ReadInt();
            test.Des = mmo.ReadUTF8String();
            return test;
        }
    }

}