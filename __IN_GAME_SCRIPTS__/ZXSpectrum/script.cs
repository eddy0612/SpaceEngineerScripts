/*
 * R e a d m e
 * -----------
 * 
 * In this file you can include any instructions or other comments you want to have injected onto the 
 * top of your final script. You can safely delete this file if you do not want any such comments.
 */

private bool debug = true;

private Core mycore = null;
private JDBG jdbg = null;
private JLCD jlcd = null;
private JCTRL jctrl = null;

List<IMyTerminalBlock> p1LCDs = null;
IMyShipController p1Ctrl = null;

public Program()
{
    jdbg = new JDBG(this, debug);
    jlcd = new JLCD(this, jdbg, true);
    jctrl = new JCTRL(this, jdbg, true);

    mycore = new Core(this);

    p1LCDs = jlcd.GetLCDsWithTag("GAMESCREEN");
    if (p1LCDs.Count == 0) {
        Echo("ERROR: No screens found - Please tag screen as GAMESCREEN");
        throw new Exception("Could not identify screen for p1 with tag GAMESCREEN");
    } else if (p1LCDs.Count != 1) {
        Echo("ERROR: " + p1LCDs.Count + " screens found - Only 1 supported");
        throw new Exception("Too many screens for p1 with tag GAMESCREEN");
    }

    List<IMyTerminalBlock> p1CTRLs = jctrl.GetCTRLsWithTag("GAMESEAT");
    if (p1CTRLs.Count != 1) {
        Echo("ERROR: " + p1CTRLs.Count + " controllers found for player 1. Please tag a seat with GAMESEAT");
        throw new Exception("Could not identify controller for p1 with tag GAMESEAT");
    } else {
        p1Ctrl = (IMyShipController)p1CTRLs[0];
    }

    jlcd.InitializeLCDs(p1LCDs, TextAlignment.LEFT);
    jlcd.SetupFontCustom(p1LCDs, 312, 416, true, 0.001F, 0.001F);
}

public void Save()
{
}

public void Main(string argument, UpdateType updateSource)
{
    Echo("Main called");
    mycore.CPU_Execute_Block();

    String screen = new string(mycore.videoScreenDataLCD);
    Echo("Drawing: " + screen.Length + " chars");
    jlcd.WriteToAllLCDs(p1LCDs, screen, false);
    Echo("Drawn");
}

public partial class Z80InstructionExecutor : IZ80InstructionExecutor
{
    private IZ80Registers RG;

    public IZ80ProcessorAgent ProcessorAgent { get; set; }

    public Z80InstructionExecutor()
    {
        Initialize_CB_InstructionsTable();
        Initialize_DD_InstructionsTable();
        Initialize_DDCB_InstructionsTable();
        Initialize_ED_InstructionsTable();
        Initialize_FD_InstructionsTable();
        Initialize_FDCB_InstructionsTable();
        Initialize_SingleByte_InstructionsTable();
        GenerateParityTable();
    }

    public int Execute(byte firstOpcodeByte)
    {
        RG = ProcessorAgent.RG;

        switch (firstOpcodeByte)
        {
            case 0xCB:
                return Execute_CB_Instruction();
            case 0xDD:
                return Execute_DD_Instruction();
            case 0xED:
                return Execute_ED_Instruction();
            case 0xFD:
                return Execute_FD_Instruction();
            default:
                return Execute_SingleByte_Instruction(firstOpcodeByte);
        }
    }

    private int Execute_CB_Instruction()
    {
        Inc_R();
        Inc_R();
        return CB_InstructionExecutors[ProcessorAgent.FetchNextOpcode()]();
    }

    private int Execute_ED_Instruction()
    {
        Inc_R();
        Inc_R();
        var secondOpcodeByte = ProcessorAgent.FetchNextOpcode();
        if (IsUnsupportedInstruction(secondOpcodeByte))
            return ExecuteUnsopported_ED_Instruction(secondOpcodeByte);
        else if (secondOpcodeByte >= 0xA0)
            return ED_Block_InstructionExecutors[secondOpcodeByte - 0xA0]();
        else
            return ED_InstructionExecutors[secondOpcodeByte - 0x40]();
    }

    private static bool IsUnsupportedInstruction(byte secondOpcodeByte)
    {
        return
            secondOpcodeByte < 0x40 ||
            Between(secondOpcodeByte, 0x80, 0x9F) ||
            Between(secondOpcodeByte, 0xA4, 0xA7) ||
            Between(secondOpcodeByte, 0xAC, 0xAF) ||
            Between(secondOpcodeByte, 0xB4, 0xB7) ||
            Between(secondOpcodeByte, 0xBC, 0xBF) ||
            secondOpcodeByte > 0xBF;
    }

protected virtual int ExecuteUnsopported_ED_Instruction(byte secondOpcodeByte)
    {
        return NOP2();
    }

    private int Execute_SingleByte_Instruction(byte firstOpcodeByte)
    {
        Inc_R();
        return SingleByte_InstructionExecutors[firstOpcodeByte]();
    }



    private void FetchFinished(bool isRet = false, bool isHalt = false, bool isLdSp = false,
        bool isEiOrDi = false)
    {
    }

    private void Inc_R()
    {
        ProcessorAgent.RG.R = Inc7Bits(ProcessorAgent.RG.R);
    }

    private short FetchWord()
    {
        return CreateShort(
            lowByte: ProcessorAgent.FetchNextOpcode(),
            highByte: ProcessorAgent.FetchNextOpcode());
    }

    private void WriteShortToMemory(ushort address, short value)
    {
        ProcessorAgent.WriteToMemory(address, GetLowByte(value));
        ProcessorAgent.WriteToMemory((ushort)(address + 1), GetHighByte(value));
    }

    private short ReadShortFromMemory(ushort address)
    {
        return CreateShort(
            ProcessorAgent.ReadFromMemory(address),
            ProcessorAgent.ReadFromMemory((ushort)(address + 1)));
    }

    private void SetFlags3and5From(byte value)
    {
        const int Flags_3_5 = 0x28;

        RG.F = (byte)((RG.F & ~Flags_3_5) | (value & Flags_3_5));
    }

    private void SetFlags3and5From(short value)
    {
        const int Flags_3_5 = 0x28;

        RG.F = (byte)((RG.F & ~Flags_3_5) | (value & Flags_3_5));
    }

}

public partial class Z80InstructionExecutor
{
byte ADC_HL_BC()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.BC;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd + RG.CF;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.SF = newValue & 0x8000;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.PF = (oldValue ^ valueToAdd ^ 0x8000) & (valueToAdd ^ newValue) & 0x8000;

        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte SBC_HL_BC()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.BC;
        var newValueInt = (ushort)oldValue - (ushort)valueToAdd - RG.CF;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.SF = newValue & 0x8000;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x8000;

        RG.NF = 1;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADC_HL_DE()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.DE;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd + RG.CF;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.SF = newValue & 0x8000;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.PF = (oldValue ^ valueToAdd ^ 0x8000) & (valueToAdd ^ newValue) & 0x8000;

        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte SBC_HL_DE()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.DE;
        var newValueInt = (ushort)oldValue - (ushort)valueToAdd - RG.CF;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.SF = newValue & 0x8000;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x8000;

        RG.NF = 1;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADC_HL_HL()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.HL;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd + RG.CF;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.SF = newValue & 0x8000;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.PF = (oldValue ^ valueToAdd ^ 0x8000) & (valueToAdd ^ newValue) & 0x8000;

        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte SBC_HL_HL()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.HL;
        var newValueInt = (ushort)oldValue - (ushort)valueToAdd - RG.CF;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.SF = newValue & 0x8000;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x8000;

        RG.NF = 1;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADC_HL_SP()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.SP;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd + RG.CF;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.SF = newValue & 0x8000;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.PF = (oldValue ^ valueToAdd ^ 0x8000) & (valueToAdd ^ newValue) & 0x8000;

        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte SBC_HL_SP()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.SP;
        var newValueInt = (ushort)oldValue - (ushort)valueToAdd - RG.CF;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.SF = newValue & 0x8000;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x8000;

        RG.NF = 1;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }
}

public partial class Z80InstructionExecutor
{
private byte ADC_A_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.A;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SBC_A_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.A;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte ADD_A_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.A;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SUB_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.A;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte CP_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.A;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 4;
    }

private byte CPI()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = ProcessorAgent.ReadFromMemory((ushort)RG.HL);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        var counter = RG.BC;
        RG.HL++;
        counter--;
        RG.BC = counter;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.PF = (RG.BC != 0);
        RG.NF = 1;
        var valueForFlags3And5 = (byte)(newValue - RG.HF);
        RG.Flag3 = GetBit(valueForFlags3And5, 3);
        RG.Flag5 = GetBit(valueForFlags3And5, 1);

        return 16;
    }

private byte CPD()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = ProcessorAgent.ReadFromMemory((ushort)RG.HL);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        var counter = RG.BC;
        RG.HL--;
        counter--;
        RG.BC = counter;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.PF = (RG.BC != 0);
        RG.NF = 1;
        var valueForFlags3And5 = (byte)(newValue - RG.HF);
        RG.Flag3 = GetBit(valueForFlags3And5, 3);
        RG.Flag5 = GetBit(valueForFlags3And5, 1);

        return 16;
    }

private byte CPIR()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = ProcessorAgent.ReadFromMemory((ushort)RG.HL);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        var counter = RG.BC;
        RG.HL++;
        counter--;
        RG.BC = counter;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.PF = (RG.BC != 0);
        RG.NF = 1;
        var valueForFlags3And5 = (byte)(newValue - RG.HF);
        RG.Flag3 = GetBit(valueForFlags3And5, 3);
        RG.Flag5 = GetBit(valueForFlags3And5, 1);

        if (counter != 0 && RG.ZF == 0)
        {
            RG.PC = (ushort)(RG.PC - 2);
            return 21;
        }

        return 16;
    }

private byte CPDR()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = ProcessorAgent.ReadFromMemory((ushort)RG.HL);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        var counter = RG.BC;
        RG.HL--;
        counter--;
        RG.BC = counter;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.PF = (RG.BC != 0);
        RG.NF = 1;
        var valueForFlags3And5 = (byte)(newValue - RG.HF);
        RG.Flag3 = GetBit(valueForFlags3And5, 3);
        RG.Flag5 = GetBit(valueForFlags3And5, 1);

        if (counter != 0 && RG.ZF == 0)
        {
            RG.PC = (ushort)(RG.PC - 2);
            return 21;
        }

        return 16;
    }

private byte ADC_A_B()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.B;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SBC_A_B()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.B;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte ADD_A_B()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.B;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SUB_B()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.B;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte CP_B()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.B;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 4;
    }

private byte ADC_A_C()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.C;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SBC_A_C()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.C;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte ADD_A_C()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.C;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SUB_C()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.C;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte CP_C()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.C;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 4;
    }

private byte ADC_A_D()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.D;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SBC_A_D()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.D;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte ADD_A_D()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.D;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SUB_D()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.D;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte CP_D()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.D;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 4;
    }

private byte ADC_A_E()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.E;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SBC_A_E()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.E;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte ADD_A_E()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.E;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SUB_E()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.E;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte CP_E()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.E;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 4;
    }

private byte ADC_A_H()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.H;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SBC_A_H()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.H;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte ADD_A_H()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.H;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SUB_H()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.H;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte CP_H()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.H;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 4;
    }

private byte ADC_A_L()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.L;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SBC_A_L()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.L;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte ADD_A_L()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.L;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte SUB_L()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.L;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte CP_L()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.L;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 4;
    }

private byte ADC_A_aHL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)RG.HL;
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte SBC_A_aHL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)RG.HL;
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte ADD_A_aHL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)RG.HL;
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte SUB_aHL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)RG.HL;
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte CP_aHL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)RG.HL;
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 7;
    }

private byte ADC_A_n()
    {
        var valueToAdd = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte SBC_A_n()
    {
        var valueToAdd = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte ADD_A_n()
    {
        var valueToAdd = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte SUB_n()
    {
        var valueToAdd = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte CP_n()
    {
        var valueToAdd = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 7;
    }

private byte ADC_A_IXH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXH;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte SBC_A_IXH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXH;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte ADD_A_IXH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXH;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte SUB_IXH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXH;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte CP_IXH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXH;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 8;
    }

private byte ADC_A_IXL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXL;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte SBC_A_IXL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXL;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte ADD_A_IXL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXL;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte SUB_IXL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXL;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte CP_IXL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IXL;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 8;
    }

private byte ADC_A_IYH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYH;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte SBC_A_IYH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYH;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte ADD_A_IYH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYH;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte SUB_IYH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYH;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte CP_IYH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYH;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 8;
    }

private byte ADC_A_IYL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYL;
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte SBC_A_IYL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYL;
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte ADD_A_IYL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYL;
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte SUB_IYL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYL;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte CP_IYL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var valueToAdd = RG.IYL;
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 8;
    }

private byte ADC_A_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte SBC_A_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte ADD_A_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte SUB_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte CP_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 19;
    }

private byte ADC_A_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue + (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte SBC_A_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd + RG.CF);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte ADD_A_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue + (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd ^ 0x80) & (valueToAdd ^ newValue) & 0x80;
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte SUB_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);
        RG.A = newValue;

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte CP_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        var valueToAdd = ProcessorAgent.ReadFromMemory(address);
        var newValueInt = (int)oldValue - (valueToAdd);
        var newValue = (byte)(newValueInt & 0xFF);

        RG.SF = newValue & 0x80;
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x10;
        RG.CF = (newValueInt & 0x100);
        RG.PF = (oldValue ^ valueToAdd) & (oldValue ^ newValue) & 0x80;
        RG.NF = 1;
        SetFlags3and5From(valueToAdd);

        return 19;
    }
}

public partial class Z80InstructionExecutor
{
byte ADD_HL_BC()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.BC;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 11;
    }

byte ADD_HL_DE()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.DE;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 11;
    }

byte ADD_HL_HL()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.HL;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 11;
    }

byte ADD_HL_SP()
    {
        FetchFinished();

        var oldValue = RG.HL;
        var valueToAdd = RG.SP;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.HL = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 11;
    }

byte ADD_IX_BC()
    {
        FetchFinished();

        var oldValue = RG.IX;
        var valueToAdd = RG.BC;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.IX = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADD_IX_DE()
    {
        FetchFinished();

        var oldValue = RG.IX;
        var valueToAdd = RG.DE;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.IX = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADD_IX_IX()
    {
        FetchFinished();

        var oldValue = RG.IX;
        var valueToAdd = RG.IX;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.IX = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADD_IX_SP()
    {
        FetchFinished();

        var oldValue = RG.IX;
        var valueToAdd = RG.SP;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.IX = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADD_IY_BC()
    {
        FetchFinished();

        var oldValue = RG.IY;
        var valueToAdd = RG.BC;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.IY = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADD_IY_DE()
    {
        FetchFinished();

        var oldValue = RG.IY;
        var valueToAdd = RG.DE;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.IY = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADD_IY_IY()
    {
        FetchFinished();

        var oldValue = RG.IY;
        var valueToAdd = RG.IY;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.IY = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }

byte ADD_IY_SP()
    {
        FetchFinished();

        var oldValue = RG.IY;
        var valueToAdd = RG.SP;
        var newValueInt = (ushort)oldValue + (ushort)valueToAdd;
        var newValue = (short)(newValueInt & 0xFFFF);
        RG.IY = newValue;

        RG.HF = (oldValue ^ newValue ^ valueToAdd) & 0x1000;
        RG.CF = (newValueInt & 0x10000);
        RG.NF = 0;
        SetFlags3and5From(GetHighByte(newValue));

        return 15;
    }
}

public class AfterInstructionExecutionEventArgs : ProcessorEventArgs
{
public AfterInstructionExecutionEventArgs(byte[] opcode, IExecutionStopper stopper, object localUserState,
        int tStates)
    {
        this.Opcode = opcode;
        this.ExecutionStopper = stopper;
        this.LocalUserState = localUserState;
        this.TotalTStates = tStates;
    }

public byte[] Opcode { get; set; }

public IExecutionStopper ExecutionStopper { get; private set; }

public int TotalTStates { get; private set; }
}

public partial class Z80InstructionExecutor
{
private byte AND_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.A;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte XOR_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.A;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte OR_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.A;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte AND_B()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.B;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte XOR_B()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.B;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte OR_B()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.B;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte AND_C()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.C;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte XOR_C()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.C;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte OR_C()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.C;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte AND_D()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.D;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte XOR_D()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.D;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte OR_D()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.D;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte AND_E()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.E;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte XOR_E()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.E;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte OR_E()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.E;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte AND_H()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.H;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte XOR_H()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.H;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte OR_H()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.H;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte AND_L()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.L;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte XOR_L()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.L;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte OR_L()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.L;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte AND_aHL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)RG.HL;
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte XOR_aHL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)RG.HL;
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte OR_aHL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)RG.HL;
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte AND_n()
    {
        var argument = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte XOR_n()
    {
        var argument = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte OR_n()
    {
        var argument = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 7;
    }

private byte AND_IXH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IXH;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte XOR_IXH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IXH;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte OR_IXH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IXH;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte AND_IXL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IXL;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte XOR_IXL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IXL;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte OR_IXL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IXL;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte AND_IYH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IYH;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte XOR_IYH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IYH;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte OR_IYH()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IYH;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte AND_IYL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IYL;
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte XOR_IYL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IYL;
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte OR_IYL()
    {
        FetchFinished();

        var oldValue = RG.A;
        var argument = RG.IYL;
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte AND_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte XOR_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte OR_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte AND_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue & argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 1;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte XOR_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue ^ argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }

private byte OR_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        var argument = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue | argument);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = 0;
        RG.PF = Parity[newValue];
        RG.NF = 0;
        RG.CF = 0;
        SetFlags3and5From(newValue);

        return 19;
    }
}

public class BeforeInstructionExecutionEventArgs : ProcessorEventArgs
{
public BeforeInstructionExecutionEventArgs(byte[] opcode, object localUserState)
    {
        this.Opcode = opcode;
        this.LocalUserState = localUserState;
    }

public byte[] Opcode { get; set; }
}

public class BeforeInstructionFetchEventArgs : ProcessorEventArgs
{
public BeforeInstructionFetchEventArgs(IExecutionStopper stopper)
    {
        this.ExecutionStopper = stopper;
    }

public IExecutionStopper ExecutionStopper { get; private set; }
}

public partial class Z80InstructionExecutor
{
byte BIT_0_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_1_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_2_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_3_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_4_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_5_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_6_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_7_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_0_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_1_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_2_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_3_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_4_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_5_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_6_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_7_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_0_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_1_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_2_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_3_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_4_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_5_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_6_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_7_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_0_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_1_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_2_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_3_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_4_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_5_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_6_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_7_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_0_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_1_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_2_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_3_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_4_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_5_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_6_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_7_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_0_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_1_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_2_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_3_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_4_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_5_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_6_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_7_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_0_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_1_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_2_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_3_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_4_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_5_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_6_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_7_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 8;
    }

byte BIT_0_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 12;
    }

byte BIT_1_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 12;
    }

byte BIT_2_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 12;
    }

byte BIT_3_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 12;
    }

byte BIT_4_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 12;
    }

byte BIT_5_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 12;
    }

byte BIT_6_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 12;
    }

byte BIT_7_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 12;
    }

byte BIT_0_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_1_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_2_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_3_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_4_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_5_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_6_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_7_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_0_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 0);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_1_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 1);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_2_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 2);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_3_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 3);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_4_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 4);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_5_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 5);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_6_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 6);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = 0;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }

byte BIT_7_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var bitValue = GetBit(oldValue, 7);
        RG.ZF = RG.PF = ~bitValue;
        RG.SF = bitValue;
        RG.HF = 1;
        RG.NF = 0;

        return 20;
    }
}

public struct Bit
{
public Bit(int value) : this()
    {
        Value = value == 0 ? 0 : 1;
    }

public int Value { get; private set; }

    public override string ToString()
    {
        return Value.ToString();
    }


    public static implicit operator Bit(int value)
    {
        return new Bit(value);
    }

    public static implicit operator int(Bit value)
    {
        return value.Value;
    }

    public static implicit operator bool(Bit value)
    {
        return value.Value == 1;
    }

    public static implicit operator Bit(bool value)
    {
        return new Bit(value ? 1 : 0);
    }



    public static Bit operator |(Bit value1, Bit value2)
    {
        return value1.Value | value2.Value;
    }

    public static Bit operator &(Bit value1, Bit value2)
    {
        return value1.Value & value2.Value;
    }

    public static Bit operator ^(Bit value1, Bit value2)
    {
        return value1.Value ^ value2.Value;
    }

    public static Bit operator ~(Bit value)
    {
        return new Bit(value.Value ^ 1);
    }

    public static Bit operator !(Bit value)
    {
        return ~value;
    }



    public static bool operator true(Bit value)
    {
        return value.Value == 1;
    }

    public static bool operator false(Bit value)
    {
        return value.Value == 0;
    }



    public static bool operator ==(Bit bitValue, int intValue)
    {
        return (bitValue.Value == 0 && intValue == 0) || (bitValue.Value == 1 && intValue != 0);
    }

    public static bool operator !=(Bit bitValue, int intValue)
    {
        return !(bitValue == intValue);
    }

public override bool Equals(object obj)
    {
        if (obj is int)
            return this == (int)obj;
        else
            return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }

}

public struct BitInfo1
{
    private byte data;

    public BitInfo1(byte data)
    {
        this.data = data == 255 ? (byte)1 : data;
    }

    public byte Bit7 => (byte)(this.data << 7);

    public byte BorderColor => (byte)((this.data >> 1) & 7);

    public bool SamRom => (this.data & 16) != 0;

    public bool Compressed => (this.data & 32) != 0;

}

public struct BitInfo2
{
    private byte data;

    public BitInfo2(byte data)
    {
        this.data = data;
    }

    public int InterruptMode => this.data & 3;

    public bool Issue2 => (this.data & 4) != 0;

    public bool DoubleFreq => (this.data & 8) != 0;

    public int VideoSyncMode => (this.data >> 4) & 3;

    public int Joystick => (this.data >> 6) & 3;
}

public partial class Z80InstructionExecutor
{
byte CCF()
    {
        FetchFinished();

        var oldCF = RG.CF;
        RG.NF = 0;
        RG.HF = oldCF;
        RG.CF = !oldCF;
        SetFlags3and5From(RG.A);

        return 4;
    }
}

public class ClockSynchronizer : IClockSynchronizer
{
    private const int MinMicrosecondsToWait = 10 * 1000;

    public decimal EffectiveClockFrequencyInMHz { get; set; }



    public void Start()
    {
    }

    public void Stop()
    {
    }

    public void TryWait(int periodLengthInCycles)
    {
    }
}

public partial class Core
{
    private static readonly int baseFreq = 60;
    MyGridProgram mypgm = null;

    public Core(MyGridProgram pgm) : this(new Core_Video())
    {
        mypgm = pgm;
    }
}

class Core_AudioIn
{
}

public partial class Core
{
    private bool audioInState = false;

    private Core(Core_AudioIn _) : this(new Core_AudioOut())
    {
    }

}

class Core_AudioOut
{
}

public partial class Core
{
    private Core(Core_AudioOut _) : this(new Core_IO())
    {
    }
}

class Core_CPU
{
}

public partial class Core : IMemory, IZ80InterruptSource
{
    private static readonly int cpuClockFreq = 3500000;
    private readonly Z80Processor cpu;
    private readonly float cpuSpeed;
    private readonly byte[] cpuRam;
    private bool cpuIrq;

    public int Size => cpuRam.Length;

    public byte this[int address]
    {
        get { return cpuRam[address]; }

        set { cpuRam[address] = value; }
    }


    public bool IntLineIsActive
    {
        get
        {
            if (cpuIrq)
            {
                cpuIrq = false;
                return true;
            }

            return false;
        }
    }

    public byte? ValueOnDataBus => 255;

    private Core(Core_CPU _)
    {
        cpu = new Z80Processor();
        cpuIrq = false;
        cpuSpeed = (float)cpuClockFreq / (float)baseFreq;
        cpuRam = new byte[65536];
        Array.Copy(ResourceLoader.Spectrum48KROM(), cpuRam, 16384);
        cpu.SetMemoryAccessMode(0, 16384, MemoryAccessMode.ReadOnly);
        cpu.Memory = this;
        cpu.RegisterInterruptSource(this);
    }

    public void CPU_Execute_Block()
    {
        mypgm.Echo("Running code");
        mypgm.Echo("Drawing screen");
        BuildVideoScreen();
    }


    public void SetContents(int startAddress, byte[] contents, int startIndex = 0, int? length = null)
    {
        Array.Copy(contents, 0, cpuRam, startIndex, length ?? contents.Length);
    }

    public byte[] GetContents(int startAddress, int length)
    {
        byte[] bytes = new byte[length];
        Array.Copy(cpuRam, startAddress, bytes, 0, length);
        return bytes;
    }
}

class Core_IO : IMemory
{
    public Action<int, byte> Set;
    public Func<int, byte> Get;

    public byte this[int address]
    {
        get { return Get(address); }

        set { Set(address, value); }
    }

    public int Size => 65536;

    public byte[] GetContents(int startAddress, int length)
    {
        throw new Exception("Not Implemented GetContents");
    }

    public void SetContents(int startAddress, byte[] contents, int startIndex = 0, int? length = null)
    {
        throw new Exception("Not Implemented SetContents");
    }
}

public partial class Core
{
    private readonly Dictionary<int, IntBuffer> ioKeyState;

    private Core(Core_IO portSpace) : this(new Core_CPU())
    {
        portSpace.Get = IO_PortIn;
        portSpace.Set = IO_PortOut;
        cpu.PortsSpace = portSpace;
        IntBuffer lastRow = new IntBuffer(0xbf);
        ioKeyState = new Dictionary<int, IntBuffer>
        {
            [0xfefe] = new IntBuffer(0xbf),
            [0xfdfe] = new IntBuffer(0xbf),
            [0xfbfe] = new IntBuffer(0xbf),
            [0xf7fe] = new IntBuffer(0xbf),
            [0xeffe] = new IntBuffer(0xbf),
            [0xdffe] = new IntBuffer(0xbf),
            [0xbffe] = new IntBuffer(0xbf),
            [0x7ffe] = lastRow,
            [0x00fe] = lastRow
        };
    }

    private byte IO_PortIn(int address)
    {
        if (ioKeyState.ContainsKey(address))
        {
            IntBuffer ks = ioKeyState[address];
            lock (ks)
            {
                byte b = (byte)ks.Value;
                if (audioInState) b |= 0x40;
                return b;
            }
        }

        return 255;
    }

    private void IO_PortOut(int address, byte value)
    {
        if (address == 254)
        {
            videoBorderColor = (byte)(value & 7);
        }
    }

    public void IO_KeyPress(Objectkey, bool down)
    {
    }
}

class Core_Video
{
}

public partial class Core
{
    private static readonly byte BRIGHT = 0xd7, NORM = 0xff;

    private byte videoBorderColor;
    private bool videoFlashInvert;
    private int videoFlashTimer;

    private char[] pal = new char[16];

    public readonly char[] videoScreenDataLCD;
    private int lastBorderColour = -1;
    public readonly char[][] borderlines;


    private Core(Core_Video coreVideo) : this(new Core_AudioIn())
    {
        videoBorderColor = 8;
        videoFlashInvert = false;

        videoScreenDataLCD = new char[312 * (416 + 2)];
        Array.Clear(videoScreenDataLCD, 0, videoScreenDataLCD.Length);
        for (int i = 1; i < 312; i++) {
            videoScreenDataLCD[(i * 418) + 416] = '\x0d';
            videoScreenDataLCD[(i * 418) + 417] = '\x0a';
        }

        videoFlashTimer = 30;
        pal[0] = ColorToChar(0, 0, 0);
        pal[1] = ColorToChar(0, 0, NORM);
        pal[2] = ColorToChar(NORM, 0, 0);
        pal[3] = ColorToChar(NORM, 0, NORM);
        pal[4] = ColorToChar(0, NORM, 0);
        pal[5] = ColorToChar(0, NORM, NORM);
        pal[6] = ColorToChar(NORM, NORM, 0);
        pal[7] = ColorToChar(NORM, NORM, NORM);
        pal[8] = ColorToChar(0, 0, 0);
        pal[9] = ColorToChar(0, 0, BRIGHT);
        pal[10] = ColorToChar(BRIGHT, 0, 0);
        pal[11] = ColorToChar(BRIGHT, 0, BRIGHT);
        pal[12] = ColorToChar(0, BRIGHT, 0);
        pal[13] = ColorToChar(0, BRIGHT, BRIGHT);
        pal[14] = ColorToChar(BRIGHT, BRIGHT, 0);
        pal[15] = ColorToChar(BRIGHT, BRIGHT, BRIGHT);

        borderlines = new char[16][];
        for (int i = 0; i < 16; i++) {
            borderlines[i] = new char[416];
            for (int j=0; j<416; j++) {
                borderlines[i][j] = pal[i];
            }
        }
    }

    public void BuildVideoScreen()
    {
        if (videoFlashTimer-- == 0)
        {
            videoFlashInvert = !videoFlashInvert;
            videoFlashTimer = 30;
        }

        for (int i = 0; i < 120; i++)
        {
            Video_DrawLine(i);
        }
        lastBorderColour = videoBorderColor;
        mypgm.Echo("Drawn");
    }

    private void Video_DrawLine(int lineNo)
    {
        if (lineNo < 8) return;

        int lineStart = lineNo * 418;
        if (lineNo < 64 || lineNo >= 256)
        {
            if (videoBorderColor != lastBorderColour) {
                Array.Copy(borderlines[videoBorderColor], 0, videoScreenDataLCD, lineStart, 416);
            }
            return;
        }

        if (videoBorderColor != lastBorderColour) {
            Array.Copy(borderlines[videoBorderColor], 0, videoScreenDataLCD, lineStart, 80);
            Array.Copy(borderlines[videoBorderColor], 0, videoScreenDataLCD, lineStart+336, 80);
        }
        lineStart += 80;
        lineNo -= 64;
        int charY = 0x5800 + ((lineNo >> 3) << 5);
        int lineAddr = ((lineNo & 0x07) << 8) | ((lineNo & 0x38) << 2) | ((lineNo & 0xC0) << 5) | 0x4000;
        for (int charX = 0; charX < 32; charX++)
        {
            byte att = cpuRam[charY + charX];
            int ink = att & 0x07;
            int paper = (att & 0x38) >> 3;
            if ((att & 0x40) != 0)
            {
                ink += 8;
                paper += 8;
            }

            bool flash = (att & 0x80) != 0;
            bool doFlash = flash && videoFlashInvert;
            byte byt = cpuRam[lineAddr++];
            for (int bit = 128; bit > 0; bit >>= 1)
            {
                if (doFlash)
                    videoScreenDataLCD[lineStart++] = pal[3];
                else
                    videoScreenDataLCD[lineStart++] = pal[3];
            }
        }
    }


    private void Fill(char[] array, char with, int start, int len)
    {
        int end = start + len;
        while (start < end) {
            array[start++] = with;
        }
    }

    public char ColorToChar(byte r, byte g, byte b)
    {
        const double bitSpacing = 255.0 / 7.0;
        return (char)(0xe100 + ((int)Math.Round(r / bitSpacing) << 6) + ((int)Math.Round(g / bitSpacing) << 3) +
                      (int)Math.Round(b / bitSpacing));
    }
}

public partial class Z80InstructionExecutor
{
byte CPL()
    {
        FetchFinished();

        RG.A = (byte)(RG.A ^ 0xFF);

        RG.HF = 1;
        RG.NF = 1;
        SetFlags3and5From(RG.A);

        return 4;
    }
}

