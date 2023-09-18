using TOPRO.HSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topro.Extension.Plc.Dtos;
using TOPRO.PLC.Dtos;
using TOPRO.PLC.Enums;

namespace TOPRO.PLC
{
    public interface IOperationManager : IDisposable
    {
        /// <summary>
        /// 默认tcp初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        OperateResult DefaultConnectionAndInit(DefaultOperationDto input, bool longConnection = true);

        /// <summary>
        /// 串口初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        OperateResult SerialConnectionAndInit(SerialOperationDto input, bool longConnection = true);

        /// <summary>
        /// ModbusTcp初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        OperateResult ModbusConnectionAndInit(ModbusOperationDto input, bool longConnection = true);

        /// <summary>
        /// ModbusRtu - Ascii初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection">是否长连接</param>
        /// <returns></returns>
        OperateResult ModbusRtuOrAsciiConnectionAndInit(ModbusRtuOperationDto input, bool longConnection = true);

        /// <summary>
        /// 欧姆龙初始化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="longConnection"></param>
        /// <returns></returns>
        OperateResult OmRonConnectionAndInit(OmRonFinsTcpOperationDto input, bool longConnection = true);

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        OperateResult CloseConnection();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optName"></param>
        /// <param name="pams"></param>
        /// <returns></returns>
        //OperateResult<T> Read<T>(string optName, Dictionary<string, object> pams);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="optName"></param>
        /// <param name="pams"></param>
        /// <returns></returns>
        //OperateResult Write<T>(string optName, Dictionary<string, object> pams);

        /// <summary>
        /// 基本读
        /// 不支持modbus线圈读和离散读
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pams"></param>
        /// <returns></returns>
        OperateResult<T> Read<T>(Dictionary<string, object> pams);

        /// <summary>
        /// 基本写
        /// 不支持modbus线圈写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pams"></param>
        /// <returns></returns>
        OperateResult Write<T>(Dictionary<string, object> pams);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="datatype"></param>
        /// <param name="pams"></param>
        /// <returns></returns>
        OperateResult Write(Type datatype, Dictionary<string, object> pams);
    }
}
