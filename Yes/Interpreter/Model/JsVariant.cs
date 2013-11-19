using System;
using System.Runtime.InteropServices;
using Yes.Runtime.Environment;
using Yes.Utility;

namespace Yes.Interpreter.Model
{
    [StructLayout(LayoutKind.Sequential)]
    public struct JsVariant
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct Value
        {
            [FieldOffset(0)] public bool Bool;
            [FieldOffset(0)] public double Double;
            [FieldOffset(0)] public int Int;
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct Reference
        {
            [FieldOffset(0)]
            public string String;
            [FieldOffset(0)]
            public IJsObject Object;
        }

        public Value Val;
        public Reference Ref;
        public JsVariantKind Kind;

        public JsVariant(bool v)
        {
            this = default(JsVariant);
            Kind = JsVariantKind.Bool;
            Val.Bool = v;
        }
        public JsVariant(int v)
        {
            this = default(JsVariant);
            Kind = JsVariantKind.Int;
            Val.Int = v;
        }
        public JsVariant(double v)
        {
            this = default(JsVariant);
            Kind = JsVariantKind.Double;
            Val.Double = v;
        }
        public JsVariant(string v)
        {
            this = default(JsVariant);
            Kind = JsVariantKind.String;
            Ref.String= v;
        }
        public JsVariant(IJsObject v)
        {
            this = default(JsVariant);
            Kind = JsVariantKind.Object;
            Ref.Object = v;
        }
        public JsVariant(IJsUndefined v)
        {
            this = default(JsVariant);
            Kind = JsVariantKind.Undefined;
        }
        public JsVariant(IJsNull v)
        {
            this = default(JsVariant);
            Kind = JsVariantKind.Null;
        }

        public JsVariant(IJsValue v)
        {
            this = v.ToVariant();
        }

        private class MethodTable<T>
        {
            public T Undefined { get; set; }
            public T Null { get; set; }
            public T Bool { get; set; }
             public T Int { get; set; }
            public T Double { get; set; }
            public T String { get; set; }
            public T Object { get; set; }

            public T[] ToArray()
            {
                return new T[]{Undefined,Null,Bool,Double,Int,String,Object};
            }
        }


        interface IConversions
        {
            int? ToArrayIndex(ref JsVariant v);
            object ToPrimitive(ref JsVariant v);
            bool ToBoolean(ref JsVariant v);
            double ToNumber(ref JsVariant v);
            int ToInteger(ref JsVariant v);
            string ToString(ref JsVariant v);
        }

        class UndefinedConversion: IConversions
        {
            public int? ToArrayIndex(ref JsVariant v)
            {
                return Conversion.ToArrayIndex(JsUndefined.Value);
            }

            public object ToPrimitive(ref JsVariant v)
            {
                return Conversion.ToPrimitive(JsUndefined.Value);
            }

            public bool ToBoolean(ref JsVariant v)
            {
                return Conversion.ToBoolean(JsUndefined.Value);
            }

            public double ToNumber(ref JsVariant v)
            {
                return Conversion.ToNumber(JsUndefined.Value);
            }

            public int ToInteger(ref JsVariant v)
            {
                return Conversion.ToInteger(JsUndefined.Value);
            }

            public string ToString(ref JsVariant v)
            {
                return Conversion.ToString(JsUndefined.Value);
            }
        }
        class NullConversions : IConversions
        {
            public int? ToArrayIndex(ref JsVariant v)
            {
                return Conversion.ToArrayIndex(JsNull.Value);
            }

            public object ToPrimitive(ref JsVariant v)
            {
                return Conversion.ToPrimitive(JsNull.Value);
            }

            public bool ToBoolean(ref JsVariant v)
            {
                return Conversion.ToBoolean(JsNull.Value);
            }

            public double ToNumber(ref JsVariant v)
            {
                return Conversion.ToNumber(JsNull.Value);
            }

            public int ToInteger(ref JsVariant v)
            {
                return Conversion.ToInteger(JsNull.Value);
            }

            public string ToString(ref JsVariant v)
            {
                return Conversion.ToString(JsNull.Value);
            }
        }

        class BoolConversions: IConversions
        {
            public int? ToArrayIndex(ref JsVariant v)
            {
                return Conversion.ToArrayIndex(v.Val.Bool);
            }

            public object ToPrimitive(ref JsVariant v)
            {
                return Conversion.ToPrimitive(v.Val.Bool);
            }

            public bool ToBoolean(ref JsVariant v)
            {
                return Conversion.ToBoolean(v.Val.Bool);
            }

            public double ToNumber(ref JsVariant v)
            {
                return Conversion.ToNumber(v.Val.Bool);
            }

            public int ToInteger(ref JsVariant v)
            {
                return Conversion.ToInteger(v.Val.Bool);
            }

            public string ToString(ref JsVariant v)
            {
                return Conversion.ToString(v.Val.Bool);
            }
        }

        class DoubleConversions: IConversions
        {
            public int? ToArrayIndex(ref JsVariant v)
            {
                return Conversion.ToArrayIndex(v.Val.Double);
            }

            public object ToPrimitive(ref JsVariant v)
            {
                return Conversion.ToPrimitive(v.Val.Double);
            }

            public bool ToBoolean(ref JsVariant v)
            {
                return Conversion.ToBoolean(v.Val.Double);
            }

            public double ToNumber(ref JsVariant v)
            {
                return Conversion.ToNumber(v.Val.Double);
            }

            public int ToInteger(ref JsVariant v)
            {
                return Conversion.ToInteger(v.Val.Double);
            }

            public string ToString(ref JsVariant v)
            {
                return Conversion.ToString(v.Val.Double);
            }
        }

        class IntConversions: IConversions
        {
            public int? ToArrayIndex(ref JsVariant v)
            {
                return Conversion.ToArrayIndex(v.Val.Int);
            }

            public object ToPrimitive(ref JsVariant v)
            {
                return Conversion.ToPrimitive(v.Val.Int);
            }

            public bool ToBoolean(ref JsVariant v)
            {
                return Conversion.ToBoolean(v.Val.Int);
            }

            public double ToNumber(ref JsVariant v)
            {
                return Conversion.ToNumber(v.Val.Int);
            }

            public int ToInteger(ref JsVariant v)
            {
                return Conversion.ToInteger(v.Val.Int);
            }

            public string ToString(ref JsVariant v)
            {
                return Conversion.ToString(v.Val.Int);
            }
        }

        class ObjectConversions: IConversions
        {
            public int? ToArrayIndex(ref JsVariant v)
            {
                return Conversion.ToArrayIndex(v.Ref.Object);
            }

            public object ToPrimitive(ref JsVariant v)
            {
                return Conversion.ToPrimitive(v.Ref.Object);
            }

            public bool ToBoolean(ref JsVariant v)
            {
                return Conversion.ToBoolean(v.Ref.Object);
            }

            public double ToNumber(ref JsVariant v)
            {
                return Conversion.ToNumber(v.Ref.Object);
            }

            public int ToInteger(ref JsVariant v)
            {
                return Conversion.ToInteger(v.Ref.Object);
            }

            public string ToString(ref JsVariant v)
            {
                return Conversion.ToString(v.Ref.Object);
            }
        }

        class StringConversions: IConversions
        {
            public int? ToArrayIndex(ref JsVariant v)
            {
                return Conversion.ToArrayIndex(v.Ref.String);
            }

            public object ToPrimitive(ref JsVariant v)
            {
                return Conversion.ToPrimitive(v.Ref.String);
            }

            public bool ToBoolean(ref JsVariant v)
            {
                return Conversion.ToBoolean(v.Ref.String);
            }

            public double ToNumber(ref JsVariant v)
            {
                return Conversion.ToNumber(v.Ref.String);
            }

            public int ToInteger(ref JsVariant v)
            {
                return Conversion.ToInteger(v.Ref.String);
            }

            public string ToString(ref JsVariant v)
            {
                return Conversion.ToString(v.Ref.String);
            }
        }
         
        private static IConversions[] _toObject
            = new MethodTable<IConversions>
                  {
                      Undefined = new UndefinedConversion(),
                      Null = new NullConversions(),
                      Bool = new BoolConversions(),
                      Int = new IntConversions(),
                      Double = new DoubleConversions(),
                      String = new StringConversions(),
                      Object = new ObjectConversions()
                  }.ToArray();
    }
}