public partial class Z80InstructionExecutor
{
byte DAA()
    {

        FetchFinished();


        var oldValue = RG.A;
        var newValue = oldValue;

        if (RG.HF || (oldValue & 0x0F) > 9) newValue = (byte)(newValue + (RG.NF ? -0x06 : 0x06));
        if (RG.CF || oldValue > 0x99) newValue = (byte)(newValue + (RG.NF ? -0x60 : 0x60));

        RG.CF |= (oldValue > 0x99);
        RG.HF = ((oldValue ^ newValue) & 0x10);
        RG.SF = (newValue & 0x80);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];
        SetFlags3and5From(newValue);

        RG.A = newValue;

        return 4;
    }
}

public partial class Z80InstructionExecutor
{
byte DI()
    {
        FetchFinished(isEiOrDi: true);

        RG.IFF1 = 0;
        RG.IFF2 = 0;

        return 4;
    }
}

public partial class Z80InstructionExecutor
{
byte DJNZ_d()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var oldValue = RG.B;
        RG.B = (byte)(oldValue - 1);

        if (oldValue == 1)
            return 8;

        RG.PC = (ushort)(RG.PC + (SByte)offset);
        return 13;
    }
}

public partial class Z80InstructionExecutor
{
byte EI()
    {
        FetchFinished(isEiOrDi: true);

        RG.IFF1 = 1;
        RG.IFF2 = 1;

        return 4;
    }
}

public partial class Z80InstructionExecutor
{
byte EX_aSP_HL()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;

        var temp = ReadShortFromMemory(sp);
        WriteShortToMemory(sp, RG.HL);
        RG.HL = temp;

        return 19;
    }

byte EX_aSP_IX()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;

        var temp = ReadShortFromMemory(sp);
        WriteShortToMemory(sp, RG.IX);
        RG.IX = temp;

        return 23;
    }

byte EX_aSP_IY()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;

        var temp = ReadShortFromMemory(sp);
        WriteShortToMemory(sp, RG.IY);
        RG.IY = temp;

        return 23;
    }
}

public partial class Z80InstructionExecutor
{
byte EX_AF_AF()
    {
        FetchFinished();

        var temp = RG.AF;
        RG.AF = RG.Alternate.AF;
        RG.Alternate.AF = temp;

        return 4;
    }
}

public partial class Z80InstructionExecutor
{
byte EX_DE_HL()
    {
        FetchFinished();

        var temp = RG.DE;
        RG.DE = RG.HL;
        RG.HL = temp;

        return 4;
    }
}

public partial class Z80InstructionExecutor
{
    private int Execute_DD_Instruction()
    {
        Inc_R();
        var secondOpcodeByte = ProcessorAgent.PeekNextOpcode();

        if (secondOpcodeByte == 0xCB)
        {
            Inc_R();
            ProcessorAgent.FetchNextOpcode();
            var offset = ProcessorAgent.FetchNextOpcode();
            return DDCB_InstructionExecutors[ProcessorAgent.FetchNextOpcode()](offset);
        }

        if (DD_InstructionExecutors.ContainsKey(secondOpcodeByte))
        {
            Inc_R();
            ProcessorAgent.FetchNextOpcode();
            return DD_InstructionExecutors[secondOpcodeByte]();
        }

        return NOP();
    }

    private int Execute_FD_Instruction()
    {
        Inc_R();
        var secondOpcodeByte = ProcessorAgent.PeekNextOpcode();

        if (secondOpcodeByte == 0xCB)
        {
            Inc_R();
            ProcessorAgent.FetchNextOpcode();
            var offset = ProcessorAgent.FetchNextOpcode();
            return FDCB_InstructionExecutors[ProcessorAgent.FetchNextOpcode()](offset);
        }

        if (FD_InstructionExecutors.ContainsKey(secondOpcodeByte))
        {
            Inc_R();
            ProcessorAgent.FetchNextOpcode();
            return FD_InstructionExecutors[secondOpcodeByte]();
        }

        return NOP();
    }
}

public partial class Z80InstructionExecutor
{
byte EXX()
    {
        FetchFinished();

        var tempBC = RG.BC;
        var tempDE = RG.DE;
        var tempHL = RG.HL;

        RG.BC = RG.Alternate.BC;
        RG.DE = RG.Alternate.DE;
        RG.HL = RG.Alternate.HL;

        RG.Alternate.BC = tempBC;
        RG.Alternate.DE = tempDE;
        RG.Alternate.HL = tempHL;

        return 4;
    }
}

public partial class Z80InstructionExecutor
{
byte HALT()
    {
        FetchFinished(isHalt: true);

        return 4;
    }
}

public interface IClockSynchronizer
{
decimal EffectiveClockFrequencyInMHz { get; set; }

void Start();

void Stop();

void TryWait(int periodLengthInCycles);
}

public interface IExecutionStopper
{
void Stop(bool isPause = false);
}

public partial class Z80InstructionExecutor
{
private byte IM_0()
    {
        FetchFinished();

        ProcessorAgent.SetInterruptMode(0);

        return 8;
    }

private byte IM_1()
    {
        FetchFinished();

        ProcessorAgent.SetInterruptMode(1);

        return 8;
    }

private byte IM_2()
    {
        FetchFinished();

        ProcessorAgent.SetInterruptMode(2);

        return 8;
    }
}

public interface IMainZ80Registers
{
short AF { get; set; }

short BC { get; set; }

short DE { get; set; }

short HL { get; set; }

byte A { get; set; }

byte F { get; set; }

byte B { get; set; }

byte C { get; set; }

byte D { get; set; }

byte E { get; set; }

byte H { get; set; }

byte L { get; set; }

Bit CF { get; set; }

Bit NF { get; set; }

Bit PF { get; set; }

Bit Flag3 { get; set; }

Bit HF { get; set; }

Bit Flag5 { get; set; }

Bit ZF { get; set; }

Bit SF { get; set; }
}

public interface IMemory
{
int Size { get; }

byte this[int address] { get; set; }

void SetContents(int startAddress, byte[] contents, int startIndex = 0, int? length = null);

byte[] GetContents(int startAddress, int length);
}

public partial class Z80InstructionExecutor
{
byte IN_A_n()
    {
        var portNumber = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        RG.A = ProcessorAgent.ReadFromPort(portNumber);

        return 11;
    }
}

public partial class Z80InstructionExecutor
{
byte IN_A_C()
    {
        FetchFinished();

        var value = ProcessorAgent.ReadFromPort((ushort)RG.BC);
        RG.A = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.NF = 0;
        RG.HF = 0;
        RG.PF = Parity[value];
        SetFlags3and5From(value);

        return 12;
    }

byte IN_B_C()
    {
        FetchFinished();

        var value = ProcessorAgent.ReadFromPort((ushort)RG.BC);
        RG.B = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.NF = 0;
        RG.HF = 0;
        RG.PF = Parity[value];
        SetFlags3and5From(value);

        return 12;
    }

byte IN_C_C()
    {
        FetchFinished();

        var value = ProcessorAgent.ReadFromPort((ushort)RG.BC);
        RG.C = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.NF = 0;
        RG.HF = 0;
        RG.PF = Parity[value];
        SetFlags3and5From(value);

        return 12;
    }

byte IN_D_C()
    {
        FetchFinished();

        var value = ProcessorAgent.ReadFromPort((ushort)RG.BC);
        RG.D = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.NF = 0;
        RG.HF = 0;
        RG.PF = Parity[value];
        SetFlags3and5From(value);

        return 12;
    }

byte IN_E_C()
    {
        FetchFinished();

        var value = ProcessorAgent.ReadFromPort((ushort)RG.BC);
        RG.E = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.NF = 0;
        RG.HF = 0;
        RG.PF = Parity[value];
        SetFlags3and5From(value);

        return 12;
    }

byte IN_H_C()
    {
        FetchFinished();

        var value = ProcessorAgent.ReadFromPort((ushort)RG.BC);
        RG.H = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.NF = 0;
        RG.HF = 0;
        RG.PF = Parity[value];
        SetFlags3and5From(value);

        return 12;
    }

byte IN_L_C()
    {
        FetchFinished();

        var value = ProcessorAgent.ReadFromPort((ushort)RG.BC);
        RG.L = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.NF = 0;
        RG.HF = 0;
        RG.PF = Parity[value];
        SetFlags3and5From(value);

        return 12;
    }

byte IN_F_C()
    {
        FetchFinished();

        var value = ProcessorAgent.ReadFromPort((ushort)RG.BC);

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.NF = 0;
        RG.HF = 0;
        RG.PF = Parity[value];
        SetFlags3and5From(value);

        return 12;
    }
}

public partial class Z80InstructionExecutor
{
private byte INC_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)(oldValue + 1);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte DEC_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)(oldValue - 1);
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte INC_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)(oldValue + 1);
        RG.B = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte DEC_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)(oldValue - 1);
        RG.B = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte INC_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)(oldValue + 1);
        RG.C = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte DEC_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)(oldValue - 1);
        RG.C = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte INC_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)(oldValue + 1);
        RG.D = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte DEC_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)(oldValue - 1);
        RG.D = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte INC_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)(oldValue + 1);
        RG.E = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte DEC_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)(oldValue - 1);
        RG.E = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte INC_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)(oldValue + 1);
        RG.H = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte DEC_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)(oldValue - 1);
        RG.H = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte INC_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)(oldValue + 1);
        RG.L = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte DEC_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)(oldValue - 1);
        RG.L = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 4;
    }

private byte INC_IXH()
    {
        FetchFinished();

        var oldValue = RG.IXH;
        var newValue = (byte)(oldValue + 1);
        RG.IXH = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte DEC_IXH()
    {
        FetchFinished();

        var oldValue = RG.IXH;
        var newValue = (byte)(oldValue - 1);
        RG.IXH = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte INC_IXL()
    {
        FetchFinished();

        var oldValue = RG.IXL;
        var newValue = (byte)(oldValue + 1);
        RG.IXL = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte DEC_IXL()
    {
        FetchFinished();

        var oldValue = RG.IXL;
        var newValue = (byte)(oldValue - 1);
        RG.IXL = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte INC_IYH()
    {
        FetchFinished();

        var oldValue = RG.IYH;
        var newValue = (byte)(oldValue + 1);
        RG.IYH = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte DEC_IYH()
    {
        FetchFinished();

        var oldValue = RG.IYH;
        var newValue = (byte)(oldValue - 1);
        RG.IYH = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte INC_IYL()
    {
        FetchFinished();

        var oldValue = RG.IYL;
        var newValue = (byte)(oldValue + 1);
        RG.IYL = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte DEC_IYL()
    {
        FetchFinished();

        var oldValue = RG.IYL;
        var newValue = (byte)(oldValue - 1);
        RG.IYL = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 8;
    }

private byte INC_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue + 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 11;
    }

private byte DEC_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue - 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 11;
    }

private byte INC_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue + 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 23;
    }

private byte DEC_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue - 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 23;
    }

private byte INC_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue + 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x00);
        RG.PF = (newValue == 0x80);
        RG.NF = 0;
        SetFlags3and5From(newValue);

        return 23;
    }

private byte DEC_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue - 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = ((newValue & 0x0F) == 0x0F);
        RG.PF = (newValue == 0x7F);
        RG.NF = 1;
        SetFlags3and5From(newValue);

        return 23;
    }
}

public partial class Z80InstructionExecutor
{
byte INC_BC()
    {
        FetchFinished();
        RG.BC++;
        return 6;
    }

byte DEC_BC()
    {
        FetchFinished();
        RG.BC--;
        return 6;
    }

byte INC_DE()
    {
        FetchFinished();
        RG.DE++;
        return 6;
    }

byte DEC_DE()
    {
        FetchFinished();
        RG.DE--;
        return 6;
    }

byte INC_HL()
    {
        FetchFinished();
        RG.HL++;
        return 6;
    }

byte DEC_HL()
    {
        FetchFinished();
        RG.HL--;
        return 6;
    }

byte INC_SP()
    {
        FetchFinished();
        RG.SP++;
        return 6;
    }

byte DEC_SP()
    {
        FetchFinished();
        RG.SP--;
        return 6;
    }

byte INC_IX()
    {
        FetchFinished();
        RG.IX++;
        return 10;
    }

byte DEC_IX()
    {
        FetchFinished();
        RG.IX--;
        return 10;
    }

byte INC_IY()
    {
        FetchFinished();
        RG.IY++;
        return 10;
    }

byte DEC_IY()
    {
        FetchFinished();
        RG.IY--;
        return 10;
    }
}

public partial class Z80InstructionExecutor
{
byte INI()
    {
        FetchFinished();

        var portNumber = RG.C;
        var address = RG.HL;
        var value = ProcessorAgent.ReadFromPort(portNumber);
        ProcessorAgent.WriteToMemory((ushort)address, value);

        RG.HL++;
        var counter = RG.B;
        counter = (byte)(counter - 1);
        RG.B = counter;
        RG.ZF = (counter == 0);
        RG.NF = 1;
        RG.SF = GetBit(counter, 7);
        SetFlags3and5From(counter);


        return 16;
    }

byte IND()
    {
        FetchFinished();

        var portNumber = RG.C;
        var address = RG.HL;
        var value = ProcessorAgent.ReadFromPort(portNumber);
        ProcessorAgent.WriteToMemory((ushort)address, value);

        RG.HL--;
        var counter = RG.B;
        counter = (byte)(counter - 1);
        RG.B = counter;
        RG.ZF = (counter == 0);
        RG.NF = 1;
        RG.SF = GetBit(counter, 7);
        SetFlags3and5From(counter);


        return 16;
    }

byte INIR()
    {
        FetchFinished();

        var portNumber = RG.C;
        var address = RG.HL;
        var value = ProcessorAgent.ReadFromPort(portNumber);
        ProcessorAgent.WriteToMemory((ushort)address, value);

        RG.HL++;
        var counter = RG.B;
        counter = (byte)(counter - 1);
        RG.B = counter;
        RG.ZF = (counter == 0);
        RG.NF = 1;
        RG.SF = GetBit(counter, 7);
        SetFlags3and5From(counter);

        if (counter != 0)
        {
            RG.PC = (ushort)(RG.PC - 2);
            return 21;
        }

        return 16;
    }

byte INDR()
    {
        FetchFinished();

        var portNumber = RG.C;
        var address = RG.HL;
        var value = ProcessorAgent.ReadFromPort(portNumber);
        ProcessorAgent.WriteToMemory((ushort)address, value);

        RG.HL--;
        var counter = RG.B;
        counter = (byte)(counter - 1);
        RG.B = counter;
        RG.ZF = (counter == 0);
        RG.NF = 1;
        RG.SF = GetBit(counter, 7);
        SetFlags3and5From(counter);

        if (counter != 0)
        {
            RG.PC = (ushort)(RG.PC - 2);
            return 21;
        }

        return 16;
    }

byte OUTI()
    {
        FetchFinished();

        var portNumber = RG.C;
        var address = RG.HL;
        var value = ProcessorAgent.ReadFromMemory((ushort)address);
        ProcessorAgent.WriteToPort(portNumber, value);

        RG.HL++;
        var counter = RG.B;
        counter = (byte)(counter - 1);
        RG.B = counter;
        RG.ZF = (counter == 0);
        RG.NF = 1;
        RG.SF = GetBit(counter, 7);
        SetFlags3and5From(counter);


        return 16;
    }

byte OUTD()
    {
        FetchFinished();

        var portNumber = RG.C;
        var address = RG.HL;
        var value = ProcessorAgent.ReadFromMemory((ushort)address);
        ProcessorAgent.WriteToPort(portNumber, value);

        RG.HL--;
        var counter = RG.B;
        counter = (byte)(counter - 1);
        RG.B = counter;
        RG.ZF = (counter == 0);
        RG.NF = 1;
        RG.SF = GetBit(counter, 7);
        SetFlags3and5From(counter);


        return 16;
    }

byte OTIR()
    {
        FetchFinished();

        var portNumber = RG.C;
        var address = RG.HL;
        var value = ProcessorAgent.ReadFromMemory((ushort)address);
        ProcessorAgent.WriteToPort(portNumber, value);

        RG.HL++;
        var counter = RG.B;
        counter = (byte)(counter - 1);
        RG.B = counter;
        RG.ZF = (counter == 0);
        RG.NF = 1;
        RG.SF = GetBit(counter, 7);
        SetFlags3and5From(counter);

        if (counter != 0)
        {
            RG.PC = (ushort)(RG.PC - 2);
            return 21;
        }

        return 16;
    }

byte OTDR()
    {
        FetchFinished();

        var portNumber = RG.C;
        var address = RG.HL;
        var value = ProcessorAgent.ReadFromMemory((ushort)address);
        ProcessorAgent.WriteToPort(portNumber, value);

        RG.HL--;
        var counter = RG.B;
        counter = (byte)(counter - 1);
        RG.B = counter;
        RG.ZF = (counter == 0);
        RG.NF = 1;
        RG.SF = GetBit(counter, 7);
        SetFlags3and5From(counter);

        if (counter != 0)
        {
            RG.PC = (ushort)(RG.PC - 2);
            return 21;
        }

        return 16;
    }
}

public class InstructionExecutionContext
{
    public InstructionExecutionContext()
    {
        StopReason = StopReason.NotApplicable;
        OpcodeBytes = new List<byte>();
    }

    public StopReason StopReason { get; set; }

    public bool MustStop
    {
        get { return StopReason != StopReason.NotApplicable; }
    }

    public void StartNewInstruction()
    {
        OpcodeBytes.Clear();
        FetchComplete = false;
        LocalUserStateFromPreviousEvent = null;
        AccummulatedMemoryWaitStates = 0;
        PeekedOpcode = null;
        IsEiOrDiInstruction = false;
    }

    public bool ExecutingBeforeInstructionEvent { get; set; }

    public bool FetchComplete { get; set; }

    public List<byte> OpcodeBytes { get; set; }

    public bool IsRetInstruction { get; set; }

    public bool IsLdSpInstruction { get; set; }

    public bool IsHaltInstruction { get; set; }

    public bool IsEiOrDiInstruction { get; set; }

    public short SpAfterInstructionFetch { get; set; }

    public object LocalUserStateFromPreviousEvent { get; set; }

    public int AccummulatedMemoryWaitStates { get; set; }

    public byte? PeekedOpcode { get; set; }

    public ushort AddressOfPeekedOpcode { get; set; }
}

public class InstructionFetchFinishedEventArgs : EventArgs
{
public bool IsRetInstruction { get; set; }

public bool IsLdSpInstruction { get; set; }

public bool IsHaltInstruction { get; set; }

public bool IsEiOrDiInstruction { get; set; }
}

public class InstructionFetchFinishedEventNotFiredException : Exception
{
public ushort InstructionAddress { get; set; }

public byte[] FetchedBytes { get; set; }

public InstructionFetchFinishedEventNotFiredException(
        ushort instructionAddress,
        byte[] fetchedBytes,
        string message = null,
        Exception innerException = null)
        : base(
            message ??
            "IZ80InstructionExecutor.Execute returned without having fired the InstructionFetchFinished event.",
            innerException)
    {
        this.InstructionAddress = instructionAddress;
        this.FetchedBytes = fetchedBytes;
    }
}

public partial class Z80InstructionExecutor
{
    private Func<byte>[] CB_InstructionExecutors;

    private void Initialize_CB_InstructionsTable()
    {
        CB_InstructionExecutors = new Func<byte>[]
        {
            RLC_B,
            RLC_C,
            RLC_D,
            RLC_E,
            RLC_H,
            RLC_L,
            RLC_aHL,
            RLC_A,
            RRC_B,
            RRC_C,
            RRC_D,
            RRC_E,
            RRC_H,
            RRC_L,
            RRC_aHL,
            RRC_A,
            RL_B,
            RL_C,
            RL_D,
            RL_E,
            RL_H,
            RL_L,
            RL_aHL,
            RL_A,
            RR_B,
            RR_C,
            RR_D,
            RR_E,
            RR_H,
            RR_L,
            RR_aHL,
            RR_A,
            SLA_B,
            SLA_C,
            SLA_D,
            SLA_E,
            SLA_H,
            SLA_L,
            SLA_aHL,
            SLA_A,
            SRA_B,
            SRA_C,
            SRA_D,
            SRA_E,
            SRA_H,
            SRA_L,
            SRA_aHL,
            SRA_A,
            SLL_B,
            SLL_C,
            SLL_D,
            SLL_E,
            SLL_H,
            SLL_L,
            SLL_aHL,
            SLL_A,
            SRL_B,
            SRL_C,
            SRL_D,
            SRL_E,
            SRL_H,
            SRL_L,
            SRL_aHL,
            SRL_A,
            BIT_0_B,
            BIT_0_C,
            BIT_0_D,
            BIT_0_E,
            BIT_0_H,
            BIT_0_L,
            BIT_0_aHL,
            BIT_0_A,
            BIT_1_B,
            BIT_1_C,
            BIT_1_D,
            BIT_1_E,
            BIT_1_H,
            BIT_1_L,
            BIT_1_aHL,
            BIT_1_A,
            BIT_2_B,
            BIT_2_C,
            BIT_2_D,
            BIT_2_E,
            BIT_2_H,
            BIT_2_L,
            BIT_2_aHL,
            BIT_2_A,
            BIT_3_B,
            BIT_3_C,
            BIT_3_D,
            BIT_3_E,
            BIT_3_H,
            BIT_3_L,
            BIT_3_aHL,
            BIT_3_A,
            BIT_4_B,
            BIT_4_C,
            BIT_4_D,
            BIT_4_E,
            BIT_4_H,
            BIT_4_L,
            BIT_4_aHL,
            BIT_4_A,
            BIT_5_B,
            BIT_5_C,
            BIT_5_D,
            BIT_5_E,
            BIT_5_H,
            BIT_5_L,
            BIT_5_aHL,
            BIT_5_A,
            BIT_6_B,
            BIT_6_C,
            BIT_6_D,
            BIT_6_E,
            BIT_6_H,
            BIT_6_L,
            BIT_6_aHL,
            BIT_6_A,
            BIT_7_B,
            BIT_7_C,
            BIT_7_D,
            BIT_7_E,
            BIT_7_H,
            BIT_7_L,
            BIT_7_aHL,
            BIT_7_A,
            RES_0_B,
            RES_0_C,
            RES_0_D,
            RES_0_E,
            RES_0_H,
            RES_0_L,
            RES_0_aHL,
            RES_0_A,
            RES_1_B,
            RES_1_C,
            RES_1_D,
            RES_1_E,
            RES_1_H,
            RES_1_L,
            RES_1_aHL,
            RES_1_A,
            RES_2_B,
            RES_2_C,
            RES_2_D,
            RES_2_E,
            RES_2_H,
            RES_2_L,
            RES_2_aHL,
            RES_2_A,
            RES_3_B,
            RES_3_C,
            RES_3_D,
            RES_3_E,
            RES_3_H,
            RES_3_L,
            RES_3_aHL,
            RES_3_A,
            RES_4_B,
            RES_4_C,
            RES_4_D,
            RES_4_E,
            RES_4_H,
            RES_4_L,
            RES_4_aHL,
            RES_4_A,
            RES_5_B,
            RES_5_C,
            RES_5_D,
            RES_5_E,
            RES_5_H,
            RES_5_L,
            RES_5_aHL,
            RES_5_A,
            RES_6_B,
            RES_6_C,
            RES_6_D,
            RES_6_E,
            RES_6_H,
            RES_6_L,
            RES_6_aHL,
            RES_6_A,
            RES_7_B,
            RES_7_C,
            RES_7_D,
            RES_7_E,
            RES_7_H,
            RES_7_L,
            RES_7_aHL,
            RES_7_A,
            SET_0_B,
            SET_0_C,
            SET_0_D,
            SET_0_E,
            SET_0_H,
            SET_0_L,
            SET_0_aHL,
            SET_0_A,
            SET_1_B,
            SET_1_C,
            SET_1_D,
            SET_1_E,
            SET_1_H,
            SET_1_L,
            SET_1_aHL,
            SET_1_A,
            SET_2_B,
            SET_2_C,
            SET_2_D,
            SET_2_E,
            SET_2_H,
            SET_2_L,
            SET_2_aHL,
            SET_2_A,
            SET_3_B,
            SET_3_C,
            SET_3_D,
            SET_3_E,
            SET_3_H,
            SET_3_L,
            SET_3_aHL,
            SET_3_A,
            SET_4_B,
            SET_4_C,
            SET_4_D,
            SET_4_E,
            SET_4_H,
            SET_4_L,
            SET_4_aHL,
            SET_4_A,
            SET_5_B,
            SET_5_C,
            SET_5_D,
            SET_5_E,
            SET_5_H,
            SET_5_L,
            SET_5_aHL,
            SET_5_A,
            SET_6_B,
            SET_6_C,
            SET_6_D,
            SET_6_E,
            SET_6_H,
            SET_6_L,
            SET_6_aHL,
            SET_6_A,
            SET_7_B,
            SET_7_C,
            SET_7_D,
            SET_7_E,
            SET_7_H,
            SET_7_L,
            SET_7_aHL,
            SET_7_A
        };
    }
}

public partial class Z80InstructionExecutor
{
    private IDictionary<byte, Func<byte>> DD_InstructionExecutors;

    private void Initialize_DD_InstructionsTable()
    {
        DD_InstructionExecutors = new Dictionary<byte, Func<byte>>
        {
            { 0x09, ADD_IX_BC },
            { 0x19, ADD_IX_DE },
            { 0x21, LD_IX_nn },
            { 0x22, LD_aa_IX },
            { 0x23, INC_IX },
            { 0x24, INC_IXH },
            { 0x25, DEC_IXH },
            { 0x26, LD_IXH_n },
            { 0x29, ADD_IX_IX },
            { 0x2A, LD_IX_aa },
            { 0x2B, DEC_IX },
            { 0x2C, INC_IXL },
            { 0x2D, DEC_IXL },
            { 0x2E, LD_IXL_n },
            { 0x34, INC_aIX_plus_n },
            { 0x35, DEC_aIX_plus_n },
            { 0x36, LD_aIX_plus_n_N },
            { 0x39, ADD_IX_SP },
            { 0x44, LD_B_IXH },
            { 0x45, LD_B_IXL },
            { 0x46, LD_B_aIX_plus_n },
            { 0x4C, LD_C_IXH },
            { 0x4D, LD_C_IXL },
            { 0x4E, LD_C_aIX_plus_n },
            { 0x54, LD_D_IXH },
            { 0x55, LD_D_IXL },
            { 0x56, LD_D_aIX_plus_n },
            { 0x5C, LD_E_IXH },
            { 0x5D, LD_E_IXL },
            { 0x5E, LD_E_aIX_plus_n },
            { 0x60, LD_IXH_B },
            { 0x61, LD_IXH_C },
            { 0x62, LD_IXH_D },
            { 0x63, LD_IXH_E },
            { 0x64, LD_IXH_IXH },
            { 0x65, LD_IXH_IXL },
            { 0x66, LD_H_aIX_plus_n },
            { 0x67, LD_IXH_A },
            { 0x68, LD_IXL_B },
            { 0x69, LD_IXL_C },
            { 0x6A, LD_IXL_D },
            { 0x6B, LD_IXL_E },
            { 0x6C, LD_IXL_H },
            { 0x6D, LD_IXL_IXL },
            { 0x6E, LD_L_aIX_plus_n },
            { 0x6F, LD_IXL_A },
            { 0x70, LD_aIX_plus_n_B },
            { 0x71, LD_aIX_plus_n_C },
            { 0x72, LD_aIX_plus_n_D },
            { 0x73, LD_aIX_plus_n_E },
            { 0x74, LD_aIX_plus_n_H },
            { 0x75, LD_aIX_plus_n_L },
            { 0x77, LD_aIX_plus_n_A },
            { 0x7C, LD_A_IXH },
            { 0x7D, LD_A_IXL },
            { 0x7E, LD_A_aIX_plus_n },
            { 0x84, ADD_A_IXH },
            { 0x85, ADD_A_IXL },
            { 0x86, ADD_A_aIX_plus_n },
            { 0x8C, ADC_A_IXH },
            { 0x8D, ADC_A_IXL },
            { 0x8E, ADC_A_aIX_plus_n },
            { 0x94, SUB_IXH },
            { 0x95, SUB_IXL },
            { 0x96, SUB_aIX_plus_n },
            { 0x9C, SBC_A_IXH },
            { 0x9D, SBC_A_IXL },
            { 0x9E, SBC_A_aIX_plus_n },
            { 0xA4, AND_IXH },
            { 0xA5, AND_IXL },
            { 0xA6, AND_aIX_plus_n },
            { 0xAC, XOR_IXH },
            { 0xAD, XOR_IXL },
            { 0xAE, XOR_aIX_plus_n },
            { 0xB4, OR_IXH },
            { 0xB5, OR_IXL },
            { 0xB6, OR_aIX_plus_n },
            { 0xBC, CP_IXH },
            { 0xBD, CP_IXL },
            { 0xBE, CP_aIX_plus_n },
            { 0xE1, POP_IX },
            { 0xE3, EX_aSP_IX },
            { 0xE5, PUSH_IX },
            { 0xE9, JP_aIX },
            { 0xF9, LD_SP_IX },
        };
    }
}

public partial class Z80InstructionExecutor
{
    private Func<byte, byte>[] DDCB_InstructionExecutors;

