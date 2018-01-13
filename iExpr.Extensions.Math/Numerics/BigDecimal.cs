using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace iExpr.Extensions.Math.Numerics
{
    public struct BigDecimal : IComparable, IComparable<BigDecimal>, IEquatable<BigDecimal>
    {
        private static readonly BigDecimal s_bnOneInt = new BigDecimal(BigInteger.One);
        private static readonly BigDecimal s_bnZeroInt = new BigDecimal(BigInteger.Zero);
        private static readonly BigDecimal s_bnMinusOneInt = new BigDecimal(BigInteger.MinusOne);

        static BigInteger GetTenPow(int n)
        {
            return BigInteger.Pow(10, n);
        }

        BigInteger Integer { get; set; }
        BigInteger Decimal { get; set; }

        public int Precision { get; private set; }

        private void Adjust()
        {
            int l = Integer.Sign, r = Decimal.Sign;
            var p = GetTenPow(Precision);
            if (BigInteger.Abs(Decimal) >= p)
            {
                Integer += BigInteger.DivRem(Decimal, p, out var rem);
                Decimal = rem;
            }
            if (l == r || Integer.IsZero || Decimal.IsZero) return;
            else if (l < r)
            {
                Integer++;
                Decimal = p - Decimal;
            }
            else
            {
                Integer--;
                Decimal = p - Decimal;
            }
        }

        public void SetPrecision(int precision)
        {
            if (Precision == precision) return;
            else if (Precision < precision)
            {
                while (Precision < precision)
                {
                    Precision++;
                    Decimal *= 10;
                }
            }
            else
            {
                while (Precision > precision)
                {
                    Precision--;
                    Decimal /= 10;
                }
            }
        }

        public void SetValue(Decimal d)
        {
            if (d == 0)
            {
                Integer = 0; Decimal = 0;
            }
            else if (d > 0)
            {
                Integer = new BigInteger(d);
                d -= System.Math.Floor(d);
                Decimal = new BigInteger(0);
                for (int i = 1; i <= Precision; i++)
                {
                    d *= 10;
                    Decimal *= 10;
                    Decimal += new BigInteger(d);
                    d -= System.Math.Floor(d);
                }
            }
            else
            {
                Integer = new BigInteger(d);
                d -= System.Math.Ceiling(d);
                Decimal = new BigInteger(0);
                for (int i = 1; i <= Precision; i++)
                {
                    d *= 10;
                    Decimal *= 10;
                    Decimal += new BigInteger(d);
                    d -= System.Math.Ceiling(d);
                }
            }
        }

        public void SetValue(Double d)
        {
            if (Double.IsInfinity(d))
                throw new OverflowException();
            if (Double.IsNaN(d))
                throw new OverflowException();
            if (d == 0)
            {
                Integer = 0; Decimal = 0;
            }
            else if (d > 0)
            {
                Integer = new BigInteger(d);
                d -= System.Math.Floor(d);
                Decimal = new BigInteger(0);
                for (int i = 1; i <= Precision; i++)
                {
                    d *= 10;
                    Decimal *= 10;
                    Decimal += new BigInteger(d);
                    d -= System.Math.Floor(d);
                }
            }
            else
            {
                Integer = new BigInteger(d);
                d -= System.Math.Ceiling(d);
                Decimal = new BigInteger(0);
                for (int i = 1; i <= Precision; i++)
                {
                    d *= 10;
                    Decimal *= 10;
                    Decimal += new BigInteger(d);
                    d -= System.Math.Ceiling(d);
                }
            }
        }

        #region public properties

        public static BigDecimal Zero
        {
            get { return s_bnZeroInt; }
        }

        public static BigDecimal One
        {
            get { return s_bnOneInt; }
        }

        public static BigDecimal MinusOne
        {
            get { return s_bnMinusOneInt; }
        }

        public bool IsInteger { get { return Decimal.IsZero; } }

        public bool IsZero { get {  return Integer.IsZero && IsInteger; } }

        public bool IsOne { get {  return Integer.IsOne && IsInteger; } }

        public bool IsEven { get {  return Integer.IsEven && IsInteger; } }

        public int Sign
        {
            get
            {
                var r = Integer.Sign;
                return r == 0 ? Decimal.Sign : r;
            }
        }

        #endregion public properties

        #region ctor methods

        public BigDecimal(BigInteger val, int precision = 20)
        {
            Precision = precision;
            Integer = val;
        }
        public BigDecimal(Int32 val, int precision = 20)
        {
            Precision = precision;
            Integer = new BigInteger(val);
        }
        public BigDecimal(UInt32 val, int precision = 20)
        {
            Precision = precision;
            Integer = new BigInteger(val);
        }
        public BigDecimal(Int64 val, int precision = 20)
        {
            Precision = precision;
            Integer = new BigInteger(val);
        }
        public BigDecimal(UInt64 val, int precision = 20)
        {
            Precision = precision;
            Integer = new BigInteger(val);
        }

        #endregion ctor methods

        public static Int32 Compare(BigDecimal left, BigDecimal right)
        {
            return left.CompareTo(right);
        }

        public static BigDecimal Abs(BigDecimal value)
        {
            return (value >= BigDecimal.Zero) ? value : -value;
        }

        public static BigDecimal Add(BigDecimal left, BigDecimal right)
        {
            return left + right;
        }

        public static BigDecimal Subtract(BigDecimal left, BigDecimal right)
        {
            return left - right;
        }

        public static BigDecimal Multiply(BigDecimal left, BigDecimal right)
        {
            return left * right;
        }

        public static BigDecimal Divide(BigDecimal dividend, BigDecimal divisor)
        {
            return dividend / divisor;
        }

        public static BigDecimal Remainder(BigDecimal dividend, BigDecimal divisor)
        {
            return dividend % divisor;
        }

        public static BigDecimal DivRem(BigDecimal dividend, BigDecimal divisor, out BigDecimal remainder)
        {
            var d = dividend / divisor;
            remainder = dividend - d * divisor;
            return d;
        }


        public static BigDecimal Negate(BigDecimal value)
        {
            return -value;
        }

        public static BigDecimal Max(BigDecimal left, BigDecimal right)
        {
            if (left.CompareTo(right) < 0)
                return right;
            return left;
        }

        public static BigDecimal Min(BigDecimal left, BigDecimal right)
        {
            if (left.CompareTo(right) <= 0)
                return left;
            return right;
        }

        public static BigDecimal ModPow(BigDecimal value, BigInteger exponent, BigDecimal modulus)
        {
            BigDecimal ans = One;
            while (!exponent.IsZero)
            {
                if (!exponent.IsEven)
                {
                    ans *= value;
                    ans %= modulus;
                }
                value *= value;
                value %= modulus;
                exponent >>= 1;
            }
            return ans;
        }

        public static BigDecimal Pow(BigDecimal value, UInt32 exponent)
        {
            BigDecimal ans = One;
            while (exponent > 0)
            {
                if ((exponent & 1) == 1) ans *= value;
                value *= value;
                exponent >>= 1;
            }
            return ans;
        }

        // -------- SECTION: public static operators -----------------*
        #region public static operators
        public static implicit operator BigDecimal(Byte value)
        {
            return new BigDecimal(value);
        }
        
        public static implicit operator BigDecimal(SByte value)
        {
            return new BigDecimal(value);
        }

        public static implicit operator BigDecimal(Int16 value)
        {
            return new BigDecimal(value);
        }
        
        public static implicit operator BigDecimal(UInt16 value)
        {
            return new BigDecimal(value);
        }

        public static implicit operator BigDecimal(int value)
        {
            return new BigDecimal(value);
        }

        public static implicit operator BigDecimal(BigInteger v)
        {
            return new BigDecimal(v);
        }

        public static implicit operator BigDecimal(uint value)
        {
            return new BigDecimal(value);
        }

        public static implicit operator BigDecimal(long value)
        {
            return new BigDecimal(value);
        }

        
        public static implicit operator BigDecimal(ulong value)
        {
            return new BigDecimal(value);
        }
        public static implicit operator BigDecimal(Single d)
        {
            var r = new BigDecimal(); r.SetValue(d);
            return r;
        }
        public static implicit operator BigDecimal(Double d)
        {
            var r = new BigDecimal(); r.SetValue(d);
            return r;
        }
        public static implicit operator BigDecimal(Decimal d)
        {
            var r = new BigDecimal(); r.SetValue(d);
            return r;
        }

        public static explicit operator Byte(BigDecimal value)
        {
            return checked((byte)((int)value));
        }
        
        public static explicit operator SByte(BigDecimal value)
        {
            return checked((sbyte)((int)value));
        }

        public static explicit operator Int16(BigDecimal value)
        {
            return checked((short)((int)value));
        }

        
        public static explicit operator UInt16(BigDecimal value)
        {
            return checked((ushort)((int)value));
        }

        public static explicit operator Int32(BigDecimal value)
        {
            return (Int32)value.Integer;
        }

        
        public static explicit operator UInt32(BigDecimal value)
        {
            return (UInt32)value.Integer;
        }

        public static explicit operator Int64(BigDecimal value)
        {
            return (Int64)value.Integer;
        }

        
        public static explicit operator UInt64(BigDecimal value)
        {
            return (UInt64)value.Integer;
        }

        public static explicit operator Single(BigDecimal value)
        {
            return (Single)((Double)value);
        }

        public static explicit operator Double(BigDecimal value)
        {
            return double.Parse(value.ToString());
        }

        public static explicit operator Decimal(BigDecimal value)
        {
            return decimal.Parse(value.ToString());
        }

        public static BigDecimal operator -(BigDecimal value)
        {
            value.Integer = -value.Integer;
            value.Decimal = -value.Decimal;
            return value;
        }

        public static BigDecimal operator +(BigDecimal value)
        {
            return value;
        }


        public static BigDecimal operator ++(BigDecimal value)
        {
            return value + BigDecimal.One;
        }

        public static BigDecimal operator --(BigDecimal value)
        {
            return value - BigDecimal.One;
        }


        public static BigDecimal operator +(BigDecimal left, BigDecimal right)
        {
            if (right.IsZero) return left;
            if (left.IsZero) return right;
            BigDecimal res = new BigDecimal() { Precision = System.Math.Max(left.Precision, right.Precision) };
            left.SetPrecision(res.Precision); right.SetPrecision(res.Precision);
            //int l = left.Sign, r = right.Sign;
            res.Integer = left.Integer + right.Integer;
            res.Decimal = left.Decimal + right.Decimal;
            res.Adjust();
            return res;
        }

        public static BigDecimal operator -(BigDecimal left, BigDecimal right)
        {
            if (right.IsZero) return left;
            if (left.IsZero) return -right;
            BigDecimal res = new BigDecimal() { Precision = System.Math.Max(left.Precision, right.Precision) };
            left.SetPrecision(res.Precision); right.SetPrecision(res.Precision);
            //int l = left.Sign, r = right.Sign;
            res.Integer = left.Integer - right.Integer;
            res.Decimal = left.Decimal - right.Decimal;
            res.Adjust();
            return res;
        }

        public static BigDecimal operator *(BigDecimal left, BigDecimal right)
        {
            if (left.IsZero) return Zero;
            if (right.IsZero) return Zero;
            if (left.IsOne) return right;
            if (right.IsOne) return left;
            //if (left.IsOne && left.Sign==-1) return -right;
            //if (right.IsOne && right.Sign == -1) return -left;

            bool flg = left.Sign == right.Sign;

            BigDecimal res = new BigDecimal() { Precision = left.Precision+right.Precision };
            //left.SetPrecision(res.Precision); right.SetPrecision(res.Precision);
            //int l = left.Sign, r = right.Sign;
                res.Integer = BigInteger.Abs(left.Integer) * BigInteger.Abs(right.Integer);
                res.Decimal = BigInteger.Abs(left.Decimal) * BigInteger.Abs(right.Decimal);
            
            return flg?res:-res;
        }

        public static BigDecimal operator /(BigDecimal dividend, BigDecimal divisor)
        {
            if (divisor.IsZero) throw new DivideByZeroException();
            if (dividend.IsZero)
            {
                return Zero;
            }
            if (divisor.IsOne)
            {
                return dividend;
            }
            //if (left.IsOne && left.Sign==-1) return -right;
            //if (right.IsOne && right.Sign == -1) return -left;
            bool flg = dividend.Sign == divisor.Sign;
            dividend = Abs(dividend);divisor = Abs(divisor);
            BigDecimal res = new BigDecimal() { Precision = System.Math.Max(dividend.Precision, divisor.Precision) };
            dividend.SetPrecision(res.Precision); divisor.SetPrecision(res.Precision);
            var p = GetTenPow(res.Precision);
            var up = dividend.Integer * p + dividend.Decimal;
            var down = divisor.Integer * p + divisor.Decimal;

            BigInteger rem;
            int cnt = 0;
            if (up >= down)
            {
                res.Integer = BigInteger.DivRem(up, down, out rem);
            }
            else rem = up;
            while (cnt < res.Precision && !rem.IsZero)
            {
                while (rem < down)//开始时rem一定小于down
                {
                    rem *= 10;
                    cnt++;
                }
                var dg = BigInteger.DivRem(rem, down, out rem);
                res.Decimal += dg * GetTenPow(res.Precision - cnt);
            }
            return flg?res:-res;
        }

        public static BigDecimal operator %(BigDecimal dividend, BigDecimal divisor)
        {
            DivRem(dividend, divisor, out var rem);
            return rem;
        }

        public static bool operator <(BigDecimal left, BigDecimal right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator <=(BigDecimal left, BigDecimal right)
        {
            return left.CompareTo(right) <= 0;
        }
        public static bool operator >(BigDecimal left, BigDecimal right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator >=(BigDecimal left, BigDecimal right)
        {
            return left.CompareTo(right) >= 0;
        }
        public static bool operator ==(BigDecimal left, BigDecimal right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(BigDecimal left, BigDecimal right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(BigDecimal left, Int64 right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator <=(BigDecimal left, Int64 right)
        {
            return left.CompareTo(right) <= 0;
        }
        public static bool operator >(BigDecimal left, Int64 right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator >=(BigDecimal left, Int64 right)
        {
            return left.CompareTo(right) >= 0;
        }
        public static bool operator ==(BigDecimal left, Int64 right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(BigDecimal left, Int64 right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Int64 left, BigDecimal right)
        {
            return right.CompareTo(left) > 0;
        }
        public static bool operator <=(Int64 left, BigDecimal right)
        {
            return right.CompareTo(left) >= 0;
        }
        public static bool operator >(Int64 left, BigDecimal right)
        {
            return right.CompareTo(left) < 0;
        }
        public static bool operator >=(Int64 left, BigDecimal right)
        {
            return right.CompareTo(left) <= 0;
        }
        public static bool operator ==(Int64 left, BigDecimal right)
        {
            return right.Equals(left);
        }
        public static bool operator !=(Int64 left, BigDecimal right)
        {
            return !right.Equals(left);
        }

        
        public static bool operator <(BigDecimal left, UInt64 right)
        {
            return left.CompareTo(right) < 0;
        }
        
        public static bool operator <=(BigDecimal left, UInt64 right)
        {
            return left.CompareTo(right) <= 0;
        }
        
        public static bool operator >(BigDecimal left, UInt64 right)
        {
            return left.CompareTo(right) > 0;
        }
        
        public static bool operator >=(BigDecimal left, UInt64 right)
        {
            return left.CompareTo(right) >= 0;
        }
        
        public static bool operator ==(BigDecimal left, UInt64 right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(BigDecimal left, UInt64 right)
        {
            return !left.Equals(right);
        }

        
        public static bool operator <(UInt64 left, BigDecimal right)
        {
            return right.CompareTo(left) > 0;
        }
        
        public static bool operator <=(UInt64 left, BigDecimal right)
        {
            return right.CompareTo(left) >= 0;
        }
        
        public static bool operator >(UInt64 left, BigDecimal right)
        {
            return right.CompareTo(left) < 0;
        }
        
        public static bool operator >=(UInt64 left, BigDecimal right)
        {
            return right.CompareTo(left) <= 0;
        }
        
        public static bool operator ==(UInt64 left, BigDecimal right)
        {
            return right.Equals(left);
        }
        
        public static bool operator !=(UInt64 left, BigDecimal right)
        {
            return !right.Equals(left);
        }

        #endregion public static operators

        #region public instance methods

        public override bool Equals(object obj)
        { 
            if (!(obj is BigDecimal))
                return false;
            return Equals((BigDecimal)obj);
        }

        public bool Equals(Int64 other)
        {
            return this.Equals((BigDecimal)other);
        }

        public bool Equals(UInt64 other)
        {
            return this.Equals((BigDecimal)other);
        }

        public bool Equals(BigInteger other)
        {
            return this.Equals((BigDecimal)other);
        }

        public int CompareTo(Int64 other)
        {
            return this.CompareTo((BigDecimal)other);
        }
        
        public int CompareTo(UInt64 other)
        {
            return this.CompareTo((BigDecimal)other);
        }

        public int CompareTo(BigInteger other)
        {
            return this.CompareTo((BigDecimal)other);
        }
        
        public int CompareTo(Object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is BigInteger))
                throw new ArgumentException();
            return this.CompareTo((BigInteger)obj);
        }

        public int CompareTo(BigDecimal v)
        {
            int k = ((IComparable<BigInteger>)Integer).CompareTo(v.Integer);
            if (k != 0) return k;
            if (Precision == v.Precision) return ((IComparable<BigInteger>)Decimal).CompareTo(v.Decimal);
            else if (Precision > v.Precision)
            {
                return ((IComparable<BigInteger>)Decimal).CompareTo(v.Decimal * GetTenPow(Precision - v.Precision));
            }
            else
            {
                return ((IComparable<BigInteger>)Decimal).CompareTo(v.Decimal / GetTenPow(Precision - v.Precision));
            }
            //return  ? k : ((IComparable<BigInteger>)Decimal).CompareTo(v.Decimal);
        }

        public bool Equals(BigDecimal other)
        {
            //Attention this!
            return this.CompareTo(other) == 0;
        }

        public override string ToString()
        {
            switch (Sign)
            {
                case 0:
                    return "0";
                case 1:
                    return $"{Integer.ToString()}.{Decimal.ToString()}";
                case -1:
                    return $"-{(-Integer).ToString()}.{(-Decimal).ToString($"D{Precision}")}";
            }
            throw new Exception();
        }

        public override int GetHashCode()
        {
            var hashCode = 1451633208;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<BigInteger>.Default.GetHashCode(Integer);
            hashCode = hashCode * -1521134295 + EqualityComparer<BigInteger>.Default.GetHashCode(Decimal);
            hashCode = hashCode * -1521134295 + Precision.GetHashCode();
            return hashCode;
        }

        #endregion public instance methods
    }
}
