using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace iExpr.Extensions.Math.Numerics
{ 
    public class BigDecimalFactory
    {
        public int Precision { get; private set; }

        public BigDecimal Get()
        {
            return new BigDecimal(0, Precision);
        }

        public BigDecimal Get(double d)
        {
            var v = new BigDecimal(d, Precision);
            return v;
        }

        public BigDecimal Get(decimal d)
        {
            var v = new BigDecimal(d, Precision);
            return v;
        }

        public BigDecimal Get(int d)
        {
            var v = new BigDecimal(d, Precision);
            return v;
        }

        public BigDecimal Get(long d)
        {
            var v = new BigDecimal(d, Precision);
            return v;
        }

        public BigDecimal Get(BigInteger d)
        {
            var v = new BigDecimal(d, Precision);
            return v;
        }

        public BigDecimalFactory(int precision)
        {
            Precision = precision;
        }
    }

    public struct BigDecimal : IComparable, IComparable<BigDecimal>, IEquatable<BigDecimal>
    {
        public static BigDecimal GetOne(int precision)
        {
            return new BigDecimal(1,precision);
        }

        public static BigDecimal GetMinusOne(int precision)
        {
            return -GetOne(precision);
        }

        public static BigDecimal GetZero(int precision)
        {
            return new BigDecimal(0, precision);
        }

        static BigInteger GetTenPow(int n)
        {
            if (n == 0) return 1;
            else if (n == 1) return 10;
            /*if (n < 1000)
            {
                return _TenPowor[n];
            }*/
            return BigInteger.Pow(10, n);
        }

        public BigInteger _Value { get; set; }

        public int Precision { get; private set; }
        
        public void SetPrecision(int precision)
        {
            if (Precision == precision) return;
            else if (Precision < precision)
            {
                while (Precision < precision)
                {
                    Precision++;
                    _Value *= 10;
                }
            }
            else
            {
                while (Precision > precision)
                {
                    Precision--;
                    _Value /= 10;
                }
            }
        }

        public void SetValue(Decimal d)
        {
            if (d == 0)
            {
                _Value = 0;
            }
            else if (d > 0)
            {
                _Value = new BigInteger(d);
                d -= System.Math.Floor(d);
                for (int i = 1; i <= Precision; i++)
                {
                    d *= 10;
                    _Value *= 10;
                    _Value += new BigInteger(d);
                    d -= System.Math.Floor(d);
                }
            }
            else
            {
                var _d = System.Math.Abs(d);
                _Value = new BigInteger(_d);
                _d -= System.Math.Floor(_d);
                for (int i = 1; i <= Precision; i++)
                {
                    _d *= 10;
                    _Value *= 10;
                    _Value += new BigInteger(_d);
                    _d -= System.Math.Floor(_d);
                }
                _Value = -_Value;
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
                _Value = 0;
            }
            else if (d > 0)
            {
                _Value = new BigInteger(d);
                d -= System.Math.Floor(d);
                for (int i = 1; i <= Precision; i++)
                {
                    d *= 10;
                    _Value *= 10;
                    _Value += new BigInteger(d);
                    d -= System.Math.Floor(d);
                }
            }
            else
            {
                var _d = System.Math.Abs(d);
                _Value = new BigInteger(_d);
                _d -= System.Math.Floor(_d);
                for (int i = 1; i <= Precision; i++)
                {
                    _d *= 10;
                    _Value *= 10;
                    _Value += new BigInteger(_d);
                    _d -= System.Math.Floor(_d);
                }
                _Value = -_Value;
            }
        }

        #region public properties
        
        public BigInteger Decimal { get => _Value % (GetTenPow(Precision)); }

        public BigInteger Integer { get => _Value / (GetTenPow(Precision)); }

        public bool IsInteger { get { return Decimal.IsZero; } }

        public bool IsZero { get { return _Value.IsZero; } }

        public bool IsOne { get { return Integer.IsOne && IsInteger; } }

        public bool IsEven { get { return Integer.IsEven && IsInteger; } }

        public int Sign
        {
            get
            {
                return _Value.Sign;
            }
        }

        #endregion public properties

        #region ctor methods

        public BigDecimal(BigInteger val, int precision = 30)
        {
            Precision = precision;
            _Value = val*GetTenPow(precision);
        }
        public BigDecimal(Int32 val, int precision=30) : this(new BigInteger(val), precision) { }
        public BigDecimal(UInt32 val, int precision = 30) : this(new BigInteger(val), precision) { }
        public BigDecimal(Int64 val, int precision = 30) : this(new BigInteger(val), precision) { }
        public BigDecimal(UInt64 val, int precision = 30) : this(new BigInteger(val), precision) { }

        public BigDecimal(double val, int precision=30)
        {
            this.Precision = precision;
            this.SetValue(val);
        }

        public BigDecimal(decimal val, int precision = 30)
        {
            this.Precision = precision;
            this.SetValue(val);
        }

        #endregion ctor methods

        public static Int32 Compare(BigDecimal left, BigDecimal right)
        {
            return left.CompareTo(right);
        }

        public static BigDecimal Abs(BigDecimal value)
        {
            return (value.Sign>=0) ? value : -value;
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
            //if (dividend.Precision != divisor.Precision) throw new ArgumentException();
            var d = (dividend / divisor).Integer;
            remainder = dividend - new BigDecimal(d,divisor.Precision) * divisor;
            return new BigDecimal(d,divisor.Precision);
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
            BigDecimal ans = GetOne(value.Precision);
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

        public static BigDecimal Pow(BigDecimal value, BigInteger exponent)
        {
            BigDecimal ans = GetOne(value.Precision);
            while (!exponent.IsZero)
            {
                if (!exponent.IsEven)
                {
                    ans *= value;
                }
                value *= value;
                exponent >>= 1;
            }
            return ans;
        }

        public static BigDecimal Pow(BigDecimal value, UInt32 exponent)
        {
            BigDecimal ans = GetOne(value.Precision);
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
            return new BigDecimal(value,20);
        }

        public static implicit operator BigDecimal(SByte value)
        {
            return new BigDecimal(value, 20);
        }

        public static implicit operator BigDecimal(Int16 value)
        {
            return new BigDecimal(value, 20);
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
            var r = new BigDecimal(d);
            return r;
        }
        public static implicit operator BigDecimal(Double d)
        {
            var r = new BigDecimal(d);
            return r;
        }
        public static implicit operator BigDecimal(Decimal d)
        {
            var r = new BigDecimal(d);
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

        public static explicit operator BigInteger(BigDecimal value)
        {
            return value.Integer;
        }

        public static BigDecimal operator -(BigDecimal value)
        {
            value._Value = -value._Value;
            return value;
        }

        public static BigDecimal operator +(BigDecimal value)
        {
            return value;
        }
        
        public static BigDecimal operator ++(BigDecimal value)
        {
            return value + GetOne(value.Precision);
        }

        public static BigDecimal operator --(BigDecimal value)
        {
            return value - GetOne(value.Precision);
        }

        public static BigDecimal operator +(BigDecimal left, BigDecimal right)
        {
            if (left.Precision != right.Precision)
            {
                int mx = System.Math.Max(left.Precision, right.Precision);
                left.SetPrecision(mx);right.SetPrecision(mx);
            }
            if (right.IsZero) return left;
            if (left.IsZero) return right;
            BigDecimal res = new BigDecimal() { Precision = left.Precision };
            res._Value = left._Value + right._Value;
            return res;
        }

        public static BigDecimal operator -(BigDecimal left, BigDecimal right)
        {
            if (left.Precision != right.Precision)
            {
                int mx = System.Math.Max(left.Precision, right.Precision);
                left.SetPrecision(mx); right.SetPrecision(mx);
            }
            if (right.IsZero) return left;
            if (left.IsZero) return -right;
            BigDecimal res = new BigDecimal() { Precision = left.Precision };
            res._Value = left._Value - right._Value;
            return res;
        }

        public static BigDecimal operator *(BigDecimal left, BigDecimal right)
        {
            if (left.Precision != right.Precision)
            {
                int mx = System.Math.Max(left.Precision, right.Precision);
                left.SetPrecision(mx); right.SetPrecision(mx);
            }
            if (left.IsZero) return GetZero(left.Precision);
            if (right.IsZero) return GetZero(right.Precision);
            if (left.IsOne) return right;
            if (right.IsOne) return left;
            //if (left.IsOne && left.Sign==-1) return -right;
            //if (right.IsOne && right.Sign == -1) return -left;

            bool flg = left.Sign == right.Sign;
            BigDecimal res = new BigDecimal() { Precision = left.Precision };
            res._Value = BigInteger.Abs(left._Value) * BigInteger.Abs(right._Value)/GetTenPow(res.Precision);
            return flg ? res : -res;
        }

        public static BigDecimal operator /(BigDecimal dividend, BigDecimal divisor)
        {
            if (dividend.Precision != divisor.Precision)
            {
                int mx = System.Math.Max(dividend.Precision, divisor.Precision);
                dividend.SetPrecision(mx); divisor.SetPrecision(mx);
            }
            if (divisor.IsZero) throw new DivideByZeroException();
            if (dividend.IsZero)
            {
                GetZero(dividend.Precision);
            }
            if (divisor.IsOne)
            {
                return dividend;
            }
            //if (left.IsOne && left.Sign==-1) return -right;
            //if (right.IsOne && right.Sign == -1) return -left;
            bool flg = dividend.Sign == divisor.Sign;
            
            BigDecimal res = new BigDecimal() { Precision = dividend.Precision };
            res._Value = BigInteger.Abs(dividend._Value)* GetTenPow(res.Precision) / BigInteger.Abs(divisor._Value);
            return flg ? res : -res;
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
            switch (obj)
            {
                case BigDecimal d: return Equals(d);
                case BigInteger d:return Equals(d);
                case int d: return Equals(d);
                case long d: return Equals(d);
                case uint d: return Equals(d);
                case UInt64 d: return Equals(d);
                case short d: return Equals(d);
                case UInt16 d: return Equals(d);
                case double d: return Equals(d);
                case float d: return Equals(d);
                case decimal d: return Equals(d);
            }
            return Equals((BigDecimal)obj);
        }

        public bool Equals(Int64 other)
        {
            return this.Equals(new BigDecimal(other,Precision));
        }

        public bool Equals(UInt64 other)
        {
            return this.Equals(new BigDecimal(other, Precision));
        }

        public bool Equals(BigInteger other)
        {
            return this.Equals(new BigDecimal(other, Precision));
        }

        public bool Equals(double other)
        {
            return this.Equals(new BigDecimal(other, Precision));
        }

        public bool Equals(decimal other)
        {
            return this.Equals(new BigDecimal(other, Precision));
        }

        public bool Equals(float other)
        {
            return this.Equals(new BigDecimal(other, Precision));
        }

        public int CompareTo(Int64 other)
        {
            return this.CompareTo(new BigDecimal(other, Precision));
        }

        public int CompareTo(UInt64 other)
        {
            return this.CompareTo(new BigDecimal(other, Precision));
        }

        public int CompareTo(BigInteger other)
        {
            return this.CompareTo(new BigDecimal(other, Precision));
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
            var x = this;
            if (x.Precision != v.Precision)
            {
                int mx = System.Math.Max(x.Precision, v.Precision);
                x.SetPrecision(mx); v.SetPrecision(mx);
            }
            return x._Value.CompareTo(v._Value);
            //return  ? k : ((IComparable<BigInteger>)Decimal).CompareTo(v.Decimal);
        }

        public bool Equals(BigDecimal v)
        {
            //Attention this!
            var x = this;
            if (x.Precision != v.Precision)
            {
                int mx = System.Math.Max(x.Precision, v.Precision);
                x.SetPrecision(mx); v.SetPrecision(mx);
            }
            return x._Value.Equals(v._Value);
        }

        public static BigDecimal Parse(string strnum)
        {
            if (strnum.Contains(".")) {
                string[] s = strnum.Split('.');
                BigInteger inte = BigInteger.Parse(s[0]);
                BigInteger dec = BigInteger.Parse(s[1]);
                BigDecimal res = new BigDecimal(inte, s[1].Length);
                res._Value += dec;
                return res;
            }
            else
            {
                return new BigDecimal(BigInteger.Parse(strnum));
            }
        }

        public override string ToString()
        {
            switch (Sign)
            {
                case 0:
                    return "0";
                case 1:
                    {
                        if (Precision == 0) return this._Value.ToString();
                        var i = BigInteger.DivRem(this._Value, GetTenPow(Precision), out var d);
                        //var t = Precision;
                        return $"{i.ToString()}.{d.ToString().PadLeft(Precision,'0')}";
                    }
                case -1:
                    {
                        if (Precision == 0) return this._Value.ToString();
                        var i = BigInteger.DivRem(-this._Value, GetTenPow(Precision), out var d);
                        return $"-{i.ToString()}.{d.ToString().PadLeft(Precision, '0')}";
                    }

            }
            throw new Exception();
        }

        public override int GetHashCode()
        {
            var hashCode = 1451633208;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<BigInteger>.Default.GetHashCode(_Value);
            hashCode = hashCode * -1521134295 + Precision.GetHashCode();
            return hashCode;
        }

        #endregion public instance methods
    }
}