    private void Initialize_DDCB_InstructionsTable()
    {
        DDCB_InstructionExecutors = new Func<byte, byte>[]
        {
            RLC_aIX_plus_n_and_load_B,
            RLC_aIX_plus_n_and_load_C,
            RLC_aIX_plus_n_and_load_D,
            RLC_aIX_plus_n_and_load_E,
            RLC_aIX_plus_n_and_load_H,
            RLC_aIX_plus_n_and_load_L,
            RLC_aIX_plus_n,
            RLC_aIX_plus_n_and_load_A,
            RRC_aIX_plus_n_and_load_B,
            RRC_aIX_plus_n_and_load_C,
            RRC_aIX_plus_n_and_load_D,
            RRC_aIX_plus_n_and_load_E,
            RRC_aIX_plus_n_and_load_H,
            RRC_aIX_plus_n_and_load_L,
            RRC_aIX_plus_n,
            RRC_aIX_plus_n_and_load_A,
            RL_aIX_plus_n_and_load_B,
            RL_aIX_plus_n_and_load_C,
            RL_aIX_plus_n_and_load_D,
            RL_aIX_plus_n_and_load_E,
            RL_aIX_plus_n_and_load_H,
            RL_aIX_plus_n_and_load_L,
            RL_aIX_plus_n,
            RL_aIX_plus_n_and_load_A,
            RR_aIX_plus_n_and_load_B,
            RR_aIX_plus_n_and_load_C,
            RR_aIX_plus_n_and_load_D,
            RR_aIX_plus_n_and_load_E,
            RR_aIX_plus_n_and_load_H,
            RR_aIX_plus_n_and_load_L,
            RR_aIX_plus_n,
            RR_aIX_plus_n_and_load_A,
            SLA_aIX_plus_n_and_load_B,
            SLA_aIX_plus_n_and_load_C,
            SLA_aIX_plus_n_and_load_D,
            SLA_aIX_plus_n_and_load_E,
            SLA_aIX_plus_n_and_load_H,
            SLA_aIX_plus_n_and_load_L,
            SLA_aIX_plus_n,
            SLA_aIX_plus_n_and_load_A,
            SRA_aIX_plus_n_and_load_B,
            SRA_aIX_plus_n_and_load_C,
            SRA_aIX_plus_n_and_load_D,
            SRA_aIX_plus_n_and_load_E,
            SRA_aIX_plus_n_and_load_H,
            SRA_aIX_plus_n_and_load_L,
            SRA_aIX_plus_n,
            SRA_aIX_plus_n_and_load_A,
            SLL_aIX_plus_n_and_load_B,
            SLL_aIX_plus_n_and_load_C,
            SLL_aIX_plus_n_and_load_D,
            SLL_aIX_plus_n_and_load_E,
            SLL_aIX_plus_n_and_load_H,
            SLL_aIX_plus_n_and_load_L,
            SLL_aIX_plus_n,
            SLL_aIX_plus_n_and_load_A,
            SRL_aIX_plus_n_and_load_B,
            SRL_aIX_plus_n_and_load_C,
            SRL_aIX_plus_n_and_load_D,
            SRL_aIX_plus_n_and_load_E,
            SRL_aIX_plus_n_and_load_H,
            SRL_aIX_plus_n_and_load_L,
            SRL_aIX_plus_n,
            SRL_aIX_plus_n_and_load_A,
            BIT_0_aIX_plus_n,
            BIT_0_aIX_plus_n,
            BIT_0_aIX_plus_n,
            BIT_0_aIX_plus_n,
            BIT_0_aIX_plus_n,
            BIT_0_aIX_plus_n,
            BIT_0_aIX_plus_n,
            BIT_0_aIX_plus_n,
            BIT_1_aIX_plus_n,
            BIT_1_aIX_plus_n,
            BIT_1_aIX_plus_n,
            BIT_1_aIX_plus_n,
            BIT_1_aIX_plus_n,
            BIT_1_aIX_plus_n,
            BIT_1_aIX_plus_n,
            BIT_1_aIX_plus_n,
            BIT_2_aIX_plus_n,
            BIT_2_aIX_plus_n,
            BIT_2_aIX_plus_n,
            BIT_2_aIX_plus_n,
            BIT_2_aIX_plus_n,
            BIT_2_aIX_plus_n,
            BIT_2_aIX_plus_n,
            BIT_2_aIX_plus_n,
            BIT_3_aIX_plus_n,
            BIT_3_aIX_plus_n,
            BIT_3_aIX_plus_n,
            BIT_3_aIX_plus_n,
            BIT_3_aIX_plus_n,
            BIT_3_aIX_plus_n,
            BIT_3_aIX_plus_n,
            BIT_3_aIX_plus_n,
            BIT_4_aIX_plus_n,
            BIT_4_aIX_plus_n,
            BIT_4_aIX_plus_n,
            BIT_4_aIX_plus_n,
            BIT_4_aIX_plus_n,
            BIT_4_aIX_plus_n,
            BIT_4_aIX_plus_n,
            BIT_4_aIX_plus_n,
            BIT_5_aIX_plus_n,
            BIT_5_aIX_plus_n,
            BIT_5_aIX_plus_n,
            BIT_5_aIX_plus_n,
            BIT_5_aIX_plus_n,
            BIT_5_aIX_plus_n,
            BIT_5_aIX_plus_n,
            BIT_5_aIX_plus_n,
            BIT_6_aIX_plus_n,
            BIT_6_aIX_plus_n,
            BIT_6_aIX_plus_n,
            BIT_6_aIX_plus_n,
            BIT_6_aIX_plus_n,
            BIT_6_aIX_plus_n,
            BIT_6_aIX_plus_n,
            BIT_6_aIX_plus_n,
            BIT_7_aIX_plus_n,
            BIT_7_aIX_plus_n,
            BIT_7_aIX_plus_n,
            BIT_7_aIX_plus_n,
            BIT_7_aIX_plus_n,
            BIT_7_aIX_plus_n,
            BIT_7_aIX_plus_n,
            BIT_7_aIX_plus_n,
            RES_0_aIX_plus_n_and_load_B,
            RES_0_aIX_plus_n_and_load_C,
            RES_0_aIX_plus_n_and_load_D,
            RES_0_aIX_plus_n_and_load_E,
            RES_0_aIX_plus_n_and_load_H,
            RES_0_aIX_plus_n_and_load_L,
            RES_0_aIX_plus_n,
            RES_0_aIX_plus_n_and_load_A,
            RES_1_aIX_plus_n_and_load_B,
            RES_1_aIX_plus_n_and_load_C,
            RES_1_aIX_plus_n_and_load_D,
            RES_1_aIX_plus_n_and_load_E,
            RES_1_aIX_plus_n_and_load_H,
            RES_1_aIX_plus_n_and_load_L,
            RES_1_aIX_plus_n,
            RES_1_aIX_plus_n_and_load_A,
            RES_2_aIX_plus_n_and_load_B,
            RES_2_aIX_plus_n_and_load_C,
            RES_2_aIX_plus_n_and_load_D,
            RES_2_aIX_plus_n_and_load_E,
            RES_2_aIX_plus_n_and_load_H,
            RES_2_aIX_plus_n_and_load_L,
            RES_2_aIX_plus_n,
            RES_2_aIX_plus_n_and_load_A,
            RES_3_aIX_plus_n_and_load_B,
            RES_3_aIX_plus_n_and_load_C,
            RES_3_aIX_plus_n_and_load_D,
            RES_3_aIX_plus_n_and_load_E,
            RES_3_aIX_plus_n_and_load_H,
            RES_3_aIX_plus_n_and_load_L,
            RES_3_aIX_plus_n,
            RES_3_aIX_plus_n_and_load_A,
            RES_4_aIX_plus_n_and_load_B,
            RES_4_aIX_plus_n_and_load_C,
            RES_4_aIX_plus_n_and_load_D,
            RES_4_aIX_plus_n_and_load_E,
            RES_4_aIX_plus_n_and_load_H,
            RES_4_aIX_plus_n_and_load_L,
            RES_4_aIX_plus_n,
            RES_4_aIX_plus_n_and_load_A,
            RES_5_aIX_plus_n_and_load_B,
            RES_5_aIX_plus_n_and_load_C,
            RES_5_aIX_plus_n_and_load_D,
            RES_5_aIX_plus_n_and_load_E,
            RES_5_aIX_plus_n_and_load_H,
            RES_5_aIX_plus_n_and_load_L,
            RES_5_aIX_plus_n,
            RES_5_aIX_plus_n_and_load_A,
            RES_6_aIX_plus_n_and_load_B,
            RES_6_aIX_plus_n_and_load_C,
            RES_6_aIX_plus_n_and_load_D,
            RES_6_aIX_plus_n_and_load_E,
            RES_6_aIX_plus_n_and_load_H,
            RES_6_aIX_plus_n_and_load_L,
            RES_6_aIX_plus_n,
            RES_6_aIX_plus_n_and_load_A,
            RES_7_aIX_plus_n_and_load_B,
            RES_7_aIX_plus_n_and_load_C,
            RES_7_aIX_plus_n_and_load_D,
            RES_7_aIX_plus_n_and_load_E,
            RES_7_aIX_plus_n_and_load_H,
            RES_7_aIX_plus_n_and_load_L,
            RES_7_aIX_plus_n,
            RES_7_aIX_plus_n_and_load_A,
            SET_0_aIX_plus_n_and_load_B,
            SET_0_aIX_plus_n_and_load_C,
            SET_0_aIX_plus_n_and_load_D,
            SET_0_aIX_plus_n_and_load_E,
            SET_0_aIX_plus_n_and_load_H,
            SET_0_aIX_plus_n_and_load_L,
            SET_0_aIX_plus_n,
            SET_0_aIX_plus_n_and_load_A,
            SET_1_aIX_plus_n_and_load_B,
            SET_1_aIX_plus_n_and_load_C,
            SET_1_aIX_plus_n_and_load_D,
            SET_1_aIX_plus_n_and_load_E,
            SET_1_aIX_plus_n_and_load_H,
            SET_1_aIX_plus_n_and_load_L,
            SET_1_aIX_plus_n,
            SET_1_aIX_plus_n_and_load_A,
            SET_2_aIX_plus_n_and_load_B,
            SET_2_aIX_plus_n_and_load_C,
            SET_2_aIX_plus_n_and_load_D,
            SET_2_aIX_plus_n_and_load_E,
            SET_2_aIX_plus_n_and_load_H,
            SET_2_aIX_plus_n_and_load_L,
            SET_2_aIX_plus_n,
            SET_2_aIX_plus_n_and_load_A,
            SET_3_aIX_plus_n_and_load_B,
            SET_3_aIX_plus_n_and_load_C,
            SET_3_aIX_plus_n_and_load_D,
            SET_3_aIX_plus_n_and_load_E,
            SET_3_aIX_plus_n_and_load_H,
            SET_3_aIX_plus_n_and_load_L,
            SET_3_aIX_plus_n,
            SET_3_aIX_plus_n_and_load_A,
            SET_4_aIX_plus_n_and_load_B,
            SET_4_aIX_plus_n_and_load_C,
            SET_4_aIX_plus_n_and_load_D,
            SET_4_aIX_plus_n_and_load_E,
            SET_4_aIX_plus_n_and_load_H,
            SET_4_aIX_plus_n_and_load_L,
            SET_4_aIX_plus_n,
            SET_4_aIX_plus_n_and_load_A,
            SET_5_aIX_plus_n_and_load_B,
            SET_5_aIX_plus_n_and_load_C,
            SET_5_aIX_plus_n_and_load_D,
            SET_5_aIX_plus_n_and_load_E,
            SET_5_aIX_plus_n_and_load_H,
            SET_5_aIX_plus_n_and_load_L,
            SET_5_aIX_plus_n,
            SET_5_aIX_plus_n_and_load_A,
            SET_6_aIX_plus_n_and_load_B,
            SET_6_aIX_plus_n_and_load_C,
            SET_6_aIX_plus_n_and_load_D,
            SET_6_aIX_plus_n_and_load_E,
            SET_6_aIX_plus_n_and_load_H,
            SET_6_aIX_plus_n_and_load_L,
            SET_6_aIX_plus_n,
            SET_6_aIX_plus_n_and_load_A,
            SET_7_aIX_plus_n_and_load_B,
            SET_7_aIX_plus_n_and_load_C,
            SET_7_aIX_plus_n_and_load_D,
            SET_7_aIX_plus_n_and_load_E,
            SET_7_aIX_plus_n_and_load_H,
            SET_7_aIX_plus_n_and_load_L,
            SET_7_aIX_plus_n,
            SET_7_aIX_plus_n_and_load_A,
        };
    }
}

public partial class Z80InstructionExecutor
{
    private Func<byte>[] ED_InstructionExecutors;
    private Func<byte>[] ED_Block_InstructionExecutors;

    private void Initialize_ED_InstructionsTable()
    {
        ED_InstructionExecutors = new Func<byte>[]
        {
            IN_B_C,
            OUT_C_B,
            SBC_HL_BC,
            LD_aa_BC,
            NEG,
            RETN,
            IM_0,
            LD_I_A,
            IN_C_C,
            OUT_C_C,
            ADC_HL_BC,
            LD_BC_aa,
            NEG,
            RETI,
            IM_0,
            LD_R_A,
            IN_D_C,
            OUT_C_D,
            SBC_HL_DE,
            LD_aa_DE,
            NEG,
            RETN,
            IM_1,
            LD_A_I,
            IN_E_C,
            OUT_C_E,
            ADC_HL_DE,
            LD_DE_aa,
            NEG,
            RETI,
            IM_2,
            LD_A_R,
            IN_H_C,
            OUT_C_H,
            SBC_HL_HL,
            LD_aa_HL,
            NEG,
            RETN,
            IM_0,
            RRD,
            IN_L_C,
            OUT_C_L,
            ADC_HL_HL,
            LD_HL_aa,
            NEG,
            RETI,
            IM_0,
            RLD,
            IN_F_C,
            OUT_C_0,
            SBC_HL_SP,
            LD_aa_SP,
            NEG,
            RETN,
            IM_1,
            NOP2,
            IN_A_C,
            OUT_C_A,
            ADC_HL_SP,
            LD_SP_aa,
            NEG,
            RETI,
            IM_2,
            NOP2
        };

        ED_Block_InstructionExecutors = new Func<byte>[]
        {
            LDI,
            CPI,
            INI,
            OUTI,
            null, null, null, null,
            LDD,
            CPD,
            IND,
            OUTD,
            null, null, null, null,
            LDIR,
            CPIR,
            INIR,
            OTIR,
            null, null, null, null,
            LDDR,
            CPDR,
            INDR,
            OTDR,
        };
    }
}

public partial class Z80InstructionExecutor
{
    private IDictionary<byte, Func<byte>> FD_InstructionExecutors;

    private void Initialize_FD_InstructionsTable()
    {
        FD_InstructionExecutors = new Dictionary<byte, Func<byte>>
        {
            { 0x09, ADD_IY_BC },
            { 0x19, ADD_IY_DE },
            { 0x21, LD_IY_nn },
            { 0x22, LD_aa_IY },
            { 0x23, INC_IY },
            { 0x24, INC_IYH },
            { 0x25, DEC_IYH },
            { 0x26, LD_IYH_n },
            { 0x29, ADD_IY_IY },
            { 0x2A, LD_IY_aa },
            { 0x2B, DEC_IY },
            { 0x2C, INC_IYL },
            { 0x2D, DEC_IYL },
            { 0x2E, LD_IYL_n },
            { 0x34, INC_aIY_plus_n },
            { 0x35, DEC_aIY_plus_n },
            { 0x36, LD_aIY_plus_n_N },
            { 0x39, ADD_IY_SP },
            { 0x44, LD_B_IYH },
            { 0x45, LD_B_IYL },
            { 0x46, LD_B_aIY_plus_n },
            { 0x4C, LD_C_IYH },
            { 0x4D, LD_C_IYL },
            { 0x4E, LD_C_aIY_plus_n },
            { 0x54, LD_D_IYH },
            { 0x55, LD_D_IYL },
            { 0x56, LD_D_aIY_plus_n },
            { 0x5C, LD_E_IYH },
            { 0x5D, LD_E_IYL },
            { 0x5E, LD_E_aIY_plus_n },
            { 0x60, LD_IYH_B },
            { 0x61, LD_IYH_C },
            { 0x62, LD_IYH_D },
            { 0x63, LD_IYH_E },
            { 0x64, LD_IYH_IYH },
            { 0x65, LD_IYH_IYL },
            { 0x66, LD_H_aIY_plus_n },
            { 0x67, LD_IYH_A },
            { 0x68, LD_IYL_B },
            { 0x69, LD_IYL_C },
            { 0x6A, LD_IYL_D },
            { 0x6B, LD_IYL_E },
            { 0x6C, LD_IYL_H },
            { 0x6D, LD_IYL_IYL },
            { 0x6E, LD_L_aIY_plus_n },
            { 0x6F, LD_IYL_A },
            { 0x70, LD_aIY_plus_n_B },
            { 0x71, LD_aIY_plus_n_C },
            { 0x72, LD_aIY_plus_n_D },
            { 0x73, LD_aIY_plus_n_E },
            { 0x74, LD_aIY_plus_n_H },
            { 0x75, LD_aIY_plus_n_L },
            { 0x77, LD_aIY_plus_n_A },
            { 0x7C, LD_A_IYH },
            { 0x7D, LD_A_IYL },
            { 0x7E, LD_A_aIY_plus_n },
            { 0x84, ADD_A_IYH },
            { 0x85, ADD_A_IYL },
            { 0x86, ADD_A_aIY_plus_n },
            { 0x8C, ADC_A_IYH },
            { 0x8D, ADC_A_IYL },
            { 0x8E, ADC_A_aIY_plus_n },
            { 0x94, SUB_IYH },
            { 0x95, SUB_IYL },
            { 0x96, SUB_aIY_plus_n },
            { 0x9C, SBC_A_IYH },
            { 0x9D, SBC_A_IYL },
            { 0x9E, SBC_A_aIY_plus_n },
            { 0xA4, AND_IYH },
            { 0xA5, AND_IYL },
            { 0xA6, AND_aIY_plus_n },
            { 0xAC, XOR_IYH },
            { 0xAD, XOR_IYL },
            { 0xAE, XOR_aIY_plus_n },
            { 0xB4, OR_IYH },
            { 0xB5, OR_IYL },
            { 0xB6, OR_aIY_plus_n },
            { 0xBC, CP_IYH },
            { 0xBD, CP_IYL },
            { 0xBE, CP_aIY_plus_n },
            { 0xE1, POP_IY },
            { 0xE3, EX_aSP_IY },
            { 0xE5, PUSH_IY },
            { 0xE9, JP_aIY },
            { 0xF9, LD_SP_IY },
        };
    }
}

public partial class Z80InstructionExecutor
{
    private Func<byte, byte>[] FDCB_InstructionExecutors;

    private void Initialize_FDCB_InstructionsTable()
    {
        FDCB_InstructionExecutors = new Func<byte, byte>[]
        {
            RLC_aIY_plus_n_and_load_B,
            RLC_aIY_plus_n_and_load_C,
            RLC_aIY_plus_n_and_load_D,
            RLC_aIY_plus_n_and_load_E,
            RLC_aIY_plus_n_and_load_H,
            RLC_aIY_plus_n_and_load_L,
            RLC_aIY_plus_n,
            RLC_aIY_plus_n_and_load_A,
            RRC_aIY_plus_n_and_load_B,
            RRC_aIY_plus_n_and_load_C,
            RRC_aIY_plus_n_and_load_D,
            RRC_aIY_plus_n_and_load_E,
            RRC_aIY_plus_n_and_load_H,
            RRC_aIY_plus_n_and_load_L,
            RRC_aIY_plus_n,
            RRC_aIY_plus_n_and_load_A,
            RL_aIY_plus_n_and_load_B,
            RL_aIY_plus_n_and_load_C,
            RL_aIY_plus_n_and_load_D,
            RL_aIY_plus_n_and_load_E,
            RL_aIY_plus_n_and_load_H,
            RL_aIY_plus_n_and_load_L,
            RL_aIY_plus_n,
            RL_aIY_plus_n_and_load_A,
            RR_aIY_plus_n_and_load_B,
            RR_aIY_plus_n_and_load_C,
            RR_aIY_plus_n_and_load_D,
            RR_aIY_plus_n_and_load_E,
            RR_aIY_plus_n_and_load_H,
            RR_aIY_plus_n_and_load_L,
            RR_aIY_plus_n,
            RR_aIY_plus_n_and_load_A,
            SLA_aIY_plus_n_and_load_B,
            SLA_aIY_plus_n_and_load_C,
            SLA_aIY_plus_n_and_load_D,
            SLA_aIY_plus_n_and_load_E,
            SLA_aIY_plus_n_and_load_H,
            SLA_aIY_plus_n_and_load_L,
            SLA_aIY_plus_n,
            SLA_aIY_plus_n_and_load_A,
            SRA_aIY_plus_n_and_load_B,
            SRA_aIY_plus_n_and_load_C,
            SRA_aIY_plus_n_and_load_D,
            SRA_aIY_plus_n_and_load_E,
            SRA_aIY_plus_n_and_load_H,
            SRA_aIY_plus_n_and_load_L,
            SRA_aIY_plus_n,
            SRA_aIY_plus_n_and_load_A,
            SLL_aIY_plus_n_and_load_B,
            SLL_aIY_plus_n_and_load_C,
            SLL_aIY_plus_n_and_load_D,
            SLL_aIY_plus_n_and_load_E,
            SLL_aIY_plus_n_and_load_H,
            SLL_aIY_plus_n_and_load_L,
            SLL_aIY_plus_n,
            SLL_aIY_plus_n_and_load_A,
            SRL_aIY_plus_n_and_load_B,
            SRL_aIY_plus_n_and_load_C,
            SRL_aIY_plus_n_and_load_D,
            SRL_aIY_plus_n_and_load_E,
            SRL_aIY_plus_n_and_load_H,
            SRL_aIY_plus_n_and_load_L,
            SRL_aIY_plus_n,
            SRL_aIY_plus_n_and_load_A,
            BIT_0_aIY_plus_n,
            BIT_0_aIY_plus_n,
            BIT_0_aIY_plus_n,
            BIT_0_aIY_plus_n,
            BIT_0_aIY_plus_n,
            BIT_0_aIY_plus_n,
            BIT_0_aIY_plus_n,
            BIT_0_aIY_plus_n,
            BIT_1_aIY_plus_n,
            BIT_1_aIY_plus_n,
            BIT_1_aIY_plus_n,
            BIT_1_aIY_plus_n,
            BIT_1_aIY_plus_n,
            BIT_1_aIY_plus_n,
            BIT_1_aIY_plus_n,
            BIT_1_aIY_plus_n,
            BIT_2_aIY_plus_n,
            BIT_2_aIY_plus_n,
            BIT_2_aIY_plus_n,
            BIT_2_aIY_plus_n,
            BIT_2_aIY_plus_n,
            BIT_2_aIY_plus_n,
            BIT_2_aIY_plus_n,
            BIT_2_aIY_plus_n,
            BIT_3_aIY_plus_n,
            BIT_3_aIY_plus_n,
            BIT_3_aIY_plus_n,
            BIT_3_aIY_plus_n,
            BIT_3_aIY_plus_n,
            BIT_3_aIY_plus_n,
            BIT_3_aIY_plus_n,
            BIT_3_aIY_plus_n,
            BIT_4_aIY_plus_n,
            BIT_4_aIY_plus_n,
            BIT_4_aIY_plus_n,
            BIT_4_aIY_plus_n,
            BIT_4_aIY_plus_n,
            BIT_4_aIY_plus_n,
            BIT_4_aIY_plus_n,
            BIT_4_aIY_plus_n,
            BIT_5_aIY_plus_n,
            BIT_5_aIY_plus_n,
            BIT_5_aIY_plus_n,
            BIT_5_aIY_plus_n,
            BIT_5_aIY_plus_n,
            BIT_5_aIY_plus_n,
            BIT_5_aIY_plus_n,
            BIT_5_aIY_plus_n,
            BIT_6_aIY_plus_n,
            BIT_6_aIY_plus_n,
            BIT_6_aIY_plus_n,
            BIT_6_aIY_plus_n,
            BIT_6_aIY_plus_n,
            BIT_6_aIY_plus_n,
            BIT_6_aIY_plus_n,
            BIT_6_aIY_plus_n,
            BIT_7_aIY_plus_n,
            BIT_7_aIY_plus_n,
            BIT_7_aIY_plus_n,
            BIT_7_aIY_plus_n,
            BIT_7_aIY_plus_n,
            BIT_7_aIY_plus_n,
            BIT_7_aIY_plus_n,
            BIT_7_aIY_plus_n,
            RES_0_aIY_plus_n_and_load_B,
            RES_0_aIY_plus_n_and_load_C,
            RES_0_aIY_plus_n_and_load_D,
            RES_0_aIY_plus_n_and_load_E,
            RES_0_aIY_plus_n_and_load_H,
            RES_0_aIY_plus_n_and_load_L,
            RES_0_aIY_plus_n,
            RES_0_aIY_plus_n_and_load_A,
            RES_1_aIY_plus_n_and_load_B,
            RES_1_aIY_plus_n_and_load_C,
            RES_1_aIY_plus_n_and_load_D,
            RES_1_aIY_plus_n_and_load_E,
            RES_1_aIY_plus_n_and_load_H,
            RES_1_aIY_plus_n_and_load_L,
            RES_1_aIY_plus_n,
            RES_1_aIY_plus_n_and_load_A,
            RES_2_aIY_plus_n_and_load_B,
            RES_2_aIY_plus_n_and_load_C,
            RES_2_aIY_plus_n_and_load_D,
            RES_2_aIY_plus_n_and_load_E,
            RES_2_aIY_plus_n_and_load_H,
            RES_2_aIY_plus_n_and_load_L,
            RES_2_aIY_plus_n,
            RES_2_aIY_plus_n_and_load_A,
            RES_3_aIY_plus_n_and_load_B,
            RES_3_aIY_plus_n_and_load_C,
            RES_3_aIY_plus_n_and_load_D,
            RES_3_aIY_plus_n_and_load_E,
            RES_3_aIY_plus_n_and_load_H,
            RES_3_aIY_plus_n_and_load_L,
            RES_3_aIY_plus_n,
            RES_3_aIY_plus_n_and_load_A,
            RES_4_aIY_plus_n_and_load_B,
            RES_4_aIY_plus_n_and_load_C,
            RES_4_aIY_plus_n_and_load_D,
            RES_4_aIY_plus_n_and_load_E,
            RES_4_aIY_plus_n_and_load_H,
            RES_4_aIY_plus_n_and_load_L,
            RES_4_aIY_plus_n,
            RES_4_aIY_plus_n_and_load_A,
            RES_5_aIY_plus_n_and_load_B,
            RES_5_aIY_plus_n_and_load_C,
            RES_5_aIY_plus_n_and_load_D,
            RES_5_aIY_plus_n_and_load_E,
            RES_5_aIY_plus_n_and_load_H,
            RES_5_aIY_plus_n_and_load_L,
            RES_5_aIY_plus_n,
            RES_5_aIY_plus_n_and_load_A,
            RES_6_aIY_plus_n_and_load_B,
            RES_6_aIY_plus_n_and_load_C,
            RES_6_aIY_plus_n_and_load_D,
            RES_6_aIY_plus_n_and_load_E,
            RES_6_aIY_plus_n_and_load_H,
            RES_6_aIY_plus_n_and_load_L,
            RES_6_aIY_plus_n,
            RES_6_aIY_plus_n_and_load_A,
            RES_7_aIY_plus_n_and_load_B,
            RES_7_aIY_plus_n_and_load_C,
            RES_7_aIY_plus_n_and_load_D,
            RES_7_aIY_plus_n_and_load_E,
            RES_7_aIY_plus_n_and_load_H,
            RES_7_aIY_plus_n_and_load_L,
            RES_7_aIY_plus_n,
            RES_7_aIY_plus_n_and_load_A,
            SET_0_aIY_plus_n_and_load_B,
            SET_0_aIY_plus_n_and_load_C,
            SET_0_aIY_plus_n_and_load_D,
            SET_0_aIY_plus_n_and_load_E,
            SET_0_aIY_plus_n_and_load_H,
            SET_0_aIY_plus_n_and_load_L,
            SET_0_aIY_plus_n,
            SET_0_aIY_plus_n_and_load_A,
            SET_1_aIY_plus_n_and_load_B,
            SET_1_aIY_plus_n_and_load_C,
            SET_1_aIY_plus_n_and_load_D,
            SET_1_aIY_plus_n_and_load_E,
            SET_1_aIY_plus_n_and_load_H,
            SET_1_aIY_plus_n_and_load_L,
            SET_1_aIY_plus_n,
            SET_1_aIY_plus_n_and_load_A,
            SET_2_aIY_plus_n_and_load_B,
            SET_2_aIY_plus_n_and_load_C,
            SET_2_aIY_plus_n_and_load_D,
            SET_2_aIY_plus_n_and_load_E,
            SET_2_aIY_plus_n_and_load_H,
            SET_2_aIY_plus_n_and_load_L,
            SET_2_aIY_plus_n,
            SET_2_aIY_plus_n_and_load_A,
            SET_3_aIY_plus_n_and_load_B,
            SET_3_aIY_plus_n_and_load_C,
            SET_3_aIY_plus_n_and_load_D,
            SET_3_aIY_plus_n_and_load_E,
            SET_3_aIY_plus_n_and_load_H,
            SET_3_aIY_plus_n_and_load_L,
            SET_3_aIY_plus_n,
            SET_3_aIY_plus_n_and_load_A,
            SET_4_aIY_plus_n_and_load_B,
            SET_4_aIY_plus_n_and_load_C,
            SET_4_aIY_plus_n_and_load_D,
            SET_4_aIY_plus_n_and_load_E,
            SET_4_aIY_plus_n_and_load_H,
            SET_4_aIY_plus_n_and_load_L,
            SET_4_aIY_plus_n,
            SET_4_aIY_plus_n_and_load_A,
            SET_5_aIY_plus_n_and_load_B,
            SET_5_aIY_plus_n_and_load_C,
            SET_5_aIY_plus_n_and_load_D,
            SET_5_aIY_plus_n_and_load_E,
            SET_5_aIY_plus_n_and_load_H,
            SET_5_aIY_plus_n_and_load_L,
            SET_5_aIY_plus_n,
            SET_5_aIY_plus_n_and_load_A,
            SET_6_aIY_plus_n_and_load_B,
            SET_6_aIY_plus_n_and_load_C,
            SET_6_aIY_plus_n_and_load_D,
            SET_6_aIY_plus_n_and_load_E,
            SET_6_aIY_plus_n_and_load_H,
            SET_6_aIY_plus_n_and_load_L,
            SET_6_aIY_plus_n,
            SET_6_aIY_plus_n_and_load_A,
            SET_7_aIY_plus_n_and_load_B,
            SET_7_aIY_plus_n_and_load_C,
            SET_7_aIY_plus_n_and_load_D,
            SET_7_aIY_plus_n_and_load_E,
            SET_7_aIY_plus_n_and_load_H,
            SET_7_aIY_plus_n_and_load_L,
            SET_7_aIY_plus_n,
            SET_7_aIY_plus_n_and_load_A,
        };
    }
}

public partial class Z80InstructionExecutor
{
    private Func<byte>[] SingleByte_InstructionExecutors;

