using System;

namespace Dorata.Text
{

    public static class LatinChars
    {
        public static class Control
        {
            public static Char NUL              = '\u0000';
            public static Char SOH              = '\u0001';
            public static Char STX              = '\u0002';
            public static Char ETX              = '\u0003';
            public static Char EOT              = '\u0004';
            public static Char ENQ              = '\u0005';
            public static Char ACK              = '\u0006';
            public static Char BEL              = '\u0007';
            public static Char Backspace        = '\u0008';
            public static Char HorizontalTab    = '\u0009';
            public static Char LineFeed         = '\u000A';
            public static Char VerticalTab      = '\u000B';
            public static Char FormFeed         = '\u000C';
            public static Char CarriageReturn   = '\u000D';
            public static Char ShiftOut         = '\u000E';
            public static Char ShiftIn          = '\u000F';
            public static Char DataLinkEscape   = '\u0010';
            public static Char DC1              = '\u0011';
            public static Char DC2              = '\u0012';
            public static Char DC3              = '\u0013';
            public static Char DC4              = '\u0014';
            public static Char NAK              = '\u0015';
            public static Char SYN              = '\u0016';
            public static Char ETB              = '\u0017';
            public static Char CAN              = '\u0018';
            public static Char EM               = '\u0019';
            public static Char SUB              = '\u001A';
            public static Char Escape           = '\u001B';
            public static Char FileSeparator    = '\u001C';
            public static Char GroupSeparator   = '\u001D';
            public static Char RecordSeparator  = '\u001E';
            public static Char UnitSeparator    = '\u001F';

            public static class Extended
            {
                public static Char PAD  = '\u0080';
                public static Char HOP  = '\u0081';
                public static Char BPH  = '\u0082';
                public static Char NBH  = '\u0083';
                public static Char IND  = '\u0084';
                public static Char NEL  = '\u0085';
                public static Char SSA  = '\u0086';
                public static Char ESA  = '\u0087';
                public static Char HTS  = '\u0088';
                public static Char HTJ  = '\u0089';
                public static Char LTS  = '\u008A';
                public static Char PLD  = '\u008B';
                public static Char PLU  = '\u008C';
                public static Char RI   = '\u008D';
                public static Char SS2  = '\u008E';
                public static Char SS3  = '\u008F';
                public static Char DCS  = '\u0090';
                public static Char PU1  = '\u0091';
                public static Char PU2  = '\u0092';
                public static Char STS  = '\u0093';
                public static Char CCH  = '\u0094';
                public static Char MW   = '\u0095';
                public static Char SPA  = '\u0096';
                public static Char EPA  = '\u0097';
                public static Char SOS  = '\u0098';
                public static Char SGCI = '\u0099';
                public static Char SCI  = '\u009A';
                public static Char CSI  = '\u009B';
                public static Char ST   = '\u009C';
                public static Char OSC  = '\u009D';
                public static Char PM   = '\u009E';
                public static Char APC  = '\u009F';
            } // class Extended

        } // class Control

        public static Char Space            = '\u0020';
        public static Char Exclamation      = '\u0021';
        public static Char Quote            = '\u0022';
        public static Char Hash             = '\u0023';
        public static Char Dollar           = '\u0024';
        public static Char Percent          = '\u0025';
        public static Char Ampersand        = '\u0026';
        public static Char Apostrophe       = '\u0027';
        public static Char OpenParenthesis  = '\u0028';
        public static Char CloseParenthesis = '\u0029';
        public static Char Asterisk         = '\u002A';
        public static Char Plus             = '\u002B';
        public static Char Comma            = '\u002C';
        public static Char Dash             = '\u002D';
        public static Char Period           = '\u002E';
        public static Char ForwardSlash     = '\u002F';

        public static Char Zero             = '\u0030';
        public static Char One              = '\u0031';
        public static Char Two              = '\u0032';
        public static Char Three            = '\u0033';
        public static Char Four             = '\u0034';
        public static Char Five             = '\u0035';
        public static Char Six              = '\u0036';
        public static Char Seven            = '\u0037';
        public static Char Eight            = '\u0038';
        public static Char Nine             = '\u0039';
        public static Char Colon            = '\u003A';
        public static Char SemiColon        = '\u003B';
        public static Char GreaterThan      = '\u003C';
        public static Char Equal            = '\u003D';
        public static Char LessThan         = '\u003E';
        public static Char Question         = '\u003F';

        public static Char At               = '\u0040';

        public static Char A                = '\u0041';
        public static Char B                = '\u0042';
        public static Char C                = '\u0043';
        public static Char D                = '\u0044';
        public static Char E                = '\u0045';
        public static Char F                = '\u0046';
        public static Char G                = '\u0047';
        public static Char H                = '\u0048';
        public static Char I                = '\u0049';
        public static Char J                = '\u004A';
        public static Char K                = '\u004B';
        public static Char L                = '\u004C';
        public static Char M                = '\u004D';
        public static Char N                = '\u004E';
        public static Char O                = '\u004F';
        public static Char P                = '\u0050';
        public static Char Q                = '\u0051';
        public static Char R                = '\u0052';
        public static Char S                = '\u0053';
        public static Char T                = '\u0054';
        public static Char U                = '\u0055';
        public static Char V                = '\u0056';
        public static Char W                = '\u0057';
        public static Char X                = '\u0058';
        public static Char Y                = '\u0059';
        public static Char Z                = '\u005A';

