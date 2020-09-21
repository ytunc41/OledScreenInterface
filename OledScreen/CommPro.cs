using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UINT8 = System.Byte;
using INT8 = System.SByte;
using UINT16 = System.UInt16;
using INT16 = System.Int16;
using UINT32 = System.UInt32;
using INT32 = System.Int32;
using UINT64 = System.UInt64;
using FLOAT32 = System.Single;
using FLOAT64 = System.Double;

namespace OledScreen
{
    public class CommPro
    {
        public PACKET_STATUS packet_status;
        public PACKET_TYPE packet_type;

        public List<UINT8> txBuffer = new List<UINT8>();
        public List<UINT8> rxBuffer = new List<UINT8>();

        public bool PAKET_HAZIR_FLAG = false;
        public PACKET_TYPE_FLAG PACKET_TYPE_FLAG = new PACKET_TYPE_FLAG();
        public Error Error = new Error();
    }

    public static class SendPacket
    {
        public static UINT8 sof1 = 58;
        public static UINT8 sof2 = 34;
        public static UINT8 packetType;
        public static UINT8 packetCounter;
        public static UINT8 dataSize;
        public static UINT8[] data = new UINT8[255];
        public static UINT8 eof1 = 41;
        public static UINT8 eof2 = 69;
    }

    public static class ReceivedPacket
    {
        public static UINT8 sof1;
        public static UINT8 sof2;
        public static UINT8 packetType;
        public static UINT8 packetCounter;
        public static UINT8 dataSize;
        public static UINT8[] data = new UINT8[255];
        public static UINT8 eof1;
        public static UINT8 eof2;
    }

    public class Error
    {
        public UINT32 sof1;
        public UINT32 sof2;
        public UINT32 eof1;
        public UINT32 eof2;
    }

    public enum PACKET_STATUS
    {
        SOF1 = 0,
        SOF2 = 1,
        PACKET_TYPE = 2,
        PACKET_COUNTER = 3,
        DATA_SIZE = 4,
        DATA = 5,
        EOF1 = 6,
        EOF2 = 7
    }

    public enum CHECK_STATUS
    {
        SOF1 = 58,
        SOF2 = 34,
        EOF1 = 41,
        EOF2 = 69
    }

    public enum PACKET_TYPE
    {
        BAGLANTI_REQUEST = 0,
        BAGLANTI_OK,

        PROGRAM_REQUEST,
        PROGRAM_OK,

        READ_REQUEST,
        READ_OK
    }

    public class PACKET_TYPE_FLAG
    {
        public bool BAGLANTI_REQUEST;
        public bool BAGLANTI_OK;
        public bool PROGRAM_REQUEST;
        public bool PROGRAM_OK;
        public bool READ_REQUEST;
        public bool READ_OK;

        public PACKET_TYPE_FLAG()
        {
            this.ClearAll();
        }

        public void ClearAll()
        {
            this.BAGLANTI_REQUEST = false;
            this.BAGLANTI_OK = false;
            this.PROGRAM_REQUEST = false;
            this.PROGRAM_OK = false;
            this.READ_REQUEST = false;
            this.READ_OK = false;
        }
    }






}