    private void Initialize_SingleByte_InstructionsTable()
    {
        SingleByte_InstructionExecutors = new Func<byte>[]
        {
            NOP,
            LD_BC_nn,
            LD_aBC_A,
            INC_BC,
            INC_B,
            DEC_B,
            LD_B_n,
            RLCA,
            EX_AF_AF,
            ADD_HL_BC,
            LD_A_aBC,
            DEC_BC,
            INC_C,
            DEC_C,
            LD_C_n,
            RRCA,
            DJNZ_d,
            LD_DE_nn,
            LD_aDE_A,
            INC_DE,
            INC_D,
            DEC_D,
            LD_D_n,
            RLA,
            JR_d,
            ADD_HL_DE,
            LD_A_aDE,
            DEC_DE,
            INC_E,
            DEC_E,
            LD_E_n,
            RRA,
            JR_NZ_d,
            LD_HL_nn,
            LD_aa_HL,
            INC_HL,
            INC_H,
            DEC_H,
            LD_H_n,
            DAA,
            JR_Z_d,
            ADD_HL_HL,
            LD_HL_aa,
            DEC_HL,
            INC_L,
            DEC_L,
            LD_L_n,
            CPL,
            JR_NC_d,
            LD_SP_nn,
            LD_aa_A,
            INC_SP,
            INC_aHL,
            DEC_aHL,
            LD_aHL_N,
            SCF,
            JR_C_d,
            ADD_HL_SP,
            LD_A_aa,
            DEC_SP,
            INC_A,
            DEC_A,
            LD_A_n,
            CCF,
            LD_B_B,
            LD_B_C,
            LD_B_D,
            LD_B_E,
            LD_B_H,
            LD_B_L,
            LD_B_aHL,
            LD_B_A,
            LD_C_B,
            LD_C_C,
            LD_C_D,
            LD_C_E,
            LD_C_H,
            LD_C_L,
            LD_C_aHL,
            LD_C_A,
            LD_D_B,
            LD_D_C,
            LD_D_D,
            LD_D_E,
            LD_D_H,
            LD_D_L,
            LD_D_aHL,
            LD_D_A,
            LD_E_B,
            LD_E_C,
            LD_E_D,
            LD_E_E,
            LD_E_H,
            LD_E_L,
            LD_E_aHL,
            LD_E_A,
            LD_H_B,
            LD_H_C,
            LD_H_D,
            LD_H_E,
            LD_H_H,
            LD_H_L,
            LD_H_aHL,
            LD_H_A,
            LD_L_B,
            LD_L_C,
            LD_L_D,
            LD_L_E,
            LD_L_H,
            LD_L_L,
            LD_L_aHL,
            LD_L_A,
            LD_aHL_B,
            LD_aHL_C,
            LD_aHL_D,
            LD_aHL_E,
            LD_aHL_H,
            LD_aHL_L,
            HALT,
            LD_aHL_A,
            LD_A_B,
            LD_A_C,
            LD_A_D,
            LD_A_E,
            LD_A_H,
            LD_A_L,
            LD_A_aHL,
            LD_A_A,
            ADD_A_B,
            ADD_A_C,
            ADD_A_D,
            ADD_A_E,
            ADD_A_H,
            ADD_A_L,
            ADD_A_aHL,
            ADD_A_A,
            ADC_A_B,
            ADC_A_C,
            ADC_A_D,
            ADC_A_E,
            ADC_A_H,
            ADC_A_L,
            ADC_A_aHL,
            ADC_A_A,
            SUB_B,
            SUB_C,
            SUB_D,
            SUB_E,
            SUB_H,
            SUB_L,
            SUB_aHL,
            SUB_A,
            SBC_A_B,
            SBC_A_C,
            SBC_A_D,
            SBC_A_E,
            SBC_A_H,
            SBC_A_L,
            SBC_A_aHL,
            SBC_A_A,
            AND_B,
            AND_C,
            AND_D,
            AND_E,
            AND_H,
            AND_L,
            AND_aHL,
            AND_A,
            XOR_B,
            XOR_C,
            XOR_D,
            XOR_E,
            XOR_H,
            XOR_L,
            XOR_aHL,
            XOR_A,
            OR_B,
            OR_C,
            OR_D,
            OR_E,
            OR_H,
            OR_L,
            OR_aHL,
            OR_A,
            CP_B,
            CP_C,
            CP_D,
            CP_E,
            CP_H,
            CP_L,
            CP_aHL,
            CP_A,
            RET_NZ,
            POP_BC,
            JP_NZ_nn,
            JP_nn,
            CALL_NZ_nn,
            PUSH_BC,
            ADD_A_n,
            RST_00,
            RET_Z,
            RET,
            JP_Z_nn,
            null,
            CALL_Z_nn,
            CALL_nn,
            ADC_A_n,
            RST_08,
            RET_NC,
            POP_DE,
            JP_NC_nn,
            OUT_n_A,
            CALL_NC_nn,
            PUSH_DE,
            SUB_n,
            RST_10,
            RET_C,
            EXX,
            JP_C_nn,
            IN_A_n,
            CALL_C_nn,
            null,
            SBC_A_n,
            RST_18,
            RET_PO,
            POP_HL,
            JP_PO_nn,
            EX_aSP_HL,
            CALL_PO_nn,
            PUSH_HL,
            AND_n,
            RST_20,
            RET_PE,
            JP_aHL,
            JP_PE_nn,
            EX_DE_HL,
            CALL_PE_nn,
            null,
            XOR_n,
            RST_28,
            RET_P,
            POP_AF,
            JP_P_nn,
            DI,
            CALL_P_nn,
            PUSH_AF,
            OR_n,
            RST_30,
            RET_M,
            LD_SP_HL,
            JP_M_nn,
            EI,
            CALL_M_nn,
            null,
            CP_n,
            RST_38
        };
    }
}

public class IntBuffer
{
    private int previousValue;
    private readonly Queue<int> buffer;

    public int Value
    {
        get
        {
            if (buffer.Count > 0)
                return buffer.Dequeue();
            return previousValue;
        }
        set
        {
            buffer.Enqueue(value);
            previousValue = value;
        }
    }

    public int LastValue => previousValue;

    public IntBuffer(int initialValue = 0)
    {
        previousValue = initialValue;
        buffer = new Queue<int>();
    }
}

public interface IZ80InstructionExecutor
{
IZ80ProcessorAgent ProcessorAgent { get; set; }

int Execute(byte firstOpcodeByte);

}

public interface IZ80InterruptSource
{

bool IntLineIsActive { get; }

byte? ValueOnDataBus { get; }
}

public interface IZ80Processor
{

void Start(object userState = null);

void Continue();

void Reset();

int ExecuteNextInstruction();



ulong TStatesElapsedSinceStart { get; }

ulong TStatesElapsedSinceReset { get; }

StopReason StopReason { get; }

ProcessorState State { get; }

object UserState { get; set; }

bool IsHalted { get; }

byte InterruptMode { get; set; }

short StartOfStack { get; }



IZ80Registers Registers { get; set; }

IMemory Memory { get; set; }

void SetMemoryAccessMode(ushort startAddress, int length, MemoryAccessMode mode);

MemoryAccessMode GetMemoryAccessMode(ushort address);

IMemory PortsSpace { get; set; }

void SetPortsSpaceAccessMode(byte startPort, int length, MemoryAccessMode mode);

MemoryAccessMode GetPortAccessMode(ushort portNumber);

void RegisterInterruptSource(IZ80InterruptSource source);

IEnumerable<IZ80InterruptSource> GetRegisteredInterruptSources();

void UnregisterAllInterruptSources();



decimal ClockFrequencyInMHz { get; set; }

decimal ClockSpeedFactor { get; set; }

bool AutoStopOnDiPlusHalt { get; set; }

bool AutoStopOnRetWithStackEmpty { get; set; }

void SetMemoryWaitStatesForM1(ushort startAddress, int length, byte waitStates);

byte GetMemoryWaitStatesForM1(ushort address);

void SetMemoryWaitStatesForNonM1(ushort startAddress, int length, byte waitStates);

byte GetMemoryWaitStatesForNonM1(ushort address);

void SetPortWaitStates(ushort startPort, int length, byte waitStates);

byte GetPortWaitStates(ushort portNumber);

IZ80InstructionExecutor InstructionExecutor { get; set; }

IClockSynchronizer ClockSynchronizer { get; set; }









void ExecuteCall(ushort address);

void ExecuteRet();

}

public interface IZ80ProcessorAgent : IExecutionStopper
{
byte FetchNextOpcode();

byte PeekNextOpcode();

byte ReadFromMemory(ushort address);

void WriteToMemory(ushort address, byte value);

byte ReadFromPort(ushort portNumber);

void WriteToPort(ushort portNumber, byte value);

IZ80Registers RG { get; }

void SetInterruptMode(byte interruptMode);
}

public interface IZ80Registers : IMainZ80Registers
{
IMainZ80Registers Alternate { get; set; }

short IX { get; set; }

short IY { get; set; }

ushort PC { get; set; }

short SP { get; set; }

short IR { get; set; }

Bit IFF1 { get; set; }

Bit IFF2 { get; set; }

byte IXH { get; set; }

byte IXL { get; set; }

byte IYH { get; set; }

byte IYL { get; set; }

byte I { get; set; }

byte R { get; set; }
}

public class JCTRL
{
    MyGridProgram mypgm = null;
    JDBG jdbg = null;
    bool singleKey = false;

    public JCTRL(MyGridProgram pgm, JDBG dbg, bool SingleKeyOnly)
    {
        mypgm = pgm;
        jdbg = dbg;
    }

    public List<IMyTerminalBlock> GetCTRLsWithTag(String tag)
    {
        List<IMyTerminalBlock> allCTRLs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allCTRLs, (IMyTerminalBlock x) => (
                                                                               (x.CustomName != null) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[" + tag.ToUpper() + "]") >= 0) &&
                                                                               (x is IMyShipController)
                                                                              ));
        jdbg.Debug("Found " + allCTRLs.Count + " controllers with tag " + tag);
        return allCTRLs;
    }

    public bool IsOccupied(IMyShipController seat)
    {
        return seat.IsUnderControl;
    }

    public bool AnyKey(IMyShipController seat, bool allowJumpOrCrouch)
    {
        bool pressed = false;
        Vector3 dirn = seat.MoveIndicator;
        if (dirn.X != 0 || (allowJumpOrCrouch && dirn.Y != 0) || dirn.Z != 0) {
            pressed = true;
        }
        return pressed;
    }

    public bool IsLeft(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X < 0 && dirn.Y == 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.X < 0) return true;
        return false;
    }
    public bool IsRight(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X > 0 && dirn.Y == 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.X > 0) return true;
        return false;
    }
    public bool IsUp(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y == 0 && dirn.Z < 0) return true;
        else if (!singleKey && dirn.Z < 0) return true;
        return false;
    }
    public bool IsDown(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y == 0 && dirn.Z > 0) return true;
        else if (!singleKey && dirn.Z > 0) return true;
        return false;
    }
    public bool IsSpace(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y > 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.Y > 0) return true;
        return false;
    }
    public bool IsCrouch(IMyShipController seat)
    {
        Vector3 dirn = seat.MoveIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y < 0 && dirn.Z == 0) return true;
        else if (!singleKey && dirn.Y < 0) return true;
        return false;
    }
    public bool IsRollLeft(IMyShipController seat)
    {
        float dirn = seat.RollIndicator;
        if (dirn < 0.0) return true;
        return false;
    }
    public bool IsRollRight(IMyShipController seat)
    {
        float dirn = seat.RollIndicator;
        if (dirn > 0.0) return true;
        return false;
    }
    public bool IsArrowLeft(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y < 0) return true;
        else if (!singleKey && dirn.Y < 0) return true;
        return false;
    }
    public bool IsArrowRight(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X == 0 && dirn.Y > 0) return true;
        else if (!singleKey && dirn.Y > 0) return true;
        return false;
    }
    public bool IsArrowDown(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X > 0 && dirn.Y == 0) return true;
        else if (!singleKey && dirn.X > 0) return true;
        return false;
    }
    public bool IsArrowUp(IMyShipController seat)
    {
        Vector2 dirn = seat.RotationIndicator;
        if (singleKey && dirn.X < 0 && dirn.Y == 0) return true;
        else if (!singleKey && dirn.X < 0) return true;
        return false;
    }
}

public class JDBG
{
    public bool debug = false;

    private MyGridProgram mypgm = null;
    private JLCD jlcd = null;

    private bool inDebug = false;
    private static List<IMyTerminalBlock> debugLCDs = null;

    public JDBG(MyGridProgram pgm, bool debugState)
    {
        mypgm = pgm;
        debug = debugState;
        jlcd = new JLCD(pgm, this, false);
    }

    public void Echo(string str)
    {
        mypgm.Echo("JDBG: " + str);
    }

    public void Debug(String str)
    {
        Debug(str, true);
    }
    public void Debug(String str, bool consoledbg)
    {
        if (debug && !inDebug)
        {
            inDebug = true;

            if (debugLCDs == null)
            {
                Echo("First run - working out debug panels");
                initializeDBGLCDs();
                ClearDebugLCDs();
            }

            Echo("D:" + str);
            jlcd.WriteToAllLCDs(debugLCDs, str + "\n", true);
            inDebug = false;
        }
    }

    public void DebugAndEcho(String str)
    {
        Echo(str);
        Debug(str, false);
    }

    private void initializeDBGLCDs()
    {
        inDebug = true;
        debugLCDs = jlcd.GetLCDsWithTag("DEBUG");
        jlcd.InitializeLCDs(debugLCDs, TextAlignment.LEFT);
        inDebug = false;
    }

    public void ClearDebugLCDs()
    {
        if (debug) {
            if (debugLCDs == null) {
                Echo("First runC - working out debug panels");
                initializeDBGLCDs();
            }
            jlcd.WriteToAllLCDs(debugLCDs, "", false);
        }
    }

    public void Alert(String alertMsg, String colour, String alertTag, String thisScript)
    {
        List<IMyTerminalBlock> allBlocksWithLCDs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allBlocksWithLCDs, (IMyTerminalBlock x) => (
                                                                                  (x.CustomName != null) &&
                                                                                  (x.CustomName.IndexOf("[" + alertTag + "]") >= 0) &&
                                                                                  (x is IMyTextSurfaceProvider)
                                                                                 ));
        DebugAndEcho("Found " + allBlocksWithLCDs.Count + " lcds with '" + alertTag + "' to alert to");

        String alertOutput = JLCD.solidcolor[colour] + " " +
                             DateTime.Now.ToShortTimeString() + ":" +
                             thisScript + " " +
                             alertMsg + "\n";
        DebugAndEcho("ALERT: " + alertMsg);
        if (allBlocksWithLCDs.Count > 0)
        {
            jlcd.WriteToAllLCDs(allBlocksWithLCDs, alertOutput, true);
        }
    }

    public void EchoCurrentInstructionCount(String tag)
    {
        Echo(tag + " instruction count: " + mypgm.Runtime.CurrentInstructionCount + "," + mypgm.Runtime.CurrentCallChainDepth);
    }
    public void EchoMaxInstructionCount()
    {
        Echo("Max instruction count: " + mypgm.Runtime.MaxInstructionCount + "," + mypgm.Runtime.MaxCallChainDepth);
    }
}

public class JINV
{
    private JDBG jdbg = null;

    public enum BLUEPRINT_TYPES { BOTTLES, COMPONENTS, AMMO, TOOLS, OTHER, ORES };

    Dictionary<String, String> oreToIngot = new Dictionary<String, String>
    {
        { "MyObjectBuilder_Ore/Cobalt", "MyObjectBuilder_Ingot/Cobalt" },
        { "MyObjectBuilder_Ore/Gold", "MyObjectBuilder_Ingot/Gold" },
        { "MyObjectBuilder_Ore/Stone", "MyObjectBuilder_Ingot/Stone" },
        { "MyObjectBuilder_Ore/Iron", "MyObjectBuilder_Ingot/Iron" },
        { "MyObjectBuilder_Ore/Magnesium", "MyObjectBuilder_Ingot/Magnesium" },
        { "MyObjectBuilder_Ore/Nickel", "MyObjectBuilder_Ingot/Nickel" },
        { "MyObjectBuilder_Ore/Platinum", "MyObjectBuilder_Ingot/Platinum" },
        { "MyObjectBuilder_Ore/Silicon", "MyObjectBuilder_Ingot/Silicon" },
        { "MyObjectBuilder_Ore/Silver", "MyObjectBuilder_Ingot/Silver" },
        { "MyObjectBuilder_Ore/Uranium", "MyObjectBuilder_Ingot/Uranium" },

    };

    Dictionary<String, String> otherCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_BlueprintDefinition/Position0040_Datapad", "MyObjectBuilder_Datapad/Datapad" },
    };

    Dictionary<String, String> toolsCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinderItem" , "MyObjectBuilder_BlueprintDefinition/Position0010_AngleGrinder" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder2Item", "MyObjectBuilder_BlueprintDefinition/Position0020_AngleGrinder2" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder3Item" , "MyObjectBuilder_BlueprintDefinition/Position0030_AngleGrinder3" },
        { "MyObjectBuilder_PhysicalGunObject/AngleGrinder4Item" , "MyObjectBuilder_BlueprintDefinition/Position0040_AngleGrinder4" },

        { "MyObjectBuilder_PhysicalGunObject/WelderItem" , "MyObjectBuilder_BlueprintDefinition/Position0090_Welder" },
        { "MyObjectBuilder_PhysicalGunObject/Welder2Item" , "MyObjectBuilder_BlueprintDefinition/Position0100_Welder2" },
        { "MyObjectBuilder_PhysicalGunObject/Welder3Item" , "MyObjectBuilder_BlueprintDefinition/Position0110_Welder3" },
        { "MyObjectBuilder_PhysicalGunObject/Welder4Item" , "MyObjectBuilder_BlueprintDefinition/Position0120_Welder4" },

        { "MyObjectBuilder_PhysicalGunObject/HandDrillItem", "MyObjectBuilder_BlueprintDefinition/Position0050_HandDrill" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill2Item", "MyObjectBuilder_BlueprintDefinition/Position0060_HandDrill2" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill3Item" , "MyObjectBuilder_BlueprintDefinition/Position0070_HandDrill3" },
        { "MyObjectBuilder_PhysicalGunObject/HandDrill4Item" , "MyObjectBuilder_BlueprintDefinition/Position0080_HandDrill4" },

    };

    Dictionary<String, String> bottlesCompToBlueprint = new Dictionary<String, String>
    {
        { "MyObjectBuilder_GasContainerObject/HydrogenBottle", "MyObjectBuilder_BlueprintDefinition/Position0020_HydrogenBottle" },
        { "MyObjectBuilder_OxygenContainerObject/OxygenBottle", "MyObjectBuilder_BlueprintDefinition/HydrogenBottlesRefill" },
    };

    Dictionary<String, String> componentsCompToBlueprint = new Dictionary<String, String>
    {
        { "myobjectbuilder_component/bulletproofglass", "myobjectbuilder_blueprintdefinition/bulletproofglass"},
        { "myobjectbuilder_component/canvas", "myobjectbuilder_blueprintdefinition/position0030_canvas"},
        { "myobjectbuilder_component/computer", "myobjectbuilder_blueprintdefinition/computercomponent"},
        { "myobjectbuilder_component/construction", "myobjectbuilder_blueprintdefinition/constructioncomponent"},
        { "myobjectbuilder_component/detector", "myobjectbuilder_blueprintdefinition/detectorcomponent"},
        { "myobjectbuilder_component/display", "myobjectbuilder_blueprintdefinition/display"},
        { "myobjectbuilder_component/explosives", "myobjectbuilder_blueprintdefinition/explosivescomponent"},
        { "myobjectbuilder_component/girder", "myobjectbuilder_blueprintdefinition/girdercomponent"},
        { "myobjectbuilder_component/gravitygenerator", "myobjectbuilder_blueprintdefinition/gravitygeneratorcomponent"},
        { "myobjectbuilder_component/interiorplate", "myobjectbuilder_blueprintdefinition/interiorplate"},
        { "myobjectbuilder_component/largetube", "myobjectbuilder_blueprintdefinition/largetube"},
        { "myobjectbuilder_component/medical", "myobjectbuilder_blueprintdefinition/medicalcomponent"},
        { "myobjectbuilder_component/metalgrid", "myobjectbuilder_blueprintdefinition/metalgrid"},
        { "myobjectbuilder_component/motor", "myobjectbuilder_blueprintdefinition/motorcomponent"},
        { "myobjectbuilder_component/powercell", "myobjectbuilder_blueprintdefinition/powercell"},
        { "myobjectbuilder_component/reactor", "myobjectbuilder_blueprintdefinition/reactorcomponent"},
        { "myobjectbuilder_component/radiocommunication", "myobjectbuilder_blueprintdefinition/radiocommunicationcomponent"},
        { "myobjectbuilder_component/smalltube", "myobjectbuilder_blueprintdefinition/smalltube"},
        { "myobjectbuilder_component/solarcell", "myobjectbuilder_blueprintdefinition/solarcell"},
        { "myobjectbuilder_component/steelplate", "myobjectbuilder_blueprintdefinition/steelplate"},
        { "myobjectbuilder_component/superconductor", "myobjectbuilder_blueprintdefinition/superconductor"},
        { "myobjectbuilder_component/thrust", "myobjectbuilder_blueprintdefinition/thrustcomponent"},
    };

    Dictionary<String, String> ammoCompToBlueprint = new Dictionary<String, String>
    {
        {"MyObjectBuilder_AmmoMagazine/NATO_25x184mm", "MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine" },
        {"MyObjectBuilder_AmmoMagazine/AutocannonClip", "MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip" },
        {"MyObjectBuilder_AmmoMagazine/Missile200mm", "MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm" },
        {"MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo" },
        {"MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo", "MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo" },
        {"MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo" },
        {"MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo", "MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo" },
        {"MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine" },
        {"MyObjectBuilder_AmmoMagazine/ElitePistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine" },
        {"MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine", "MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine" },
        {"MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd", "MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd" },
        {"MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd", "MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd" },
        {"MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd", "MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd" },
        {"MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd", "MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd" },
        {"MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm", null},
    };
    public JINV(JDBG dbg)
    {
        jdbg = dbg;
    }
    public void addBluePrints(BLUEPRINT_TYPES types, ref Dictionary<String, String> into)
    {
        switch (types)
        {
            case BLUEPRINT_TYPES.BOTTLES:
                into = into.Concat(bottlesCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.COMPONENTS:
                into = into.Concat(componentsCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.AMMO:
                into = into.Concat(ammoCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.TOOLS:
                into = into.Concat(toolsCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.OTHER:
                into = into.Concat(otherCompToBlueprint).ToDictionary(x => x.Key, x => x.Value);
                break;
            case BLUEPRINT_TYPES.ORES:
                into = into.Concat(oreToIngot).ToDictionary(x => x.Key, x => x.Value);
                break;
        }
    }
}

public class JLCD
{
    public MyGridProgram mypgm = null;
    public JDBG jdbg = null;
    bool suppressDebug = false;

    public static Dictionary<String, char> solidcolor = new Dictionary<String, char>
    {
        { "YELLOW", '' },
        { "RED", '' },
        { "ORANGE", '' },
        { "GREEN", '' },
        { "CYAN", '' },
        { "PURPLE", ''},
        { "BLUE", '' },
        { "WHITE", ''},
        { "BLACK", ''},
        { "GREY", '' }
    };

    public static char COLOUR_YELLOW = '';
    public static char COLOUR_RED = '';
    public static char COLOUR_ORANGE = '';
    public static char COLOUR_GREEN = '';
    public static char COLOUR_CYAN = '';
    public static char COLOUR_PURPLE = '';
    public static char COLOUR_BLUE = '';
    public static char COLOUR_WHITE = '';
    public static char COLOUR_BLACK = '';
    public static char COLOUR_GREY = '';

    public JLCD(MyGridProgram pgm, JDBG dbg, bool suppressDebug)
    {
        this.mypgm = pgm;
        this.jdbg = dbg;
        this.suppressDebug = suppressDebug;
    }

    public List<IMyTerminalBlock> GetLCDsWithTag(String tag)
    {
        List<IMyTerminalBlock> allLCDs = new List<IMyTerminalBlock>();
        mypgm.GridTerminalSystem.GetBlocksOfType(allLCDs, (IMyTerminalBlock x) => (
                                                                               (x.CustomName != null) &&
                                                                               (x.CustomName.ToUpper().IndexOf("[" + tag.ToUpper() + "]") >= 0) &&
                                                                               (x is IMyTextSurfaceProvider)
                                                                              ));
        jdbg.Debug("Found " + allLCDs.Count + " lcds to update with tag " + tag);
        return allLCDs;
    }

    public void InitializeLCDs(List<IMyTerminalBlock> allLCDs, TextAlignment align)
    {
        foreach (var thisLCD in allLCDs)
        {
            jdbg.Debug("Setting up the font for " + thisLCD.CustomName);

            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;
            thisSurface.Font = "Monospace";
            thisSurface.ContentType = ContentType.TEXT_AND_IMAGE;
            thisSurface.BackgroundColor = Color.Black;
            thisSurface.Alignment = align;
        }
    }

    public void SetLCDFontColour(List<IMyTerminalBlock> allLCDs, Color colour)
    {
        foreach (var thisLCD in allLCDs) {
            if (thisLCD is IMyTextPanel) {
                jdbg.Debug("Setting up the color for " + thisLCD.CustomName);
                ((IMyTextPanel)thisLCD).FontColor = colour;
            }
        }
    }

    public void SetLCDRotation(List<IMyTerminalBlock> allLCDs, float Rotation)
    {
        foreach (var thisLCD in allLCDs) {
            if (thisLCD is IMyTextPanel) {
                jdbg.Debug("Setting up the rotation for " + thisLCD.CustomName);
                thisLCD.SetValueFloat("Rotate", Rotation);
            }
        }
    }

    public void SetupFont(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial)
    {
        SetupFontCalc(allLCDs, rows, cols, mostlySpecial, 0.05F, 0.05F);
    }
    public void SetupFontCustom(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial, float size, float incr)
    {
        SetupFontCalc(allLCDs, rows, cols, mostlySpecial, size,incr);
    }

    public void SetupFontCalc(List<IMyTerminalBlock> allLCDs, int rows, int cols, bool mostlySpecial, float startSize, float startIncr)
    {
        foreach (var thisLCD in allLCDs)
        {
            jdbg.Debug("Setting up font on screen: " + thisLCD.CustomName + " (" + rows + " x " + cols + ")");
            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;

            float size = startSize;
            float incr = startIncr;

            StringBuilder teststr = new StringBuilder("".PadRight(cols, (mostlySpecial? solidcolor["BLACK"] : 'W')));
            Vector2 actualScreenSize = thisSurface.TextureSize;
            while (true)
            {
                thisSurface.FontSize = size;
                Vector2 actualSize = thisSurface.TextureSize;

                Vector2 thisSize = thisSurface.MeasureStringInPixels(teststr, thisSurface.Font, size);

                int displayrows = (int)Math.Floor(actualScreenSize.Y / thisSize.Y);

                if ((thisSize.X < actualSize.X) && (displayrows > rows))
                {
                    size += incr;
                }
                else
                {
                    break;
                }
            }
            thisSurface.FontSize = size - incr;
            jdbg.Debug("Calc size of " + thisSurface.FontSize);

            if (thisLCD.DefinitionDisplayNameText.Contains("Corner LCD")) {
                jdbg.Debug("INFO: Avoiding bug, multiplying by 4: " + thisLCD.DefinitionDisplayNameText);
                thisSurface.FontSize *= 4;
            }
        }
    }

    public void WriteToAllLCDs(List<IMyTerminalBlock> allLCDs, String msg, bool append)
    {
        foreach (var thisLCD in allLCDs)
        {
            if (!this.suppressDebug) jdbg.Debug("Writing to display " + thisLCD.CustomName);
            IMyTextSurface thisSurface = ((IMyTextSurfaceProvider)thisLCD).GetSurface(0);
            if (thisSurface == null) continue;
            thisSurface.WriteText(msg, append);
        }
    }

    public char ColorToChar(byte r, byte g, byte b)
    {
        const double bitSpacing = 255.0 / 7.0;
        return (char)(0xe100 + ((int)Math.Round(r / bitSpacing) << 6) + ((int)Math.Round(g / bitSpacing) << 3) + (int)Math.Round(b / bitSpacing));
    }

}

public partial class Z80InstructionExecutor
{
byte JP_aHL()
    {
        FetchFinished();

        RG.PC = (ushort)RG.HL;

        return 4;
    }

byte JP_aIX()
    {
        FetchFinished();

        RG.PC = (ushort)RG.IX;

        return 8;
    }

byte JP_aIY()
    {
        FetchFinished();

        RG.PC = (ushort)RG.IY;

        return 8;
    }
}

public partial class Z80InstructionExecutor
{
byte JR_d()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        RG.PC = (ushort)(RG.PC + (SByte)offset);

        return 12;
    }
}

public partial class Z80InstructionExecutor
{
byte JR_C_d()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        if (RG.CF == 0)
            return 7;

        RG.PC = (ushort)(RG.PC + (SByte)offset);
        return 12;
    }

byte JR_NC_d()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        if (RG.CF == 1)
            return 7;

        RG.PC = (ushort)(RG.PC + (SByte)offset);
        return 12;
    }

byte JR_Z_d()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        if (RG.ZF == 0)
            return 7;

        RG.PC = (ushort)(RG.PC + (SByte)offset);
        return 12;
    }

byte JR_NZ_d()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        if (RG.ZF == 1)
            return 7;

        RG.PC = (ushort)(RG.PC + (SByte)offset);
        return 12;
    }
}

public partial class Z80InstructionExecutor
{
private byte LD_aa_A()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        ProcessorAgent.WriteToMemory(address, RG.A);

        return 13;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_aa_HL()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        WriteShortToMemory(address, RG.HL);

        return 16;
    }

byte LD_aa_DE()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        WriteShortToMemory(address, RG.DE);

        return 20;
    }

byte LD_aa_BC()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        WriteShortToMemory(address, RG.BC);

        return 20;
    }

byte LD_aa_SP()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        WriteShortToMemory(address, RG.SP);

        return 20;
    }

byte LD_aa_IX()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        WriteShortToMemory(address, RG.IX);

        return 20;
    }

byte LD_aa_IY()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        WriteShortToMemory(address, RG.IY);

        return 20;
    }
}

public partial class Z80InstructionExecutor
{
private byte LD_aHL_N()
    {
        var newValue = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)RG.HL;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 10;
    }

private byte LD_aIX_plus_n_N()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        var newValue = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

private byte LD_aIY_plus_n_N()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        var newValue = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }
}

public partial class Z80InstructionExecutor
{
private byte LD_A_aa()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        RG.A = ProcessorAgent.ReadFromMemory(address);

        return 13;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_A_I()
    {
        FetchFinished();

        var value = RG.I;
        RG.A = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.HF = 0;
        RG.NF = 0;
        RG.PF = RG.IFF2;
        SetFlags3and5From(value);

        return 9;
    }

byte LD_A_R()
    {
        FetchFinished();

        var value = RG.R;
        RG.A = value;

        RG.SF = GetBit(value, 7);
        RG.ZF = (value == 0);
        RG.HF = 0;
        RG.NF = 0;
        RG.PF = RG.IFF2;
        SetFlags3and5From(value);

        return 9;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_I_A()
    {
        FetchFinished();

        RG.I = RG.A;

        return 9;
    }

byte LD_R_A()
    {
        FetchFinished();

        RG.R = RG.A;

        return 9;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_A_aBC()
    {
        FetchFinished();

        var address = (ushort)RG.BC;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.A = oldValue;

        return 7;
    }

byte LD_aBC_A()
    {
        FetchFinished();

        var newValue = RG.A;
        var address = (ushort)RG.BC;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_A_aDE()
    {
        FetchFinished();

        var address = (ushort)RG.DE;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.A = oldValue;

        return 7;
    }

byte LD_aDE_A()
    {
        FetchFinished();

        var newValue = RG.A;
        var address = (ushort)RG.DE;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_A_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.A = oldValue;

        return 7;
    }

byte LD_aHL_A()
    {
        FetchFinished();

        var newValue = RG.A;
        var address = (ushort)RG.HL;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_B_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.B = oldValue;

        return 7;
    }

byte LD_aHL_B()
    {
        FetchFinished();

        var newValue = RG.B;
        var address = (ushort)RG.HL;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_C_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.C = oldValue;

        return 7;
    }

byte LD_aHL_C()
    {
        FetchFinished();

        var newValue = RG.C;
        var address = (ushort)RG.HL;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_D_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.D = oldValue;

        return 7;
    }

byte LD_aHL_D()
    {
        FetchFinished();

        var newValue = RG.D;
        var address = (ushort)RG.HL;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_E_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.E = oldValue;

        return 7;
    }

byte LD_aHL_E()
    {
        FetchFinished();

        var newValue = RG.E;
        var address = (ushort)RG.HL;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_H_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.H = oldValue;

        return 7;
    }

byte LD_aHL_H()
    {
        FetchFinished();

        var newValue = RG.H;
        var address = (ushort)RG.HL;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_L_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.L = oldValue;

        return 7;
    }

byte LD_aHL_L()
    {
        FetchFinished();

        var newValue = RG.L;
        var address = (ushort)RG.HL;
        ProcessorAgent.WriteToMemory(address, newValue);

        return 7;
    }

byte LD_A_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.A = oldValue;

        return 19;
    }

byte LD_aIX_plus_n_A()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.A;
        var address = (ushort)(RG.IX + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_B_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.B = oldValue;

        return 19;
    }

byte LD_aIX_plus_n_B()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.B;
        var address = (ushort)(RG.IX + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_C_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.C = oldValue;

        return 19;
    }

byte LD_aIX_plus_n_C()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.C;
        var address = (ushort)(RG.IX + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_D_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.D = oldValue;

        return 19;
    }

byte LD_aIX_plus_n_D()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.D;
        var address = (ushort)(RG.IX + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_E_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.E = oldValue;

        return 19;
    }

byte LD_aIX_plus_n_E()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.E;
        var address = (ushort)(RG.IX + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_H_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.H = oldValue;

        return 19;
    }

byte LD_aIX_plus_n_H()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.H;
        var address = (ushort)(RG.IX + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_L_aIX_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.L = oldValue;

        return 19;
    }

byte LD_aIX_plus_n_L()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.L;
        var address = (ushort)(RG.IX + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_A_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.A = oldValue;

        return 19;
    }

byte LD_aIY_plus_n_A()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.A;
        var address = (ushort)(RG.IY + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_B_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.B = oldValue;

        return 19;
    }

byte LD_aIY_plus_n_B()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.B;
        var address = (ushort)(RG.IY + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_C_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.C = oldValue;

        return 19;
    }

byte LD_aIY_plus_n_C()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.C;
        var address = (ushort)(RG.IY + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_D_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.D = oldValue;

        return 19;
    }

byte LD_aIY_plus_n_D()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.D;
        var address = (ushort)(RG.IY + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_E_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.E = oldValue;

        return 19;
    }

byte LD_aIY_plus_n_E()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.E;
        var address = (ushort)(RG.IY + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_H_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.H = oldValue;

        return 19;
    }

byte LD_aIY_plus_n_H()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.H;
        var address = (ushort)(RG.IY + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }

byte LD_L_aIY_plus_n()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        RG.L = oldValue;

        return 19;
    }

byte LD_aIY_plus_n_L()
    {
        var offset = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        var newValue = RG.L;
        var address = (ushort)(RG.IY + (SByte)offset);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 19;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_A_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.A = value;
        return 7;
    }

byte LD_B_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.B = value;
        return 7;
    }

byte LD_C_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.C = value;
        return 7;
    }