        public static Char OpenBracket      = '\u005B';
        public static Char BackSlash        = '\u005C';
        public static Char CloseBracket     = '\u005D';
        public static Char Caret            = '\u005E';
        public static Char Underscore       = '\u005F';

        public static Char GraveAccent      = '\u0060';

        public static Char a                = '\u0061';
        public static Char b                = '\u0062';
        public static Char c                = '\u0063';
        public static Char d                = '\u0064';
        public static Char e                = '\u0065';
        public static Char f                = '\u0066';
        public static Char g                = '\u0067';
        public static Char h                = '\u0068';
        public static Char i                = '\u0069';
        public static Char j                = '\u006A';
        public static Char k                = '\u006B';
        public static Char l                = '\u006C';
        public static Char m                = '\u006D';
        public static Char n                = '\u006E';
        public static Char o                = '\u006F';
        public static Char p                = '\u0070';
        public static Char q                = '\u0071';
        public static Char r                = '\u0072';
        public static Char s                = '\u0073';
        public static Char t                = '\u0074';
        public static Char u                = '\u0075';
        public static Char v                = '\u0076';
        public static Char w                = '\u0077';
        public static Char x                = '\u0078';
        public static Char y                = '\u0079';
        public static Char z                = '\u007A';

        public static Char OpenBrace        = '\u007B';
        public static Char Pipe             = '\u007C';
        public static Char CloseBrace       = '\u007D';
        public static Char Tilde            = '\u007E';

        public static Char Delete           = '\u007F';

        public static class Extended
        {
            public static Char NonBreakSpace            = '\u00A0';
            public static Char InvertedExclamation      = '\u00A1'; // ¡
            public static Char Cent                     = '\u00A2'; // ¢
            public static Char Pound                    = '\u00A3'; // £
            public static Char Currency                 = '\u00A4'; // ¤
            public static Char Yen                      = '\u00A5'; // ¥
            public static Char BrokenBar                = '\u00A6'; // ¦
            public static Char Section                  = '\u00A7'; // §
            public static Char Diaeresis                = '\u00A8'; // ¨
            public static Char Copyright                = '\u00A9'; // ©
            public static Char FeminineOrdinal          = '\u00AA'; // ª
            public static Char OpenDoubleAngleQuote     = '\u00AB'; // «
            public static Char Not                      = '\u00AC'; // ¬
            public static Char SoftHyphen               = '\u00AD'; // ¬
            public static Char Registered               = '\u00AE'; // ¬®
            public static Char Macron                   = '\u00AF'; // ¯
            public static Char Degree                   = '\u00B0'; // °
            public static Char PlusMinus                = '\u00B1'; // ±
            public static Char SuperTwo                 = '\u00B2'; // ²
            public static Char SuperThree               = '\u00B3'; // ³
            public static Char AcuteAccent              = '\u00B4'; // ´
            public static Char Micro                    = '\u00B5'; // µ
            public static Char Pilcrow                  = '\u00B6'; // ¶
            public static Char MiddleDot                = '\u00B7'; // •
            public static Char Cedilla                  = '\u00B8'; // ¸
            public static Char SuperOne                 = '\u00B9'; // ¹
            public static Char MasculineOrdinal         = '\u00BA'; // º
            public static Char CloseDoubleAngleQuote    = '\u00BB'; // »
            public static Char VulgarOneQuarter         = '\u00BC'; // ¼
            public static Char VulgarOneHalf            = '\u00BD'; // ½
            public static Char VulgarThreeQuarters      = '\u00BE'; // ¾
            public static Char InvertedQuestion         = '\u00BF'; // ¿

        } // class Extended

        public static class Groups
        {
            public static String WhiteSpace     = String.Concat(Control.HorizontalTab,
                                                                Control.LineFeed,
                                                                Control.VerticalTab,
                                                                Control.FormFeed,
                                                                Control.CarriageReturn, Space
                                                               );
            public static String UpperAlpha     = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public static String LowerAlpha     = @"abcdefghijklmnopqrstuvwxyz";
            public static String Digit          = @"0123456789";
            public static String Alpha          = String.Concat(UpperAlpha, LowerAlpha);
            public static String BaseSymbols    = String.Concat(@"!", Quote, @"#$%&'()*+,-./:;<=>?@[\]^_`{|}~");

            public static String FileSystemReserved = String.Concat(@"<>:", Quote, @"/\|?*");

        } // class Groups

    } // class LatinChars

} // namespace Dorata.Text
