![Create new chip](https://github.com/JuliProg/W29N02GVSIAA/workflows/Create%20new%20chip/badge.svg?event=repository_dispatch)
![ChipUpdate](https://github.com/JuliProg/W29N02GVSIAA/workflows/ChipUpdate/badge.svg)
# Join the development of the project ([list of tasks](https://github.com/users/JuliProg/projects/1))


# W29N02GVSIAA
Implementation of the W29N02GVSIAA chip for the JuliProg programmer

Dependency injection, DI based on MEF framework is used to connect the chip to the programmer.

<section class = "listing">

# Chip parameters
```c#


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

```
# Chip operations
```c#


            //------- Add chip operations    https://github.com/JuliProg/Wiki#command-set----------------------------------------------------

            myChip.Operations("Reset_FFh").                   // https://github.com/JuliProg/Wiki/wiki/Command-Sets#reset_ffhdll
                   Operations("Erase_60h_D0h").               // https://github.com/JuliProg/Wiki/wiki/Command-Sets#erase_60h_d0hdll
                   Operations("Read_00h_30h").                // https://github.com/JuliProg/Wiki/wiki/Command-Sets#read_00h_30hdll
                   Operations("PageProgram_80h_10h");         // https://github.com/JuliProg/Wiki/wiki/Command-Sets#pageprogram_80h_10hdll

```
# Chip registers (optional)
```c#


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
                

```
# Interpretation of ID-register values ​​(optional)
```c#


        public string ID_interpreting(Register register)   
        
```
</section>













footer
