using NAND_Prog;
using System;
using System.ComponentModel.Composition;

namespace W29N02GVSIAA
{
    /*
     use the design :

      # region
         <some code>
      # endregion

    for automatically include <some code> in the READMY.md file in the repository
    */

   
    public class ChipAssembly
    {
        [Export("Chip")]
        ChipPrototype myChip = new ChipPrototype();



        #region Chip parameters

        //--------------------Vendor Specific Pin configuration---------------------------

        //  VSP1(38pin) - NC    
        //  VSP2(35pin) - NC
        //  VSP3(20pin) - NC

        ChipAssembly()
        {
            myChip.devManuf = "WINBOND";
            myChip.name = "W29N02GVSIAA";
            myChip.chipID = "EFDA909504";      // device ID - EFh DAh 90h 95h 04h (w29n02gv_reva.pdf page 26)

            myChip.width = Organization.x8;    // chip width - 8 bit
            myChip.bytesPP = 2048;             // page size - 2048 byte (2Kb)
            myChip.spareBytesPP = 64;          // size Spare Area - 64 byte
            myChip.pagesPB = 64;               // the number of pages per block - 64 
            myChip.bloksPLUN = 2048;           // number of blocks in CE - 2048
            myChip.LUNs = 1;                   // the amount of CE in the chip
            myChip.colAdrCycles = 2;           // cycles for column addressing
            myChip.rowAdrCycles = 3;           // cycles for row addressing 
            myChip.vcc = Vcc.v3_3;             // supply voltage

        #endregion


            #region Chip operations

            //------- Add chip operations    https://github.com/JuliProg/Wiki#command-set----------------------------------------------------

            myChip.Operations("Reset_FFh").                   // https://github.com/JuliProg/Wiki/wiki/Command-Sets#reset_ffhdll
                   Operations("Erase_60h_D0h").               // https://github.com/JuliProg/Wiki/wiki/Command-Sets#erase_60h_d0hdll
                   Operations("Read_00h_30h").                // https://github.com/JuliProg/Wiki/wiki/Command-Sets#read_00h_30hdll
                   Operations("PageProgram_80h_10h");         // https://github.com/JuliProg/Wiki/wiki/Command-Sets#pageprogram_80h_10hdll

            #endregion



            #region Chip registers (optional)

            //------- Add chip registers (optional)----------------------------------------------------

            myChip.registers.Add(                   // https://github.com/JuliProg/Wiki/wiki/StatusRegister
                "Status Register").
                Size(1).
                Operations("ReadStatus_70h").       // https://github.com/JuliProg/Wiki/wiki/Status-Register-operations#readstatus_70hdll
                Interpretation("SR_Interpreted").   // https://github.com/JuliProg/Wiki/wiki/Status-Register-Interpretation
                UseAsStatusRegister();



            myChip.registers.Add(                  // https://github.com/JuliProg/Wiki/wiki/ID-Register
                "Id Register").
                Size(5).
                Operations("ReadId_90h").          // https://github.com/JuliProg/Wiki/wiki/ID-Register-operations#readid_90hdll     
                Interpretation(ID_interpreting);
            
           
            myChip.registers.Add(                  // https://github.com/JuliProg/Wiki/wiki/UNIQUE-ID-Register
                "UNIQUE ID Register").
                Size(16).
                Operations("ReadUniqueId_EDh");     // https://github.com/JuliProg/Wiki/wiki/UNIQUE-ID-Register-operations         
                

            #endregion


        }

        #region Interpretation of ID-register values ​​(optional)

        public string ID_interpreting(Register register)   
        
        #endregion
        {
            byte[] content = register.GetContent();


            //BitConverter.ToString(register.GetContent(), 0, 1)
            //BitConverter.ToString(register.GetContent(), 1, 1)
            string messsage = "1st Byte    Maker Code = " + content[0].ToString("X2") + Environment.NewLine;
            messsage += ID_decoding(content[0],0) + Environment.NewLine;

            messsage += "2nd Byte    Device Code = " + content[1].ToString("X2") + Environment.NewLine;
            messsage += ID_decoding(content[1], 1) + Environment.NewLine;

            messsage += "3rd ID Data = " + content[2].ToString("X2") + Environment.NewLine;
            messsage += ID_decoding(content[2], 2) + Environment.NewLine;

            messsage += "4th ID Data = " + content[3].ToString("X2") + Environment.NewLine;
            messsage += ID_decoding(content[3], 3) + Environment.NewLine;

            messsage += "5th ID Data = " + content[4].ToString("X2") + Environment.NewLine;
            messsage += ID_decoding(content[4], 4) + Environment.NewLine;

            return messsage;
        }  
        private string ID_decoding(byte bt, int pos)
        {
            string str_result = String.Empty;

            var IO = new System.Collections.BitArray(new[] { bt });

            switch (pos)
            {
                case 0:
                    str_result += "Maker ";
                    if (bt == 0xEF)
                        str_result += "is Winbond";
                    else
                        str_result += "is not Winbond";
                    str_result += Environment.NewLine;
                    break;

                case 1:
                    str_result += "Device ";
                    if (bt == 0xDA)
                        str_result += "is W29N02GVSIAA";
                    else
                        str_result += "is not W29N02GVSIAA";
                    str_result += Environment.NewLine;
                    break;

                case 2:
                    
                    if (bt == 0x90)
                        str_result += "Cach Programming Supported";
                    else
                        str_result += "Not define";
                    str_result += Environment.NewLine;
                    break;
                    

                case 3:
                    if (bt == 0x95)
                    {
                        str_result += "Page Size:2KB \r\n";
                        str_result += "Spare Area Size:64b \r\n";
                        str_result += "BLK Size w/o Spare:128KB \r\n";
                        str_result += "Organized:x8 or x16 \r\n";
                        str_result += "Serial Access:25ns \r\n";
                    }                   
                    else
                        str_result += "Not define";
                    
                    str_result += Environment.NewLine;
                    break;

                case 4:
                    
                    str_result += "Not define";
                    str_result += Environment.NewLine;

                    break;
            }
            return str_result;
        }

       
    }

}
