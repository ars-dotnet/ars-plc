using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TOPRO.HSL.Core
{
    /// <summary>
    /// 按照字节错位的数据转换类
    /// </summary>
    public class ReverseWordTransform : ByteTransformBase
    {
        #region Constructor

        /// <summary>
        /// 实例化一个默认的对象
        /// </summary>
        public ReverseWordTransform( )
        {
            DataFormat = DataFormat.ABCD;
        }

        #endregion

        #region Public Properties
        
        /// <summary>
        /// 字符串数据是否按照字来反转
        /// </summary>
        public bool IsStringReverse { get; set; }


        #endregion

        #region Get Value From Bytes

        /// <summary>
        /// 从缓存中提取byte数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns></returns>
        public override byte[] TransByteByType(byte[] buffer, int index, int length)
        {
            return ReverseBytesByWord(buffer, index, length);
        }

        /// <summary>
        /// 从缓存中提取short结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>short对象</returns>
        public override short TransInt16( byte[] buffer, int index )
        {
            return base.TransInt16( ReverseBytesByWord( buffer, index, 2 ), 0 );
        }



        /// <summary>
        /// 从缓存中提取ushort结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ushort对象</returns>
        public override ushort TransUInt16( byte[] buffer, int index )
        {
            return base.TransUInt16( ReverseBytesByWord( buffer, index, 2 ), 0 );
        }

        

        /// <summary>
        /// 从缓存中提取string结果，使用指定的编码
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">byte数组长度</param>
        /// <param name="encoding">字符串的编码</param>
        /// <returns>string对象</returns>
        public override string TransString( byte[] buffer, int index, int length, Encoding encoding )
        {
            byte[] tmp = TransByte( buffer, index, length );

            if(IsStringReverse)
            {
                return encoding.GetString( ReverseBytesByWord( tmp ) );
            }
            else
            {
                return encoding.GetString( tmp );
            }
        }

        #endregion

        #region Get Bytes From Value
        

        /// <summary>
        /// short数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( short[] values )
        {
            byte[] buffer = base.TransByte( values );
            return ReverseBytesByWord( buffer );
        }


        /// <summary>
        /// ushort数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( ushort[] values )
        {
            byte[] buffer = base.TransByte( values );
            return ReverseBytesByWord( buffer );
        }
        

        /// <summary>
        /// 使用指定的编码字符串转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <param name="encoding">字符串的编码方式</param>
        /// <returns>buffer数据</returns>
        public override byte[] TransByte( string value, Encoding encoding )
        {
            if (value == null) return null;
            byte[] buffer = encoding.GetBytes( value );
            buffer = BasicFramework.SoftBasic.ArrayExpandToLengthEven( buffer );
            if (IsStringReverse)
            {
                return ReverseBytesByWord( buffer );
            }
            else
            {
                return buffer;
            }
        }

        #endregion

    }
}