byte LD_D_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.D = value;
        return 7;
    }

byte LD_E_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.E = value;
        return 7;
    }

byte LD_H_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.H = value;
        return 7;
    }

byte LD_L_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.L = value;
        return 7;
    }

byte LD_IXH_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.IXH = value;
        return 11;
    }

byte LD_IXL_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.IXL = value;
        return 11;
    }

byte LD_IYH_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.IYH = value;
        return 11;
    }

byte LD_IYL_n()
    {
        var value = ProcessorAgent.FetchNextOpcode();
        FetchFinished();
        RG.IYL = value;
        return 11;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_A_A()
    {
        FetchFinished();

        return 4;
    }

byte LD_B_A()
    {
        FetchFinished();

        RG.B = RG.A;

        return 4;
    }

byte LD_C_A()
    {
        FetchFinished();

        RG.C = RG.A;

        return 4;
    }

byte LD_D_A()
    {
        FetchFinished();

        RG.D = RG.A;

        return 4;
    }

byte LD_E_A()
    {
        FetchFinished();

        RG.E = RG.A;

        return 4;
    }

byte LD_H_A()
    {
        FetchFinished();

        RG.H = RG.A;

        return 4;
    }

byte LD_L_A()
    {
        FetchFinished();

        RG.L = RG.A;

        return 4;
    }

byte LD_IXH_A()
    {
        FetchFinished();

        RG.IXH = RG.A;

        return 8;
    }

byte LD_IXL_A()
    {
        FetchFinished();

        RG.IXL = RG.A;

        return 8;
    }

byte LD_IYH_A()
    {
        FetchFinished();

        RG.IYH = RG.A;

        return 8;
    }

byte LD_IYL_A()
    {
        FetchFinished();

        RG.IYL = RG.A;

        return 8;
    }

byte LD_A_B()
    {
        FetchFinished();

        RG.A = RG.B;

        return 4;
    }

byte LD_B_B()
    {
        FetchFinished();

        return 4;
    }

byte LD_C_B()
    {
        FetchFinished();

        RG.C = RG.B;

        return 4;
    }

byte LD_D_B()
    {
        FetchFinished();

        RG.D = RG.B;

        return 4;
    }

byte LD_E_B()
    {
        FetchFinished();

        RG.E = RG.B;

        return 4;
    }

byte LD_H_B()
    {
        FetchFinished();

        RG.H = RG.B;

        return 4;
    }

byte LD_L_B()
    {
        FetchFinished();

        RG.L = RG.B;

        return 4;
    }

byte LD_IXH_B()
    {
        FetchFinished();

        RG.IXH = RG.B;

        return 8;
    }

byte LD_IXL_B()
    {
        FetchFinished();

        RG.IXL = RG.B;

        return 8;
    }

byte LD_IYH_B()
    {
        FetchFinished();

        RG.IYH = RG.B;

        return 8;
    }

byte LD_IYL_B()
    {
        FetchFinished();

        RG.IYL = RG.B;

        return 8;
    }

byte LD_A_C()
    {
        FetchFinished();

        RG.A = RG.C;

        return 4;
    }

byte LD_B_C()
    {
        FetchFinished();

        RG.B = RG.C;

        return 4;
    }

byte LD_C_C()
    {
        FetchFinished();

        return 4;
    }

byte LD_D_C()
    {
        FetchFinished();

        RG.D = RG.C;

        return 4;
    }

byte LD_E_C()
    {
        FetchFinished();

        RG.E = RG.C;

        return 4;
    }

byte LD_H_C()
    {
        FetchFinished();

        RG.H = RG.C;

        return 4;
    }

byte LD_L_C()
    {
        FetchFinished();

        RG.L = RG.C;

        return 4;
    }

byte LD_IXH_C()
    {
        FetchFinished();

        RG.IXH = RG.C;

        return 8;
    }

byte LD_IXL_C()
    {
        FetchFinished();

        RG.IXL = RG.C;

        return 8;
    }

byte LD_IYH_C()
    {
        FetchFinished();

        RG.IYH = RG.C;

        return 8;
    }

byte LD_IYL_C()
    {
        FetchFinished();

        RG.IYL = RG.C;

        return 8;
    }

byte LD_A_D()
    {
        FetchFinished();

        RG.A = RG.D;

        return 4;
    }

byte LD_B_D()
    {
        FetchFinished();

        RG.B = RG.D;

        return 4;
    }

byte LD_C_D()
    {
        FetchFinished();

        RG.C = RG.D;

        return 4;
    }

byte LD_D_D()
    {
        FetchFinished();

        return 4;
    }

byte LD_E_D()
    {
        FetchFinished();

        RG.E = RG.D;

        return 4;
    }

byte LD_H_D()
    {
        FetchFinished();

        RG.H = RG.D;

        return 4;
    }

byte LD_L_D()
    {
        FetchFinished();

        RG.L = RG.D;

        return 4;
    }

byte LD_IXH_D()
    {
        FetchFinished();

        RG.IXH = RG.D;

        return 8;
    }

byte LD_IXL_D()
    {
        FetchFinished();

        RG.IXL = RG.D;

        return 8;
    }

byte LD_IYH_D()
    {
        FetchFinished();

        RG.IYH = RG.D;

        return 8;
    }

byte LD_IYL_D()
    {
        FetchFinished();

        RG.IYL = RG.D;

        return 8;
    }

byte LD_A_E()
    {
        FetchFinished();

        RG.A = RG.E;

        return 4;
    }

byte LD_B_E()
    {
        FetchFinished();

        RG.B = RG.E;

        return 4;
    }

byte LD_C_E()
    {
        FetchFinished();

        RG.C = RG.E;

        return 4;
    }

byte LD_D_E()
    {
        FetchFinished();

        RG.D = RG.E;

        return 4;
    }

byte LD_E_E()
    {
        FetchFinished();

        return 4;
    }

byte LD_H_E()
    {
        FetchFinished();

        RG.H = RG.E;

        return 4;
    }

byte LD_L_E()
    {
        FetchFinished();

        RG.L = RG.E;

        return 4;
    }

byte LD_IXH_E()
    {
        FetchFinished();

        RG.IXH = RG.E;

        return 8;
    }

byte LD_IXL_E()
    {
        FetchFinished();

        RG.IXL = RG.E;

        return 8;
    }

byte LD_IYH_E()
    {
        FetchFinished();

        RG.IYH = RG.E;

        return 8;
    }

byte LD_IYL_E()
    {
        FetchFinished();

        RG.IYL = RG.E;

        return 8;
    }

byte LD_A_H()
    {
        FetchFinished();

        RG.A = RG.H;

        return 4;
    }

byte LD_B_H()
    {
        FetchFinished();

        RG.B = RG.H;

        return 4;
    }

byte LD_C_H()
    {
        FetchFinished();

        RG.C = RG.H;

        return 4;
    }

byte LD_D_H()
    {
        FetchFinished();

        RG.D = RG.H;

        return 4;
    }

byte LD_E_H()
    {
        FetchFinished();

        RG.E = RG.H;

        return 4;
    }

byte LD_H_H()
    {
        FetchFinished();

        return 4;
    }

byte LD_L_H()
    {
        FetchFinished();

        RG.L = RG.H;

        return 4;
    }

byte LD_IXH_H()
    {
        FetchFinished();

        RG.IXH = RG.H;

        return 8;
    }

byte LD_IXL_H()
    {
        FetchFinished();

        RG.IXL = RG.H;

        return 8;
    }

byte LD_IYL_H()
    {
        FetchFinished();

        RG.IYL = RG.H;

        return 8;
    }

byte LD_A_L()
    {
        FetchFinished();

        RG.A = RG.L;

        return 4;
    }

byte LD_B_L()
    {
        FetchFinished();

        RG.B = RG.L;

        return 4;
    }

byte LD_C_L()
    {
        FetchFinished();

        RG.C = RG.L;

        return 4;
    }

byte LD_D_L()
    {
        FetchFinished();

        RG.D = RG.L;

        return 4;
    }

byte LD_E_L()
    {
        FetchFinished();

        RG.E = RG.L;

        return 4;
    }

byte LD_H_L()
    {
        FetchFinished();

        RG.H = RG.L;

        return 4;
    }

byte LD_L_L()
    {
        FetchFinished();

        return 4;
    }

byte LD_A_IXH()
    {
        FetchFinished();

        RG.A = RG.IXH;

        return 8;
    }

byte LD_B_IXH()
    {
        FetchFinished();

        RG.B = RG.IXH;

        return 8;
    }

byte LD_C_IXH()
    {
        FetchFinished();

        RG.C = RG.IXH;

        return 8;
    }

byte LD_D_IXH()
    {
        FetchFinished();

        RG.D = RG.IXH;

        return 8;
    }

byte LD_E_IXH()
    {
        FetchFinished();

        RG.E = RG.IXH;

        return 8;
    }

byte LD_IXH_IXH()
    {
        FetchFinished();

        return 8;
    }

byte LD_A_IXL()
    {
        FetchFinished();

        RG.A = RG.IXL;

        return 8;
    }

byte LD_B_IXL()
    {
        FetchFinished();

        RG.B = RG.IXL;

        return 8;
    }

byte LD_C_IXL()
    {
        FetchFinished();

        RG.C = RG.IXL;

        return 8;
    }

byte LD_D_IXL()
    {
        FetchFinished();

        RG.D = RG.IXL;

        return 8;
    }

byte LD_E_IXL()
    {
        FetchFinished();

        RG.E = RG.IXL;

        return 8;
    }

byte LD_IXH_IXL()
    {
        FetchFinished();

        RG.IXH = RG.IXL;

        return 8;
    }

byte LD_IXL_IXL()
    {
        FetchFinished();

        return 8;
    }

byte LD_A_IYH()
    {
        FetchFinished();

        RG.A = RG.IYH;

        return 8;
    }

byte LD_B_IYH()
    {
        FetchFinished();

        RG.B = RG.IYH;

        return 8;
    }

byte LD_C_IYH()
    {
        FetchFinished();

        RG.C = RG.IYH;

        return 8;
    }

byte LD_D_IYH()
    {
        FetchFinished();

        RG.D = RG.IYH;

        return 8;
    }

byte LD_E_IYH()
    {
        FetchFinished();

        RG.E = RG.IYH;

        return 8;
    }

byte LD_IYH_IYH()
    {
        FetchFinished();

        return 8;
    }

byte LD_A_IYL()
    {
        FetchFinished();

        RG.A = RG.IYL;

        return 8;
    }

byte LD_B_IYL()
    {
        FetchFinished();

        RG.B = RG.IYL;

        return 8;
    }

byte LD_C_IYL()
    {
        FetchFinished();

        RG.C = RG.IYL;

        return 8;
    }

byte LD_D_IYL()
    {
        FetchFinished();

        RG.D = RG.IYL;

        return 8;
    }

byte LD_E_IYL()
    {
        FetchFinished();

        RG.E = RG.IYL;

        return 8;
    }

byte LD_IYH_IYL()
    {
        FetchFinished();

        RG.IYH = RG.IYL;

        return 8;
    }

byte LD_IYL_IYL()
    {
        FetchFinished();

        return 8;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_HL_aa()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        RG.HL = ReadShortFromMemory(address);

        return 16;
    }

byte LD_DE_aa()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        RG.DE = ReadShortFromMemory(address);

        return 20;
    }

byte LD_BC_aa()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        RG.BC = ReadShortFromMemory(address);

        return 20;
    }

byte LD_SP_aa()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        RG.SP = ReadShortFromMemory(address);

        return 20;
    }

byte LD_IX_aa()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        RG.IX = ReadShortFromMemory(address);

        return 20;
    }

byte LD_IY_aa()
    {
        var address = (ushort)FetchWord();
        FetchFinished();

        RG.IY = ReadShortFromMemory(address);

        return 20;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_BC_nn()
    {
        var value = FetchWord();
        FetchFinished();
        RG.BC = value;
        return 10;
    }

byte LD_DE_nn()
    {
        var value = FetchWord();
        FetchFinished();
        RG.DE = value;
        return 10;
    }

byte LD_HL_nn()
    {
        var value = FetchWord();
        FetchFinished();
        RG.HL = value;
        return 10;
    }

byte LD_SP_nn()
    {
        var value = FetchWord();
        FetchFinished(isLdSp: true);
        RG.SP = value;
        return 10;
    }

byte LD_IX_nn()
    {
        var value = FetchWord();
        FetchFinished();
        RG.IX = value;
        return 14;
    }

byte LD_IY_nn()
    {
        var value = FetchWord();
        FetchFinished();
        RG.IY = value;
        return 14;
    }
}

public partial class Z80InstructionExecutor
{
byte LD_SP_HL()
    {
        FetchFinished(isLdSp: true);

        RG.SP = RG.HL;

        return 6;
    }

byte LD_SP_IX()
    {
        FetchFinished(isLdSp: true);

        RG.SP = RG.IX;

        return 10;
    }

byte LD_SP_IY()
    {
        FetchFinished(isLdSp: true);

        RG.SP = RG.IY;

        return 10;
    }
}

public partial class Z80InstructionExecutor
{
byte LDI()
    {
        FetchFinished();

        var sourceAddress = RG.HL;
        var destAddress = RG.DE;
        var counter = RG.BC;
        var value = ProcessorAgent.ReadFromMemory((ushort)sourceAddress);
        ProcessorAgent.WriteToMemory((ushort)destAddress, value);

        RG.HL = (short)(sourceAddress + 1);
        RG.DE = (short)(destAddress + 1);
        counter--;
        RG.BC = counter;

        RG.HF = 0;
        RG.NF = 0;
        RG.PF = (counter != 0);

        var valuePlusA = (byte)(value + RG.A);
        RG.Flag3 = GetBit(valuePlusA, 3);
        RG.Flag5 = GetBit(valuePlusA, 1);


        return 16;
    }

byte LDD()
    {
        FetchFinished();

        var sourceAddress = RG.HL;
        var destAddress = RG.DE;
        var counter = RG.BC;
        var value = ProcessorAgent.ReadFromMemory((ushort)sourceAddress);
        ProcessorAgent.WriteToMemory((ushort)destAddress, value);

        RG.HL = (short)(sourceAddress - 1);
        RG.DE = (short)(destAddress - 1);
        counter--;
        RG.BC = counter;

        RG.HF = 0;
        RG.NF = 0;
        RG.PF = (counter != 0);

        var valuePlusA = (byte)(value + RG.A);
        RG.Flag3 = GetBit(valuePlusA, 3);
        RG.Flag5 = GetBit(valuePlusA, 1);


        return 16;
    }

byte LDIR()
    {
        FetchFinished();

        var sourceAddress = RG.HL;
        var destAddress = RG.DE;
        var counter = RG.BC;
        var value = ProcessorAgent.ReadFromMemory((ushort)sourceAddress);
        ProcessorAgent.WriteToMemory((ushort)destAddress, value);

        RG.HL = (short)(sourceAddress + 1);
        RG.DE = (short)(destAddress + 1);
        counter--;
        RG.BC = counter;

        RG.HF = 0;
        RG.NF = 0;
        RG.PF = (counter != 0);

        var valuePlusA = (byte)(value + RG.A);
        RG.Flag3 = GetBit(valuePlusA, 3);
        RG.Flag5 = GetBit(valuePlusA, 1);

        if (counter != 0)
        {
            RG.PC = (ushort)(RG.PC - 2);
            return 21;
        }

        return 16;
    }

byte LDDR()
    {
        FetchFinished();

        var sourceAddress = RG.HL;
        var destAddress = RG.DE;
        var counter = RG.BC;
        var value = ProcessorAgent.ReadFromMemory((ushort)sourceAddress);
        ProcessorAgent.WriteToMemory((ushort)destAddress, value);

        RG.HL = (short)(sourceAddress - 1);
        RG.DE = (short)(destAddress - 1);
        counter--;
        RG.BC = counter;

        RG.HF = 0;
        RG.NF = 0;
        RG.PF = (counter != 0);

        var valuePlusA = (byte)(value + RG.A);
        RG.Flag3 = GetBit(valuePlusA, 3);
        RG.Flag5 = GetBit(valuePlusA, 1);

        if (counter != 0)
        {
            RG.PC = (ushort)(RG.PC - 2);
            return 21;
        }

        return 16;
    }
}

