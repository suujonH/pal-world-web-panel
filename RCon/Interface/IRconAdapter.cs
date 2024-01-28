using System.Text;

namespace PalWebControl.RCon.Interface
{
    public interface IRconAdapter
    {
        /// <summary>
        /// 已经连接
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 连接
        /// </summary>
        bool Connect(string address, int port);


        /// <summary>
        /// 验证
        /// </summary>
        bool Authenticate(string password);

        /// <summary>
        /// 断开连接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 执行命令
        /// </summary>

        string Run(string command);

        Encoding GetEncoding();
        void SetEncoding(Encoding encoding);
    }
}