class ResourceLoader
{
    private static string rom48 =
        "868R///DyxEqXVwiX1wYQ8PyFf//////Kl1cfs19ANDNdAAY9////8NbM///////xSphXOXDnhb15Sp4XCMieFx8tSAD/TRAxdXNvwLRweHx+8nhbv11AO17PVzDxRb/////////9eUqsFx8tSAB6eHx7UUqXVwjIl1cfsn+IdD+Dcj+ENj+GD/YI/4WOAEjNyJdXMm/Uk7ESU5LRVmkUMlGzlBPSU7UU0NSRUVOpEFUVNJB1FRBwlZBTKRDT0TFVkHMTEXOU0nOQ0/TVEHOQVPOQUPTQVTOTM5FWNBJTtRTUdJTR85BQtNQRUXLSc5VU9JTVFKkQ0hSpE5P1EJJzk/SQU7EPL0+vTy+TElOxVRIRc5Uz1NURdBERUYgRs5DQdRGT1JNQdRNT1bFRVJBU8VPUEVOIKNDTE9TRSCjTUVSR8VWRVJJRtlCRUXQQ0lSQ0zFSU7LUEFQRdJGTEFTyEJSSUdI1ElOVkVSU8VPVkXST1XUTFBSSU7UTExJU9RTVE/QUkVBxERBVMFSRVNUT1LFTkXXQk9SREXSQ09OVElOVcVESc1SRc1GT9JHTyBUz0dPIFNVwklOUFXUTE9BxExJU9RMRdRQQVVTxU5FWNRQT0vFUFJJTtRQTE/UUlXOU0FWxVJBTkRPTUlaxUnGQ0zTRFJB10NMRUHSUkVUVVLOQ09Q2UJIWTY1VEdWTkpVNzRSRkNNS0k4M0VEWA5MTzkyV1NaIA1QMDFRQePE4OS0vL27r7CxwKemvq2yuuWlwuGzucG4ftzaXLd7fdi/rqqr3d7ff7XWfNVd27bZW9cMBwYEBQgKCwkP4io/zcjMy16sLSs9Liw7Isc8wz7FL8lgxjrQzqjK09TR0qnPLi8R//8B/v7teC/mHygOZ30UwNYIyzww+lNfIPQtywA45no8yP4oyP4ZyHtaV/4Yyc2OAsAhAFzLfiAHIzUrIAI2/30hBFy9IO7NHgPQIQBcvigu6yEEXL4oJ8t+IATry37IX3cjNgUjOglcdyP9Tgf9VgHlzTMD4XcyCFz9ywHuySM2BSM1wDoKXHcjfhjqQhYAe/4n0P4YIAPLeMAhBQIZfjfJe/46OC8N+k8DKAPGT8kh6wEEKAMhBQIWABl+ySEpAstAKPTLWigK/cswXsAEwMYgycalyf4w2A36nQMgGSFUAstoKNP+ODAH1iAEyMYIydY2BMjG/skhMAL+OSi6/jAotuYHxoAEyO4PyQTIy2ghMAIgpNYQ/iIoBv4gwD5fyT5AyfN9yz3LPS/mA08GAN0h0QPdCTpIXOY4Dw8P9ggAAAAEDA0g/Q4/BcLWA+4Q0/5ET8tnIAl6sygJeU0b3elNDN3p+8nvMSfAAzTsbJgf9QShDzghklx+pyBeI04jRngXn7kgVCO+IFB4xjzyJQTibAQG+gTWDDD7xgzFIW4EzQY0zbQz7wQ48YZ378ACMTjNlB7+CzAi7+AE4DSAQ1WfgAEFNDVxAzjNmR7FzZke4VBZerPIG8O1A88KiQLQEoaJCpdgdYkS1RcfiRuQQQKJJNBTyokunTaxiTj/ST6JQ/9qc4lPpwBUiVwAAACJaRT2JIl28RAFzfskOjtch/qKHOHQ5c3xK2JrDfgJy/7JIT8F5SGAH8t/KAMhmAwIE90r8z4CRxD+0/7uDwakLSD1BSXy2AQGLxD+0/4+DQY3EP7T/gEOOwhvwwcFerMoDN1uAHytZz4BN8MlBWwY9HnLeBD+MAQGQhD+0/4GPiDvBa88yxXCFAUb3SMGMT5/2/4f0Ho8wv4EBjsQ/sn1Okhc5jgPDw/T/j5/2/4f+zgCzwzxyRQIFfM+D9P+IT8F5dv+H+Yg9gJPv8DN5wUw+iEVBBD+K3y1IPnN4wUw6waczeMFMOQ+xrgw4CQg8QbJzecFMNV4/tQw9M3nBdB57gNPJgAGsBgfCCAHMA/ddQAYD8sRrcB5H08TGAfdfgCtwN0jGwgGsi4BzeMF0D7LuMsVBrDSygV8rWd6syDKfP4Byc3nBdA+Fj0g/acEyD5/2/4f0KnmICjzeS9P5gf2CNP+N8nxOnRc1uAydFzNjBzNMCUoPAERADp0XKcoAg4i99Xd4QYLPiASExD83TYB/83xKyH2/wsJAzAPOnRcpyACzw54sSgKAQoA3eXhI+vtsN/+5CBJOnRc/gPKihznzbIoy/kwCyEAADp0XD0oFc8BwooczTAlKBgjft13CyN+3XcMI91xDj4By3EoATzddwDr5/4pINrnze4b68NaB/6qIB86dFz+A8qKHOfN7hvdNgsA3TYMGyEAQN11Dd10DhhN/q8gTzp0XP4Dyooc581IICAMOnRcp8qKHM3mHBgPzYIc3/4sKAw6dFynyooczeYcGATnzYIcze4bzZke3XEL3XAMzZke3XEN3XAOYGndNgADGET+yigJze4b3TYOgBgXOnRcp8KKHOfNghzN7hvNmR7dcQ3dcA7dNgAAKllc7VtTXDftUt11C910DCpLXO1S3XUP3XQQ6zp0XKfKcAnlAREA3Qnd5RERAK83zVYF3eEw8j7+zQEW/TZSAw6A3X4A3b7vIAIO9v4EMNkRwAnFzQoMwd3l0SHw/xkGCn48IAN5gE8TGr4jIAEM1xD2y3kgsz4N1+HdfgD+AygMOnRcPcoICP4CyrYI5d1u+t1m+91eC91WDHy1KA3tUjgmKAfdfgD+AyAd4Xy1IAbdbg3dZg7l3eE6dFz+AjcgAac+/81WBdjPGt1eC91WDOV8tSAGExMT6xgM3W763Wb76zftUjgJEQUAGURNzQUf4d1+AKcoPny1KBMrRitOKwMDA90iX1zN6BndKl9cKllcK91OC91GDMUDAwPdfv31zVUWI/F30SNzI3Ij5d3hNz7/wwII6ypZXCvdIl9c3U4L3UYMxc3lGcHlxc1VFt0qX1wj3U4P3UYQCSJLXN1mDnzmwCAK3W4NIkJc/TYKANHd4Tc+/8MCCN1OC91GDMUD9zaA69Hl5d3hNz7/zQII4e1bU1x+5sAgGRoTviMgAhq+GyswCOXrzbgZ4RjszSwJGOJ+T/6AyOUqS1x+/oAoJbkoCMXNuBnB6xjw5uD+oCAS0dXlIxMaviAGFzD34RgD4RjgPv/R6zw3zSwJGMQgEAgiX1zrzbgZzegZ6ypfXAgI1c24GSJfXCpTXOPFCDgHK81VFiMYA81VFiPB0e1TU1ztW19cxdXr7bDhwdXN6BnRyeU+/c0BFq8RoQnNCgz9ywLuzdQV3eUREQCvzcIE3eEGMnYQ/d1eC91WDD7/3eHDwgSAU3RhcnQgdGFwZSwgdGhlbiBwcmVzcyBhbnkga2V5rg1Qcm9ncmFtOqANTnVtYmVyIGFycmF5OqANQ2hhcmFjdGVyIGFycmF5OqANQnl0ZXM6oM0DC/4g0tkK/gY4af4YMGUhCwpfFgAZXhnlwwMLTlcQKVRTUjdQT19eXVxbWlRTDD4iuSAR/csBTiAJBA4CPhi4IAMFDiHD2Q06kVz1/TZXAT4gzWUL8TKRXMn9ywFOws0ODiHNVQwFw9kNzQMLeT095hAYWj4/GGwRhwoyD1wYCxFtChgDEYcKMg5cKlFccyNyyRH0Cc2ACioOXFd9/hbaESIgKURKPh+ROAzGAk/9ywFOIBY+FpDanx48RwT9ywJGwlUM/b4x2oYMw9kNfM0DC4E95h/IV/3LAcY+IM07DBUg+MnNJAv9ywFOIBr9ywJGIAjtQ4hcIoRcye1DilztQ4JcIoZcyf1xRSKAXMn9ywFOIBTtS4hcKoRc/csCRsjtS4pcKoZcyf1ORSqAXMn+gDg9/pAwJkfNOAvNAwsRklwYRyGSXM0+C8sYn+YPT8sYn+bwsQ4EdyMNIPvJ1qUwCcYVxe1Le1wYC80QDMMDC8XtSzZc6yE7XMuG/iAgAsvGJgBvKSkpCcHreT0+ISAOBU/9ywFOKAbVzc0O0Xm51cxVDNHF5TqRXAb/HzgBBB8fn08+CKf9ywFOKAX9yzDON+sIGqCuqRIIOBMUIz0g8usl/csBTszbC+HBDSPJCD4gg18IGOZ8Dw8P5gP2WGftW49cfquiq/3LV3YoCObHy1cgAu44/ctXZigI5vjLbyAC7gd3yeUmAOMYBBGVAPXNQQw4CT4g/csBRsw7DBrmf807DBoThzD10f5IKAP+gth6/gPYPiDV2dfZ0cn16zzLfiMo+z0g+Ovx/iDYGtZByf3LAU7AEdkN1Xj9ywJGwgIN/b4xOBvA/csCZigW/V4tHShaPgDNARbtez9c/csCpsnPBP01UiBFPhiQMoxcKo9c5TqRXPU+/c0BFq8R+AzNCgz9ywLuITtcy97LrtnN1BXZ/iAoRf7iKEH2IP5uKDs+/s0BFvEykVzhIo9czf4N/UYxBA4hxc2bDnwPDw/mA/ZYZxHgWhpOBiDrEnETIxD6wcmAc2Nyb2xsv88M/gI4gP2GMdYZ0O1ExUcqj1zlKpFc5c1NDXj1IWtcRng8dyGJXL44AzQGGM0ADvE9IOjh/XVX4SKPXO1LiFz9ywKGzdkN/csCxsHJryqNXP3LAkYoBGf9bg4ij1whkVwgAn4PruZVrnfJza8NITxcy67Lxs1NDf1GMc1EDiHAWjqNXAUYBw4gK3cNIPsQ9/02MQI+/c0BFipRXBH0CadzI3IjEagQPzj2ASEXGCohAAAifVz9yzCGzZQNPv7NARbNTQ0GGM1EDipRXBH0CXMjcv02UgEBIRghAFv9ywFOIBJ4/csCRigF/YYx1hjFR82bDsE+IZFfFgAZw9wKBhfNmw4OCMXleOYHeCAM6yHg+BnrASAAPe2w6yHg/xnrR+YHDw8PT3gGAO2wBgcJ5vgg2+EkwQ0gzc2IDiHg/xnr7bAGAcXNmw4OCMXleOYHDw8PT3gGAA1UXTYAE+2wEQEHGT3m+Ecg5eEkwQ0g3M2IDmJrEzqNXP3LAkYoAzpIXHcL7bDBDiHJfA8PDz32UGfrYWgpKSkpKURNyT4YkFcPDw/m4G965hj2QGfJ8wawIQBA5cXN9A7B4SR85gcgCn3GIG8/n+b4hGcQ5xgN8yEAWwYIxc30DsEQ+T4E0/v7IQBb/XVGr0d3IxD8/cswjg4hw9kNeP4Dn+YC0/tXzVQfOAo+BNP7+83fDs8M2/uH+DDrDiBeIwYIyxLLE8sa2/sfMPt60/sQ8A0g6ckqPVzlIX8Q5e1zPVzN1BX1FgD9Xv8hyADNtQPxITgP5f4YMDH+Bzgt/hA4OgECAFf+FjgMA/3LN37KHhDN1BVfzdQV1SpbXP3LB4bNVRbBI3AjcRgK/csHhipbXM1SFhIT7VNbXMlfFgAhmQ8ZXhnlKltcyQlmalC1cH7P1CpJXP3LN27ClxDNbhnNlRZ6s8qXEOUjTiNGIQoACURNzQUfzZcQKlFc4+U+/80BFuEr/TUPzVUY/TQPKllcIyMjIyJbXOHNFRbJ/cs3biAIIUlczQ8ZGG39NgAQGB3NMRAYBX7+DcgjIltcyc0xEAEBAMPoGc3UFc3UFeHh4SI9XP3LAH7A+ck3zZUR7VIZI8HYxURNYmsjGubw/hAgCSMa1hfOACABI6ftQgnrOObJ/cs3bsAqSVzNbhnrzZUWIUpczRwZzZUXPgDDARb9yzd+KKjDgQ/9yzBmKKH9NgD/FgD9Xv4hkBrNtQPDMA/lzZARK83lGSJbXP02BwDhyf3LAl7EHRGn/csBbsg6CFz9ywGu9f3LAm7Ebg3x/iAwUv4QMC3+BjAKR+YBT3gfxhIYKiAJIWpcPgiudxgO/g7Y1g0hQVy+dyACNgD9ywLev8lH5gdPPhDLWCABPP1x0xENERgGOg1cEagQKk9cIyNzI3I3yc1NDf3LAp79ywKuKopc5So9XOUhZxHl7XM9XCqCXOU3zZUR6819GOvN4Rgqilzj681NDTqLXJI4JiAGe/2WUDAePiDVzfQJ0RjpFgD9Xv4hkBrNtQP9NgD/7VuKXBgC0eHhIj1cwdXN2Q3hIoJc/TYmAMkqYVwrp+1bWVz9yzduyO1bYVzYKmNcyX7+DgEGAMzoGX4j/g0g8cnzPv/tW7Jc2e1LtFztWzhcKntc2Uc+B9P+Pj/tRwAAAAAAAGJrNgIrvCD6p+1SGSMwBjUoAzUo8yvZ7UO0XO1TOFwie1zZBCgZIrRcEa8+AagA6+246yMie1wrAUAA7UM4XCKyXCEAPCI2XCqyXDY+K/krKyI9XO1W/SE6XPshtlwiT1wRrxUBFQDr7bDrKyJXXCMiU1wiS1w2gCMiWVw2DSM2gCMiYVwiY1wiZVw+ODKNXDKPXDJIXCEjBSIJXP01xv01yiHGFREQXAEOAO2w/csBzs3fDv02MQLNaw2vETgVzQoM/csC7hgH/TYxAs2VF82wFj4AzQEWzSwPzRcb/csAfiAS/cswZihAKllczacR/TYA/xjdKllcIl1czfsZeLHCXRXf/g0owP3LMEbErw3Nbg0+Gf2WTzKMXP3LAf79NgD//TYKAc2KG3b9ywGu/cswTsTNDjo6XDz1IQAA/XQ3/XQmIgtcIQEAIhZczbAW/cs3rs1uDf3LAu7xR/4KOALGB83vFT4g13gRkRPNCgyvETYVzQoM7UtFXM0bGj461/1ODQYAzRsazZcQOjpcPCgb/gkoBP4VIAP9NA0BAwARcFwhRFzLfigBCe24/TYK//3LAZ7DrBKAT8tORVhUIHdpdGhvdXQgRk/SVmFyaWFibGUgbm90IGZvdW7kU3Vic2NyaXB0IHdyb27nT3V0IG9mIG1lbW9y+U91dCBvZiBzY3JlZe5OdW1iZXIgdG9vIGJp51JFVFVSTiB3aXRob3V0IEdPU1XCRW5kIG9mIGZpbOVTVE9QIHN0YXRlbWVu9EludmFsaWQgYXJndW1lbvRJbnRlZ2VyIG91dCBvZiByYW5n5U5vbnNlbnNlIGluIEJBU0nDQlJFQUsgLSBDT05UIHJlcGVhdPNPdXQgb2YgREFUwUludmFsaWQgZmlsZSBuYW3lTm8gcm9vbSBmb3IgbGlu5VNUT1AgaW4gSU5QVdRGT1Igd2l0aG91dCBORVjUSW52YWxpZCBJL08gZGV2aWPlSW52YWxpZCBjb2xvdfJCUkVBSyBpbnRvIHByb2dyYe1SQU1UT1Agbm8gZ29v5FN0YXRlbWVudCBsb3P0SW52YWxpZCBzdHJlYe1GTiB3aXRob3V0IERFxlBhcmFtZXRlciBlcnJv8lRhcGUgbG9hZGluZyBlcnJv8iygfyAxOTgyIFNpbmNsYWlyIFJlc2VhcmNoIEx05D4QAQAAwxMT7UNJXCpdXOshVRXlKmFcN+1S5WBpzW4ZIAbNuBnN6BnBeT2wKCjFAwMDAyvtW1Nc1c1VFuEiU1zBxRMqYVwrK+24Kklc68FwK3Ercyty8cOiEvQJqBBL9AnEFVOBD8QVUvQJxBVQgM8SAQAGAAsAAQABAAYAEAD9ywJuIAT9ywLezeYV2Cj6zwfZ5SpRXCMjGAgeMIPZ5SpRXF4jVuvNLBbh2cmHxhZvJlxeI1Z6syACzxcbKk9cGSJRXP3LMKYjIyMjTiEtFs3cFtAWAF4Z6UsGUxJQGwD9ywLG/csBrv3LMOYYBP3LAob9ywGOw00N/csBzskBAQDlzQUf4c1kFiplXOvtuMn15SFLXD4OXiNW46ftUhnjMAnV6wnrcitzI9EjPSDo69Hxp+1SRE0DGevJAADrEY8WfubAIPdWI17JKmNcK81VFiMjwe1DYVzB6yPJKllcNg0iW1wjNoAjImFcKmFcImNcKmNcImVc5SGSXCJoXOHJ7VtZXMPlGSN+p8i5IyD4N8nNHhfNARcBAAAR4qPrGTgHAdQVCU4jRutxI3DJ5SpPXAkjIyNO6yEWF83cFk4GAAnpSwVTA1AB4cnNlB7+EDgCzxfGAwchEFxPBgAJTiNGK8nvATjNHhd4sSgW6ypPXAkjIyN+6/5LKAj+UygE/lAgz81dF3MjcsnlzfEreLEgAs8OxRrm308hehfN3BYw8U4GAAnB6UsGUwhQCgAeARgGHgYYAh4QC3ixINVX4ckYkO1zP1z9NgIQza8N/csCxv1GMc1EDv3LAob9yzDGKklc7VtsXKftUhk4ItXNbhkRwALr7VLjzW4ZwcXNuBnBCTgO61YjXivtU2xcGO0ibFwqbFzNbhkoAevNMxj9ywKmyT4DGAI+Av02AgDNMCXEARbfzXAgOBTf/jsoBP4sIAbnzYIcGAjN5hwYA83eHM3uG82ZHnjmP2dpIklczW4ZHgHNVRjX/csCZij2Omtc/ZZPIO6ryOXVIWxczQ8Z0eEY4O1LSVzNgBkWPigFEQAAyxP9cy1+/kDB0MXNKBojIyP9ywGGeqcoBdf9ywHG1ev9yzCWITtcy5b9yzduKALL1ipfXKftUiAFPj/NwRjN4Rjrfs22GCP+DSgG6803GRjg0cn+DsAjIyMjIyN+ydkqj1zly7zL/SKPXCGRXFbVNgDN9Anh/XRX4SKPXNnJKltcp+1SwDpBXMsHKATGQxgWITtcy54+S8tWKAvL3jz9yzBeKAI+Q9XNwRjRyV4jVuXrI81uGc2VFuH9yzduwHIrc8l7p/gYDa8JPDj87UI9KPHD7xXNGy0wMP4hOCz9ywGW/ssoJP46IA79yzduIBb9yzBWKBQYDv4iIAr1Ompc7gQyalzx/csB1tfJ5SpTXFRdwc2AGdDFzbgZ6xj0frjAI34ruckjIyMiXVwOABXI57sgBKfJI37NthgiXVz+IiABDf46KAT+yyAEy0Eo3/4NIOMVN8nlfv5AOBfLbygUh/rHGT8BBQAwAg4SFyN+MPsYBiMjTiNGIwnRp+1SRE0Z68nN3RnFeC9HeS9PA81kFuvhGdXtsOHJKllcKyJdXOchklwiZVzNOy3Noi04BCHw2AnaihzDxRbV5a/LeCAgYGke/xgI1VYjXuXrHiABGPzNKhkBnP/NKhkO9s0qGX3N7xXh0cmxy7y/xK+0k5GSlZiYmJiYmJh/gS5sbnBIlFY/QSsXHzd3RA9ZK0MtUTptQg1JXEQVXQE9AgYAZx4GywXwHAYA7R4A7hwAIx8EPQbMBgUDHQQAqx0FzR8FiSAFAiwFshsAtxEDoR4F+RcIAIAeA08eAF8eA6weAGsNCQDcIgYAOh8F7R0FJx4DQh4JBYIjAKwOBckfBfUXCwsLCwgA+AMJBSAjBwcHBwcHCAB6HgYAlCIFYB8GLAoANhcGAOUWCgCTFwosCgCTFwoAkxcAkxf9ywG+zfsZrzJHXD0yOlwYAefNvxb9NA36ihzfBgD+DSh6/joo6yF2G+VP53nWztqKHE8hSBoJTgkYAyp0XH4jInRcAVIbxU/+IDAMIQEcBgAJTgnl3wXJ37nCihznyc1UHzgCzxT9ywp+IHEqQlzLfCgUIf7/IkVcKmFcK+1bWVwbOkRcGDPNbhk6RFwoGacgQ0d+5sB4KA/P/8HNMCXIKlVcPsCmwK/+Ac4AViNe7VNFXCNeI1brGSMiVVzrIl1cVx4A/TYK/xX9cg3KKBsUzYsZKAjPFs0wJcDBwd/+DSi6/jrKKBvDihwPHUsJZwt7jnG0gc/N3hy/wczuG+sqdFxOI0brxcnNsij9NjcAMAj9yzfOIBjPAcyWKf3LAXYgDa/NMCXE8SshcVy2d+vtQ3JcIk1cycHNVhzN7hvJOjtc9c37JPH9VgGq5kAgJMt6wv8qyc2yKPV59p88IBTxGKnnzYIc/iwgCefN+yT9ywF2wM8Lzfsk/csBdsgY9P3LAX79ywKGxE0N8Tp0XNYTzfwhze4bKo9cIo1cIZFcfgeu5qqud8nNMCUoE/3LAobNTQ0hkFx+9vh3/ctXtt/N4iEYn8MFBv4NKAT+OiCczTAlyO+gOMnPCMHNMCUoCu8COOvN6TTasxvDKRv+zSAJ582CHM3uGxgGze4b76E478ACAeABOM3/KiJoXCt+y/4BBgAJBzgGDg3NVRYj5e8CAjjh6w4K7bAqRVzrcyNy/VYNFCNyzdod0P1GOCpFXCJCXDpHXO1EVypdXB7zxe1LVVzNhh3tQ1VcwTgR5/YguCgD5xjo5z4BkjJEXMnPEX7+OigYI37mwDfARiNO7UNCXCNOI0blCURN4RYAxc2LGcHQGOD9yzdOwi4cKk1cy34oHyMiaFzv4OIPwAI4zdod2CpoXBEPABleI1YjZuvDcx7PAO/h4OI2AAIBAzcABDinyTg3yefNHxzNMCUoKd8iX1wqV1x+/iwoCR7kzYYdMALPDc13AM1WHN8iV1wqX1z9NiYAzXgA3/4sKMnN7hvJzTAlIAvN+yT+LMTuG+cY9T7kR+25EQACw4sZzZkeYGnNbhkrIldcyc2ZHnixIATtS3hc7UN2XMkqblz9VjYYDM2ZHmBpFgB8/vAwLCJCXP1yCsnNhR7tecnNhR4Cyc3VLTgVKALtRPXNmR7xyc3VLRgDzaItOAHIzwrNZx4BAADNRR4YA82ZHnixIATtS7Jcxe1bS1wqWVwrzeUZzWsNKmVcETIAGdHtUjAIKrRcp+1SMALPFesislzRwTY+K/nF7XM9XOvp0f1mDSTjM+1LRVzF5e1zPVzVzWceARQAKmVcCTgK6yFQABk4A+1y2C4Dw1UAAQAAzQUfRE3JweHRev4+KAs74+vtcz1cxcNzHtXlzwbNmR52C3ixKAx4oTwgAQP9ywFuKO79ywGuyT5/2/4f2D7+2/4fyc0wJSgFPs7DOR79ywH2zY0sMBbn/iQgBf3LAbbn/iggPOf+KSggzY0s0ooc6+f+JCAC6+frAQYAzVUWIyM2Dv4sIAPnGOD+KSAT5/49IA7nOjtc9c37JPH9rgHmQMKKHM3uG80wJeHI6T4DGAI+As0wJcQBFs1NDc3fH83uG8nfzUUgKA3NTiAo+838H81OICjz/inIzcMfPg3Xyd/+rCANzXkczcMfzQcjPhYYEP6tIBLnzYIczcMfzZkePhfXedd418nN8iHQzXAg0M37JM3DH/3LAXbM8SvC4y14sQvIGhPXGPf+Kcj+Dcj+Osnf/jsoFP4sIArNMCUoCz4G1xgG/ifAzfUf581FICABwb/J/iM3wOfNghynzcMfzZQe/hDSDhbNARanyc0wJSgIPgHNARbNbg39NgIBzcEgze4b7UuIXDprXLg4Aw4hR+1DiFw+GZAyjFz9ywKGzdkNw24NzU4gKPv+KCAO583fH9/+KcKKHOfDsiH+yiAR580fHP3LN/79ywF2woocGA3NjSzSryHNHxz9yze+zTAlyrIhzb8WIXFcy7bL7gEBAMt+IAs6O1zmQCACDgO2d/c2DXkPDzAFPiISK3ciW1z9yzd+ICwqXVzlKj1c5SE6IeX9yzBmKATtcz1cKmFczacR/TYA/80sD/3LAb7NuSEYA80sD/02IgDN1iEgCs0dEe1LglzN2Q0hcVzLrst+y74gHOHhIj1c4SJfXP3LAf7NuSEqX1z9NiYAIl1cGBcqY1ztW2FcN+1SRE3NsirN/yoYA838H81OIMrBIMkqYVwiXVzf/uIoDDpxXM1ZHN/+DcjPC80wJcjPECpRXCMjIyN+/kvJ583yIdjf/iwo9v47KPLDihz+2dj+3z/Y9efx1sn1zYIc8afNwx/1zZQeV/HXetfJ1hHOACgd1gLOAChW/gF6BgEgBAcHBgRPev4CMBZ5IZFcGDh6Bgc4BQcHBwY4T3r+CjgCzxMhj1z+CDgLfigHsC/mJCgBeE95zWwiPge6n81sIgcH5lBHPgi6n66grncjeMmfeg8GgCADDwZAT3r+CCgE/gIwvXkhj1zNbCJ5Dw8PGNjNlB7+CDCp0/4HBwfLbyAC7gcySFzJPq+Q2vkkR6cfNx+nH6jm+KhneQcHB6jmx6gHB2955gfJzQcjzaoiRwR+BxD95gHDKC3NByPN5SLDTQ3tQ31czaoiRwQ+/g8Q/Ud+/U5Xy0EgAaDLUSACqC93w9sLzRQjR8XNFCNZwVFPyc3VLdr5JA4ByA7/yd/+LMKKHOfNghzN7hvvKj04fv6BMAXvAjgYoe+jODaD78UCOM19JMXvMeEEOH7+gDAI7wICOMHD3CLvwgHAAgMB4A/AATHgATHgoMECOP00Ys2UHm/lzZQe4WcifVzBwyAk3/4sKAbN7hvDdyTnzYIcze4b78WiBB8xMDAABgI4w3ckwALBAjEq4QHhKg/gBSrgAT04fv6BMAfvAgI4w3ckzX0kxe8C4QEFwQIBMeEEwgIBMeEE4uXgA6IEMR/FAiDAAsICweUE4OIED+EBwQLgBOLlBAPCKuEqDwI4Gv6Bwdp3JMXvATg6fVzNKC3vwA8BODp+XM0oLe/FD+DlOMEFKDwYFO/hMeME4uQEA8EC5ATi4wQPwgI4xe/AAuEPMTg6fVzNKC3vA+DiD8AB4Dg6flzNKC3vAzjNtyTBEMbvAgIBODp9XM0oLe8DATg6flzNKC3vAzjNtyTDTQ3vMSg0MgABBeUBBSo4zdUtOAbm/MYEMAI+/PXNKC3v5QEFMR/EAjGiBB/BAcACMQQxD6EDG8MCOMHJzQcjebgwBmnVr18YB7HIaEHVFgBgeB+FOAO8OAeUT9nBxRgET9XZwSp9XHiER3k8hTgNKA09T83lItl5ENnRySjzzwrfBgDFTyGWJc3cFnnShCYGAE4J6c10AAP+DcqKHP4iIPPNdAD+Isnn/iggBs15HN/+KcKKHP3LAX7JzQcjKjZcEQABGXkPDw/m4KhfeeYY7kBXBmDF1eUarigEPCAaPU8GBxQjGq6pIA8Q98HBwT6AkAEBAPcSGArhEQgAGdHBENNIw7IqzQcjeQ8PD0/m4KhveeYD7lhnfsMoLSIcKE8u8isSqFalV6eEpo/E5qq/q8epzgDnw/8k3yPlAQAAzQ8lIBvNDyUo+80wJSgR9+HVfiMSE/4iIPh+I/4iKPIL0SE7XMu2y37EsirDEifnzfsk/inCihznwxInw70nzTAlKCjtS3ZczSst76EPNDcWBDSAQQAAgDICoQMxOM2iLe1Ddlx+pygD1hB3GAnNMCUoBO+jODTnw8MmAVoQ5/4jyg0nITtcy7bLfigfzY4CDgAgE80eAzAOFV/NMwP1AQEA9/ESDgEGAM2yKsMSJ80iJcQ1JefD2yXNIiXEgCXnGEjNIiXEyyLnGD/NiCwwVv5BMDzNMCUgI82bLN8BBgDNVRYjNg4j6yplXA4Fp+1CImVc7bDrK813ABgO3yN+/g4g+iPNtDMiXVz9ywH2GBTNsijaLhzMlik6O1z+wDgEI820MxgzAdsJ/i0oJwEYEP6uKCDWr9qKHAHwBP4UKBTSihwGEMbcT/7fMALLsf7uOALLucXnw/8k3/4oIAz9ywF2IBfNUirnGPAGAE8hlSfN3BYwBk4h7SYJRtF6uDg6p8oYAMUhO1x7/u0gBst2IAIemdXNMCUoCXvmP0fvOzgYCXv9rgHmQMKKHNEhO1zL9st7IALLtsEYwdV5/csBdiAV5j/GCE/+ECAEy/EYCDjX/hcoAsv5xefD/yQrzy3DKsQvxV7GPc4+zDzNx8nIysnLxcfGyAAGCAgKAgMFBQUFBQUGzTAlIDXnzY0s0ooc5/4k9SAB5/4oIBLn/ikoEM37JN/+LCAD5xj1/inCihznITtcy7bxKALL9sMSJ+fm30fn1iRPIAHn5+UqU1wrEc4Axc2GHcEwAs8Y5c2rKObfuCAIzaso1iS5KAzhKxEAAsXNixnBGNenzKso0dHtU11czaso5f4pKEIjfv4OFkAoByvNqygjFgAj5dXN+yTx/a4B5kAgK+HrKmVcAQUA7UIiZVztsOsrzaso/ikoDeXf/iwgDefhzasoGL7l3/4pKALPGdHrIl1cKgtc4yILXNXn5837JOEiXVzhIgtc58MSJyN+/iE4+sn9ywH2382NLNKKHOXmH0/n5f4oKCjL8f4kKBHL6c2ILDAPzYgsMBbLsecY9uf9ywG2OgxcpygGzTAlwlEpQc0wJSAIeebgy/9PGDcqS1x+5n8oLbkgIheH8j8pODDR1eUjGhP+ICj69iC+KPT2gL4gBhrNiCwwFeHFzbgZ68EYzsv40d/+KCgJy+gYDdHR0eXfzYgsMAPnGPjhyxDLcMkqC1x+/inK7yh+9mBHI37+DigHK82rKCPLqHi5KBIjIyMjI82rKP4pyu8ozasoGNnLaSAMI+1bZVzNwDPrImVc0dGvPMmvR8t5IEvLfiAOPCNOI0Yj682yKt/DSSojIyNGy3EoCgUo6Ovf/iggYevrGCTl3+H+LCggy3koUstxIAb+KSA858n+KShs/swgMt8rIl1cGF4hAADl5+F5/sAgCd/+KShR/swo5cXlze4q4+vNzCo4GQvN9CoJ0cEQs8t5IGbly3EgE0JL3/4pKALPAufhEQUAzfQqCcnN7irjzfQqwQkjQkvrzbEq3/4pKAf+LCDbzVIq5/4oKPj9ywG2yc0wJcTxK+f+KShQ1a/1xREBAN/h/swoF/HNzSr1UFnl3+H+zCgJ/inCihxiaxgT5efh/ikoDPHNzSr132Bp/ikg5vHjGSvjp+1SAQAAOAcjp/ogKkRN0f3LAbbNMCXIr/3LAbbFzakzwSplXHcjcyNyI3EjcCMiZVzJr9Xl9c2CHPHNMCUoEvXNmR7ReLE3KAXh5aftQnreAOHRyesjXiNWyc0wJcjNqTDaFR/JKk1c/cs3TiheAQUAAyN+/iAo+jAL/hA4Ef4WMA0jGO3NiCw45/4kysAreSpZXCvNVRYjI+vVKk1cG9YGRygRI37+ITj69iATEhD09oASPsAqTVyu9iDhzeor5e8COOEBBQCn7UIYQP3LAXYoBhEGABkY5ypNXO1Lclz9yzdGIDB4scjl99XFVF0jNiDtuOXN8Svh46ftQgkwAkRN4+t4sSgC7bDB0eHreLHI1e2w4ckrKyt+5cXNxivB4QMDA8PoGT7fKk1cpvXN8SvrCcUrIk1cAwMDKllcK81VFipNXMHFA+246yPBcCtx8St3KllcK8kqZVwrRitOK1YrXit+ImVcyc2yKMKKHM0wJSAIy7HNlinN7hs4CMXNuBnN6BnBy/kGAMUhAQDLcSACLgXr5yb/zcwq2iAq4cUk5WBpzfQq69/+LCjo/ikgu+fBeWgmACMjKRnaFR/VxeVETSpZXCvNVRYjd8ELCwsjcSNwwXgjd2JrGzYAy3EoAjYgwe24wXArcSs9IPjJzRstP9j+QT/Q/lvY/mE/0P57yf7EIBkRAADn1jHOACAK6z/tatqtMesY70JLwyst/i4oD807Lf4uICjnzRstOCIYCufNGy3aihzvoDjvocACON/NIi04C+/gpAXABA845xjv/kUoA/5lwAb/5/4rKAX+LSACBOfNGy04y8XNOy3N1S3B2q0xp/qtMQQoAu1Ew08t/jDY/jo/yc0bLdjWME8GAP0hOlyvX1FIR822Ku84p8n176A48c0iLdjvAaQEDzjNdAAY8QcPMAIvPPUhklzNCzXvpDjxyz8wDfXvweAABAQzAgXhOPEoCPXvMQQ48Rjl7wI4ySNOI36pkV8jfompV8kOAOU2ACNxI3upkXcjeompdyM2AOHJ7zh+pygF76IPJzjvAjjl1etGzX8tr5DLeUJLe9HhyVcXn19Pr0fNtirvNO8aIJqFBCc4zaIt2PUFBCgD8TfJ8cnvMTYACzE3AA0COD4w18kqOD4t1++gw8TFAjjZ5dnvMSfCA+IBwgI4fqcgR81/LQYQeqcgBrMoCVMGCNXZ0dkYV+/iOH7Wfs3BLVc6rFySMqxces1PLe8xJ8ED4TjN1S3lMqFcPRefPCGrXHcjhnfhw88u1oD+HDgTzcEt1gdHIaxchnd47UTNTy0YkuvNui/Zy/p92daAR8sjyxLZyxPLEtkhqlwOBX6PJ3crDSD4EOevIaZcEaFcBgntbw7/7W8gBA0MIAoSE/00cf00cg4Ay0AoASMQ5zqrXNYJOAr9NXE+BP2+bxhB7wLiOOvNui/ZPoCVLgDL+tnN3S/9fnH+CDgG2csS2RggAQACe82LL196zYsvV8XZwRDxIaFcef1OcQl3/TRxGNP1IaFc/U5xBgAJQfErfs4Ad6coBf4KPzAIEPE2AQT9NHL9cHHvAjjZ4dntS6tcIaFceP4JOAT+/Dgmp8zvFa+Q+lIvRxgMeacoA34jDc3vFRD0eafIBD4u1z4wEPtBGOZQFQYBzUovPkXXSnmn8oMv7URPPi0YAj4r1wYAwxsa1W8mAF1UKSkZKVkZTH3RyX42AKfII8t+y/4ryMUBBQAJQU83K34vzgB3EPh5wcnl9U4jRncjeU7FI04jRutXXtUjViNe1dnR4cHZI1YjXvHhyafI/iEwFsVH2cstyxrLG9nLGssbEPLB0M0EMMDZry4AV13ZEQAAyRzAFMDZHCABFNnJ681uNOsatiAm1SPlI14jViMjI34jTiNG4esJ644PzgAgC593I3MjcisrK9HJK9HNkzLZ5dnV5c2bL0frzZsvT7gwA3hB6/WQzbovzd0v8eF35WhhGdnr7UrrfI1vH63Z6+EfMAg+Ac3dLzQoI9l95oDZI3crKB977UQ/X3ovzgBX2XsvzgBfei/OADAHH9k0yq0x2VfZr8NVMcUGEHxNIQAAKTgKyxEXMAMZOAIQ88HJzek02COuy/4ryRq2ICLV5dXNfy3r40HNfy14qU/hzakw6+E4CnqzIAFPzY4t0cnRzZMyr83AMNjZ5dnV683AMOs4WuXNui94p+1i2eXtYtkGIRgRMAUZ2e1a2dnLHMsd2cscyx3ZyxjLGdnLGR8Q5OvZ69nB4XiBIAGnPT8XPx/yRjEwaKc8IAg4BtnLetkgXHfZeNkwFX6nPoAoAa/Zos37Lwd3OC4jdysYKQYg2ct62SASB8sTyxLZyxPLEtk1KNcQ6hjXFzAMzQQwIAfZFoDZNCgY5SPZ1dnBeBfLFh93I3EjciNz4dHZ4dnJzwXNkzLrr83AMDj0683AMNjZ5dnV5c26L9nlYGnZYWivBt8YEBfLEdnLEcsQ2SnZ7WrZOBDtUtntUtkwDxnZ7VrZpxgIp+1S2e1S2TcE+tIx9SjhX1HZWVDxyxjxyxjZweF4kcM9MX6nyP6BMAY2AD4gGFH+kSAaIyMjPoCmK7YrIAM+gK4rIDZ3Izb/Kz4YGDMwLNUvxpEjViNeKysOAMt6KAENy/oGCJCAOARaFgCQKAdHyzrLGxD6zY4t0cl+1qDw7UTV6ytHyzjLOMs4KAU2ACsQ++YHKAlHPv/LJxD8pnfr0cnNljLrfqfA1c1/La8jdyt3BpF6pyAIs0IoEFNYBonrBSkw/MsJyxzLHesrcytyK3DRyQCwAECwAAEwAPFJD9qiQLAACo82PDShMw8wyjCvMVE4GzUkNTs1OzU7NTs1OzU7NRQwLTU7NTs1OzU7NTs1OzWcNd41vDRFNm40aTbeNXQ2tTeqN9o3MzhDOOI3EzfENq82SjiSNGo0rDSlNLM0HzbJNQE1wDOgNoY2xjN6NgY1+TSbNoM3FDKiM08tlzJJNBs0LTQPNM2/NXgyZ1zZ49ntU2Vc2X4j5afygDNX5mAPDw8PxnxveuYfGA7+GDAI2QH7/1RdCdkHbxHXMiYAGV4jViFlM+PV2e1LZlzJ8TpnXNkYw9XlAQUAzQUf4dHJ7VtlXM3AM+1TZVzJzakz7bDJYmvNqTPZ5dnjxX7mwAcHTwx+5j8gAiN+xlASPgWRIxMGAO2wwePZ4dlHrwXIEhMY+qfI9dURAADNyDPR8T0Y8k8HB4FPBgAJydUqaFzNBjTNwDPhyWJr2eUhxTLZzfczzcgz2eHZyeXrKmhczQY0683AM+vhyQYFGk7rEnEjExD368lHzV4zMQ/AAqDCMeAE4sEDOM3GM81iMw8BwgI17uEDOMkG/xgGzek02AYAfqcoCyN45oC2Fz8fdyvJ1eXNfy3heLEvT82OLdHJzek02NURAQAjyxYrn0/Nji3Ryc2ZHu14GATNmR4KwygtzZkeISst5cXJzfErC3ixICMazY0sOAnWkDgZ/hUwFTw9h4eH/qgwDO1Le1yBTzABBMMrLc8J5cVHfiO2I7YjtnjB4cA3yc3pNNg+/xgGzek0GAWvI64rB+U+AHcjdyMXdx8jdyN34cnrzek069g3GOfrzek069CnGN7rzek069DVG68SGxLRyXjWCMtXIAE9DzAI9eXNPDTR6/HLVyAHD/XNDzAYMw/1zfEr1cXN8SvhfLXjeCALscEoBPE/GBbxGBOxKA0aljgJIO0LEyPjKxjfwfGn9e+gOPH13AE18fXU+TTxD9QBNcnN8SvVxc3xK+Hl1cUJRE33zbIqweF4sSgC7bDB4XixKALtsCplXBH7/+UZ0cnN1S04DiAM9QEBAPfxEs2yKuvJzwoqXVzleMbjn/XN8SvVA/fh7VNdXNXtsOsrNg39ywG+zfsk3/4NIAfh8f2uAeZAwoocIl1c/csB/s37JOEiXVwYoAEBAPciW1zlKlFc5T7/zQEWzeMt4c0VFtEqW1yn7VJETc2yKuvJzZQe/hDSnx4qUVzlzQEWzeYVAQAAMAMM9xLNsirhzRUWw781zfEreLEoARrDKC3N8SvDKy3Z5SFnXDXhIAQj2cnZXnsXn1cZ2ckTExobG6cg79kj2cnx2ePZye/AAjHgBSfgAcAEA+A4ye8xNgAEOjjJMTrAA+ABMAADoQM4ye89NPE4qjspBDEnwwMxD6EDiBM2WGVmnXhlQKJgMsnnIfevJOsvsLAU7n67lFjxOn74z+M4zdUtIAc4A4YwCc8FOAeWMATtRHfJ7wKgOMnvPTE3AAQ4zwmgAjh+NoDNKC3vNDgAAwExNPBMzMzNAzcACAGhAwE4NO8BNPAxchf4BAGiA6IDMTQyIASiA4wRrBQJVtqlWTDFXJCqnnBvYaHL2pakMZ+056D+XPzqG0PKNu2nnH5e8G4jgJMEDzjJ7z007iL5g24EMaIPJwMxDzEPMSqhAzE3wAAEAjjJoQMBNgACGzjJ7zkqoQPgAAYbMwPvOTExBDEPoQOGFOZcHwujjzju6RVjuyPukg3N7fEjXRvqBDjJ7zEfASAFOMnNlzJ+/oE4Du+hGwEFMTajAQAGGzMD76ABMTEEMQ+hA4wQshMOVeSNWDm8W5j9ngA2daDb6LRjQsTmtQk2vuk2cxtd7NjeY77wYaGzDAQPOMnvMTEEoQMbKKEPBSQxDzjJ7yKjAxs4ye8xMAAeojjvATEwAAclBDjDxDYCMTAACaABNwAGoQEFAqE4yf///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////wAAAAAAAAAAABAQEBAAEAAAJCQAAAAAAAAkfiQkfiQAAAg+KD4KPggAYmQIECZGAAAQKBAqRDoAAAgQAAAAAAAABAgICAgEAAAgEBAQECAAAAAUCD4IFAAAAAgIPggIAAAAAAAACAgQAAAAAD4AAAAAAAAAABgYAAAAAgQIECAAADxGSlJiPAAAGCgICAg+AAA8QgI8QH4AADxCDAJCPAAACBgoSH4IAAB+QHwCQjwAADxAfEJCPAAAfgIECBAQAAA8QjxCQjwAADxCQj4CPAAAAAAQAAAQAAAAEAAAEBAgAAAECBAIBAAAAAA+AD4AAAAAEAgECBAAADxCBAgACAAAPEpWXkA8AAA8QkJ+QkIAAHxCfEJCfAAAPEJAQEI8AAB4REJCRHgAAH5AfEBAfgAAfkB8QEBAAAA8QkBOQjwAAEJCfkJCQgAAPggICAg+AAACAgJCQjwAAERIcEhEQgAAQEBAQEB+AABCZlpCQkIAAEJiUkpGQgAAPEJCQkI8AAB8QkJ8QEAAADxCQlJKPAAAfEJCfERCAAA8QDwCQjwAAP4QEBAQEAAAQkJCQkI8AABCQkJCJBgAAEJCQkJaJAAAQiQYGCRCAACCRCgQEBAAAH4ECBAgfgAADggICAgOAAAAQCAQCAQAAHAQEBAQcAAAEDhUEBAQAAAAAAAAAAD/ABwieCAgfgAAADgEPEQ8AAAgIDwiIjwAAAAcICAgHAAABAQ8REQ8AAAAOER4QDwAAAwQGBAQEAAAADxERDwEOABAQHhEREQAABAAMBAQOAAABAAEBAQkGAAgKDAwKCQAABAQEBAQDAAAAGhUVFRUAAAAeEREREQAAAA4REREOAAAAHhERHhAQAAAPEREPAQGAAAcICAgIAAAADhAOAR4AAAQOBAQEAwAAABEREREOAAAAEREKCgQAAAARFRUVCgAAABEKBAoRAAAAERERDwEOAAAfAgQIHwAAA4IMAgIDgAACAgICAgIAABwEAwQEHAAABQoAAAAAAA8QpmhoZlCPA==";

    public static byte[] Spectrum48KROM()
    {
        return Convert.FromBase64String(rom48);
    }
}

public class MainZ80Registers : IMainZ80Registers
{
    public short AF { get; set; }

    public short BC { get; set; }

    public short DE { get; set; }

    public short HL { get; set; }

    public byte A
    {
        get { return Z80InstructionExecutor.GetHighByte(AF); }
        set { AF = Z80InstructionExecutor.SetHighByte(AF, value); }
    }

    public byte F
    {
        get { return Z80InstructionExecutor.GetLowByte(AF); }
        set { AF = Z80InstructionExecutor.SetLowByte(AF, value); }
    }

    public byte B
    {
        get { return Z80InstructionExecutor.GetHighByte(BC); }
        set { BC = Z80InstructionExecutor.SetHighByte(BC, value); }
    }

    public byte C
    {
        get { return Z80InstructionExecutor.GetLowByte(BC); }
        set { BC = Z80InstructionExecutor.SetLowByte(BC, value); }
    }

    public byte D
    {
        get { return Z80InstructionExecutor.GetHighByte(DE); }
        set { DE = Z80InstructionExecutor.SetHighByte(DE, value); }
    }

    public byte E
    {
        get { return Z80InstructionExecutor.GetLowByte(DE); }
        set { DE = Z80InstructionExecutor.SetLowByte(DE, value); }
    }

    public byte H
    {
        get { return Z80InstructionExecutor.GetHighByte(HL); }
        set { HL = Z80InstructionExecutor.SetHighByte(HL, value); }
    }

    public byte L
    {
        get { return Z80InstructionExecutor.GetLowByte(HL); }
        set { HL = Z80InstructionExecutor.SetLowByte(HL, value); }
    }

    public Bit CF
    {
        get { return Z80InstructionExecutor.GetBit(F, 0); }
        set { F = Z80InstructionExecutor.WithBit(F, 0, value); }
    }

    public Bit NF
    {
        get { return Z80InstructionExecutor.GetBit(F, 1); }
        set { F = Z80InstructionExecutor.WithBit(F, 1, value); }
    }

    public Bit PF
    {
        get { return Z80InstructionExecutor.GetBit(F, 2); }
        set { F = Z80InstructionExecutor.WithBit(F, 2, value); }
    }

    public Bit Flag3
    {
        get { return Z80InstructionExecutor.GetBit(F, 3); }
        set { F = Z80InstructionExecutor.WithBit(F, 3, value); }
    }

    public Bit HF
    {
        get { return Z80InstructionExecutor.GetBit(F, 4); }
        set { F = Z80InstructionExecutor.WithBit(F, 4, value); }
    }

    public Bit Flag5
    {
        get { return Z80InstructionExecutor.GetBit(F, 5); }
        set { F = Z80InstructionExecutor.WithBit(F, 5, value); }
    }

    public Bit ZF
    {
        get { return Z80InstructionExecutor.GetBit(F, 6); }
        set { F = Z80InstructionExecutor.WithBit(F, 6, value); }
    }

    public Bit SF
    {
        get { return Z80InstructionExecutor.GetBit(F, 7); }
        set { F = Z80InstructionExecutor.WithBit(F, 7, value); }
    }
}

public class MemoryAccessEventArgs : ProcessorEventArgs
{
public MemoryAccessEventArgs(
        MemoryAccessEventType eventType,
        ushort address,
        byte value,
        object localUserState = null,
        bool cancelMemoryAccess = false)
    {
        this.EventType = eventType;
        this.Address = address;
        this.Value = value;
        this.LocalUserState = localUserState;
        this.CancelMemoryAccess = cancelMemoryAccess;
    }

public MemoryAccessEventType EventType { get; private set; }

public ushort Address { get; private set; }

public byte Value { get; set; }

public bool CancelMemoryAccess { get; set; }
}

public enum MemoryAccessEventType
{
BeforeMemoryRead,

AfterMemoryRead,

BeforeMemoryWrite,

AfterMemoryWrite,

BeforePortRead,

AfterPortRead,

BeforePortWrite,

AfterPortWrite
}

public enum MemoryAccessMode
{
ReadAndWrite,

ReadOnly,

WriteOnly,

NotConnected
}

public partial class Z80InstructionExecutor
{
byte NEG()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)-oldValue;
        RG.A = newValue;

        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.HF = (oldValue ^ newValue) & 0x10;
        RG.PF = (oldValue == 0x80);
        RG.NF = 1;
        RG.CF = (oldValue != 0);
        SetFlags3and5From(newValue);

        return 8;
    }
}

public partial class Z80InstructionExecutor
{
byte NOP()
    {
        FetchFinished();
        return 4;
    }
}

public partial class Z80InstructionExecutor
{
byte NOP2()
    {
        FetchFinished();
        return 8;
    }
}

public partial class Z80InstructionExecutor
{
public static byte GetHighByte(short value)
    {
        return (byte)(value >> 8);
    }

public static byte GetHighByte(ushort value)
    {
        return (byte)(value >> 8);
    }

public static ushort SetHighByte(ushort value, byte highByte)
    {
        return (ushort)((value & 0x00FF) | (highByte << 8));
    }

public static short SetHighByte(short value, byte highByte)
    {
        return ToShort(((value & 0x00FF) | (highByte << 8)));
    }

public static byte GetLowByte(short value)
    {
        return (byte)(value & 0xFF);
    }

public static byte GetLowByte(ushort value)
    {
        return (byte)(value & 0xFF);
    }

public static ushort SetLowByte(ushort value, byte lowByte)
    {
        return (ushort)((value & 0xFF00) | lowByte);
    }

public static short SetLowByte(short value, byte lowByte)
    {
        return (short)((value & 0xFF00) | lowByte);
    }

public static short CreateShort(byte lowByte, byte highByte)
    {
        return (short)((highByte << 8) | lowByte);
    }

public static ushort CreateUshort(byte lowByte, byte highByte)
    {
        return (ushort)((highByte << 8) | lowByte);
    }

public static Bit GetBit(byte value, int bitPosition)
    {
        if (bitPosition < 0 || bitPosition > 7)
            throw new Exception("bit position must be between 0 and 7");

        return (value & (1 << bitPosition));
    }

public static byte WithBit(byte number, int bitPosition, Bit value)
    {
        if (bitPosition < 0 || bitPosition > 7)
            throw new Exception("bit position must be between 0 and 7");

        if (value)
        {
            return (byte)(number | (1 << bitPosition));
        }
        else
        {
            return (byte)(number & ~(1 << bitPosition));
        }
    }

public static short ToShort(int value)
    {
        return (short)(ushort)value;
    }

public static short ToShort(ushort value)
    {
        return (short)value;
    }

public static ushort ToUShort(int value)
    {
        return (ushort)value;
    }

public static ushort ToUShort(short value)
    {
        return (ushort)value;
    }

public static SByte ToSignedByte(int value)
    {
        return (SByte)value;
    }

public static SByte ToSignedByte(byte value)
    {
        return (SByte)value;
    }

public static short Inc(short value)
    {
        return (short)(value + 1);
    }

public static ushort Inc(ushort value)
    {
        return (ushort)(value + 1);
    }

public static ushort Dec(ushort value)
    {
        return (ushort)(value - 1);
    }

public static short Dec(short value)
    {
        return (short)(value - 1);
    }

public static short Add(short value, short amount)
    {
        return (short)(value + amount);
    }

public static ushort Add(ushort value, ushort amount)
    {
        return (ushort)(value + amount);
    }

public static ushort Add(ushort value, SByte amount)
    {
        return (ushort)(value + amount);
    }

public static short Sub(short value, short amount)
    {
        return (short)(value - amount);
    }

public static ushort Sub(ushort value, ushort amount)
    {
        return (ushort)(value - amount);
    }

public static byte Inc(byte value)
    {
        return (byte)(value + 1);
    }

public static byte Dec(byte value)
    {
        return (byte)(value - 1);
    }

public static short Add(byte value, byte amount)
    {
        return (byte)(value + amount);
    }

public static short Add(byte value, int amount)
    {
        return (byte)(value + (byte)amount);
    }

public static short Sub(byte value, byte amount)
    {
        return (short)(value - amount);
    }

public static short Sub(byte value, int amount)
    {
        return (byte)(value - (byte)amount);
    }

public static byte Inc7Bits(byte value)
    {
        return (byte)((value & 0x80) == 0 ? (value + 1) & 0x7F : (value + 1) & 0x7F | 0x80);
    }

public static bool Between(byte value, byte fromInclusive, byte toInclusive)
    {
        return value >= fromInclusive && value <= toInclusive;
    }

public static ushort AddSignedByte(ushort value, byte amount)
    {
        return amount < 0x80 ? Add(value, amount) : Sub(value, (ushort)(256 - amount));
    }

public static byte[] ToByteArray(short value)
    {
        return new[] { GetLowByte(value), GetHighByte(value) };
    }

public static byte[] ToByteArray(ushort value)
    {
        return new[] { GetLowByte(value), GetHighByte(value) };
    }
}

public partial class Z80InstructionExecutor
{
byte OUT_C_A()
    {
        FetchFinished();

        ProcessorAgent.WriteToPort(RG.C, RG.A);

        return 12;
    }

byte OUT_C_B()
    {
        FetchFinished();

        ProcessorAgent.WriteToPort(RG.C, RG.B);

        return 12;
    }

byte OUT_C_C()
    {
        FetchFinished();

        ProcessorAgent.WriteToPort(RG.C, RG.C);

        return 12;
    }

byte OUT_C_D()
    {
        FetchFinished();

        ProcessorAgent.WriteToPort(RG.C, RG.D);

        return 12;
    }

byte OUT_C_E()
    {
        FetchFinished();

        ProcessorAgent.WriteToPort(RG.C, RG.E);

        return 12;
    }

byte OUT_C_H()
    {
        FetchFinished();

        ProcessorAgent.WriteToPort(RG.C, RG.H);

        return 12;
    }

byte OUT_C_L()
    {
        FetchFinished();

        ProcessorAgent.WriteToPort(RG.C, RG.L);

        return 12;
    }

byte OUT_C_0()
    {
        FetchFinished();

        ProcessorAgent.WriteToPort(RG.C, 0);

        return 12;
    }
}

public partial class Z80InstructionExecutor
{
byte OUT_n_A()
    {
        var portNumber = ProcessorAgent.FetchNextOpcode();
        FetchFinished();

        ProcessorAgent.WriteToPort(portNumber, RG.A);

        return 11;
    }
}

public partial class Z80InstructionExecutor
{
    private Bit[] Parity;

    private void GenerateParityTable()
    {
        Parity = new Bit[256];

        for (var result = 0; result <= 255; result++)
        {
            var ones = 0;
            var temp = result;
            for (var i = 0; i <= 7; i++)
            {
                ones += (temp & 1);
                temp >>= 1;
            }

            Parity[result] = (ones & 1) ^ 1;
        }
    }
}

public class PlainMemory : IMemory
{
    private readonly byte[] memory;

    public PlainMemory(int size)
    {
        if (size < 1)
            throw new InvalidOperationException("Memory size must be greater than zero");

        memory = new byte[size];
        Size = size;
    }

    public int Size { get; private set; }

    public byte this[int address]
    {
        get { return memory[address]; }
        set { memory[address] = value; }
    }

    public void SetContents(int startAddress, byte[] contents, int startIndex = 0, int? length = null)
    {
        if (contents == null)
            throw new ArgumentNullException("contents");

        if (length == null)
            length = contents.Length;

        if ((startIndex + length) > contents.Length)
            throw new Exception("startIndex + length cannot be greater than contents.length");

        if (startIndex < 0)
            throw new Exception("startIndex cannot be negative");

        if (startAddress + length > Size)
            throw new Exception("startAddress + length cannot go beyond the memory size");

        Array.Copy(
            sourceArray: contents,
            sourceIndex: startIndex,
            destinationArray: memory,
            destinationIndex: startAddress,
            length: length.Value
        );
    }

    public byte[] GetContents(int startAddress, int length)
    {
        if (startAddress >= memory.Length)
            throw new Exception("startAddress cannot go beyond memory size");

        if (startAddress + length > memory.Length)
            throw new Exception("startAddress + length cannot go beyond memory size");

        if (startAddress < 0)
            throw new Exception("startAddress cannot be negative");

        return memory.Skip(startAddress).Take(length).ToArray();
    }
}

public abstract class ProcessorEventArgs : EventArgs
{
public object LocalUserState { get; set; }
}

public enum ProcessorState
{
Stopped,

Paused,

Running,

ExecutingOneInstruction
}

public partial class Z80InstructionExecutor
{
private byte PUSH_AF()
    {
        FetchFinished();

        var valueToPush = RG.AF;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;

        return 11;
    }

private byte POP_AF()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;
        var newAF = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.AF = newAF;

        RG.SP += 2;

        return 10;
    }

private byte PUSH_BC()
    {
        FetchFinished();

        var valueToPush = RG.BC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;

        return 11;
    }

private byte POP_BC()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;
        var newBC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.BC = newBC;

        RG.SP += 2;

        return 10;
    }

private byte PUSH_DE()
    {
        FetchFinished();

        var valueToPush = RG.DE;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;

        return 11;
    }

private byte POP_DE()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;
        var newDE = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.DE = newDE;

        RG.SP += 2;

        return 10;
    }

private byte PUSH_HL()
    {
        FetchFinished();

        var valueToPush = RG.HL;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;

        return 11;
    }

private byte POP_HL()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;
        var newHL = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.HL = newHL;

        RG.SP += 2;

        return 10;
    }

private byte PUSH_IX()
    {
        FetchFinished();

        var valueToPush = RG.IX;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;

        return 15;
    }

private byte POP_IX()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;
        var newIX = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.IX = newIX;

        RG.SP += 2;

        return 14;
    }

private byte PUSH_IY()
    {
        FetchFinished();

        var valueToPush = RG.IY;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;

        return 15;
    }

private byte POP_IY()
    {
        FetchFinished();

        var sp = (ushort)RG.SP;
        var newIY = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.IY = newIY;

        RG.SP += 2;

        return 14;
    }
}

public partial class Z80InstructionExecutor
{
private byte RET()
    {
        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 10;
    }

private byte RETI()
    {
        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 14;
    }

private byte JP_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }

private byte RET_C()
    {
        if (RG.CF == 0)
        {
            FetchFinished(isRet: false);
            return 5;
        }

        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 11;
    }

private byte JP_C_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.CF == 0)
            return 10;

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_C_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.CF == 0)
            return 10;

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }

private byte RET_NC()
    {
        if (RG.CF == 1)
        {
            FetchFinished(isRet: false);
            return 5;
        }

        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 11;
    }

private byte JP_NC_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.CF == 1)
            return 10;

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_NC_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.CF == 1)
            return 10;

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }

private byte RET_Z()
    {
        if (RG.ZF == 0)
        {
            FetchFinished(isRet: false);
            return 5;
        }

        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 11;
    }

private byte JP_Z_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.ZF == 0)
            return 10;

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_Z_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.ZF == 0)
            return 10;

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }

private byte RET_NZ()
    {
        if (RG.ZF == 1)
        {
            FetchFinished(isRet: false);
            return 5;
        }

        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 11;
    }

private byte JP_NZ_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.ZF == 1)
            return 10;

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_NZ_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.ZF == 1)
            return 10;

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }

private byte RET_PE()
    {
        if (RG.PF == 0)
        {
            FetchFinished(isRet: false);
            return 5;
        }

        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 11;
    }

private byte JP_PE_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.PF == 0)
            return 10;

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_PE_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.PF == 0)
            return 10;

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }

private byte RET_PO()
    {
        if (RG.PF == 1)
        {
            FetchFinished(isRet: false);
            return 5;
        }

        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 11;
    }

private byte JP_PO_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.PF == 1)
            return 10;

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_PO_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.PF == 1)
            return 10;

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }

private byte RET_M()
    {
        if (RG.SF == 0)
        {
            FetchFinished(isRet: false);
            return 5;
        }

        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 11;
    }

private byte JP_M_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.SF == 0)
            return 10;

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_M_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.SF == 0)
            return 10;

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }

private byte RET_P()
    {
        if (RG.SF == 1)
        {
            FetchFinished(isRet: false);
            return 5;
        }

        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        return 11;
    }

private byte JP_P_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.SF == 1)
            return 10;

        RG.PC = newAddress;

        return 10;
    }

private byte CALL_P_nn()
    {
        var newAddress = (ushort)FetchWord();

        FetchFinished();
        if (RG.SF == 1)
            return 10;

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = newAddress;

        return 17;
    }
}

public partial class Z80InstructionExecutor
{
private byte RETN()
    {
        FetchFinished(isRet: true);

        var sp = (ushort)RG.SP;
        var newPC = CreateShort(
            ProcessorAgent.ReadFromMemory(sp),
            ProcessorAgent.ReadFromMemory((ushort)(sp + 1)));
        RG.PC = (ushort)newPC;

        RG.SP += 2;

        RG.IFF1 = RG.IFF2;

        return 14;
    }
}

public partial class Z80InstructionExecutor
{
byte RLC_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RLC_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RLC_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RLC_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RLC_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RLC_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RLC_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RLC_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 15;
    }

byte RLCA()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        return 4;
    }

byte RLC_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RLC_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (oldValue >> 7));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RRC_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RRC_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RRC_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RRC_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RRC_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RRC_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RRC_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 15;
    }

byte RRCA()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        return 4;
    }

byte RRC_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RRC_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue << 7));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RL_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RL_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RL_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RL_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RL_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RL_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RL_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 15;
    }

byte RLA()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        return 4;
    }

byte RL_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RL_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | (byte)RG.CF);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RR_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RR_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RR_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RR_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RR_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RR_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte RR_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 15;
    }

byte RRA()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        return 4;
    }

byte RR_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte RR_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (RG.CF ? 0x80 : 0));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)(oldValue << 1);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLA_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)(oldValue << 1);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLA_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)(oldValue << 1);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLA_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)(oldValue << 1);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLA_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)(oldValue << 1);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLA_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)(oldValue << 1);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLA_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)(oldValue << 1);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLA_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 15;
    }

byte SLA_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLA_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue << 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRA_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRA_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRA_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRA_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRA_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRA_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRA_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 15;
    }

byte SRA_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRA_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue >> 1) | (oldValue & 0x80));
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)((oldValue << 1) | 1);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLL_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)((oldValue << 1) | 1);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLL_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)((oldValue << 1) | 1);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLL_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)((oldValue << 1) | 1);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLL_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)((oldValue << 1) | 1);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLL_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)((oldValue << 1) | 1);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLL_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)((oldValue << 1) | 1);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SLL_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 15;
    }

byte SLL_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SLL_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)((oldValue << 1) | 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 7);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = (byte)(oldValue >> 1);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRL_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = (byte)(oldValue >> 1);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRL_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = (byte)(oldValue >> 1);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRL_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = (byte)(oldValue >> 1);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRL_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = (byte)(oldValue >> 1);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRL_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = (byte)(oldValue >> 1);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRL_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = (byte)(oldValue >> 1);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 8;
    }

byte SRL_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 15;
    }

byte SRL_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }

byte SRL_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = (byte)(oldValue >> 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        RG.CF = GetBit(oldValue, 0);
        RG.HF = 0;
        RG.NF = 0;
        SetFlags3and5From(newValue);
        RG.SF = GetBit(newValue, 7);
        RG.ZF = (newValue == 0);
        RG.PF = Parity[newValue];

        return 23;
    }
}

public partial class Z80InstructionExecutor
{
byte RRD()
    {
        FetchFinished();

        var memoryAddress = (ushort)RG.HL;

        var Avalue = RG.A;
        var HLcontents = ProcessorAgent.ReadFromMemory(memoryAddress);

        var newAvalue = (byte)((Avalue & 0xF0) | (HLcontents & 0x0F));
        var newHLcontents = (byte)(((HLcontents >> 4) & 0x0F) | ((Avalue << 4) & 0xF0));
        RG.A = newAvalue;
        ProcessorAgent.WriteToMemory(memoryAddress, newHLcontents);

        RG.SF = GetBit(newAvalue, 7);
        RG.ZF = (newAvalue == 0);
        RG.HF = 0;
        RG.PF = Parity[newAvalue];
        RG.NF = 0;
        SetFlags3and5From(newAvalue);

        return 18;
    }

byte RLD()
    {
        FetchFinished();

        var memoryAddress = (ushort)RG.HL;

        var Avalue = RG.A;
        var HLcontents = ProcessorAgent.ReadFromMemory(memoryAddress);

        var newAvalue = (byte)((Avalue & 0xF0) | ((HLcontents >> 4) & 0x0F));
        var newHLcontents = (byte)(((HLcontents << 4) & 0xF0) | (Avalue & 0x0F));
        RG.A = newAvalue;
        ProcessorAgent.WriteToMemory(memoryAddress, newHLcontents);

        RG.SF = GetBit(newAvalue, 7);
        RG.ZF = (newAvalue == 0);
        RG.HF = 0;
        RG.PF = Parity[newAvalue];
        RG.NF = 0;
        SetFlags3and5From(newAvalue);

        return 18;
    }
}

public partial class Z80InstructionExecutor
{
private byte RST_00()
    {
        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = 0x00;

        return 11;
    }

private byte RST_08()
    {
        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = 0x08;

        return 11;
    }

private byte RST_10()
    {
        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = 0x10;

        return 11;
    }

private byte RST_18()
    {
        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = 0x18;

        return 11;
    }

private byte RST_20()
    {
        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = 0x20;

        return 11;
    }

private byte RST_28()
    {
        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = 0x28;

        return 11;
    }

private byte RST_30()
    {
        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = 0x30;

        return 11;
    }

private byte RST_38()
    {
        FetchFinished();

        var valueToPush = (short)RG.PC;
        var sp = (ushort)(RG.SP - 1);
        ProcessorAgent.WriteToMemory(sp, GetHighByte(valueToPush));
        sp--;
        ProcessorAgent.WriteToMemory(sp, GetLowByte(valueToPush));
        RG.SP = (short)sp;
        RG.PC = 0x38;

        return 11;
    }
}

public partial class Z80InstructionExecutor
{
    private const int HF_NF_reset = 0xED;
    private const int CF_set = 1;

byte SCF()
    {
        FetchFinished();

        RG.F = (byte)(RG.F & HF_NF_reset | CF_set);
        SetFlags3and5From(RG.A);

        return 4;
    }
}

public partial class Z80InstructionExecutor
{
byte SET_0_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 0, 1);
        RG.A = newValue;

        return 8;
    }

byte SET_1_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 1, 1);
        RG.A = newValue;

        return 8;
    }

byte SET_2_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 2, 1);
        RG.A = newValue;

        return 8;
    }

byte SET_3_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 3, 1);
        RG.A = newValue;

        return 8;
    }

byte SET_4_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 4, 1);
        RG.A = newValue;

        return 8;
    }

byte SET_5_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 5, 1);
        RG.A = newValue;

        return 8;
    }

byte SET_6_A()
    {
        FetchFinished();
        RG.A = WithBit(RG.A, 6, 1);
        return 8;
    }

byte SET_7_A()
    {
        FetchFinished();
        RG.A = WithBit(RG.A, 7, 1);
        return 8;
    }

byte RES_0_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 0, 0);
        RG.A = newValue;

        return 8;
    }

byte RES_1_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 1, 0);
        RG.A = newValue;

        return 8;
    }

byte RES_2_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 2, 0);
        RG.A = newValue;

        return 8;
    }

byte RES_3_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 3, 0);
        RG.A = newValue;

        return 8;
    }

byte RES_4_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 4, 0);
        RG.A = newValue;

        return 8;
    }

byte RES_5_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 5, 0);
        RG.A = newValue;

        return 8;
    }

byte RES_6_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 6, 0);
        RG.A = newValue;

        return 8;
    }

byte RES_7_A()
    {
        FetchFinished();

        var oldValue = RG.A;
        var newValue = WithBit(oldValue, 7, 0);
        RG.A = newValue;

        return 8;
    }

byte SET_0_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 0, 1);
        RG.B = newValue;

        return 8;
    }

byte SET_1_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 1, 1);
        RG.B = newValue;

        return 8;
    }

byte SET_2_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 2, 1);
        RG.B = newValue;

        return 8;
    }

byte SET_3_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 3, 1);
        RG.B = newValue;

        return 8;
    }

byte SET_4_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 4, 1);
        RG.B = newValue;

        return 8;
    }

byte SET_5_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 5, 1);
        RG.B = newValue;

        return 8;
    }

byte SET_6_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 6, 1);
        RG.B = newValue;

        return 8;
    }

byte SET_7_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 7, 1);
        RG.B = newValue;

        return 8;
    }

byte RES_0_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 0, 0);
        RG.B = newValue;

        return 8;
    }

byte RES_1_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 1, 0);
        RG.B = newValue;

        return 8;
    }

byte RES_2_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 2, 0);
        RG.B = newValue;

        return 8;
    }

byte RES_3_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 3, 0);
        RG.B = newValue;

        return 8;
    }

byte RES_4_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 4, 0);
        RG.B = newValue;

        return 8;
    }

byte RES_5_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 5, 0);
        RG.B = newValue;

        return 8;
    }

byte RES_6_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 6, 0);
        RG.B = newValue;

        return 8;
    }

byte RES_7_B()
    {
        FetchFinished();

        var oldValue = RG.B;
        var newValue = WithBit(oldValue, 7, 0);
        RG.B = newValue;

        return 8;
    }

byte SET_0_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 0, 1);
        RG.C = newValue;

        return 8;
    }

byte SET_1_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 1, 1);
        RG.C = newValue;

        return 8;
    }

byte SET_2_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 2, 1);
        RG.C = newValue;

        return 8;
    }

byte SET_3_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 3, 1);
        RG.C = newValue;

        return 8;
    }

byte SET_4_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 4, 1);
        RG.C = newValue;

        return 8;
    }

byte SET_5_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 5, 1);
        RG.C = newValue;

        return 8;
    }

byte SET_6_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 6, 1);
        RG.C = newValue;

        return 8;
    }

byte SET_7_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 7, 1);
        RG.C = newValue;

        return 8;
    }

byte RES_0_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 0, 0);
        RG.C = newValue;

        return 8;
    }

byte RES_1_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 1, 0);
        RG.C = newValue;

        return 8;
    }

byte RES_2_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 2, 0);
        RG.C = newValue;

        return 8;
    }

byte RES_3_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 3, 0);
        RG.C = newValue;

        return 8;
    }

byte RES_4_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 4, 0);
        RG.C = newValue;

        return 8;
    }

byte RES_5_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 5, 0);
        RG.C = newValue;

        return 8;
    }

byte RES_6_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 6, 0);
        RG.C = newValue;

        return 8;
    }

byte RES_7_C()
    {
        FetchFinished();

        var oldValue = RG.C;
        var newValue = WithBit(oldValue, 7, 0);
        RG.C = newValue;

        return 8;
    }

byte SET_0_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 0, 1);
        RG.D = newValue;

        return 8;
    }

byte SET_1_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 1, 1);
        RG.D = newValue;

        return 8;
    }

byte SET_2_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 2, 1);
        RG.D = newValue;

        return 8;
    }

byte SET_3_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 3, 1);
        RG.D = newValue;

        return 8;
    }

byte SET_4_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 4, 1);
        RG.D = newValue;

        return 8;
    }

byte SET_5_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 5, 1);
        RG.D = newValue;

        return 8;
    }

byte SET_6_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 6, 1);
        RG.D = newValue;

        return 8;
    }

byte SET_7_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 7, 1);
        RG.D = newValue;

        return 8;
    }

byte RES_0_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 0, 0);
        RG.D = newValue;

        return 8;
    }

byte RES_1_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 1, 0);
        RG.D = newValue;

        return 8;
    }

byte RES_2_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 2, 0);
        RG.D = newValue;

        return 8;
    }

byte RES_3_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 3, 0);
        RG.D = newValue;

        return 8;
    }

byte RES_4_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 4, 0);
        RG.D = newValue;

        return 8;
    }

byte RES_5_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 5, 0);
        RG.D = newValue;

        return 8;
    }

byte RES_6_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 6, 0);
        RG.D = newValue;

        return 8;
    }

byte RES_7_D()
    {
        FetchFinished();

        var oldValue = RG.D;
        var newValue = WithBit(oldValue, 7, 0);
        RG.D = newValue;

        return 8;
    }

byte SET_0_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 0, 1);
        RG.E = newValue;

        return 8;
    }

byte SET_1_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 1, 1);
        RG.E = newValue;

        return 8;
    }

byte SET_2_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 2, 1);
        RG.E = newValue;

        return 8;
    }

byte SET_3_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 3, 1);
        RG.E = newValue;

        return 8;
    }

byte SET_4_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 4, 1);
        RG.E = newValue;

        return 8;
    }

byte SET_5_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 5, 1);
        RG.E = newValue;

        return 8;
    }

byte SET_6_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 6, 1);
        RG.E = newValue;

        return 8;
    }

byte SET_7_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 7, 1);
        RG.E = newValue;

        return 8;
    }

byte RES_0_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 0, 0);
        RG.E = newValue;

        return 8;
    }

byte RES_1_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 1, 0);
        RG.E = newValue;

        return 8;
    }

byte RES_2_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 2, 0);
        RG.E = newValue;

        return 8;
    }

byte RES_3_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 3, 0);
        RG.E = newValue;

        return 8;
    }

byte RES_4_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 4, 0);
        RG.E = newValue;

        return 8;
    }

byte RES_5_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 5, 0);
        RG.E = newValue;

        return 8;
    }

byte RES_6_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 6, 0);
        RG.E = newValue;

        return 8;
    }

byte RES_7_E()
    {
        FetchFinished();

        var oldValue = RG.E;
        var newValue = WithBit(oldValue, 7, 0);
        RG.E = newValue;

        return 8;
    }

byte SET_0_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 0, 1);
        RG.H = newValue;

        return 8;
    }

byte SET_1_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 1, 1);
        RG.H = newValue;

        return 8;
    }

byte SET_2_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 2, 1);
        RG.H = newValue;

        return 8;
    }

byte SET_3_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 3, 1);
        RG.H = newValue;

        return 8;
    }

byte SET_4_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 4, 1);
        RG.H = newValue;

        return 8;
    }

byte SET_5_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 5, 1);
        RG.H = newValue;

        return 8;
    }

byte SET_6_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 6, 1);
        RG.H = newValue;

        return 8;
    }

byte SET_7_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 7, 1);
        RG.H = newValue;

        return 8;
    }

byte RES_0_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 0, 0);
        RG.H = newValue;

        return 8;
    }

byte RES_1_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 1, 0);
        RG.H = newValue;

        return 8;
    }

byte RES_2_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 2, 0);
        RG.H = newValue;

        return 8;
    }

byte RES_3_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 3, 0);
        RG.H = newValue;

        return 8;
    }

byte RES_4_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 4, 0);
        RG.H = newValue;

        return 8;
    }

byte RES_5_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 5, 0);
        RG.H = newValue;

        return 8;
    }

byte RES_6_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 6, 0);
        RG.H = newValue;

        return 8;
    }

byte RES_7_H()
    {
        FetchFinished();

        var oldValue = RG.H;
        var newValue = WithBit(oldValue, 7, 0);
        RG.H = newValue;

        return 8;
    }

byte SET_0_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 0, 1);
        RG.L = newValue;

        return 8;
    }

byte SET_1_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 1, 1);
        RG.L = newValue;

        return 8;
    }

byte SET_2_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 2, 1);
        RG.L = newValue;

        return 8;
    }

byte SET_3_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 3, 1);
        RG.L = newValue;

        return 8;
    }

byte SET_4_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 4, 1);
        RG.L = newValue;

        return 8;
    }

byte SET_5_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 5, 1);
        RG.L = newValue;

        return 8;
    }

byte SET_6_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 6, 1);
        RG.L = newValue;

        return 8;
    }

byte SET_7_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 7, 1);
        RG.L = newValue;

        return 8;
    }

byte RES_0_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 0, 0);
        RG.L = newValue;

        return 8;
    }

byte RES_1_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 1, 0);
        RG.L = newValue;

        return 8;
    }

byte RES_2_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 2, 0);
        RG.L = newValue;

        return 8;
    }

byte RES_3_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 3, 0);
        RG.L = newValue;

        return 8;
    }

byte RES_4_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 4, 0);
        RG.L = newValue;

        return 8;
    }

byte RES_5_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 5, 0);
        RG.L = newValue;

        return 8;
    }

byte RES_6_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 6, 0);
        RG.L = newValue;

        return 8;
    }

byte RES_7_L()
    {
        FetchFinished();

        var oldValue = RG.L;
        var newValue = WithBit(oldValue, 7, 0);
        RG.L = newValue;

        return 8;
    }

byte SET_0_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte SET_1_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte SET_2_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte SET_3_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte SET_4_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte SET_5_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte SET_6_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte SET_7_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte RES_0_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte RES_1_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte RES_2_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte RES_3_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte RES_4_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte RES_5_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte RES_6_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte RES_7_aHL()
    {
        FetchFinished();

        var address = (ushort)RG.HL;
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 15;
    }

byte SET_0_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_1_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_2_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_3_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_4_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_5_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_6_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_7_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_0_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_1_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_2_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_3_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_4_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_5_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_6_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_7_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_0_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_1_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_2_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_3_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_4_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_5_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_6_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_7_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_0_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_1_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_2_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_3_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_4_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_5_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_6_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_7_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_0_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_1_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_2_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_3_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_4_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_5_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_6_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_7_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_0_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_1_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_2_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_3_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_4_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_5_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_6_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_7_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_0_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_1_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_2_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_3_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_4_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_5_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_6_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_7_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_0_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_1_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_2_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_3_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_4_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_5_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_6_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_7_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_0_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_1_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_2_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_3_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_4_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_5_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_6_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_7_aIX_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_0_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_1_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_2_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_3_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_4_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_5_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_6_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_7_aIX_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_0_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_1_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_2_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_3_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_4_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_5_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_6_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_7_aIX_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_0_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_1_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_2_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_3_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_4_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_5_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_6_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_7_aIX_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_0_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_1_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_2_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_3_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_4_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_5_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_6_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_7_aIX_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_0_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_1_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_2_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_3_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_4_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_5_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_6_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_7_aIX_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_0_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_1_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_2_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_3_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_4_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_5_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_6_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_7_aIX_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_0_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_1_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_2_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_3_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_4_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_5_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_6_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_7_aIX_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IX + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_0_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_1_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_2_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_3_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_4_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_5_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_6_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_7_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte SET_0_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_1_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_2_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_3_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_4_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_5_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_6_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_7_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte SET_0_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_1_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_2_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_3_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_4_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_5_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_6_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_7_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte SET_0_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_1_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_2_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_3_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_4_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_5_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_6_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_7_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte SET_0_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_1_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_2_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_3_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_4_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_5_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_6_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_7_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte SET_0_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_1_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_2_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_3_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_4_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_5_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_6_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_7_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte SET_0_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_1_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_2_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_3_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_4_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_5_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_6_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_7_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte SET_0_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_1_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_2_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_3_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_4_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_5_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_6_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte SET_7_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 1);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_0_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_1_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_2_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_3_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_4_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_5_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_6_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_7_aIY_plus_n_and_load_A(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.A = newValue;

        return 23;
    }

byte RES_0_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_1_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_2_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_3_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_4_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_5_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_6_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_7_aIY_plus_n_and_load_B(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.B = newValue;

        return 23;
    }

byte RES_0_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_1_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_2_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_3_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_4_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_5_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_6_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_7_aIY_plus_n_and_load_C(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.C = newValue;

        return 23;
    }

byte RES_0_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_1_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_2_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_3_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_4_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_5_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_6_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_7_aIY_plus_n_and_load_D(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.D = newValue;

        return 23;
    }

byte RES_0_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_1_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_2_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_3_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_4_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_5_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_6_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_7_aIY_plus_n_and_load_E(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.E = newValue;

        return 23;
    }

byte RES_0_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_1_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_2_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_3_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_4_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_5_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_6_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_7_aIY_plus_n_and_load_H(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.H = newValue;

        return 23;
    }

byte RES_0_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_1_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_2_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_3_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_4_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_5_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_6_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_7_aIY_plus_n_and_load_L(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);
        RG.L = newValue;

        return 23;
    }

byte RES_0_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 0, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_1_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 1, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_2_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 2, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_3_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 3, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_4_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 4, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_5_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 5, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_6_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 6, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }

byte RES_7_aIY_plus_n(byte offset)
    {
        FetchFinished();

        var address = (ushort)(RG.IY + (SByte)offset);
        var oldValue = ProcessorAgent.ReadFromMemory(address);
        var newValue = WithBit(oldValue, 7, 0);
        ProcessorAgent.WriteToMemory(address, newValue);

        return 23;
    }
}

public enum StopReason
{
NotApplicable,

NeverRan,

StopInvoked,

PauseInvoked,

ExecuteNextInstructionInvoked,

DiPlusHalt,

RetWithStackEmpty,

ExceptionThrown
}

public class Z80Processor : IZ80Processor, IZ80ProcessorAgent
{
    private const int MemorySpaceSize = 65536;
    private const int PortSpaceSize = 65536;

    private const decimal MaxEffectiveClockSpeed = 100M;
    private const decimal MinEffectiveClockSpeed = 0.001M;

    private const ushort NmiServiceRoutine = 0x66;
    private const byte NOP_opcode = 0x00;
    private const byte RST38h_opcode = 0xFF;

    public Z80Processor()
    {
        ClockSynchronizer = new ClockSynchronizer();

        ClockFrequencyInMHz = 4;
        ClockSpeedFactor = 1;

        AutoStopOnDiPlusHalt = true;
        AutoStopOnRetWithStackEmpty = false;
        unchecked
        {
            StartOfStack = (short)0xFFFF;
        }

        SetMemoryWaitStatesForM1(0, MemorySpaceSize, 0);
        SetMemoryWaitStatesForNonM1(0, MemorySpaceSize, 0);
        SetPortWaitStates(0, PortSpaceSize, 0);

        Memory = new PlainMemory(MemorySpaceSize);
        PortsSpace = new PlainMemory(PortSpaceSize);

        SetMemoryAccessMode(0, MemorySpaceSize, MemoryAccessMode.ReadAndWrite);
        SetPortsSpaceAccessMode(0, PortSpaceSize, MemoryAccessMode.ReadAndWrite);

        Registers = new Z80Registers();
        InterruptSources = new List<IZ80InterruptSource>();

        InstructionExecutor = new Z80InstructionExecutor();

        StopReason = StopReason.NeverRan;
        State = ProcessorState.Stopped;
    }


    public void Start(object userState = null)
    {
        if (userState != null)
            this.UserState = userState;

        Reset();
        TStatesElapsedSinceStart = 0;

        InstructionExecutionLoop();
    }

    public void Continue()
    {
        InstructionExecutionLoop();
    }

    private int InstructionExecutionLoop(bool isSingleInstruction = false)
    {
        try
        {
            return InstructionExecutionLoopCore(isSingleInstruction);
        }
        catch
        {
            State = ProcessorState.Stopped;
            StopReason = StopReason.ExceptionThrown;

            throw;
        }
    }

    private int InstructionExecutionLoopCore(bool isSingleInstruction)
    {
        if (clockSynchronizer != null) clockSynchronizer.Start();
        executionContext = new InstructionExecutionContext();
        StopReason = StopReason.NotApplicable;
        State = ProcessorState.Running;
        var totalTStates = 0;

        while (!executionContext.MustStop)
        {
            executionContext.StartNewInstruction();

            FireBeforeInstructionFetchEvent();
            if (executionContext.MustStop)
                break;

            var executionTStates = ExecuteNextOpcode();

            totalTStates = executionTStates + executionContext.AccummulatedMemoryWaitStates;
            TStatesElapsedSinceStart += (ulong)totalTStates;
            TStatesElapsedSinceReset += (ulong)totalTStates;

            ThrowIfNoFetchFinishedEventFired();

            if (!isSingleInstruction)
            {
                CheckAutoStopForHaltOnDi();
                CheckForAutoStopForRetWithStackEmpty();
                CheckForLdSpInstruction();
            }

            FireAfterInstructionExecutionEvent(totalTStates);

            if (!IsHalted)
                IsHalted = executionContext.IsHaltInstruction;

            var interruptTStates = AcceptPendingInterrupt();
            totalTStates += interruptTStates;
            TStatesElapsedSinceStart += (ulong)interruptTStates;
            TStatesElapsedSinceReset += (ulong)interruptTStates;

            if (isSingleInstruction)
                executionContext.StopReason = StopReason.ExecuteNextInstructionInvoked;
            else if (clockSynchronizer != null)
                clockSynchronizer.TryWait(totalTStates);
        }

        if (clockSynchronizer != null)
            clockSynchronizer.Stop();
        this.StopReason = executionContext.StopReason;
        this.State =
            StopReason == StopReason.PauseInvoked
                ? ProcessorState.Paused
                : ProcessorState.Stopped;

        executionContext = null;

        return totalTStates;
    }

    private int ExecuteNextOpcode()
    {
        if (IsHalted)
        {
            executionContext.OpcodeBytes.Add(NOP_opcode);
            return InstructionExecutor.Execute(NOP_opcode);
        }

        return InstructionExecutor.Execute(FetchNextOpcode());
    }

    private int AcceptPendingInterrupt()
    {
        if (executionContext.IsEiOrDiInstruction)
            return 0;

        if (NmiInterruptPending)
        {
            IsHalted = false;
            Registers.IFF1 = 0;
            ExecuteCall(NmiServiceRoutine);
            return 11;
        }

        if (!InterruptsEnabled)
            return 0;

        var activeIntSource = InterruptSources.FirstOrDefault(s => s.IntLineIsActive);
        if (activeIntSource == null)
            return 0;

        Registers.IFF1 = 0;
        Registers.IFF2 = 0;
        IsHalted = false;

        switch (InterruptMode)
        {
            case 0:
                var opcode = activeIntSource.ValueOnDataBus.GetValueOrDefault(0xFF);
                InstructionExecutor.Execute(opcode);
                return 13;
            case 1:
                InstructionExecutor.Execute(RST38h_opcode);
                return 13;
            case 2:
                var pointerAddress = Z80InstructionExecutor.CreateUshort(
                    lowByte: activeIntSource.ValueOnDataBus.GetValueOrDefault(0xFF),
                    highByte: Registers.I);
                var callAddress = Z80InstructionExecutor.CreateUshort(
                    lowByte: ReadFromMemoryInternal(pointerAddress),
                    highByte: ReadFromMemoryInternal((ushort)(pointerAddress + 1)));
                ExecuteCall(callAddress);
                return 19;
        }

        return 0;
    }

    public void ExecuteCall(ushort address)
    {
        var oldAddress = (short)Registers.PC;
        var sp = (ushort)(Registers.SP - 1);
        WriteToMemoryInternal(sp, Z80InstructionExecutor.GetHighByte(oldAddress));
        sp = (ushort)(sp - 1);
        WriteToMemoryInternal(sp, Z80InstructionExecutor.GetLowByte(oldAddress));

        Registers.SP = (short)sp;
        Registers.PC = address;
    }

    public void ExecuteRet()
    {
        var sp = (ushort)Registers.SP;
        var newPC = Z80InstructionExecutor.CreateShort(ReadFromMemoryInternal(sp),
            ReadFromMemoryInternal((ushort)(sp + 1)));

        Registers.PC = (ushort)newPC;
        Registers.SP += 2;
    }

    private void ThrowIfNoFetchFinishedEventFired()
    {
        if (executionContext.FetchComplete)
            return;

        throw new InstructionFetchFinishedEventNotFiredException(
            instructionAddress: (ushort)(Registers.PC - executionContext.OpcodeBytes.Count),
            fetchedBytes: executionContext.OpcodeBytes.ToArray());
    }

    private void CheckAutoStopForHaltOnDi()
    {
        if (AutoStopOnDiPlusHalt && executionContext.IsHaltInstruction && !InterruptsEnabled)
            executionContext.StopReason = StopReason.DiPlusHalt;
    }

    private void CheckForAutoStopForRetWithStackEmpty()
    {
        if (AutoStopOnRetWithStackEmpty && executionContext.IsRetInstruction && StackIsEmpty())
            executionContext.StopReason = StopReason.RetWithStackEmpty;
    }

    private void CheckForLdSpInstruction()
    {
        if (executionContext.IsLdSpInstruction)
            StartOfStack = Registers.SP;
    }

    private bool StackIsEmpty()
    {
        return executionContext.SpAfterInstructionFetch == StartOfStack;
    }

    private bool InterruptsEnabled
    {
        get { return Registers.IFF1 == 1; }
    }

    void FireAfterInstructionExecutionEvent(int tStates)
    {
    }

    void InstructionExecutor_InstructionFetchFinished(object sender, InstructionFetchFinishedEventArgs e)
    {
        if (executionContext.FetchComplete)
            return;

        executionContext.FetchComplete = true;

        executionContext.IsRetInstruction = e.IsRetInstruction;
        executionContext.IsLdSpInstruction = e.IsLdSpInstruction;
        executionContext.IsHaltInstruction = e.IsHaltInstruction;
        executionContext.IsEiOrDiInstruction = e.IsEiOrDiInstruction;

        executionContext.SpAfterInstructionFetch = Registers.SP;

        var eventArgs = FireBeforeInstructionExecutionEvent();
        executionContext.LocalUserStateFromPreviousEvent = eventArgs.LocalUserState;
    }

    void FireBeforeInstructionFetchEvent()
    {
        var eventArgs = new BeforeInstructionFetchEventArgs(stopper: this);
        executionContext.LocalUserStateFromPreviousEvent = eventArgs.LocalUserState;
    }

    BeforeInstructionExecutionEventArgs FireBeforeInstructionExecutionEvent()
    {
        var eventArgs = new BeforeInstructionExecutionEventArgs(
            executionContext.OpcodeBytes.ToArray(),
            executionContext.LocalUserStateFromPreviousEvent);
        return eventArgs;
    }

    public void Reset()
    {
        Registers.IFF1 = 0;
        Registers.IFF2 = 0;
        Registers.PC = 0;
        unchecked
        {
            Registers.AF = (short)0xFFFF;
        }

        unchecked
        {
            Registers.SP = (short)0xFFFF;
        }

        InterruptMode = 0;

        NmiInterruptPending = false;
        IsHalted = false;

        TStatesElapsedSinceReset = 0;
        StartOfStack = Registers.SP;
    }

    public int ExecuteNextInstruction()
    {
        return InstructionExecutionLoop(isSingleInstruction: true);
    }



    public ulong TStatesElapsedSinceStart { get; private set; }

    public ulong TStatesElapsedSinceReset { get; private set; }

    public StopReason StopReason { get; private set; }

    public ProcessorState State { get; private set; }

    public object UserState { get; set; }

    public bool IsHalted { get; protected set; }

    private byte _InterruptMode;

    public byte InterruptMode
    {
        get { return _InterruptMode; }
        set
        {
            if (value > 2)
                throw new ArgumentException("Interrupt mode can be set to 0, 1 or 2 only");

            _InterruptMode = value;
        }
    }

    public short StartOfStack { get; protected set; }



    private IZ80Registers _Registers;

    public IZ80Registers Registers
    {
        get { return _Registers; }
        set
        {
            if (value == null)
                throw new ArgumentNullException("Registers");

            _Registers = value;
        }
    }

    private IMemory _Memory;

    public IMemory Memory
    {
        get { return _Memory; }
        set
        {
            if (value == null)
                throw new ArgumentNullException("Memory");

            _Memory = value;
        }
    }

    private MemoryAccessMode[] memoryAccessModes = new MemoryAccessMode[MemorySpaceSize];

    public void SetMemoryAccessMode(ushort startAddress, int length, MemoryAccessMode mode)
    {
        SetArrayContents(memoryAccessModes, startAddress, length, mode);
    }

    private void SetArrayContents<T>(T[] array, ushort startIndex, int length, T value)
    {
        if (length < 0)
            throw new ArgumentException("length can't be negative");
        if (startIndex + length > array.Length)
            throw new ArgumentException("start + length go beyond " + (array.Length - 1));

        var data = Enumerable.Repeat(value, length).ToArray();
        Array.Copy(data, 0, array, startIndex, length);
    }

    public MemoryAccessMode GetMemoryAccessMode(ushort address)
    {
        return memoryAccessModes[address];
    }

    private IMemory _PortsSpace;

    public IMemory PortsSpace
    {
        get { return _PortsSpace; }
        set
        {
            if (value == null)
                throw new ArgumentNullException("PortsSpace");

            _PortsSpace = value;
        }
    }

    private MemoryAccessMode[] portsAccessModes = new MemoryAccessMode[PortSpaceSize];

    public void SetPortsSpaceAccessMode(byte startPort, int length, MemoryAccessMode mode)
    {
        SetArrayContents(portsAccessModes, startPort, length, mode);
    }

    public MemoryAccessMode GetPortAccessMode(ushort portNumber)
    {
        return portsAccessModes[portNumber];
    }

    private IList<IZ80InterruptSource> InterruptSources { get; set; }

    public void RegisterInterruptSource(IZ80InterruptSource source)
    {
        if (InterruptSources.Contains(source))
            return;

        InterruptSources.Add(source);
    }

    private readonly object nmiInterruptPendingSync = new object();
    private bool _nmiInterruptPending;

    private bool NmiInterruptPending
    {
        get
        {
            lock (nmiInterruptPendingSync)
            {
                var value = _nmiInterruptPending;
                _nmiInterruptPending = false;
                return value;
            }
        }
        set
        {
            lock (nmiInterruptPendingSync)
            {
                _nmiInterruptPending = value;
            }
        }
    }

    public IEnumerable<IZ80InterruptSource> GetRegisteredInterruptSources()
    {
        return InterruptSources.ToArray();
    }

    public void UnregisterAllInterruptSources()
    {

        InterruptSources.Clear();
    }



    private decimal effectiveClockFrequency;

    private decimal _ClockFrequencyInMHz = 1;

    public decimal ClockFrequencyInMHz
    {
        get { return _ClockFrequencyInMHz; }
        set
        {
            SetEffectiveClockFrequency(value, ClockSpeedFactor);
            _ClockFrequencyInMHz = value;
        }
    }

    private void SetEffectiveClockFrequency(decimal clockFrequency, decimal clockSpeedFactor)
    {
        decimal effectiveClockFrequency = clockFrequency * clockSpeedFactor;
        if ((effectiveClockFrequency > MaxEffectiveClockSpeed) ||
            (effectiveClockFrequency < MinEffectiveClockSpeed))
            throw new ArgumentException(string.Format(
                "Clock frequency multiplied by clock speed factor must be a number between {0} and {1}",
                MinEffectiveClockSpeed, MaxEffectiveClockSpeed));

        this.effectiveClockFrequency = effectiveClockFrequency;
        if (clockSynchronizer != null)
            clockSynchronizer.EffectiveClockFrequencyInMHz = effectiveClockFrequency;
    }

    private decimal _ClockSpeedFactor = 1;

    public decimal ClockSpeedFactor
    {
        get { return _ClockSpeedFactor; }
        set
        {
            SetEffectiveClockFrequency(ClockFrequencyInMHz, value);
            _ClockSpeedFactor = value;
        }
    }

    public bool AutoStopOnDiPlusHalt { get; set; }

    public bool AutoStopOnRetWithStackEmpty { get; set; }

    private byte[] memoryWaitStatesForM1 = new byte[MemorySpaceSize];

    public void SetMemoryWaitStatesForM1(ushort startAddress, int length, byte waitStates)
    {
        SetArrayContents(memoryWaitStatesForM1, startAddress, length, waitStates);
    }

    public byte GetMemoryWaitStatesForM1(ushort address)
    {
        return memoryWaitStatesForM1[address];
    }

    private byte[] memoryWaitStatesForNonM1 = new byte[MemorySpaceSize];

    public void SetMemoryWaitStatesForNonM1(ushort startAddress, int length, byte waitStates)
    {
        SetArrayContents(memoryWaitStatesForNonM1, startAddress, length, waitStates);
    }

    public byte GetMemoryWaitStatesForNonM1(ushort address)
    {
        return memoryWaitStatesForNonM1[address];
    }

    private byte[] portWaitStates = new byte[PortSpaceSize];

    public void SetPortWaitStates(ushort startPort, int length, byte waitStates)
    {
        SetArrayContents(portWaitStates, startPort, length, waitStates);
    }

    public byte GetPortWaitStates(ushort portNumber)
    {
        return portWaitStates[portNumber];
    }

    private IZ80InstructionExecutor _InstructionExecutor;

    public IZ80InstructionExecutor InstructionExecutor
    {
        get { return _InstructionExecutor; }
        set
        {
            if (value == null)
                throw new ArgumentNullException("InstructionExecutor");


            _InstructionExecutor = value;
            _InstructionExecutor.ProcessorAgent = this;
        }
    }

    private IClockSynchronizer clockSynchronizer;

    public IClockSynchronizer ClockSynchronizer
    {
        get { return clockSynchronizer; }
        set
        {
            clockSynchronizer = value;
            if (value == null)
                return;

            clockSynchronizer.EffectiveClockFrequencyInMHz = effectiveClockFrequency;
        }
    }









    public byte FetchNextOpcode()
    {
        FailIfNoExecutionContext();

        if (executionContext.FetchComplete)
            throw new InvalidOperationException(
                "FetchNextOpcode can be invoked only before the InstructionFetchFinished event has been raised.");

        byte opcode;
        if (executionContext.PeekedOpcode == null)
        {
            var address = Registers.PC;
            opcode = ReadFromMemoryOrPort(
                address,
                Memory,
                GetMemoryAccessMode(address),
                MemoryAccessEventType.BeforeMemoryRead,
                MemoryAccessEventType.AfterMemoryRead,
                GetMemoryWaitStatesForM1(address));
        }
        else
        {
            executionContext.AccummulatedMemoryWaitStates +=
                GetMemoryWaitStatesForM1(executionContext.AddressOfPeekedOpcode);
            opcode = executionContext.PeekedOpcode.Value;
            executionContext.PeekedOpcode = null;
        }

        executionContext.OpcodeBytes.Add(opcode);
        Registers.PC++;
        return opcode;
    }

    public byte PeekNextOpcode()
    {
        FailIfNoExecutionContext();

        if (executionContext.FetchComplete)
            throw new InvalidOperationException(
                "PeekNextOpcode can be invoked only before the InstructionFetchFinished event has been raised.");

        if (executionContext.PeekedOpcode == null)
        {
            var address = Registers.PC;
            var opcode = ReadFromMemoryOrPort(
                address,
                Memory,
                GetMemoryAccessMode(address),
                MemoryAccessEventType.BeforeMemoryRead,
                MemoryAccessEventType.AfterMemoryRead,
                waitStates: 0);

            executionContext.PeekedOpcode = opcode;
            executionContext.AddressOfPeekedOpcode = Registers.PC;
            return opcode;
        }
        else
        {
            return executionContext.PeekedOpcode.Value;
        }
    }

    private void FailIfNoExecutionContext()
    {
        if (executionContext == null)
            throw new InvalidOperationException(
                "This method can be invoked only when an instruction is being executed.");
    }

    public byte ReadFromMemory(ushort address)
    {
        FailIfNoExecutionContext();
        FailIfNoInstructionFetchComplete();

        return ReadFromMemoryInternal(address);
    }

    private byte ReadFromMemoryInternal(ushort address)
    {
        return ReadFromMemoryOrPort(
            address,
            Memory,
            GetMemoryAccessMode(address),
            MemoryAccessEventType.BeforeMemoryRead,
            MemoryAccessEventType.AfterMemoryRead,
            GetMemoryWaitStatesForNonM1(address));
    }

    protected virtual void FailIfNoInstructionFetchComplete()
    {
        if (executionContext != null && !executionContext.FetchComplete)
            throw new Exception(
                "IZ80ProcessorAgent members other than FetchNextOpcode can be invoked only after the InstructionFetchFinished event has been raised.");
    }

    private byte ReadFromMemoryOrPort(
        ushort address,
        IMemory memory,
        MemoryAccessMode accessMode,
        MemoryAccessEventType beforeEventType,
        MemoryAccessEventType afterEventType,
        byte waitStates)
    {
        var beforeEventArgs = FireMemoryAccessEvent(beforeEventType, address, 0xFF);

        byte value;
        if (!beforeEventArgs.CancelMemoryAccess &&
            (accessMode == MemoryAccessMode.ReadAndWrite || accessMode == MemoryAccessMode.ReadOnly))
            value = memory[address];
        else
            value = beforeEventArgs.Value;

        if (executionContext != null)
            executionContext.AccummulatedMemoryWaitStates += waitStates;

        var afterEventArgs = FireMemoryAccessEvent(
            afterEventType,
            address,
            value,
            beforeEventArgs.LocalUserState,
            beforeEventArgs.CancelMemoryAccess);
        return afterEventArgs.Value;
    }

    MemoryAccessEventArgs FireMemoryAccessEvent(
        MemoryAccessEventType eventType,
        ushort address,
        byte value,
        object localUserState = null,
        bool cancelMemoryAccess = false)
    {
        var eventArgs =
            new MemoryAccessEventArgs(eventType, address, value, localUserState, cancelMemoryAccess);
        return eventArgs;
    }

    public void WriteToMemory(ushort address, byte value)
    {
        FailIfNoExecutionContext();
        FailIfNoInstructionFetchComplete();

        WriteToMemoryInternal(address, value);
    }

    private void WriteToMemoryInternal(ushort address, byte value)
    {
        WritetoMemoryOrPort(
            address,
            value,
            Memory,
            GetMemoryAccessMode(address),
            MemoryAccessEventType.BeforeMemoryWrite,
            MemoryAccessEventType.AfterMemoryWrite,
            GetMemoryWaitStatesForNonM1(address));
    }

    private void WritetoMemoryOrPort(
        ushort address,
        byte value,
        IMemory memory,
        MemoryAccessMode accessMode,
        MemoryAccessEventType beforeEventType,
        MemoryAccessEventType afterEventType,
        byte waitStates)
    {
        var beforeEventArgs = FireMemoryAccessEvent(beforeEventType, address, value);

        if (!beforeEventArgs.CancelMemoryAccess &&
            (accessMode == MemoryAccessMode.ReadAndWrite || accessMode == MemoryAccessMode.WriteOnly))
            memory[address] = beforeEventArgs.Value;

        if (executionContext != null)
            executionContext.AccummulatedMemoryWaitStates += waitStates;

        FireMemoryAccessEvent(
            afterEventType,
            address,
            beforeEventArgs.Value,
            beforeEventArgs.LocalUserState,
            beforeEventArgs.CancelMemoryAccess);
    }

    public byte ReadFromPort(ushort portNumber)
    {
        FailIfNoExecutionContext();
        FailIfNoInstructionFetchComplete();

        return ReadFromMemoryOrPort(
            portNumber,
            PortsSpace,
            GetPortAccessMode(portNumber),
            MemoryAccessEventType.BeforePortRead,
            MemoryAccessEventType.AfterPortRead,
            GetPortWaitStates(portNumber));
    }

    public void WriteToPort(ushort portNumber, byte value)
    {
        FailIfNoExecutionContext();
        FailIfNoInstructionFetchComplete();

        WritetoMemoryOrPort(
            portNumber,
            value,
            PortsSpace,
            GetPortAccessMode(portNumber),
            MemoryAccessEventType.BeforePortWrite,
            MemoryAccessEventType.AfterPortWrite,
            GetPortWaitStates(portNumber));
    }

    public void SetInterruptMode(byte interruptMode)
    {
        FailIfNoExecutionContext();
        FailIfNoInstructionFetchComplete();

        this.InterruptMode = interruptMode;
    }

    public void Stop(bool isPause = false)
    {
        FailIfNoExecutionContext();

        if (!executionContext.ExecutingBeforeInstructionEvent)
            FailIfNoInstructionFetchComplete();

        executionContext.StopReason =
            isPause ? StopReason.PauseInvoked : StopReason.StopInvoked;
    }

    IZ80Registers IZ80ProcessorAgent.RG
    {
        get { return _Registers; }
    }



    protected InstructionExecutionContext executionContext;

}

public class Z80Registers : MainZ80Registers, IZ80Registers
{
    public Z80Registers()
    {
        Alternate = new MainZ80Registers();
    }

    private IMainZ80Registers _Alternate;

    public IMainZ80Registers Alternate
    {
        get { return _Alternate; }
        set
        {
            if (value == null)
                throw new ArgumentNullException("Alternate");

            _Alternate = value;
        }
    }

    public short IX { get; set; }

    public short IY { get; set; }

    public ushort PC { get; set; }

    public short SP { get; set; }

    public short IR { get; set; }

    public Bit IFF1 { get; set; }

    public Bit IFF2 { get; set; }

    public byte IXH
    {
        get { return Z80InstructionExecutor.GetHighByte(IX); }
        set { IX = Z80InstructionExecutor.SetHighByte(IX, value); }
    }

    public byte IXL
    {
        get { return Z80InstructionExecutor.GetLowByte(IX); }
        set { IX = Z80InstructionExecutor.SetLowByte(IX, value); }
    }

    public byte IYH
    {
        get { return Z80InstructionExecutor.GetHighByte(IY); }
        set { IY = Z80InstructionExecutor.SetHighByte(IY, value); }
    }

    public byte IYL
    {
        get { return Z80InstructionExecutor.GetLowByte(IY); }
        set { IY = Z80InstructionExecutor.SetLowByte(IY, value); }
    }

    public byte I
    {
        get { return Z80InstructionExecutor.GetHighByte(IR); }
        set { IR = Z80InstructionExecutor.SetHighByte(IR, value); }
    }

    public byte R
    {
        get { return Z80InstructionExecutor.GetLowByte(IR); }
        set { IR = Z80InstructionExecutor.SetLowByte(IR, value); }
    }
}