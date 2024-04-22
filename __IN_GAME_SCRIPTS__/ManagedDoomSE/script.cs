/*
 * R e a d m e
 * -----------
 * 
 * In this file you can include any instructions or other comments you want to have injected onto the 
 * top of your final script. You can safely delete this file if you do not want any such comments.
 */
bool ᓎ=false;String ᓏ="GAME";int ᓃ=49000;ḛ ᓐ=null;ᵼ ᓑ=null;const int ᓒ=250;const int ᓔ=250;List<IMyTerminalBlock>ᓚ=null;
List<IMyTerminalBlock>ᓕ=null;List<IMyTerminalBlock>ᓖ=null;IMyShipController ᓗ=null;int ю=0;int ᓘ=0;int ᓙ=0;int ᓛ=0;enum ᎁ{ᓍ,
ᓌ,ᓋ,ᓊ,ᓉ,ᓈ,ᓇ,ᓆ};ᎁ ᓅ=ᎁ.ᓍ;Program(){Echo("Start");ᓐ=new ḛ(this,ᓎ);ᓑ=new ᵼ(this,ᓐ,true);MyIniParseResult ŀ;MyIni ᓄ=new MyIni(
);if(!ᓄ.TryParse(Me.CustomData,out ŀ))throw new Exception(ŀ.ToString());ᓏ=ᓄ.Get("config","tag").ToString();if(ᓏ!=null){ᓏ=
(ᓏ.Split(';')[0]).Trim().ToUpper();}else{Echo("ERROR: No tag configured\nPlease add [config] for tag=<substring>");return
;}ᓐ.Ḗ("Config: tag="+ᓏ);int ᓃ=ᓄ.Get("config","speed").ToInt32(45000);if(ᓃ>1000&&ᓃ<49000){this.ᓃ=ᓃ;}Echo("Using tag of "+ᓏ
);Echo("Using speed of "+ᓃ);List<IMyTerminalBlock>ᓂ=new List<IMyTerminalBlock>();GridTerminalSystem.GetBlocksOfType(ᓂ,(
IMyTerminalBlock ǃ)=>((ǃ.CustomName!=null)&&(ǃ.CustomName.IndexOf("["+ᓏ+"SEAT]")>=0)&&(ǃ is IMyShipController)));Echo("Found "+ᓂ.Count+
" controllers");if(ᓂ.Count>0){foreach(var ᓓ in ᓂ){ᓐ.Ḗ("- "+ᓓ.CustomName);}if(ᓂ.Count>1){Echo("ERROR: Too many controllers");return;}ᓗ=
(IMyShipController)ᓂ[0];}else if(ᓂ.Count==0){Echo("ERROR: No controllers");return;}ᓚ=ᓑ.ᵻ(ᓏ+"SCREEN");Echo("Found "+ᓚ.
Count+" displays");ᓑ.ᶄ(ᓚ,ᓔ,ᓒ,true,0.001F,0.001F);ᓑ.ᶅ(ᓚ,TextAlignment.LEFT);ᓕ=ᓑ.ᵻ(ᓏ+"FPS");ᓑ.ᶅ(ᓕ,TextAlignment.CENTER);ᓑ.ᶄ(ᓕ,1
,2,false,0.25F,0.25F);Echo("Found "+ᓕ.Count+" fpsdisplays");ᓖ=ᓑ.ᵻ(ᓏ+"NAME");ᓑ.ᶅ(ᓖ,TextAlignment.CENTER);ᓑ.ᶄ(ᓖ,1,8,false,
0.25F,0.25F);Echo("Found "+ᓖ.Count+" namedisplays");Runtime.UpdateFrequency=UpdateFrequency.Update1;}void Save(){}void Main(
string ᓤ,UpdateType ᓥ){if(ᓤ==null){ᓐ.ḗ("Launched with empty parms"+ᓤ);}else{ᓐ.ḗ("Launched with parms '"+ᓤ+"'");}String ᓦ=null;
if(ᓅ==ᎁ.ᓍ){String[]ᓧ={""};}if(ᓦ!=null){Echo(ᓦ);ᓑ.ᵛ(ᓚ,ᓦ,false);Runtime.UpdateFrequency=UpdateFrequency.None;return;}int ᓨ=
DateTime.UtcNow.Second;if(ᓨ!=ᓘ){ᓛ=ᓙ;ᓑ.ᵛ(ᓕ,""+ᓛ,false);ᓘ=ᓨ;ᓙ=0;}ᓐ.ḗ("Frame "+ю+++", fps: "+ᓛ);}public enum ᓩ{З,ᓪ,ظ,પ,ŏ,ᓫ}struct ɡ
{public static ɡ ᓬ=new ɡ(0x00000000);public static ɡ ᓭ=new ɡ(0x20000000);public static ɡ ᓮ=new ɡ(0x40000000);public
static ɡ ᓣ=new ɡ(0x80000000);public static ɡ ᓢ=new ɡ(0xC0000000);private uint f;public ɡ(uint f){this.f=f;}public ɡ(int f){
this.f=(uint)f;}public static ɡ ᓡ(double ᓠ){var f=Math.Round(0x100000000*(ᓠ/(2*Math.PI)));return new ɡ((uint)f);}public
static ɡ ᓟ(double ᓞ){var f=Math.Round(0x100000000*(ᓞ/360));return new ɡ((uint)f);}public double ᓝ(){return 2*Math.PI*((double)
f/0x100000000);}public double ᓜ(){return 360*((double)f/0x100000000);}public static ɡ Ꭼ(ɡ Ş){var f=(int)Ş.f;if(f<0){
return new ɡ((uint)-f);}else{return Ş;}}public static ɡ operator+(ɡ Ã){return Ã;}public static ɡ operator-(ɡ Ã){return new ɡ((
uint)-(int)Ã.f);}public static ɡ operator+(ɡ Ã,ɡ Â){return new ɡ(Ã.f+Â.f);}public static ɡ operator-(ɡ Ã,ɡ Â){return new ɡ(Ã
.f-Â.f);}public static ɡ operator*(uint Ã,ɡ Â){return new ɡ(Ã*Â.f);}public static ɡ operator*(ɡ Ã,uint Â){return new ɡ(Ã.
f*Â);}public static ɡ operator/(ɡ Ã,uint Â){return new ɡ(Ã.f/Â);}public static bool operator==(ɡ Ã,ɡ Â){return Ã.f==Â.f;}
public static bool operator!=(ɡ Ã,ɡ Â){return Ã.f!=Â.f;}public static bool operator<(ɡ Ã,ɡ Â){return Ã.f<Â.f;}public static
bool operator>(ɡ Ã,ɡ Â){return Ã.f>Â.f;}public static bool operator<=(ɡ Ã,ɡ Â){return Ã.f<=Â.f;}public static bool operator
>=(ɡ Ã,ɡ Â){return Ã.f>=Â.f;}public override bool Equals(object Ꮂ){throw new NotSupportedException();}public override int
GetHashCode(){return f.GetHashCode();}public override string ToString(){return ᓜ().ToString();}public uint Ꮁ{get{return f;}}}sealed
class ᒳ{private ᕅ ᒲ;private int Ć;private ᔞ ˌ;private int ᒱ;private int ᒰ;private int ᒯ;private int ᒴ;private int f;private
string[]ഉ;private int ᒼ;private int ᒽ;public ᒳ(ᕅ Ȏ,ᒶ ݤ,int Ć){ᒲ=Ȏ;this.Ć=Ć;ˌ=ݤ.ܫ;ᒱ=ݤ.ቊ;ᒰ=ݤ.ŏ;ᒯ=ݤ.ޚ;ᒴ=ݤ.ޙ;f=ݤ.Ꮁ;ഉ=new string[ᒰ]
;for(var Ä=0;Ä<ᒰ;Ä++){if(ᒲ.ᘿ.ᰔ!=1||Ć!=8){ഉ[Ä]="WIA"+ᒲ.ᘿ.ᰔ+Ć.ToString("00")+Ä.ToString("00");}else{ഉ[Ä]="WIA104"+Ä.
ToString("00");}}}public void ʕ(int ᒾ){ᒼ=-1;if(ˌ==ᔞ.ᔝ){ᒽ=ᒾ+1+(ᒲ.ષ.ѐ()%ᒱ);}else if(ˌ==ᔞ.ષ){ᒽ=ᒾ+1+(ᒲ.ષ.ѐ()%f);}else if(ˌ==ᔞ.ᔜ){ᒽ=ᒾ
+1;}}public void ڞ(int ᒾ){if(ᒾ==ᒽ){switch(ˌ){case ᔞ.ᔝ:if(++ᒼ>=ᒰ){ᒼ=0;}ᒽ=ᒾ+ᒱ;break;case ᔞ.ષ:ᒼ++;if(ᒼ==ᒰ){ᒼ=-1;ᒽ=ᒾ+(ᒲ.ષ.ѐ()
%f);}else{ᒽ=ᒾ+ᒱ;}break;case ᔞ.ᔜ:if(!(ᒲ.Ŷ==ᶴ.ᶭ&&Ć==7)&&ᒲ.ᘿ.ᰇ==Ꮁ){ᒼ++;if(ᒼ==ᒰ){ᒼ--;}ᒽ=ᒾ+ᒱ;}break;}}}public int ᒿ=>ᒯ;public
int ᓀ=>ᒴ;public int Ꮁ=>f;public IReadOnlyList<string>ജ=>ഉ;public int ᓁ=>ᒼ;}sealed class ᒻ{private bool ᅥ;private string ᒺ;
private string ᒹ;private int Ζ;public ᒻ(bool ᅥ,string ᒺ,string ᒹ,int Ζ){this.ᅥ=ᅥ;this.ᒺ=ᒺ;this.ᒹ=ᒹ;this.Ζ=Ζ;}public bool ᅬ=>ᅥ;
public string ᒸ=>ᒺ;public string ᒷ=>ᒹ;public int ݏ=>Ζ;}sealed class ᒶ{private ᔞ ˌ;private int ᒱ;private int ú;private int ǃ;
private int ǂ;private int f;public ᒶ(ᔞ ˌ,int ᒱ,int ú,int ǃ,int ǂ){this.ˌ=ˌ;this.ᒱ=ᒱ;this.ú=ú;this.ǃ=ǃ;this.ǂ=ǂ;}public ᒶ(ᔞ ˌ,
int ᒱ,int ú,int ǃ,int ǂ,int f){this.ˌ=ˌ;this.ᒱ=ᒱ;this.ú=ú;this.ǃ=ǃ;this.ǂ=ǂ;this.f=f;}public ᔞ ܫ=>ˌ;public int ቊ=>ᒱ;public
int ŏ=>ú;public int ޚ=>ǃ;public int ޙ=>ǂ;public int Ꮁ=>f;public static IReadOnlyList<IReadOnlyList<ᒶ>>ᔟ=new ᒶ[][]{new ᒶ[]{
new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,224,104),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,184,160),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,112,136),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,72,112),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,
3,88,96),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,64,48),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,192,40),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,136,16),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,80,16),new ᒶ(ᔞ.ᔝ,
ᱺ.ᱹ/3,3,64,24)},new ᒶ[]{new ᒶ(ᔞ.ᔜ,ᱺ.ᱹ/3,1,128,136,1),new ᒶ(ᔞ.ᔜ,ᱺ.ᱹ/3,1,128,136,2),new ᒶ(ᔞ.ᔜ,ᱺ.ᱹ/3,1,128,136,3),new ᒶ(ᔞ.ᔜ,
ᱺ.ᱹ/3,1,128,136,4),new ᒶ(ᔞ.ᔜ,ᱺ.ᱹ/3,1,128,136,5),new ᒶ(ᔞ.ᔜ,ᱺ.ᱹ/3,1,128,136,6),new ᒶ(ᔞ.ᔜ,ᱺ.ᱹ/3,1,128,136,7),new ᒶ(ᔞ.ᔜ,ᱺ.ᱹ/3
,3,192,144,8),new ᒶ(ᔞ.ᔜ,ᱺ.ᱹ/3,1,128,136,8)},new ᒶ[]{new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,104,168),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,40,136),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,
3,160,96),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,104,80),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/3,3,120,32),new ᒶ(ᔞ.ᔝ,ᱺ.ᱹ/4,3,40,0)}};}public enum ᔞ{ᔝ,ષ,ᔜ}sealed class
ଏ{private ଦ l;private Ꮖ ᔎ;private Ꮖ ᔖ;private Ꮖ ᔑ;private Ꮖ ᔒ;private Ꮖ न;private Ꮖ ऩ;private bool ᔯ;private ᑟ Í;private
Ꮖ ᔕ;private bool ᔬ;private bool ᔭ;private bool ᔮ;private bool ල;private bool ර;private bool ᔰ;private bool ᔴ;private List
<ட>ᔱ;private int ᔲ;public ଏ(ଦ l){this.l=l;ᔎ=Ꮖ.Ꮑ;ᔖ=Ꮖ.Ꮐ;ᔑ=Ꮖ.Ꮑ;ᔒ=Ꮖ.Ꮐ;foreach(var ᔳ in l.શ.ᛨ){if(ᔳ.ޚ<ᔎ){ᔎ=ᔳ.ޚ;}if(ᔳ.ޚ>ᔖ){ᔖ=ᔳ.
ޚ;}if(ᔳ.ޙ<ᔑ){ᔑ=ᔳ.ޙ;}if(ᔳ.ޙ>ᔒ){ᔒ=ᔳ.ޙ;}}न=ᔎ+(ᔖ-ᔎ)/2;ऩ=ᔑ+(ᔒ-ᔑ)/2;ᔯ=false;Í=ᑟ.ந;ᔕ=Ꮖ.Ꮒ;ᔬ=true;ᔭ=false;ᔮ=false;ල=false;ර=false;
ᔰ=false;ᔴ=false;ᔱ=new List<ட>();ᔲ=0;}public void ڞ(){if(ᔭ){ᔕ+=ᔕ/16;}if(ᔮ){ᔕ-=ᔕ/16;}if(ᔕ<Ꮖ.Ꮒ/2){ᔕ=Ꮖ.Ꮒ/2;}else if(ᔕ>Ꮖ.Ꮒ*32)
{ᔕ=Ꮖ.Ꮒ*32;}if(ල){न-=64/ᔕ;}if(ර){न+=64/ᔕ;}if(ᔰ){ऩ+=64/ᔕ;}if(ᔴ){ऩ-=64/ᔕ;}if(न<ᔎ){न=ᔎ;}else if(न>ᔖ){न=ᔖ;}if(ऩ<ᔑ){ऩ=ᔑ;}else
if(ऩ>ᔒ){ऩ=ᔒ;}if(ᔬ){var Ð=l.ଙ.Ɠ;न=Ð.ޚ;ऩ=Ð.ޙ;}}public bool ଅ(ᕨ Ň){if(Ň.ᕤ==ኈ.ᄩ||Ň.ᕤ==ኈ.ቋ||Ň.ᕤ==ኈ.ቒ){if(Ň.ܫ==ፎ.ፏ){ᔭ=true;}else
if(Ň.ܫ==ፎ.ፐ){ᔭ=false;}return true;}else if(Ň.ᕤ==ኈ.ቃ||Ň.ᕤ==ኈ.ቛ||Ň.ᕤ==ኈ.ሹ){if(Ň.ܫ==ፎ.ፏ){ᔮ=true;}else if(Ň.ܫ==ፎ.ፐ){ᔮ=false;}
return true;}else if(Ň.ᕤ==ኈ.ቀ){if(Ň.ܫ==ፎ.ፏ){ල=true;}else if(Ň.ܫ==ፎ.ፐ){ල=false;}return true;}else if(Ň.ᕤ==ኈ.ሿ){if(Ň.ܫ==ፎ.ፏ){ර=
true;}else if(Ň.ܫ==ፎ.ፐ){ර=false;}return true;}else if(Ň.ᕤ==ኈ.ĳ){if(Ň.ܫ==ፎ.ፏ){ᔰ=true;}else if(Ň.ܫ==ፎ.ፐ){ᔰ=false;}return true;
}else if(Ň.ᕤ==ኈ.Ķ){if(Ň.ܫ==ፎ.ፏ){ᔴ=true;}else if(Ň.ܫ==ፎ.ፐ){ᔴ=false;}return true;}else if(Ň.ᕤ==ኈ.ኁ){if(Ň.ܫ==ፎ.ፏ){ᔬ=!ᔬ;if(ᔬ)
{l.ଙ.ۍ(ኑ.ጓ.ᆽ);}else{l.ଙ.ۍ(ኑ.ጓ.ᆼ);}return true;}}else if(Ň.ᕤ==ኈ.ኰ){if(Ň.ܫ==ፎ.ፏ){if(ᔱ.Count<10){ᔱ.Add(new ட(न,ऩ));}else{ᔱ[ᔲ
]=new ட(न,ऩ);}ᔲ++;if(ᔲ==10){ᔲ=0;}l.ଙ.ۍ(ኑ.ጓ.ᆹ);return true;}}else if(Ň.ᕤ==ኈ.ኄ){if(Ň.ܫ==ፎ.ፏ){ᔱ.Clear();ᔲ=0;l.ଙ.ۍ(ኑ.ጓ.ᆸ);
return true;}}return false;}public void ರ(){ᔯ=true;}public void ಯ(){ᔯ=false;ᔭ=false;ᔮ=false;ල=false;ර=false;ᔰ=false;ᔴ=false;}
public void ᔫ(){Í++;if((int)Í==3){Í=ᑟ.ந;}}public Ꮖ ᔪ=>ᔎ;public Ꮖ ᔩ=>ᔖ;public Ꮖ ᔨ=>ᔑ;public Ꮖ ᔧ=>ᔒ;public Ꮖ ᔦ=>न;public Ꮖ ᔥ=>ऩ;
public Ꮖ ᔤ=>ᔕ;public bool ᔣ=>ᔬ;public bool ᔢ=>ᔯ;public ᑟ Ŷ=>Í;public IReadOnlyList<ட>ᔡ=>ᔱ;}sealed class ᔠ{private static float
ᔛ=8*ኑ.ዃ[(int)ё.ђ].ᙊ.Ꭽ()/7;private static float[]ᔚ=new float[]{-ᔛ+ᔛ/8,0,ᔛ,0,ᔛ,0,ᔛ-ᔛ/2,ᔛ/4,ᔛ,0,ᔛ-ᔛ/2,-ᔛ/4,-ᔛ+ᔛ/8,0,-ᔛ-ᔛ/8,ᔛ
/4,-ᔛ+ᔛ/8,0,-ᔛ-ᔛ/8,-ᔛ/4,-ᔛ+3*ᔛ/8,0,-ᔛ+ᔛ/8,ᔛ/4,-ᔛ+3*ᔛ/8,0,-ᔛ+ᔛ/8,-ᔛ/4};private static float ᓯ=16;private static float[]ᓾ=
new float[]{-0.5F*ᓯ,-0.7F*ᓯ,ᓯ,0F,ᓯ,0F,-0.5F*ᓯ,0.7F*ᓯ,-0.5F*ᓯ,0.7F*ᓯ,-0.5F*ᓯ,-0.7F*ᓯ};private static int ᓿ=(256-5*16);
private static int ᔀ=16;private static int ᔁ=(7*16);private static int ᔂ=16;private static int ᔃ=(6*16);private static int ᔅ=16
;private static int ᔌ=(4*16);private static int ᔆ=16;private static int ᔇ=(256-32+7);private static int ᔈ=1;private
static int ᔉ=0;private static int ᔊ=(256-47);private static int ᔋ=ᔉ;private static int ᔍ=ᓿ;private static int ᓽ=ᔀ;private
static int ᓼ=ᔃ;private static int ᓻ=ᔅ;private static int ᓺ=ᔌ;private static int ᓹ=ᔆ;private static int ᓸ=ᔇ;private static int
ᓷ=ᔈ;private static int ᓶ=ᔁ;private static int ᓵ=ᔂ;private static int ᓴ=ᔍ;private static int ᓳ=ᓽ;private static int[]ᓲ=new
int[]{ᔁ,ᔃ,ᔌ,ᓿ};private Ꮝ ȋ;private int ǉ;private int ᓱ;private int ᓰ;private float ᔄ;private float ᔎ;private float ᔖ;
private float ځ;private float ᔑ;private float ᔒ;private float ˢ;private float ᔓ;private float ᔔ;private float ᔕ;private float ᔗ
;private float ᔙ;private ڦ[]ᔘ;public ᔠ(ಆ þ,Ꮝ ȋ){this.ȋ=ȋ;ǉ=ȋ.Ǒ/320;ᓱ=ȋ.Ǒ;ᓰ=ȋ.ǡ-ǉ*ള.ǡ;ᔄ=(float)ǉ/16;ᔘ=new ڦ[10];for(var Ä=
0;Ä<ᔘ.Length;Ä++){ᔘ[Ä]=ڦ.ÿ(þ,"AMMNUM"+Ä);}}public void Ǌ(ђ Ð){ȋ.অ(0,0,ᓱ,ᓰ,ᔋ);var l=Ð.Ɠ.ଦ;var ᆁ=l.ଏ;ᔎ=ᆁ.ᔪ.Ꭽ();ᔖ=ᆁ.ᔩ.Ꭽ();ځ=
ᔖ-ᔎ;ᔑ=ᆁ.ᔨ.Ꭽ();ᔒ=ᆁ.ᔧ.Ꭽ();ˢ=ᔒ-ᔑ;ᔓ=ᆁ.ᔦ.Ꭽ();ᔔ=ᆁ.ᔥ.Ꭽ();ᔕ=ᆁ.ᔤ.Ꭽ();ᔗ=(float)Math.Round(ᔕ*ᔄ*ᔓ)/(ᔕ*ᔄ);ᔙ=(float)Math.Round(ᔕ*ᔄ*ᔔ)/(
ᔕ*ᔄ);foreach(var ò in l.શ.đ){var ܡ=ᑣ(ò.ɝ);var ھ=ᑣ(ò.ɞ);var ᔐ=ᆁ.Ŷ!=ᑟ.ந;if(ᔐ||(ò.ᘾ&ᶘ.ᶎ)!=0){if((ò.ᘾ&ᶘ.ᶏ)!=0&&!ᔐ){continue;}
if(ò.ɤ==null){ȋ.ᐟ(ܡ.ޚ,ܡ.ޙ,ھ.ޚ,ھ.ޙ,ᔍ);}else{if(ò.Ě==(ᶍ)39){ȋ.ᐟ(ܡ.ޚ,ܡ.ޙ,ھ.ޚ,ھ.ޙ,ᔍ+ᓽ/2);}else if((ò.ᘾ&ᶘ.ᶑ)!=0){if(ᔐ){ȋ.ᐟ(ܡ.ޚ,
ܡ.ޙ,ھ.ޚ,ھ.ޙ,ᓴ);}else{ȋ.ᐟ(ܡ.ޚ,ܡ.ޙ,ھ.ޚ,ھ.ޙ,ᔍ);}}else if(ò.ɤ.Ģ!=ò.ɣ.Ģ){ȋ.ᐟ(ܡ.ޚ,ܡ.ޙ,ھ.ޚ,ھ.ޙ,ᓺ);}else if(ò.ɤ.ģ!=ò.ɣ.ģ){ȋ.ᐟ(ܡ.ޚ
,ܡ.ޙ,ھ.ޚ,ھ.ޙ,ᓸ);}else if(ᔐ){ȋ.ᐟ(ܡ.ޚ,ܡ.ޙ,ھ.ޚ,ھ.ޙ,ᓼ);}}}else if(Ð.Ɲ[(int)Ů.ũ]>0){if((ò.ᘾ&ᶘ.ᶏ)==0){ȋ.ᐟ(ܡ.ޚ,ܡ.ޙ,ھ.ޚ,ھ.ޙ,ᔃ+3);
}}}for(var Ä=0;Ä<ᆁ.ᔡ.Count;Ä++){var ࠆ=ᑣ(ᆁ.ᔡ[Ä]);ȋ.Ꮘ(ᔘ[Ä],(int)Math.Round(ࠆ.ޚ),(int)Math.Round(ࠆ.ޙ),ǉ);}if(ᆁ.Ŷ==ᑟ.ᑥ){ᒵ(l);
}ᔏ(l);if(!ᆁ.ᔣ){ȋ.ᐟ(ᓱ/2-2*ǉ,ᓰ/2,ᓱ/2+2*ǉ,ᓰ/2,ᔃ);ȋ.ᐟ(ᓱ/2,ᓰ/2-2*ǉ,ᓱ/2,ᓰ/2+2*ǉ,ᔃ);}ȋ.ᐭ(l.શ.ښ,0,ᓰ-ǉ,ǉ);}private void ᔏ(ଦ l){var
Å=l.હ;var Ë=Å.ᰁ;var Ƹ=l.ଙ;var ᆁ=l.ଏ;if(!Å.ᴶ){ᑤ(Ƹ.Ɠ,ᔚ,ᔊ);return;}for(var Ä=0;Ä<ђ.ۇ;Ä++){var Ð=Ë[Ä];if(Å.ᴵ!=0&&Ð!=Ƹ){
continue;}if(!Ð.Ÿ){continue;}int ᐚ;if(Ð.Ɲ[(int)Ů.ū]>0){ᐚ=246;}else{ᐚ=ᓲ[Ä];}ᑤ(Ð.Ɠ,ᔚ,ᐚ);}}private void ᒵ(ଦ l){foreach(var â in l.વ
){var ã=â as Ɠ;if(ã!=null){ᑤ(ã,ᓾ,ᔁ);}}}private void ᑤ(Ɠ ã,float[]f,int ᐚ){var ࠆ=ᑣ(ã.ޚ,ã.ޙ);var ᑦ=(float)Math.Sin(ã.ɡ.ᓝ())
;var ჽ=(float)Math.Cos(ã.ɡ.ᓝ());for(var Ä=0;Ä<f.Length;Ä+=4){var ǎ=ࠆ.ޚ+ᔕ*ᔄ*(ჽ*f[Ä+0]-ᑦ*f[Ä+1]);var ǆ=ࠆ.ޙ-ᔕ*ᔄ*(ᑦ*f[Ä+0]+ჽ*
f[Ä+1]);var ǐ=ࠆ.ޚ+ᔕ*ᔄ*(ჽ*f[Ä+2]-ᑦ*f[Ä+3]);var ǅ=ࠆ.ޙ-ᔕ*ᔄ*(ᑦ*f[Ä+2]+ჽ*f[Ä+3]);ȋ.ᐟ(ǎ,ǆ,ǐ,ǅ,ᐚ);}}private ᑠ ᑣ(Ꮖ ǃ,Ꮖ ǂ){var ᑢ=ᔕ
*ᔄ*(ǃ.Ꭽ()-ᔗ)+ᓱ/2;var ᑡ=-ᔕ*ᔄ*(ǂ.Ꭽ()-ᔙ)+ᓰ/2;return new ᑠ(ᑢ,ᑡ);}private ᑠ ᑣ(ட ݫ){var ᑢ=ᔕ*ᔄ*(ݫ.ޚ.Ꭽ()-ᔗ)+ᓱ/2;var ᑡ=-ᔕ*ᔄ*(ݫ.ޙ.Ꭽ
()-ᔙ)+ᓰ/2;return new ᑠ(ᑢ,ᑡ);}private struct ᑠ{public float ޚ;public float ޙ;public ᑠ(float ǃ,float ǂ){ޚ=ǃ;ޙ=ǂ;}}}public
enum ᑟ{ந,ũ,ᑥ}public enum ᑧ{ɳ,ᑹ,ᑺ,ᑻ,ᑼ,ᑽ,ᑾ,ᑿ,ᒀ,ᒇ,ᒁ,ᒂ,ᒃ,ᒄ,ᒅ,ᒆ,ᒈ,ᑸ,ᑷ,ᑶ,ᑵ,ᑴ,ᑳ,ᑲ,ᑱ,ᑰ,ᑯ,ᑮ,ᑭ,ᑬ,ᑫ,ᑪ,ᑩ,ᑨ,ᑞ,ᑝ,ᐴ,ᑅ,ᑆ,ᑇ,ᑈ,ᑉ,ᑊ,ᑌ,ᑓ,ᑍ,ᑎ,ᑏ,ᑐ
,ᑑ,ᑒ,ᑔ,ᑄ,ᑃ,ᑂ,ᑁ,ᑀ,ᐿ,ᐾ,ᐽ,ᐼ,ᐻ,ᐺ,ᐹ,ᐸ,ᐷ,ᐶ,ᐵ}sealed class ᑋ{public static int ᑕ=128;public static Ꮖ ᑛ=Ꮖ.Ꭸ(ᑕ);public static int
ᑘ=ᑛ.Ꮁ-1;public static int ᑙ=Ꮖ.Ꮕ+7;public static int ᑚ=ᑙ-Ꮖ.Ꮕ;private Ꮖ ᅀ;private Ꮖ ᄿ;private int ځ;private int ˢ;private
short[]Ᏹ;private ɢ[]ß;private Ɠ[]ᑜ;private ᑋ(Ꮖ ᅀ,Ꮖ ᄿ,int ځ,int ˢ,short[]Ᏹ,ɢ[]ß){this.ᅀ=ᅀ;this.ᄿ=ᄿ;this.ځ=ځ;this.ˢ=ˢ;this.Ᏹ=Ᏹ;
this.ß=ß;ᑜ=new Ɠ[ځ*ˢ];}public static ᑋ ÿ(ಆ þ,int ý,ɢ[]ß){var f=þ.ಠ(ý);var Ᏹ=new short[f.Length/2];for(var Ä=0;Ä<Ᏹ.Length;Ä++
){var ù=2*Ä;Ᏹ[Ä]=BitConverter.ToInt16(f,ù);}var ᅀ=Ꮖ.Ꭸ(Ᏹ[0]);var ᄿ=Ꮖ.Ꭸ(Ᏹ[1]);var ځ=Ᏹ[2];var ˢ=Ᏹ[3];return new ᑋ(ᅀ,ᄿ,ځ,ˢ,Ᏹ,
ß);}public int ᑗ(Ꮖ ǃ){return(ǃ-ᅀ).Ꮁ>>ᑙ;}public int ᑖ(Ꮖ ǂ){return(ǂ-ᄿ).Ꮁ>>ᑙ;}public int ᒉ(int ᒐ,int ᒜ){if(0<=ᒐ&&ᒐ<ځ&&0<=ᒜ
&&ᒜ<ˢ){return ځ*ᒜ+ᒐ;}else{return-1;}}public int ᒉ(Ꮖ ǃ,Ꮖ ǂ){var ᒐ=ᑗ(ǃ);var ᒜ=ᑖ(ǂ);return ᒉ(ᒐ,ᒜ);}public bool ᒝ(int ᒐ,int ᒜ,
Func<ɢ,bool>ܥ,int Ĺ){var ı=ᒉ(ᒐ,ᒜ);if(ı==-1){return true;}for(var ù=Ᏹ[4+ı];Ᏹ[ù]!=-1;ù++){var ò=ß[Ᏹ[ù]];if(ò.Ĕ==Ĺ){continue;}ò
.Ĕ=Ĺ;if(!ܥ(ò)){return false;}}return true;}public bool ᒟ(int ᒐ,int ᒜ,Func<Ɠ,bool>ܥ){var ı=ᒉ(ᒐ,ᒜ);if(ı==-1){return true;}
for(var ã=ᑜ[ı];ã!=null;ã=ã.ᙆ){if(!ܥ(ã)){return false;}}return true;}public Ꮖ ᅌ=>ᅀ;public Ꮖ ᅍ=>ᄿ;public int Ǒ=>ځ;public int
ǡ=>ˢ;public Ɠ[]ᒛ=>ᑜ;}static class ᒚ{public const int Ꮀ=0;public const int Ꭿ=1;public const int ቀ=2;public const int ሿ=3;
public static void ŷ(Ꮖ[]Ꭾ){Ꭾ[Ꮀ]=Ꭾ[ሿ]=Ꮖ.Ꮐ;Ꭾ[Ꭿ]=Ꭾ[ቀ]=Ꮖ.Ꮑ;}public static void ᒞ(Ꮖ[]Ꭾ,Ꮖ ǃ,Ꮖ ǂ){if(ǃ<Ꭾ[ቀ]){Ꭾ[ቀ]=ǃ;}else if(ǃ>Ꭾ[ሿ]){
Ꭾ[ሿ]=ǃ;}if(ǂ<Ꭾ[Ꭿ]){Ꭾ[Ꭿ]=ǂ;}else if(ǂ>Ꭾ[Ꮀ]){Ꭾ[Ꮀ]=ǂ;}}}sealed class ᒩ{private ɢ ò;private ᒬ ť;private int ϋ;private int ߒ;
private Ɠ ĺ;public void ŷ(){ò=null;ť=0;ϋ=0;ߒ=0;ĺ=null;}public ɢ ᒭ{get{return ò;}set{ò=value;}}public ᒬ ᒪ{get{return ť;}set{ť=
value;}}public int ථ{get{return ϋ;}set{ϋ=value;}}public int ᒫ{get{return ߒ;}set{ߒ=value;}}public Ɠ ĕ{get{return ĺ;}set{ĺ=
value;}}}public enum ᒬ{Ꮀ,ᒮ,Ꭿ}public enum ᒨ{ᒧ,ᒦ,ᒥ,ᒤ,ᒣ,ᒢ,ŏ}sealed class ᒡ:Ⴞ{private ଦ l;private ᒓ ˌ;private Ŀ ô;private Ꮖ ᒠ;
private Ꮖ ಧ;private Ꮖ Ζ;private bool Δ;private int Β;private int Ā;private int ᒊ;public ᒡ(ଦ l){this.l=l;}public override void Ⴛ
(){ί ŀ;var Ü=l.Đ;switch(Β){case 0:break;case 1:ŀ=Ü.Ξ(ô,Ζ,ಧ,false,1,Β);if(((l.ଖ+ô.ġ)&7)==0){switch(ˌ){case ᒓ.ᒋ:break;
default:l.ૐ(ô.ĕ,ɴ.Ȳ,ʡ.ʝ);break;}}if(ŀ==ί.ά){switch(ˌ){case ᒓ.ᒏ:Ü.τ(this);ô.Ĝ();break;case ᒓ.ᒋ:case ᒓ.ᒌ:case ᒓ.ᒍ:if(ˌ==ᒓ.ᒋ){l.ૐ(
ô.ĕ,ɴ.Ȩ,ʡ.ʝ);}Β=-1;break;default:break;}}break;case-1:ŀ=Ü.Ξ(ô,Ζ,ᒠ,Δ,1,Β);if(((l.ଖ+ô.ġ)&7)==0){switch(ˌ){case ᒓ.ᒋ:break;
default:l.ૐ(ô.ĕ,ɴ.Ȳ,ʡ.ʝ);break;}}if(ŀ==ί.ά){switch(ˌ){case ᒓ.ᒋ:case ᒓ.ᒍ:case ᒓ.ᒌ:if(ˌ==ᒓ.ᒋ){l.ૐ(ô.ĕ,ɴ.Ȩ,ʡ.ʝ);Ζ=Đ.ω;}if(ˌ==ᒓ.ᒍ){
Ζ=Đ.ω;}Β=1;break;case ᒓ.ᒎ:case ᒓ.ᒔ:Ü.τ(this);ô.Ĝ();break;default:break;}}else{if(ŀ==ί.έ){switch(ˌ){case ᒓ.ᒋ:case ᒓ.ᒍ:case
ᒓ.ᒎ:Ζ=Đ.ω/8;break;default:break;}}}break;}}public ᒓ ܫ{get{return ˌ;}set{ˌ=value;}}public Ŀ Ŀ{get{return ô;}set{ô=value;}}
public Ꮖ ᒑ{get{return ᒠ;}set{ᒠ=value;}}public Ꮖ ತ{get{return ಧ;}set{ಧ=value;}}public Ꮖ ݏ{get{return Ζ;}set{Ζ=value;}}public
bool ܬ{get{return Δ;}set{Δ=value;}}public int ಣ{get{return Β;}set{Β=value;}}public int ę{get{return Ā;}set{Ā=value;}}public
int ᒒ{get{return ᒊ;}set{ᒊ=value;}}}public enum ᒓ{ᒔ,ᒏ,ᒎ,ᒍ,ᒌ,ᒋ}sealed class ଐ{private static ᖺ[]ǯ=new ᖺ[]{new ᖺ("idfa",(அ,ප)
=>அ.ᖬ(),false),new ᖺ("idkfa",(அ,ප)=>அ.ᖭ(),false),new ᖺ("iddqd",(அ,ප)=>அ.ᖱ(),false),new ᖺ("idclip",(அ,ප)=>அ.ᖮ(),false),new
ᖺ("idspispopd",(அ,ප)=>அ.ᖮ(),false),new ᖺ("iddt",(அ,ප)=>அ.ᖯ(),true),new ᖺ("idbehold",(அ,ප)=>அ.ᖰ(),false),new ᖺ("idbehold?"
,(அ,ප)=>அ.ᖲ(ප),false),new ᖺ("idchoppers",(அ,ප)=>அ.ᖥ(),false),new ᖺ("tntem",(அ,ප)=>அ.ᖤ(),false),new ᖺ("killem",(அ,ප)=>அ.ᖤ(
),false),new ᖺ("fhhall",(அ,ප)=>அ.ᖤ(),false),new ᖺ("idclev??",(அ,ප)=>அ.ᖹ(ප),true),new ᖺ("idmus??",(அ,ප)=>அ.ᖻ(ප),false)};
private static int ᒙ=ǯ.Max(ݤ=>ݤ.ᖼ.Length);private ଦ l;private char[]ᒘ;private int É;public ଐ(ଦ l){this.l=l;ᒘ=new char[ᒙ];É=0;}
public bool ଅ(ᕨ Ň){if(Ň.ܫ==ፎ.ፏ){ᒘ[É]=ቨ.ቧ(Ň.ᕤ);É=(É+1)%ᒘ.Length;ᒗ();}return true;}private void ᒗ(){for(var Ä=0;Ä<ǯ.Length;Ä++){
var ᐞ=ǯ[Ä].ᖼ;var ᒖ=É;int ͼ;for(ͼ=0;ͼ<ᐞ.Length;ͼ++){ᒖ--;if(ᒖ==-1){ᒖ=ᒘ.Length-1;}var ම=ᐞ[ᐞ.Length-ͼ-1];if(ᒘ[ᒖ]!=ම&&ම!='?'){
break;}}if(ͼ==ᐞ.Length){var ප=new char[ᐞ.Length];var ཚ=ᐞ.Length;ᒖ=É;for(ͼ=0;ͼ<ᐞ.Length;ͼ++){ཚ--;ᒖ--;if(ᒖ==-1){ᒖ=ᒘ.Length-1;}ප
[ཚ]=ᒘ[ᒖ];}if(l.હ.ᴷ!=ᵅ.ᵇ||ǯ[Ä].ᖸ){ǯ[Ä].ʊ(this,new string(ප));}}}}private void ᒕ(){var Ð=l.ଙ;if(l.હ.ಖ==ಖ.ᴉ){for(var Ä=0;Ä<(
int)ਖ਼.ŏ;Ä++){Ð.ƍ[Ä]=true;}}else{for(var Ä=0;Ä<=(int)ਖ਼.પ;Ä++){Ð.ƍ[Ä]=true;}Ð.ƍ[(int)ਖ਼.ନ]=true;if(l.હ.ಖ!=ಖ.ᴋ){Ð.ƍ[(int)ਖ਼.Ϲ]=
true;Ð.ƍ[(int)ਖ਼.Ϻ]=true;}}Ð.Ɛ=true;for(var Ä=0;Ä<(int)ᓩ.ŏ;Ä++){Ð.Ƌ[Ä]=2*ኑ.ᕸ.Ꮅ[Ä];Ð.ƌ[Ä]=2*ኑ.ᕸ.Ꮅ[Ä];}}private void ᖬ(){ᒕ();
var Ð=l.ଙ;Ð.ƛ=ኑ.ᕺ.ᕯ;Ð.ƚ=ኑ.ᕺ.ᕰ;Ð.ۍ(ኑ.ጓ.ᆲ);}private void ᖭ(){ᒕ();var Ð=l.ଙ;Ð.ƛ=ኑ.ᕺ.ᕭ;Ð.ƚ=ኑ.ᕺ.ᕮ;for(var Ä=0;Ä<(int)ᒨ.ŏ;Ä++){Ð.
Ƒ[Ä]=true;}Ð.ۍ(ኑ.ጓ.ᆳ);}private void ᖱ(){var Ð=l.ଙ;if((Ð.ƈ&ᖶ.ᖱ)!=0){Ð.ƈ&=~ᖶ.ᖱ;Ð.ۍ(ኑ.ጓ.ᆴ);}else{Ð.ƈ|=ᖶ.ᖱ;Ð.ƙ=Math.Max(ኑ.ᕺ.ᕱ
,Ð.ƙ);Ð.Ɠ.ƙ=Ð.ƙ;Ð.ۍ(ኑ.ጓ.ᆵ);}}private void ᖮ(){var Ð=l.ଙ;if((Ð.ƈ&ᖶ.ᖮ)!=0){Ð.ƈ&=~ᖶ.ᖮ;Ð.ۍ(ኑ.ጓ.ᆰ);}else{Ð.ƈ|=ᖶ.ᖮ;Ð.ۍ(ኑ.ጓ.ᆱ);}
}private void ᖯ(){l.ଏ.ᔫ();}private void ᖰ(){var Ð=l.ଙ;Ð.ۍ(ኑ.ጓ.ᆯ);}private void ᖲ(string ප){switch(ප.Last()){case'v':ᖫ();
break;case's':ᖪ();break;case'i':ᖩ();break;case'r':ᖨ();break;case'a':ᖧ();break;case'l':ᖦ();break;}}private void ᖫ(){var Ð=l.ଙ;
if(Ð.Ɲ[(int)Ů.ŭ]>0){Ð.Ɲ[(int)Ů.ŭ]=0;}else{Ð.Ɲ[(int)Ů.ŭ]=ኑ.ጞ.ŭ;}Ð.ۍ(ኑ.ጓ.ᇆ);}private void ᖪ(){var Ð=l.ଙ;if(Ð.Ɲ[(int)Ů.Ŭ]!=0)
{Ð.Ɲ[(int)Ů.Ŭ]=0;}else{Ð.Ɲ[(int)Ů.Ŭ]=1;}Ð.ۍ(ኑ.ጓ.ᇆ);}private void ᖩ(){var Ð=l.ଙ;if(Ð.Ɲ[(int)Ů.ū]>0){Ð.Ɲ[(int)Ů.ū]=0;Ð.Ɠ.ᘾ
&=~ళ.ᘋ;}else{Ð.Ɲ[(int)Ů.ū]=ኑ.ጞ.ū;Ð.Ɠ.ᘾ|=ళ.ᘋ;}Ð.ۍ(ኑ.ጓ.ᇆ);}private void ᖨ(){var Ð=l.ଙ;if(Ð.Ɲ[(int)Ů.Ū]>0){Ð.Ɲ[(int)Ů.Ū]=0;}
else{Ð.Ɲ[(int)Ů.Ū]=ኑ.ጞ.Ū;}Ð.ۍ(ኑ.ጓ.ᇆ);}private void ᖧ(){var Ð=l.ଙ;if(Ð.Ɲ[(int)Ů.ũ]!=0){Ð.Ɲ[(int)Ů.ũ]=0;}else{Ð.Ɲ[(int)Ů.ũ]=1;
}Ð.ۍ(ኑ.ጓ.ᇆ);}private void ᖦ(){var Ð=l.ଙ;if(Ð.Ɲ[(int)Ů.Ũ]>0){Ð.Ɲ[(int)Ů.Ũ]=0;}else{Ð.Ɲ[(int)Ů.Ũ]=ኑ.ጞ.Ũ;}Ð.ۍ(ኑ.ጓ.ᇆ);}
private void ᖥ(){var Ð=l.ଙ;Ð.ƍ[(int)ਖ਼.ନ]=true;Ð.ۍ(ኑ.ጓ.ᇐ);}private void ᖤ(){var Ð=l.ଙ;var ú=0;foreach(var â in l.વ){var ã=â as Ɠ
;if(ã!=null&&ã.ђ==null&&((ã.ᘾ&ళ.ᘇ)!=0||ã.ܫ==ё.м)&&ã.ƙ>0){l.ર.ᅹ(ã,null,Ð.Ɠ,10000);ú++;}}Ð.ۍ(ú+" monsters killed");}private
void ᖹ(string ප){if(l.હ.ಖ==ಖ.ᴉ){int ࠎ;if(!int.TryParse(ප.Substring(ප.Length-2,2),out ࠎ)){return;}var ᔽ=l.હ.ᴷ;l.સ.ᔼ(ᔽ,1,ࠎ);}
else{int ᔾ;if(!int.TryParse(ප.Substring(ප.Length-2,1),out ᔾ)){return;}int ࠎ;if(!int.TryParse(ප.Substring(ප.Length-1,1),out ࠎ
)){return;}var ᔽ=l.હ.ᴷ;l.સ.ᔼ(ᔽ,ᔾ,ࠎ);}}private void ᖻ(string ප){var Å=new ᴆ();Å.ಖ=l.હ.ಖ;if(l.હ.ಖ==ಖ.ᴉ){int ࠎ;if(!int.
TryParse(ප.Substring(ප.Length-2,2),out ࠎ)){return;}Å.શ=ࠎ;}else{int ᔾ;if(!int.TryParse(ප.Substring(ප.Length-2,1),out ᔾ)){return;}
int ࠎ;if(!int.TryParse(ප.Substring(ප.Length-1,1),out ࠎ)){return;}Å.ᰔ=ᔾ;Å.શ=ࠎ;}l.હ.ᴾ.ᯉ(શ.ᛦ(Å),true);l.ଙ.ۍ(ኑ.ጓ.ᆷ);}private
class ᖺ{public string ᖼ;public Action<ଐ,string>ʊ;public bool ᖸ;public ᖺ(string ᐞ,Action<ଐ,string>Ǳ,bool ᖷ){ᖼ=ᐞ;ʊ=Ǳ;ᖸ=ᖷ;}}}
public enum ᖶ{ᖮ=1,ᖱ=2,ᖵ=4}sealed class Ƭ{public static int ᖴ=32;private byte[][]f;public Ƭ(ಆ þ){try{ᕣ.ᔵ.འ("Load color map: ");
var ᖳ=þ.ಠ("COLORMAP");var ܣ=ᖳ.Length/256;f=new byte[ܣ][];for(var Ä=0;Ä<ܣ;Ä++){f[Ä]=new byte[256];var ù=256*Ä;for(var Á=0;Á<
256;Á++){f[Ä][Á]=ᖳ[ù+Á];}}ᕣ.ᔵ.འ("OK");}catch(Exception e){ᕣ.ᔵ.འ("Failed");throw(e);}}public byte[]this[int ı]{get{return f[
ı];}}public byte[]ᖑ{get{return f[0];}}}sealed class ᖒ{public const int జ=0xFF;private int ٿ;private byte[]f;private int ù
;private int û;public ᖒ(int ٿ,byte[]f,int ù,int û){this.ٿ=ٿ;this.f=f;this.ù=ù;this.û=û;}public int ᖔ=>ٿ;public byte[]Ꮁ=>f
;public int ɟ=>ù;public int ᖏ=>û;}sealed class ᖎ{public ᖐ<string>ᖍ;public ᖐ<string[]>ᖌ;public ᖐ<string[]>ᖋ;public ᖐ<
MyTuple<int,int>>ᖊ;public ᖐ<int>ᔾ;public ᖐ<int>ᔽ;public ᖐ ᖉ;public ᖐ ᖈ;public ᖐ ᖇ;public ᖐ ᖆ;public ᖐ ᖅ;public ᖐ ᖄ;public ᖐ<
string>ᖃ;public ᖐ<string>ᖓ;public ᖐ<int>ᖕ;public ᖐ ᖡ;public ᖐ ᖜ;public ᖐ ᖝ;public ᖐ ᖞ;public ᖐ ᖟ;public ᖎ(string[]ᕜ){ᖍ=ᖛ(ᕜ,
"-iwad");ᖌ=ᖠ(ᕜ);ᖋ=ᖢ(ᕜ);ᖊ=ᖣ(ᕜ);ᔾ=ᖚ(ᕜ,"-episode");ᔽ=ᖚ(ᕜ,"-skill");ᖉ=new ᖐ(ᕜ.Contains("-deathmatch"));ᖈ=new ᖐ(ᕜ.Contains(
"-altdeath"));ᖇ=new ᖐ(ᕜ.Contains("-fast"));ᖆ=new ᖐ(ᕜ.Contains("-respawn"));ᖅ=new ᖐ(ᕜ.Contains("-nomonsters"));ᖄ=new ᖐ(ᕜ.Contains(
"-solo-net"));ᖃ=ᖛ(ᕜ,"-playdemo");ᖓ=ᖛ(ᕜ,"-timedemo");ᖕ=ᖚ(ᕜ,"-loadgame");ᖡ=new ᖐ(ᕜ.Contains("-nomouse"));ᖜ=new ᖐ(ᕜ.Contains(
"-nosound"));ᖝ=new ᖐ(ᕜ.Contains("-nosfx"));ᖞ=new ᖐ(ᕜ.Contains("-nomusic"));ᖟ=new ᖐ(ᕜ.Contains("-nodeh"));}private static ᖐ<string[
]>ᖠ(string[]ᕜ){var ᖙ=ᖘ(ᕜ,"-file");if(ᖙ.Length>=1){return new ᖐ<string[]>(ᖙ,true);}return new ᖐ<string[]>(null,false);}
private static ᖐ<string[]>ᖢ(string[]ᕜ){var ᖙ=ᖘ(ᕜ,"-deh");if(ᖙ.Length>=1){return new ᖐ<string[]>(ᖙ,true);}return new ᖐ<string[]>
(null,false);}private static ᖐ<MyTuple<int,int>>ᖣ(string[]ᕜ){var ᖙ=ᖘ(ᕜ,"-warp");if(ᖙ.Length==1){int ࠎ;if(int.TryParse(ᖙ[0
],out ࠎ)){return new ᖐ<MyTuple<int,int>>(MyTuple.Create(1,ࠎ),true);}}else if(ᖙ.Length==2){int ᔾ;int ࠎ;if(int.TryParse(ᖙ[0
],out ᔾ)&&int.TryParse(ᖙ[1],out ࠎ)){return new ᖐ<MyTuple<int,int>>(MyTuple.Create(ᔾ,ࠎ),true);}}return new ᖐ<MyTuple<int,
int>>(new MyTuple<int,int>(),false);}private static ᖐ<string>ᖛ(string[]ᕜ,string Ĭ){var ᖙ=ᖘ(ᕜ,Ĭ);if(ᖙ.Length==1){return new
ᖐ<string>(ᖙ[0],true);}return new ᖐ<string>(null,false);}private static ᖐ<int>ᖚ(string[]ᕜ,string Ĭ){var ᖙ=ᖘ(ᕜ,Ĭ);if(ᖙ.
Length==1){int ŀ;if(int.TryParse(ᖙ[0],out ŀ)){return new ᖐ<int>(ŀ,true);}}return new ᖐ<int>(0,false);}private static string[]ᖘ
(string[]ᕜ,string Ĭ){return ᕜ.SkipWhile(ᖗ=>ᖗ!=Ĭ).Skip(1).TakeWhile(ᖗ=>ᖗ[0]!='-').ToArray();}public class ᖐ{private bool ᖖ
;public ᖐ(){this.ᖖ=false;}public ᖐ(bool ᖖ){this.ᖖ=ᖖ;}public bool ᗪ=>ᖖ;}public class ᖐ<ኯ>{private bool ᖖ;private ኯ u;
public ᖐ(ኯ u,bool ᗬ){this.ᖖ=ᗬ;this.u=u;}public bool ᗪ=>ᖖ;public ኯ ᗭ=>u;}}sealed class ᗮ{public ᵒ ᗯ;public ᵒ ᗰ;public ᵒ ᗱ;
public ᵒ ᗩ;public ᵒ ᗨ;public ᵒ ᗧ;public ᵒ ᗦ;public ᵒ ᗥ;public ᵒ ᗤ;public ᵒ ᗣ;public int ᗢ;public bool ᗡ;public bool ᗠ;public
int ᗟ;public int ᗞ;public bool ᗝ;public bool ᗜ;public bool ᗛ;public int ᗚ;public int ᗫ;public int ᗙ;public int ᗲ;public int
ᗷ;public bool ᗸ;public string ᗹ;public bool ᗺ;private bool ᗻ;public ᗮ(){ᗯ=new ᵒ(new ኈ[]{ኈ.ĳ,ኈ.ኳ});ᗰ=new ᵒ(new ኈ[]{ኈ.Ķ,ኈ.ኮ
});ᗱ=new ᵒ(new ኈ[]{ኈ.ኆ});ᗩ=new ᵒ(new ኈ[]{ኈ.ኃ});ᗨ=new ᵒ(new ኈ[]{ኈ.ቀ});ᗧ=new ᵒ(new ኈ[]{ኈ.ሿ});ᗦ=new ᵒ(new ኈ[]{ኈ.አ,ኈ.ኜ},new ᐆ
[]{ᐆ.ᐅ});ᗥ=new ᵒ(new ኈ[]{ኈ.ቓ},new ᐆ[]{ᐆ.ᐄ});ᗤ=new ᵒ(new ኈ[]{ኈ.ኟ,ኈ.ኛ});ᗣ=new ᵒ(new ኈ[]{ኈ.ኞ,ኈ.ኚ});ᗢ=8;ᗡ=false;ᗠ=true;ᗟ=640;
ᗞ=400;ᗝ=false;ᗜ=true;ᗚ=7;ᗛ=true;ᗫ=2;ᗙ=2;ᗲ=8;ᗷ=8;ᗸ=true;ᗹ="TimGM6mb.sf2";ᗺ=true;ᗻ=false;}public ᗮ(string ᗼ):this(){try{ᕣ.ᔵ
.འ("Restore settings: ");var ᗳ=new Dictionary<string,string>();String[]ᗽ=ᗼ.Split('\n');foreach(var ò in ᗽ){var ᗾ=ò.Split(
'=');if(ᗾ.Length==2){ᗳ[ᗾ[0].Trim()]=ᗾ[1].Trim();}}ᗯ=ᗶ(ᗳ,nameof(ᗯ),ᗯ);ᗰ=ᗶ(ᗳ,nameof(ᗰ),ᗰ);ᗱ=ᗶ(ᗳ,nameof(ᗱ),ᗱ);ᗩ=ᗶ(ᗳ,nameof(ᗩ),
ᗩ);ᗨ=ᗶ(ᗳ,nameof(ᗨ),ᗨ);ᗧ=ᗶ(ᗳ,nameof(ᗧ),ᗧ);ᗦ=ᗶ(ᗳ,nameof(ᗦ),ᗦ);ᗥ=ᗶ(ᗳ,nameof(ᗥ),ᗥ);ᗤ=ᗶ(ᗳ,nameof(ᗤ),ᗤ);ᗣ=ᗶ(ᗳ,nameof(ᗣ),ᗣ);ᗢ=ᖚ(
ᗳ,nameof(ᗢ),ᗢ);ᗡ=ᗵ(ᗳ,nameof(ᗡ),ᗡ);ᗠ=ᗵ(ᗳ,nameof(ᗠ),ᗠ);ᗟ=ᖚ(ᗳ,nameof(ᗟ),ᗟ);ᗞ=ᖚ(ᗳ,nameof(ᗞ),ᗞ);ᗝ=ᗵ(ᗳ,nameof(ᗝ),ᗝ);ᗜ=ᗵ(ᗳ,
nameof(ᗜ),ᗜ);ᗛ=ᗵ(ᗳ,nameof(ᗛ),ᗛ);ᗚ=ᖚ(ᗳ,nameof(ᗚ),ᗚ);ᗫ=ᖚ(ᗳ,nameof(ᗫ),ᗫ);ᗙ=ᖚ(ᗳ,nameof(ᗙ),ᗙ);ᗲ=ᖚ(ᗳ,nameof(ᗲ),ᗲ);ᗷ=ᖚ(ᗳ,nameof(ᗷ),ᗷ)
;ᗸ=ᗵ(ᗳ,nameof(ᗸ),ᗸ);ᗹ=ᖛ(ᗳ,nameof(ᗹ),ᗹ);ᗺ=ᗵ(ᗳ,nameof(ᗺ),ᗺ);ᗻ=true;ᕣ.ᔵ.འ("OK");}catch{ᕣ.ᔵ.འ("Failed");}}public String ï(){
String ᗿ="";ᗿ=ᗿ+nameof(ᗯ)+" = "+ᗯ+"\n";ᗿ=ᗿ+nameof(ᗰ)+" = "+ᗰ+"\n";ᗿ=ᗿ+nameof(ᗱ)+" = "+ᗱ+"\n";ᗿ=ᗿ+nameof(ᗩ)+" = "+ᗩ+"\n";ᗿ=ᗿ+
nameof(ᗨ)+" = "+ᗨ+"\n";ᗿ=ᗿ+nameof(ᗧ)+" = "+ᗧ+"\n";ᗿ=ᗿ+nameof(ᗦ)+" = "+ᗦ+"\n";ᗿ=ᗿ+nameof(ᗥ)+" = "+ᗥ+"\n";ᗿ=ᗿ+nameof(ᗤ)+" = "+ᗤ+
"\n";ᗿ=ᗿ+nameof(ᗣ)+" = "+ᗣ+"\n";ᗿ=ᗿ+nameof(ᗢ)+" = "+ᗢ+"\n";ᗿ=ᗿ+nameof(ᗡ)+" = "+ᗂ(ᗡ)+"\n";ᗿ=ᗿ+nameof(ᗠ)+" = "+ᗂ(ᗠ)+"\n";ᗿ=ᗿ+
nameof(ᗟ)+" = "+ᗟ+"\n";ᗿ=ᗿ+nameof(ᗞ)+" = "+ᗞ+"\n";ᗿ=ᗿ+nameof(ᗝ)+" = "+ᗂ(ᗝ)+"\n";ᗿ=ᗿ+nameof(ᗜ)+" = "+ᗂ(ᗜ)+"\n";ᗿ=ᗿ+nameof(ᗛ)+
" = "+ᗂ(ᗛ)+"\n";ᗿ=ᗿ+nameof(ᗚ)+" = "+ᗚ+"\n";ᗿ=ᗿ+nameof(ᗫ)+" = "+ᗫ+"\n";ᗿ=ᗿ+nameof(ᗙ)+" = "+ᗙ+"\n";ᗿ=ᗿ+nameof(ᗲ)+" = "+ᗲ+"\n";ᗿ
=ᗿ+nameof(ᗷ)+" = "+ᗷ+"\n";ᗿ=ᗿ+nameof(ᗸ)+" = "+ᗂ(ᗸ)+"\n";ᗿ=ᗿ+nameof(ᗹ)+" = "+ᗹ+"\n";ᗿ=ᗿ+nameof(ᗺ)+" = "+ᗂ(ᗺ)+"\n";return ᗿ
;}private static int ᖚ(Dictionary<string,string>ᗳ,string Ĭ,int ᗴ){string ᗘ;if(ᗳ.TryGetValue(Ĭ,out ᗘ)){int u;if(int.
TryParse(ᗘ,out u)){return u;}}return ᗴ;}private static string ᖛ(Dictionary<string,string>ᗳ,string Ĭ,string ᗴ){string ᗘ;if(ᗳ.
TryGetValue(Ĭ,out ᗘ)){return ᗘ;}return ᗴ;}private static bool ᗵ(Dictionary<string,string>ᗳ,string Ĭ,bool ᗴ){string ᗘ;if(ᗳ.
TryGetValue(Ĭ,out ᗘ)){if(ᗘ=="true"){return true;}else if(ᗘ=="false"){return false;}}return ᗴ;}private static ᵒ ᗶ(Dictionary<string,
string>ᗳ,string Ĭ,ᵒ ᗴ){string ᗘ;if(ᗳ.TryGetValue(Ĭ,out ᗘ)){return ᵒ.ቤ(ᗘ);}return ᗴ;}private static string ᗂ(bool u){return u?
"true":"false";}public bool ᗃ=>ᗻ;}static class ᗄ{private static string[]ᗅ=new string[]{"DOOM2.WAD","PLUTONIA.WAD","TNT.WAD",
"DOOM.WAD","DOOM1.WAD","FREEDOOM2.WAD","FREEDOOM1.WAD"};public static string ᗆ(){return".";}public static string ᗇ(){return
".\\managed-doom.cfg";}public static string ᗈ(){return"DOOM1.WAD";}public static bool ᗊ(string ð){var Ĭ=ð.ToUpper();return ᗅ.Contains(Ĭ);}
public static string[]ᗋ(ᖎ ᕜ){var ᗌ=new List<string>();if(ᕜ.ᖍ.ᗪ){ᗌ.Add(ᕜ.ᖍ.ᗭ);}else{ᗌ.Add(ᗄ.ᗈ());}if(ᕜ.ᖌ.ᗪ){foreach(var ð in ᕜ.
ᖌ.ᗭ){ᗌ.Add(ð);}}return ᗌ.ToArray();}}sealed class ژ{private int É;private byte[]f;private ᴆ Å;private int ᗁ;public ژ(byte
[]f){É=0;if(f[É++]!=109){throw new Exception("Demo is from a different game version!");}this.f=f;Å=new ᴆ();Å.ᴷ=(ᵅ)f[É++];
Å.ᰔ=f[É++];Å.શ=f[É++];Å.ᴵ=f[É++];Å.ᴳ=f[É++]!=0;Å.ᴴ=f[É++]!=0;Å.ᴲ=f[É++]!=0;Å.ଙ=f[É++];Å.ᰁ[0].Ÿ=f[É++]!=0;Å.ᰁ[1].Ÿ=f[É++]
!=0;Å.ᰁ[2].Ÿ=f[É++]!=0;Å.ᰁ[3].Ÿ=f[É++]!=0;ᗁ=0;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Å.ᰁ[Ä].Ÿ){ᗁ++;}}if(ᗁ>=2){Å.ᴶ=true;}}public bool ᗀ(
థ[]ߑ){if(É==f.Length){return false;}if(f[É]==0x80){return false;}if(É+4*ᗁ>f.Length){return false;}var Ë=Å.ᰁ;for(var Ä=0;Ä
<ђ.ۇ;Ä++){if(Ë[Ä].Ÿ){var Ƥ=ߑ[Ä];Ƥ.Ʊ=(sbyte)f[É++];Ƥ.Ƶ=(sbyte)f[É++];Ƥ.Ʋ=(short)(f[É++]<<8);Ƥ.ఠ=f[É++];}}return true;}
public ᴆ હ=>Å;}public enum ಣ{ᖿ,ᖾ,ᖽ,ᗉ,ᗍ,ᗖ,ᗓ,ᗔ,ந,ŏ}sealed class ᗕ{private Ꮖ ǃ;private Ꮖ ǂ;private Ꮖ ߝ;private Ꮖ Ǆ;public void ᗗ(
ɢ ò){ǃ=ò.ɝ.ޚ;ǂ=ò.ɝ.ޙ;ߝ=ò.ޘ;Ǆ=ò.ޗ;}public Ꮖ ޚ{get{return ǃ;}set{ǃ=value;}}public Ꮖ ޙ{get{return ǂ;}set{ǂ=value;}}public Ꮖ
ޘ{get{return ߝ;}set{ߝ=value;}}public Ꮖ ޗ{get{return Ǆ;}set{Ǆ=value;}}}class ጟ{private ᖎ ᕜ;private ᗮ ǽ;private ᴍ Ǽ;private
Ḿ Ꮳ;private ᶫ Ŗ;private ᯈ Ꮿ;private Ṉ Ꮲ;private List<ᕨ>ᗒ;private ᴆ Å;private ባ ĭ;private ߕ ᗑ;private థ[]ߑ;private ᕣ Æ;
private ବ ᗐ;private bool ᗏ;private Ꮣ ᓅ;private Ꮣ ᗎ;private bool ᖂ;private bool ᕙ;private bool ᕀ;private string ᕚ;private bool ᕛ
;public ጟ(ᖎ ᕜ,ᗮ ǽ,ᴍ Ǽ,Ḿ Ꮳ,ᶫ Ŗ,ᯈ Ꮿ,Ṉ Ꮲ){Ꮳ=Ꮳ??ߖ.ߏ();Ŗ=ߘ.ߏ();Ꮿ=ޔ.ߏ();Ꮲ=Ꮲ??ߜ.ߏ();this.ᕜ=ᕜ;this.ǽ=ǽ;this.Ǽ=Ǽ;this.Ꮳ=Ꮳ;this.Ŗ=Ŗ
;this.Ꮿ=Ꮿ;this.Ꮲ=Ꮲ;ᗒ=new List<ᕨ>();Å=new ᴆ(ᕜ,Ǽ);Å.ᴱ=Ꮳ;Å.ᴽ=Ŗ;Å.ᴾ=Ꮿ;Å.ᵉ=Ꮲ;ĭ=new ባ(this);ᗑ=new ߕ(Ǽ,Å);ߑ=new థ[ђ.ۇ];for(var Ä
=0;Ä<ђ.ۇ;Ä++){ߑ[Ä]=new థ();}Æ=new ᕣ(Ǽ,Å);ᗐ=new ବ(Ꮳ.ǚ,Ꮳ.Ǜ);ᗏ=false;ᓅ=Ꮣ.ந;ᗎ=Ꮣ.Ꮤ;ᖂ=false;ᕙ=false;ᕀ=false;ᕚ=null;ᕛ=false;ᕝ();
}private void ᕝ(){if(ᕜ.ᖊ.ᗪ){ᗎ=Ꮣ.સ;Å.ᰔ=ᕜ.ᖊ.ᗭ.Item1;Å.શ=ᕜ.ᖊ.ᗭ.Item2;Æ.ᔼ();}else if(ᕜ.ᔾ.ᗪ){ᗎ=Ꮣ.સ;Å.ᰔ=ᕜ.ᔾ.ᗭ;Å.શ=1;Æ.ᔼ();}if(ᕜ
.ᔽ.ᗪ){Å.ᴷ=(ᵅ)(ᕜ.ᔽ.ᗭ-1);}if(ᕜ.ᖉ.ᗪ){Å.ᴵ=1;}if(ᕜ.ᖈ.ᗪ){Å.ᴵ=2;}if(ᕜ.ᖇ.ᗪ){Å.ᴴ=true;}if(ᕜ.ᖆ.ᗪ){Å.ᴳ=true;}if(ᕜ.ᖅ.ᗪ){Å.ᴲ=true;}if(
ᕜ.ᖕ.ᗪ){ᗎ=Ꮣ.સ;Æ.Î(ᕜ.ᖕ.ᗭ);}}public void ᔻ(ᵅ ᔽ,int ᔾ,int ࠎ){Æ.ᔼ(ᔽ,ᔾ,ࠎ);ᗎ=Ꮣ.સ;}public void ᐐ(){ᗎ=Ꮣ.Ꮤ;}private void ᕘ(){if(ᗏ){
return;}foreach(var Ň in ᗒ){if(ĭ.ଅ(Ň)){continue;}if(Ň.ܫ==ፎ.ፏ){if(ᕗ(Ň.ᕤ)){continue;}}if(ᓅ==Ꮣ.સ){if(Ň.ᕤ==ኈ.ஓ&&Ň.ܫ==ፎ.ፏ){ᕙ=true;
continue;}if(Æ.ଅ(Ň)){continue;}}}ᗒ.Clear();}private bool ᕗ(ኈ ብ){switch(ብ){case ኈ.ቬ:ĭ.Ꮻ();return true;case ኈ.ቭ:ĭ.Ᏸ();return true;
case ኈ.ቮ:ĭ.ᐍ();return true;case ኈ.ቯ:ĭ.ᐇ();return true;case ኈ.ቲ:ĭ.ᐈ();return true;case ኈ.ቹ:if(ᓅ==Ꮣ.સ){ĭ.ᐐ();}else{Å.ᴽ.ૐ(ɴ.Ȟ);
}return true;case ኈ.ታ:Ꮳ.Ǟ=!Ꮳ.Ǟ;if(ᓅ==Ꮣ.સ&&Æ.Ŷ==ᵈ.ᔜ){string ᕕ;if(Ꮳ.Ǟ){ᕕ=ኑ.ጓ.ጸ;}else{ᕕ=ኑ.ጓ.ጷ;}Æ.ଦ.ଙ.ۍ(ᕕ);}ĭ.ૐ(ɴ.Ȭ);return
true;case ኈ.ቴ:ĭ.ᐋ();return true;case ኈ.ት:ĭ.ᐎ();return true;case ኈ.ቶ:var ᕖ=Ꮳ.Ǡ;ᕖ++;if(ᕖ>Ꮳ.ǟ){ᕖ=0;}Ꮳ.Ǡ=ᕖ;if(ᓅ==Ꮣ.સ&&Æ.Ŷ==ᵈ.ᔜ){
string ᕕ;if(ᕖ==0){ᕕ=ኑ.ጓ.ጼ;}else{ᕕ="Gamma correction level "+ᕖ;}Æ.ଦ.ଙ.ۍ(ᕕ);}return true;case ኈ.ᄩ:case ኈ.ቋ:case ኈ.ቒ:if(ᓅ==Ꮣ.સ&&Æ
.Ŷ==ᵈ.ᔜ&&Æ.ଦ.ଏ.ᔢ){return false;}else{Ꮳ.ǝ=Math.Min(Ꮳ.ǝ+1,Ꮳ.ǜ);Ŗ.ૐ(ɴ.Ȳ);return true;}case ኈ.ቃ:case ኈ.ቛ:case ኈ.ሹ:if(ᓅ==Ꮣ.સ&&
Æ.Ŷ==ᵈ.ᔜ&&Æ.ଦ.ଏ.ᔢ){return false;}else{Ꮳ.ǝ=Math.Max(Ꮳ.ǝ-1,0);Ŗ.ૐ(ɴ.Ȳ);return true;}default:return false;}}public ன ڞ(){ᕘ()
;if(!ᗏ){ĭ.ڞ();if(ᗎ!=ᓅ){if(ᗎ!=Ꮣ.Ꮤ){ᗑ.ʕ();}ᓅ=ᗎ;}if(ᕀ){return ன.த;}if(ᖂ){ᖂ=false;ᕒ();}}if(!ᗏ){switch(ᓅ){case Ꮣ.Ꮤ:if(ᗑ.ڞ()==ன
.ண){ᕒ();}break;case Ꮣ.સ:Ꮲ.ṃ(ߑ[Å.ଙ]);if(ᕙ){ᕙ=false;ߑ[Å.ଙ].ఠ|=(byte)(ట.Ě|ట.ஓ);}if(Æ.ڞ(ߑ)==ன.ண){ᕒ();}break;default:throw new
Exception("Invalid application state!");}}if(ᗏ){if(ᗐ.ڞ()==ன.த){ᗏ=false;}}Ŗ.ڞ();ᕔ();return ன.ந;}private void ᕔ(){bool ᕓ;if(!Ꮳ.ḽ())
{ᕓ=false;}else if(ǽ.ᗝ){ᕓ=true;}else{ᕓ=ᓅ==Ꮣ.સ&&!ĭ.ᄧ;}if(ᕛ){if(!ᕓ){Ꮲ.ṁ();ᕛ=false;}}else{if(ᕓ){Ꮲ.Ṃ();ᕛ=true;}}}private void
ᕒ(){ᗐ.ମ();Ꮳ.ƾ();ᗏ=true;}public void ᕑ(){if(ᓅ==Ꮣ.સ&&Æ.Ŷ==ᵈ.ᔜ&&!Æ.ᕄ&&!ᕙ){ᕙ=true;}}public void ᕐ(){if(ᓅ==Ꮣ.સ&&Æ.Ŷ==ᵈ.ᔜ&&Æ.ᕄ
&&!ᕙ){ᕙ=true;}}public bool Ő(int ł,string ì){if(ᓅ==Ꮣ.સ&&Æ.Ŷ==ᵈ.ᔜ){Æ.Ő(ł,ì);return true;}else{return false;}}public void Î(
int ł){Æ.Î(ł);ᗎ=Ꮣ.સ;}public void ᐎ(){ᕀ=true;}public void ᐎ(string ی){ᕀ=true;ᕚ=ی;}public void ᕥ(ᕨ Ň){if(ᗒ.Count<64){ᗒ.Add(Ň)
;}}public Ꮣ Ŷ=>ᓅ;public ߕ Ꮤ=>ᗑ;public ᴆ હ=>Å;public ᕣ સ=>Æ;public ባ ኘ=>ĭ;public ବ ବ=>ᗐ;public bool ᕦ=>ᗏ;public string ᕧ=>
ᕚ;}sealed class ᕨ{private ፎ ˌ;private ኈ ብ;public ᕨ(ፎ ˌ,ኈ ብ){this.ˌ=ˌ;this.ብ=ብ;}public ፎ ܫ=>ˌ;public ኈ ᕤ=>ብ;}sealed class
ᕣ{private ᴍ Ǽ;private ᴆ Å;private ᕃ ᕢ;private ᵈ ᕡ;private int ᕠ;private ଦ l;private ᕅ Ȏ;private ፓ ȃ;private bool ᕟ;
private int ᕞ;private int ᕏ;private string ᕎ;public static ཟ ᔵ=new ཟ();public ᕣ(ᴍ Ǽ,ᴆ Å){this.Ǽ=Ǽ;this.Å=Å;ᕢ=ᕃ.ᕂ;ᕠ=0;}public
void ᔼ(){ᕢ=ᕃ.ᔻ;}public void ᔼ(ᵅ ᔽ,int ᔾ,int ࠎ){Å.ᴷ=ᔽ;Å.ᰔ=ᔾ;Å.શ=ࠎ;ᕢ=ᕃ.ᔻ;}public void Î(int ł){ᕞ=ł;ᕢ=ᕃ.Î;}public void Ő(int ł,
string ì){ᕏ=ł;ᕎ=ì;ᕢ=ᕃ.Ő;}public ன ڞ(థ[]ߑ){var Ë=Å.ᰁ;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Ë[Ä].Ÿ&&Ë[Ä].Ų==Ų.ů){ᕆ(Ä);}}while(ᕢ!=ᕃ.ᕂ){switch
(ᕢ){case ᕃ.ᕁ:ᔺ();break;case ᕃ.ᔻ:ᔹ();break;case ᕃ.Î:ᔸ();break;case ᕃ.Ő:ᔷ();break;case ᕃ.த:ᔿ();break;case ᕃ.ᕶ:ᕉ();break;
case ᕃ.ᕷ:ᕈ();break;case ᕃ.ᕂ:break;}}for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Ë[Ä].Ÿ){var Ƥ=Ë[Ä].ƕ;Ƥ.ఝ(ߑ[Ä]);if(Ƥ.Ʊ>ᱺ.ᴅ&&(l.ଖ&31)==0&&((l.ଖ
>>5)&3)==Ä){var Ð=Ë[Å.ଙ];Ð.ۍ(Ë[Ä].Ń+" is turbo!");}}}for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Ë[Ä].Ÿ){if((Ë[Ä].ƕ.ఠ&ట.Ě)!=0){if((Ë[Ä].ƕ.ఠ&
ట.ஈ)==ట.ஓ){ᕟ=!ᕟ;if(ᕟ){Å.ᴽ.ஓ();}else{Å.ᴽ.ḁ();}}}}}var ŀ=ன.ந;switch(ᕡ){case ᵈ.ᔜ:if(!ᕟ||l.ଛ){ŀ=l.ڞ();if(ŀ==ன.த){ᕢ=ᕃ.த;}}
break;case ᵈ.ᕅ:ŀ=Ȏ.ڞ();if(ŀ==ன.த){ᕢ=ᕃ.ᕷ;if(l.ଘ){Ë[Å.ଙ].Ʈ=true;}if(Å.ಖ==ಖ.ᴉ){switch(Å.શ){case 6:case 11:case 20:case 30:ᕉ();ŀ=
ன.ண;break;case 15:case 31:if(l.ଘ){ᕉ();ŀ=ன.ண;}break;}}}break;case ᵈ.ፓ:ŀ=ȃ.ڞ();if(ŀ==ன.த){ᕢ=ᕃ.ᕷ;}break;}ᕠ++;if(ŀ==ன.ண){
return ன.ண;}else{return ன.ந;}}private void ᔺ(){ᕢ=ᕃ.ᕂ;ᕡ=ᵈ.ᔜ;var Ë=Å.ᰁ;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Ë[Ä].Ÿ&&Ë[Ä].Ų==Ų.Ű){Ë[Ä].Ų=Ų.ů
;}Array.Clear(Ë[Ä].Ž,0,Ë[Ä].Ž.Length);}Ȏ=null;Å.ᴽ.ʕ();l=new ଦ(Ǽ,Å,this);Å.ᵉ.ʕ();}private void ᔹ(){ᕢ=ᕃ.ᕂ;ᕌ(Å.ᴷ,Å.ᰔ,Å.શ);}
private void ᔸ(){ᕢ=ᕃ.ᕂ;var ᔶ=ᗄ.ᗆ();var ð=".\\doomsav"+ᕞ+".dsg";Ǣ.Ç(this,ð);}private void ᔷ(){ᕢ=ᕃ.ᕂ;var ᔶ=ᗄ.ᗆ();var ð=
".\\doomsav"+ᕞ+".dsg";Ǣ.ï(this,ᕎ,ð);l.ଙ.ۍ(ኑ.ጓ.ዎ);}private void ᔿ(){ᕢ=ᕃ.ᕂ;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Å.ᰁ[Ä].Ÿ){Å.ᰁ[Ä].ێ();}}if(Å.ಖ!=ಖ.
ᴉ){switch(Å.શ){case 8:ᕢ=ᕃ.ᕶ;return;case 9:for(var Ä=0;Ä<ђ.ۇ;Ä++){Å.ᰁ[Ä].Ʈ=true;}break;}}if((Å.શ==8)&&(Å.ಖ!=ಖ.ᴉ)){ᕢ=ᕃ.ᕶ;
return;}if((Å.શ==9)&&(Å.ಖ!=ಖ.ᴉ)){for(var Ä=0;Ä<ђ.ۇ;Ä++){Å.ᰁ[Ä].Ʈ=true;}}var ᕇ=Å.ᰋ;ᕇ.Ʈ=Å.ᰁ[Å.ଙ].Ʈ;ᕇ.ᰔ=Å.ᰔ-1;ᕇ.ᰈ=Å.શ-1;if(Å.ಖ==ಖ
.ᴉ){if(l.ଘ){switch(Å.શ){case 15:ᕇ.ᰇ=30;break;case 31:ᕇ.ᰇ=31;break;}}else{switch(Å.શ){case 31:case 32:ᕇ.ᰇ=15;break;default
:ᕇ.ᰇ=Å.શ;break;}}}else{if(l.ଘ){ᕇ.ᰇ=8;}else if(Å.શ==9){switch(Å.ᰔ){case 1:ᕇ.ᰇ=3;break;case 2:ᕇ.ᰇ=5;break;case 3:ᕇ.ᰇ=6;
break;case 4:ᕇ.ᰇ=2;break;}}else{ᕇ.ᰇ=Å.શ;}}ᕇ.ᰆ=l.ଓ;ᕇ.ᰅ=l.କ;ᕇ.ᰄ=l.ଜ;ᕇ.ᰃ=0;if(Å.ಖ==ಖ.ᴉ){ᕇ.ᰂ=35*ኑ.ዂ.ኾ[Å.શ-1];}else{ᕇ.ᰂ=35*ኑ.ዂ.ዀ[Å
.ᰔ-1][Å.શ-1];}var Ë=Å.ᰁ;for(var Ä=0;Ä<ђ.ۇ;Ä++){ᕇ.ᰁ[Ä].Ÿ=Ë[Ä].Ÿ;ᕇ.ᰁ[Ä].Ź=Ë[Ä].Ź;ᕇ.ᰁ[Ä].ź=Ë[Ä].ź;ᕇ.ᰁ[Ä].Ż=Ë[Ä].Ż;ᕇ.ᰁ[Ä].ż=l
.ଖ;Array.Copy(Ë[Ä].Ž,ᕇ.ᰁ[Ä].Ž,ђ.ۇ);}ᕡ=ᵈ.ᕅ;Ȏ=new ᕅ(Å,ᕇ);}private void ᕈ(){ᕢ=ᕃ.ᕂ;ᕡ=ᵈ.ᔜ;Å.શ=Å.ᰋ.ᰇ+1;ᔺ();}private void ᕉ(){ᕢ=
ᕃ.ᕂ;ᕡ=ᵈ.ፓ;ȃ=new ፓ(Å);}public static int ᕊ(int ᕍ,int ː,int ᕋ){return Math.Min(Math.Max(ᕍ,ː),ᕋ);}public void ᕌ(ᵅ ᔽ,int ᔾ,
int ࠎ){Å.ᴷ=(ᵅ)ᕊ((int)ᔽ,(int)ᵅ.Р,(int)ᵅ.ᵇ);if(Å.ಖ==ಖ.ᴈ){Å.ᰔ=ᕊ(ᔾ,1,4);}else if(Å.ಖ==ಖ.ᴋ){Å.ᰔ=1;}else{Å.ᰔ=ᕊ(ᔾ,1,4);}if(Å.ಖ==ಖ.
ᴉ){Å.શ=ᕊ(ࠎ,1,32);}else{Å.શ=ᕊ(ࠎ,1,9);}Å.ષ.ŷ();for(var Ä=0;Ä<ђ.ۇ;Ä++){Å.ᰁ[Ä].Ų=Ų.ů;}ᔺ();}public bool ଅ(ᕨ Ň){if(ᕡ==ᵈ.ᔜ){
return l.ଅ(Ň);}else if(ᕡ==ᵈ.ፓ){return ȃ.ଅ(Ň);}return false;}private void ᕆ(int ä){if(!Å.ᴶ){ᕢ=ᕃ.ᕁ;}else{Å.ᰁ[ä].Ɠ.ђ=null;var æ=l
.ળ;if(Å.ᴵ!=0){æ.ᆝ(ä);return;}if(æ.ᆩ(ä,æ.ᅱ[ä])){æ.ᅉ(æ.ᅱ[ä]);return;}for(var Ä=0;Ä<ђ.ۇ;Ä++){if(æ.ᆩ(ä,æ.ᅱ[Ä])){æ.ᅱ[Ä].ܫ=ä+1;
l.ળ.ᅉ(æ.ᅱ[Ä]);æ.ᅱ[Ä].ܫ=Ä+1;return;}}l.ળ.ᅉ(æ.ᅱ[ä]);}}public ᴆ હ=>Å;public ᵈ Ŷ=>ᕡ;public int ଗ=>ᕠ;public ଦ ଦ=>l;public ᕅ ᕅ
=>Ȏ;public ፓ ፓ=>ȃ;public bool ᕄ=>ᕟ;private enum ᕃ{ᕂ,ᕁ,ᔻ,Î,Ő,த,ᕶ,ᕷ}}static partial class ኑ{public static class ᕸ{public
static int[]Ꮅ=new int[]{200,50,300,50};public static int[]З=new int[]{10,4,20,1};}}static partial class ኑ{public static Ꮥ[]ᕿ=
new Ꮥ[]{new Ꮥ("none"),new Ꮥ("e1m1"),new Ꮥ("e1m2"),new Ꮥ("e1m3"),new Ꮥ("e1m4"),new Ꮥ("e1m5"),new Ꮥ("e1m6"),new Ꮥ("e1m7"),new
Ꮥ("e1m8"),new Ꮥ("e1m9"),new Ꮥ("e2m1"),new Ꮥ("e2m2"),new Ꮥ("e2m3"),new Ꮥ("e2m4"),new Ꮥ("e2m5"),new Ꮥ("e2m6"),new Ꮥ("e2m7")
,new Ꮥ("e2m8"),new Ꮥ("e2m9"),new Ꮥ("e3m1"),new Ꮥ("e3m2"),new Ꮥ("e3m3"),new Ꮥ("e3m4"),new Ꮥ("e3m5"),new Ꮥ("e3m6"),new Ꮥ(
"e3m7"),new Ꮥ("e3m8"),new Ꮥ("e3m9"),new Ꮥ("inter"),new Ꮥ("intro"),new Ꮥ("bunny"),new Ꮥ("victor"),new Ꮥ("introa"),new Ꮥ(
"runnin"),new Ꮥ("stalks"),new Ꮥ("countd"),new Ꮥ("betwee"),new Ꮥ("doom"),new Ꮥ("the_da"),new Ꮥ("shawn"),new Ꮥ("ddtblu"),new Ꮥ(
"in_cit"),new Ꮥ("dead"),new Ꮥ("stlks2"),new Ꮥ("theda2"),new Ꮥ("doom2"),new Ꮥ("ddtbl2"),new Ꮥ("runni2"),new Ꮥ("dead2"),new Ꮥ(
"stlks3"),new Ꮥ("romero"),new Ꮥ("shawn2"),new Ꮥ("messag"),new Ꮥ("count2"),new Ꮥ("ddtbl3"),new Ꮥ("ampie"),new Ꮥ("theda3"),new Ꮥ(
"adrian"),new Ꮥ("messg2"),new Ꮥ("romer2"),new Ꮥ("tense"),new Ꮥ("shawn3"),new Ꮥ("openin"),new Ꮥ("evil"),new Ꮥ("ultima"),new Ꮥ(
"read_m"),new Ꮥ("dm2ttl"),new Ꮥ("dm2int")};}static partial class ኑ{public static class ᕺ{public static int ᕻ{get;set;}=100;
public static int ᕼ{get;set;}=50;public static int ᕽ{get;set;}=200;public static int ᕾ{get;set;}=200;public static int ᖀ{get;
set;}=1;public static int ᕵ{get;set;}=2;public static int ᕴ{get;set;}=200;public static int ᕳ{get;set;}=100;public static
int ᕲ{get;set;}=200;public static int ᕱ{get;set;}=100;public static int ᕰ{get;set;}=200;public static int ᕯ{get;set;}=2;
public static int ᕮ{get;set;}=200;public static int ᕭ{get;set;}=2;public static int ᕬ{get;set;}=40;public static bool ᕫ{get;
set;}=false;}}static partial class ኑ{public static class ᕪ{public static IReadOnlyList<IReadOnlyList<Ꮥ>>ጟ=new Ꮥ[][]{new Ꮥ[]
{ጓ.ው,ጓ.ዌ,ጓ.ዋ,ጓ.ዊ,ጓ.ዉ,ጓ.ወ,ጓ.ዅ,ጓ.ዟ,ጓ.ዩ},new Ꮥ[]{ጓ.ጂ,ጓ.ዻ,ጓ.ዼ,ጓ.ዽ,ጓ.ዾ,ጓ.ዿ,ጓ.ጀ,ጓ.ጁ,ጓ.ጃ},new Ꮥ[]{ጓ.ጊ,ጓ.ጄ,ጓ.ጅ,ጓ.ጆ,ጓ.ጇ,ጓ.ገ,ጓ.ጉ,ጓ.
ጋ,ጓ.ዺ},new Ꮥ[]{ጓ.ዹ,ጓ.ዸ,ጓ.ዷ,ጓ.ዶ,ጓ.ድ,ጓ.ዴ,ጓ.ዳ,ጓ.ዲ,ጓ.ዱ}};public static IReadOnlyList<Ꮥ>ኾ=new Ꮥ[]{ጓ.ደ,ጓ.ዯ,ጓ.ዮ,ጓ.ይ,ጓ.ዬ,ጓ.ያ,ጓ.ዪ,
ጓ.ኼ,ጓ.ኻ,ጓ.ᇑ,ጓ.ህ,ጓ.ሆ,ጓ.ሇ,ጓ.ለ,ጓ.ሉ,ጓ.ሊ,ጓ.ሌ,ጓ.ሓ,ጓ.ል,ጓ.ሎ,ጓ.ሏ,ጓ.ሐ,ጓ.ሑ,ጓ.ሒ,ጓ.ሔ,ጓ.ሄ,ጓ.ሃ,ጓ.ሂ,ጓ.ሁ,ጓ.ሀ,ጓ.ᇿ,ጓ.ᇾ};public static
IReadOnlyList<Ꮥ>ᕩ=new Ꮥ[]{ጓ.ᇽ,ጓ.ᇼ,ጓ.ᇻ,ጓ.ᇺ,ጓ.ᇹ,ጓ.ᇸ,ጓ.ᇷ,ጓ.ᇶ,ጓ.ᇵ,ጓ.ላ,ጓ.ሕ,ጓ.ሮ,ጓ.ሧ,ጓ.ረ,ጓ.ሩ,ጓ.ሪ,ጓ.ራ,ጓ.ሬ,ጓ.ር,ጓ.ሯ,ጓ.ሶ,ጓ.ሰ,ጓ.ሱ,ጓ.ሲ,ጓ.ሳ,ጓ.ሴ,ጓ.ስ
,ጓ.ሷ,ጓ.ሦ,ጓ.ሥ,ጓ.ሤ,ጓ.ሣ};public static IReadOnlyList<Ꮥ>ᕹ=new Ꮥ[]{ጓ.ሢ,ጓ.ሡ,ጓ.ሠ,ጓ.ሟ,ጓ.ሞ,ጓ.ም,ጓ.ሜ,ጓ.ማ,ጓ.ሚ,ጓ.ሙ,ጓ.መ,ጓ.ሗ,ጓ.ሖ,ጓ.ᇴ,ጓ.ᇳ
,ጓ.ᆮ,ጓ.ᇀ,ጓ.ᇁ,ጓ.ᇂ,ጓ.ᇃ,ጓ.ᇄ,ጓ.ᇅ,ጓ.ᇇ,ጓ.ᇎ,ጓ.ᇈ,ጓ.ᇉ,ጓ.ᇊ,ጓ.ᇋ,ጓ.ᇌ,ጓ.ᇍ,ጓ.ᇏ,ጓ.ᆾ};}}static partial class ኑ{private class ᖁ{public
void ઈ(ଦ l,Ɠ ї){l.ଊ.ઈ(ї);}public void ޏ(ଦ l,Ɠ ї){l.Ҳ.ޏ(ї);}public void Ϭ(ଦ l,Ɠ ї){l.Ҳ.Ϭ(ї);}public void ŗ(ଦ l,Ɠ ї){l.Ư.ŗ(ї);
}public void ѱ(ଦ l,Ɠ ї){l.Ҳ.ѱ(ї);}public void Ѱ(ଦ l,Ɠ ї){l.Ҳ.Ѱ(ї);}public void Ҥ(ଦ l,Ɠ ї){l.Ҳ.Ҥ(ї);}public void ј(ଦ l,Ɠ ї
){l.Ҳ.ј(ї);}public void Ѳ(ଦ l,Ɠ ї){l.Ҳ.Ѳ(ї);}public void ѳ(ଦ l,Ɠ ї){l.Ҳ.ѳ(ї);}public void Ѥ(ଦ l,Ɠ ї){l.Ҳ.Ѥ(ї);}public
void Ѵ(ଦ l,Ɠ ї){l.Ҳ.Ѵ(ї);}public void ހ(ଦ l,Ɠ ї){l.Ҳ.ހ(ї);}public void ݥ(ଦ l,Ɠ ї){l.Ҳ.ݥ(ї);}public void ݨ(ଦ l,Ɠ ї){l.Ҳ.ݨ(ї);
}public void ݡ(ଦ l,Ɠ ї){l.Ҳ.ݡ(ї);}public void ݦ(ଦ l,Ɠ ї){l.Ҳ.ݦ(ї);}public void ъ(ଦ l,Ɠ ї){l.Ҳ.ъ(ї);}public void ݧ(ଦ l,Ɠ ї
){l.Ҳ.ݧ(ї);}public void ш(ଦ l,Ɠ ї){l.Ҳ.ш(ї);}public void ݱ(ଦ l,Ɠ ї){l.Ҳ.ݱ(ї);}public void ݯ(ଦ l,Ɠ ї){l.Ҳ.ݯ(ї);}public
void ݟ(ଦ l,Ɠ ї){l.Ҳ.ݟ(ї);}public void ݾ(ଦ l,Ɠ ї){l.Ҳ.ݾ(ї);}public void ݸ(ଦ l,Ɠ ї){l.Ҳ.ݸ(ї);}public void ݷ(ଦ l,Ɠ ї){l.Ҳ.ݷ(ї);
}public void ݶ(ଦ l,Ɠ ї){l.Ҳ.ݶ(ї);}public void ݪ(ଦ l,Ɠ ї){l.Ҳ.ݪ(ї);}public void ѯ(ଦ l,Ɠ ї){l.Ҳ.ѯ(ї);}public void Ѭ(ଦ l,Ɠ ї
){l.Ҳ.Ѭ(ї);}public void ѫ(ଦ l,Ɠ ї){l.Ҳ.ѫ(ї);}public void ٺ(ଦ l,Ɠ ї){l.Ҳ.ٺ(ї);}public void ݺ(ଦ l,Ɠ ї){l.Ҳ.ݺ(ї);}public
void ݻ(ଦ l,Ɠ ї){l.Ҳ.ݻ(ї);}public void ݽ(ଦ l,Ɠ ї){l.Ҳ.ݽ(ї);}public void ސ(ଦ l,Ɠ ї){l.Ҳ.ސ(ї);}public void ތ(ଦ l,Ɠ ї){l.Ҳ.ތ(ї);
}public void ޑ(ଦ l,Ɠ ї){l.Ҳ.ޑ(ї);}public void ދ(ଦ l,Ɠ ї){l.Ҳ.ދ(ї);}public void ޒ(ଦ l,Ɠ ї){l.Ҳ.ޒ(ї);}public void ގ(ଦ l,Ɠ ї
){l.Ҳ.ގ(ї);}public void ݵ(ଦ l,Ɠ ї){l.Ҳ.ݵ(ї);}public void ݮ(ଦ l,Ɠ ї){l.Ҳ.ݮ(ї);}public void ݩ(ଦ l,Ɠ ї){l.Ҳ.ݩ(ї);}public
void Ӱ(ଦ l,Ɠ ї){l.Ҳ.Ӱ(ї);}public void ߨ(ଦ l,Ɠ ї){l.Ҳ.ߨ(ї);}public void ߡ(ଦ l,Ɠ ї){l.Ҳ.ߡ(ї);}public void ߧ(ଦ l,Ɠ ї){l.Ҳ.ߧ(ї);
}public void ߠ(ଦ l,Ɠ ї){l.Ҳ.ߠ(ї);}public void ߟ(ଦ l,Ɠ ї){l.Ҳ.ߟ(ї);}public void ߞ(ଦ l,Ɠ ї){l.Ҳ.ߞ(ї);}public void ߣ(ଦ l,Ɠ ї
){l.Ҳ.ߣ(ї);}}}static partial class ኑ{public static ᘁ[]ዃ=new ᘁ[]{new ᘁ(-1,ᚏ.ᨊ,100,ᚏ.ᨋ,ɴ.ɳ,0,ɴ.ɳ,ᚏ.ᨐ,255,ɴ.Ȯ,ᚏ.ᚠ,ᚏ.ᨎ,ᚏ.ᨓ,ᚏ.
ᧅ,ɴ.ɔ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(56),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘐ|ళ.ᘗ|ళ.ᘘ|ళ.ᘄ,ᚏ.ᚠ),new ᘁ(3004,ᚏ.ᦧ,20,ᚏ.ᦅ,ɴ.Ȝ,8,ɴ.ɲ,ᚏ.ᤙ,200,ɴ.Ȱ,ᚏ.ᚠ,ᚏ.ᤖ,ᚏ.ᤛ,ᚏ.ᤏ,ɴ.
ɏ,8,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(56),100,0,ɴ.Ⱥ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᤆ),new ᘁ(9,ᚏ.ᤂ,30,ᚏ.ᤀ,ɴ.ț,8,ɴ.ɳ,ᚏ.ᥰ,170,ɴ.Ȱ,ᚏ.ᚠ,ᚏ.ᥪ,ᚏ.ᥱ,ᚏ.ᦁ,ɴ.ɐ,8,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ
(56),100,0,ɴ.Ⱥ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᥞ),new ᘁ(64,ᚏ.ᥙ,700,ᚏ.ᥗ,ɴ.Ɍ,8,ɴ.ɳ,ᚏ.ᬝ,10,ɴ.ȱ,ᚏ.ᚠ,ᚏ.ᬫ,ᚏ.ᬛ,ᚏ.ᚠ,ɴ.Ⱦ,15,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(56),500,0,ɴ.
ȵ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᭅ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘖ,ᚏ.ᚠ)
,new ᘁ(66,ᚏ.ᨲ,300,ᚏ.ᨴ,ɴ.ʤ,8,ɴ.ɳ,ᚏ.ᨥ,100,ɴ.Ȱ,ᚏ.ᨭ,ᚏ.ᨩ,ᚏ.ᨣ,ᚏ.ᚠ,ɴ.Ȼ,10,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(56),500,0,ɴ.ʥ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᩀ),new ᘁ(-1,ᚏ.
ш,1000,ᚏ.ᚠ,ɴ.ʣ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᨕ,ᚏ.ᚠ,ɴ.ɺ,10*Ꮖ.Ꮔ,Ꮖ.Ꭸ(11),Ꮖ.Ꭸ(8),100,10,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᭊ,
1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(67,ᚏ.ᬊ,600,ᚏ.ᬌ,ɴ.Ʌ,8,ɴ.ɳ,ᚏ.ᥕ
,80,ɴ.ȳ,ᚏ.ᚠ,ᚏ.ᩊ,ᚏ.ឈ,ᚏ.ᚠ,ɴ.ʪ,8,Ꮖ.Ꭸ(48),Ꮖ.Ꭸ(64),1000,0,ɴ.Ⱥ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᠰ),new ᘁ(-1,ᚏ.ᬅ,1000,ᚏ.ᚠ,ɴ.ȥ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ
.ᚠ,ᚏ.ᬇ,ᚏ.ᚠ,ɴ.Ȧ,20*Ꮖ.Ꮔ,Ꮖ.Ꭸ(6),Ꮖ.Ꭸ(8),100,8,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(65,ᚏ.ᠣ,70,ᚏ.ᠡ,ɴ.ț,8,ɴ.ɳ,ᚏ.ᠭ,170,ɴ.Ȱ,ᚏ.ᚠ,ᚏ.ឯ,ᚏ.ᡐ,
ᚏ.ᡏ,ɴ.ɐ,8,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(56),100,0,ɴ.Ⱥ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᡕ),new ᘁ(3001,ᚏ.ᡅ,60,ᚏ.ᡃ,ɴ.ș,8,ɴ.ɳ,ᚏ.ᠸ,200,ɴ.Ȱ,ᚏ.ᠻ,ᚏ.ᠻ,ᚏ.ឪ,ᚏ.ᝪ,ɴ.ɒ,8,Ꮖ.
Ꭸ(20),Ꮖ.Ꭸ(56),100,0,ɴ.ȹ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.គ),new ᘁ(3002,ᚏ.ᝤ,150,ᚏ.ᝢ,ɴ.ȗ,8,ɴ.Ɉ,ᚏ.ᝉ,180,ɴ.ȯ,ᚏ.ᝌ,ᚏ.ᚠ,ᚏ.ᝮ,ᚏ.ᚠ,ɴ.ɕ,10,Ꮖ.Ꭸ(30),Ꮖ.Ꭸ(
56),400,0,ɴ.ȸ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.វ),new ᘁ(58,ᚏ.ᝤ,150,ᚏ.ᝢ,ɴ.ȗ,8,ɴ.Ɉ,ᚏ.ᝉ,180,ɴ.ȯ,ᚏ.ᝌ,ᚏ.ᚠ,ᚏ.ᝮ,ᚏ.ᚠ,ɴ.ɕ,10,Ꮖ.Ꭸ(30),Ꮖ.Ꭸ(56),400,0,ɴ.ȸ
,ళ.ᘏ|ళ.ᘐ|ళ.ᘋ|ళ.ᘇ,ᚏ.វ),new ᘁ(3005,ᚏ.អ,400,ᚏ.ឣ,ɴ.Ȗ,8,ɴ.ɳ,ᚏ.ឧ,128,ɴ.ȯ,ᚏ.ᚠ,ᚏ.ឤ,ᚏ.ភ,ᚏ.ᚠ,ɴ.Ʉ,8,Ꮖ.Ꭸ(31),Ꮖ.Ꭸ(56),400,0,ɴ.ȸ,ళ.ᘏ|ళ.
ᘐ|ళ.ᘍ|ళ.ᘖ|ళ.ᘇ,ᚏ.ទ),new ᘁ(3003,ᚏ.ᡬ,1000,ᚏ.ᣁ,ɴ.ȕ,8,ɴ.ɳ,ᚏ.ᣌ,50,ɴ.ȯ,ᚏ.ᣉ,ᚏ.ᣉ,ᚏ.ᣎ,ᚏ.ᚠ,ɴ.ɂ,8,Ꮖ.Ꭸ(24),Ꮖ.Ꭸ(64),1000,0,ɴ.ȸ,ళ.ᘏ|ళ.ᘐ|
ళ.ᘇ,ᚏ.ᢺ),new ᘁ(-1,ᚏ.ឋ,1000,ᚏ.ᚠ,ɴ.ȥ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ញ,ᚏ.ᚠ,ɴ.Ȧ,15*Ꮖ.Ꮔ,Ꮖ.Ꭸ(6),Ꮖ.Ꭸ(8),100,8,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ
),new ᘁ(69,ᚏ.ᢳ,500,ᚏ.ᢱ,ɴ.ȴ,8,ɴ.ɳ,ᚏ.ᣩ,50,ɴ.ȯ,ᚏ.ᣦ,ᚏ.ᣦ,ᚏ.ᣲ,ᚏ.ᚠ,ɴ.Ƚ,8,Ꮖ.Ꭸ(24),Ꮖ.Ꭸ(64),1000,0,ɴ.ȸ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᣳ),new ᘁ(3006,
ᚏ.ᣜ,100,ᚏ.ᣚ,ɴ.ɳ,8,ɴ.ɇ,ᚏ.ᣔ,256,ɴ.ȯ,ᚏ.ᚠ,ᚏ.ᣘ,ᚏ.ᣒ,ᚏ.ᚠ,ɴ.Ȧ,8,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(56),50,3,ɴ.ȸ,ళ.ᘏ|ళ.ᘐ|ళ.ᘍ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(7,ᚏ.ᡯ,3000,ᚏ.
ᡱ,ɴ.ȓ,8,ɴ.ɱ,ᚏ.ᡦ,40,ɴ.ȯ,ᚏ.ᚠ,ᚏ.ᡪ,ᚏ.ᡤ,ᚏ.ᚠ,ɴ.ɀ,12,Ꮖ.Ꭸ(128),Ꮖ.Ꭸ(100),1000,0,ɴ.ȸ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᚠ),new ᘁ(68,ᚏ.ᢞ,500,ᚏ.ᢘ,ɴ.Ȫ,8,ɴ.
ɳ,ᚏ.ᢔ,128,ɴ.ȯ,ᚏ.ᚠ,ᚏ.ᢥ,ᚏ.ᢒ,ᚏ.ᚠ,ɴ.ȿ,12,Ꮖ.Ꭸ(64),Ꮖ.Ꭸ(64),600,0,ɴ.ȷ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᢋ),new ᘁ(16,ᚏ.ԓ,4000,ᚏ.Ԝ,ɴ.Ȕ,8,ɴ.ɳ,ᚏ.Ԇ,20,ɴ.
ȯ,ᚏ.ᚠ,ᚏ.Ԍ,ᚏ.ԅ,ᚏ.ᚠ,ɴ.Ɂ,16,Ꮖ.Ꭸ(40),Ꮖ.Ꭸ(110),1000,0,ɴ.ȸ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᚠ),new ᘁ(71,ᚏ.Ԟ,400,ᚏ.Հ,ɴ.Ɇ,8,ɴ.ɳ,ᚏ.Ղ,128,ɴ.Ȣ,ᚏ.ᚠ,ᚏ.Ծ,
ᚏ.Մ,ᚏ.ᚠ,ɴ.ȼ,8,Ꮖ.Ꭸ(31),Ꮖ.Ꭸ(56),400,0,ɴ.ȸ,ళ.ᘏ|ళ.ᘐ|ళ.ᘍ|ళ.ᘖ|ళ.ᘇ,ᚏ.Է),new ᘁ(84,ᚏ.Ա,50,ᚏ.Ԧ,ɴ.ʩ,8,ɴ.ɳ,ᚏ.Ӌ,170,ɴ.Ȱ,ᚏ.ᚠ,ᚏ.Ӽ,ᚏ.Ӎ,ᚏ.
Ӓ,ɴ.ʨ,8,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(56),100,0,ɴ.Ⱥ,ళ.ᘏ|ళ.ᘐ|ళ.ᘇ,ᚏ.ӂ),new ᘁ(72,ᚏ.ҽ,100,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ӭ,256,ɴ.ʧ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.Ҽ,ᚏ.ᚠ,ɴ.ʦ,0,Ꮖ.Ꭸ(
16),Ꮖ.Ꭸ(72),10000000,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ|ళ.ᘐ|ళ.ᘇ,ᚏ.ᚠ),new ᘁ(88,ᚏ.ӯ,250,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.Ӱ,255,ɴ.ʷ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.Ӳ,ᚏ.ᚠ,ɴ.ʸ,0,Ꮖ.Ꭸ(
16),Ꮖ.Ꭸ(16),10000000,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘐ,ᚏ.ᚠ),new ᘁ(89,ᚏ.ӵ,1000,ᚏ.Ӷ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(32),
100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘑ,ᚏ.ᚠ),new ᘁ(87,ᚏ.ᚠ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(32),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘑ,ᚏ.
ᚠ),new ᘁ(-1,ᚏ.Ӹ,1000,ᚏ.ᚠ,ɴ.ʴ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.Ȧ,10*Ꮖ.Ꮔ,Ꮖ.Ꭸ(6),Ꮖ.Ꭸ(32),100,3,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ|ళ.ᖮ,ᚏ.ᚠ)
,new ᘁ(-1,ᚏ.ӧ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(2035,ᚏ.Պ,20
,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.إ,ᚏ.ᚠ,ɴ.ɺ,0,Ꮖ.Ꭸ(10),Ꮖ.Ꭸ(42),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘐ|ళ.ᘊ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᙴ,1000,ᚏ.ᚠ,ɴ.ȥ,8,ɴ.ɳ
,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᙲ,ᚏ.ᚠ,ɴ.Ȧ,10*Ꮖ.Ꮔ,Ꮖ.Ꭸ(6),Ꮖ.Ꭸ(8),100,3,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᙯ,1000,ᚏ.ᚠ,ɴ.ȥ,8,ɴ.ɳ,ᚏ.ᚠ,0,
ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᙫ,ᚏ.ᚠ,ɴ.Ȧ,10*Ꮖ.Ꮔ,Ꮖ.Ꭸ(6),Ꮖ.Ꭸ(8),100,5,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ϸ,1000,ᚏ.ᚠ,ɴ.Ȓ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ
,ᚏ.ᚠ,ᚏ.ᦑ,ᚏ.ᚠ,ɴ.ɺ,20*Ꮖ.Ꮔ,Ꮖ.Ꭸ(11),Ꮖ.Ꭸ(8),100,20,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᠦ,1000,ᚏ.ᚠ,ɴ.ɫ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,
ᚏ.ᦖ,ᚏ.ᚠ,ɴ.Ȧ,25*Ꮖ.Ꮔ,Ꮖ.Ꭸ(13),Ꮖ.Ꭸ(8),100,5,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᦜ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᦝ,ᚏ.
ᚠ,ɴ.Ȥ,25*Ꮖ.Ꮔ,Ꮖ.Ꭸ(13),Ꮖ.Ꭸ(8),100,100,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ԍ,1000,ᚏ.ᚠ,ɴ.ɫ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.Ԏ,ᚏ.ᚠ,ɴ.
Ȧ,25*Ꮖ.Ꮔ,Ꮖ.Ꭸ(13),Ꮖ.Ꭸ(8),100,5,ɴ.ɳ,ళ.ᘒ|ళ.પ|ళ.ᘗ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᙸ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.
Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᙻ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,
0,ɴ.ɳ,ళ.ᘒ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.Ϩ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘖ,ᚏ.ᚠ),new
ᘁ(-1,ᚏ.ϧ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(14,ᚏ.ᚠ,1000,ᚏ.ᚠ,
ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘑ,ᚏ.ᚠ),new ᘁ(-1,ᚏ.ᦤ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.
ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(2018,ᚏ.Ӝ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.
ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2019,ᚏ.Ӛ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(
16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2014,ᚏ.ش,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘆ
,ᚏ.ᚠ),new ᘁ(2015,ᚏ.س,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘆ,ᚏ.ᚠ),new ᘁ(5,ᚏ.
ؠ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘄ,ᚏ.ᚠ),new ᘁ(13,ᚏ.ױ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ
.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘄ,ᚏ.ᚠ),new ᘁ(6,ᚏ.ת,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.
ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘄ,ᚏ.ᚠ),new ᘁ(39,ᚏ.פ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.
Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘄ,ᚏ.ᚠ),new ᘁ(38,ᚏ.צ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,
0,ɴ.ɳ,ళ.Ě|ళ.ᘄ,ᚏ.ᚠ),new ᘁ(40,ᚏ.ר,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘄ,ᚏ.ᚠ)
,new ᘁ(2011,ᚏ.ج,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2012,ᚏ.ض,1000
,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2013,ᚏ.ٱ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0
,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘆ,ᚏ.ᚠ),new ᘁ(2022,ᚏ.ٮ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ
,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘆ,ᚏ.ᚠ),new ᘁ(2023,ᚏ.ٳ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(
20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘆ,ᚏ.ᚠ),new ᘁ(2024,ᚏ.ٴ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0
,ɴ.ɳ,ళ.Ě|ళ.ᘆ,ᚏ.ᚠ),new ᘁ(2025,ᚏ.ك,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),
new ᘁ(2026,ᚏ.ق,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘆ,ᚏ.ᚠ),new ᘁ(2045,ᚏ.ؼ,
1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘆ,ᚏ.ᚠ),new ᘁ(83,ᚏ.Е,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.
ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě|ళ.ᘆ,ᚏ.ᚠ),new ᘁ(2007,ᚏ.З,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,
ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2048,ᚏ.ƌ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20
),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2010,ᚏ.غ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,
ళ.Ě,ᚏ.ᚠ),new ᘁ(2046,ᚏ.ع,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2047,
ᚏ.ظ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(17,ᚏ.ط,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ
,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2008,ᚏ.ע,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ
.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2049,ᚏ.ס,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20)
,Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(8,ᚏ.Ջ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,
ᚏ.ᚠ),new ᘁ(2006,ᚏ.զ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2002,ᚏ.է,
1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2005,ᚏ.ը,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,
0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2003,ᚏ.թ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.
ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2004,ᚏ.ժ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(
16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(2001,ᚏ.ի,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ
),new ᘁ(82,ᚏ.խ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.Ě,ᚏ.ᚠ),new ᘁ(85,ᚏ.ж,1000,ᚏ.
ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(86,ᚏ.л,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,
ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(2028,ᚏ.մ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,
0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(30,ᚏ.Վ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,
0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(31,ᚏ.Ս,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(
32,ᚏ.Ռ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(33,ᚏ.լ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ
.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(37,ᚏ.א,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ
.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(36,ᚏ.כ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ
.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(41,ᚏ.ה,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ
.ᚠ),new ᘁ(42,ᚏ.ן,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(43,ᚏ.ב,1000,
ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(44,ᚏ.ם,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.
ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(45,ᚏ.ֆ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,
0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(46,ᚏ.ւ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,
0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(55,ᚏ.վ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(
56,ᚏ.պ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(57,ᚏ.ә,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ
.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(47,ᚏ.Տ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ
.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(48,ᚏ.ד,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ
.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(34,ᚏ.ն,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ
),new ᘁ(35,ᚏ.ח,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(49,ᚏ.կ,1000,ᚏ.
ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(68),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(50,ᚏ.Փ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.
ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(84),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(51,ᚏ.Ւ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.
ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(84),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(52,ᚏ.Ց,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,
0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(68),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(53,ᚏ.Ր,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(
52),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(59,ᚏ.Փ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(84),100,0,ɴ.ɳ,ళ
.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(60,ᚏ.Ց,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(68),100,0,ɴ.ɳ,ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(
61,ᚏ.Ւ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(52),100,0,ɴ.ɳ,ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(62,ᚏ.Ր,1000,ᚏ.ᚠ,ɴ.ɳ
,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(52),100,0,ɴ.ɳ,ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(63,ᚏ.կ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ
.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(68),100,0,ɴ.ɳ,ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(22,ᚏ.ធ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ
,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(15,ᚏ.ᧆ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0
,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(18,ᚏ.ᥓ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(21,ᚏ.ល
,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(23,ᚏ.ᡮ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,
0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(20,ᚏ.ᝩ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.
ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(19,ᚏ.ᦀ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,
0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(10,ᚏ.ᦨ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(12,ᚏ.
ᦨ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(28,ᚏ.դ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ
,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(24,ᚏ.գ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ
,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,0,ᚏ.ᚠ),new ᘁ(27,ᚏ.բ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),
100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(29,ᚏ.ա,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ
(25,ᚏ.Ֆ,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(26,ᚏ.Օ,1000,ᚏ.ᚠ,ɴ.ɳ,8
,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(54,ᚏ.ג,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ
,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(32),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(70,ᚏ.ت,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16)
,Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘏ,ᚏ.ᚠ),new ᘁ(73,ᚏ.Ю,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(88),100,0,ɴ.ɳ,ళ.ᘏ
|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(74,ᚏ.Я,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(88),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),
new ᘁ(75,ᚏ.а,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(64),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(76,ᚏ.б,
1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(64),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(77,ᚏ.в,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.
ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(64),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(78,ᚏ.г,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.
ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(16),Ꮖ.Ꭸ(64),100,0,ɴ.ɳ,ళ.ᘏ|ళ.ᘕ|ళ.ᘖ,ᚏ.ᚠ),new ᘁ(79,ᚏ.к,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,
ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ,ᚏ.ᚠ),new ᘁ(80,ᚏ.д,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),
100,0,ɴ.ɳ,ళ.ᘒ,ᚏ.ᚠ),new ᘁ(81,ᚏ.е,1000,ᚏ.ᚠ,ɴ.ɳ,8,ɴ.ɳ,ᚏ.ᚠ,0,ɴ.ɳ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ᚏ.ᚠ,ɴ.ɳ,0,Ꮖ.Ꭸ(20),Ꮖ.Ꭸ(16),100,0,ɴ.ɳ,ళ.ᘒ,ᚏ.ᚠ)};}
static partial class ኑ{public static class ዂ{public static IReadOnlyList<IList<int>>ዀ=new int[][]{new int[]{30,75,120,90,165,
180,180,30,165},new int[]{90,90,90,120,90,360,240,30,170},new int[]{90,45,90,150,90,90,165,30,135},new int[]{165,255,135,
150,180,390,135,360,180}};public static IList<int>ኾ=new int[]{30,90,120,120,90,150,120,120,270,90,210,150,150,150,210,150,
420,150,210,150,240,150,180,150,150,300,330,420,300,180,120,30};}}static partial class ኑ{private class ኽ{public void ચ(ଦ l,
ђ Ð,Ɓ Ś){l.ଊ.ચ(Ð);}public void ઝ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ઝ(Ð,Ś);}public void ઞ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ઞ(Ð,Ś);}public void દ(ଦ l,ђ Ð,Ɓ
Ś){l.ଊ.દ(Ð,Ś);}public void ધ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ધ(Ð);}public void ણ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ણ(Ð);}public void ઌ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ઌ
(Ð);}public void ਤ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਤ(Ð);}public void ਮ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਮ(Ð);}public void ਯ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਯ(Ð);}
public void ਲ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਲ(Ð);}public void ਭ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਭ(Ð);}public void ਬ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਬ(Ð);}public void ਫ(ଦ
l,ђ Ð,Ɓ Ś){l.ଊ.ਫ(Ð);}public void ਪ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਪ(Ð);}public void ਰ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਰ(Ð,Ś);}public void ਨ(ଦ l,ђ Ð,Ɓ
Ś){l.ଊ.ਨ(Ð);}public void ਧ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਧ(Ð);}public void ન(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ન(Ð);}public void ਦ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਦ(Ð
);}public void ዘ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਥ(Ð);}public void ਵ(ଦ l,ђ Ð,Ɓ Ś){l.ଊ.ਵ(Ð);}}}static partial class ኑ{public static class
ጞ{public static int ŭ=30*ᱺ.ᱹ;public static int ū=60*ᱺ.ᱹ;public static int Ũ=120*ᱺ.ᱹ;public static int Ū=60*ᱺ.ᱹ;}}static
partial class ኑ{public static class ጝ{public static IReadOnlyList<Ꮥ>ጟ=new Ꮥ[]{ጓ.ጏ,new Ꮥ(
"please don't leave, there's more\ndemons to toast!"),new Ꮥ("let's beat it -- this is turning\ninto a bloodbath!"),new Ꮥ(
"i wouldn't leave if i were you.\ndos is much worse."),new Ꮥ("you're trying to say you like dos\nbetter than me, right?"),new Ꮥ(
"don't leave yet -- there's a\ndemon around that corner!"),new Ꮥ("ya know, next time you come in here\ni'm gonna toast ya."),new Ꮥ("go ahead and leave. see if i care.")};public
static IReadOnlyList<Ꮥ>ኾ=new Ꮥ[]{new Ꮥ("you want to quit?\nthen, thou hast lost an eighth!"),new Ꮥ(
"don't go now, there's a \ndimensional shambler waiting\nat the dos prompt!"),new Ꮥ("get outta here and go back\nto your boring programs."),new Ꮥ(
"if i were your boss, i'd \n deathmatch ya in a minute!"),new Ꮥ("look, bud. you leave now\nand you forfeit your body count!"),new Ꮥ(
"just leave. when you come\nback, i'll be waiting with a bat."),new Ꮥ("you're lucky i don't smack\nyou for thinking about leaving.")};public static IReadOnlyList<Ꮥ>ጛ=new Ꮥ[]{new Ꮥ(
"fuck you, pussy!\nget the fuck out!"),new Ꮥ("you quit and i'll jizz\nin your cystholes!"),new Ꮥ("if you leave, i'll make\nthe lord drink my jizz."),new Ꮥ(
"hey, ron! can we say\n'fuck' in the game?"),new Ꮥ("i'd leave: this is just\nmore monsters and levels.\nwhat a load."),new Ꮥ(
"suck it down, asshole!\nyou're a fucking wimp!"),new Ꮥ("don't quit now! we're \nstill spending your money!")};}}static partial class ኑ{public static Ꮥ[]ጚ=new Ꮥ[]{new Ꮥ
("none"),new Ꮥ("pistol"),new Ꮥ("shotgn"),new Ꮥ("sgcock"),new Ꮥ("dshtgn"),new Ꮥ("dbopn"),new Ꮥ("dbcls"),new Ꮥ("dbload"),
new Ꮥ("plasma"),new Ꮥ("bfg"),new Ꮥ("sawup"),new Ꮥ("sawidl"),new Ꮥ("sawful"),new Ꮥ("sawhit"),new Ꮥ("rlaunc"),new Ꮥ("rxplod")
,new Ꮥ("firsht"),new Ꮥ("firxpl"),new Ꮥ("pstart"),new Ꮥ("pstop"),new Ꮥ("doropn"),new Ꮥ("dorcls"),new Ꮥ("stnmov"),new Ꮥ(
"swtchn"),new Ꮥ("swtchx"),new Ꮥ("plpain"),new Ꮥ("dmpain"),new Ꮥ("popain"),new Ꮥ("vipain"),new Ꮥ("mnpain"),new Ꮥ("pepain"),new Ꮥ(
"slop"),new Ꮥ("itemup"),new Ꮥ("wpnup"),new Ꮥ("oof"),new Ꮥ("telept"),new Ꮥ("posit1"),new Ꮥ("posit2"),new Ꮥ("posit3"),new Ꮥ(
"bgsit1"),new Ꮥ("bgsit2"),new Ꮥ("sgtsit"),new Ꮥ("cacsit"),new Ꮥ("brssit"),new Ꮥ("cybsit"),new Ꮥ("spisit"),new Ꮥ("bspsit"),new Ꮥ(
"kntsit"),new Ꮥ("vilsit"),new Ꮥ("mansit"),new Ꮥ("pesit"),new Ꮥ("sklatk"),new Ꮥ("sgtatk"),new Ꮥ("skepch"),new Ꮥ("vilatk"),new Ꮥ(
"claw"),new Ꮥ("skeswg"),new Ꮥ("pldeth"),new Ꮥ("pdiehi"),new Ꮥ("podth1"),new Ꮥ("podth2"),new Ꮥ("podth3"),new Ꮥ("bgdth1"),new Ꮥ(
"bgdth2"),new Ꮥ("sgtdth"),new Ꮥ("cacdth"),new Ꮥ("skldth"),new Ꮥ("brsdth"),new Ꮥ("cybdth"),new Ꮥ("spidth"),new Ꮥ("bspdth"),new Ꮥ(
"vildth"),new Ꮥ("kntdth"),new Ꮥ("pedth"),new Ꮥ("skedth"),new Ꮥ("posact"),new Ꮥ("bgact"),new Ꮥ("dmact"),new Ꮥ("bspact"),new Ꮥ(
"bspwlk"),new Ꮥ("vilact"),new Ꮥ("noway"),new Ꮥ("barexp"),new Ꮥ("punch"),new Ꮥ("hoof"),new Ꮥ("metal"),new Ꮥ("chgun"),new Ꮥ("tink"
),new Ꮥ("bdopn"),new Ꮥ("bdcls"),new Ꮥ("itmbk"),new Ꮥ("flame"),new Ꮥ("flamst"),new Ꮥ("getpow"),new Ꮥ("bospit"),new Ꮥ(
"boscub"),new Ꮥ("bossit"),new Ꮥ("bospn"),new Ꮥ("bosdth"),new Ꮥ("manatk"),new Ꮥ("mandth"),new Ꮥ("sssit"),new Ꮥ("ssdth"),new Ꮥ(
"keenpn"),new Ꮥ("keendt"),new Ꮥ("skeact"),new Ꮥ("skesit"),new Ꮥ("skeatk"),new Ꮥ("radio")};}static partial class ኑ{public static
Ꮥ[]ጙ=new Ꮥ[]{new Ꮥ("TROO"),new Ꮥ("SHTG"),new Ꮥ("PUNG"),new Ꮥ("PISG"),new Ꮥ("PISF"),new Ꮥ("SHTF"),new Ꮥ("SHT2"),new Ꮥ(
"CHGG"),new Ꮥ("CHGF"),new Ꮥ("MISG"),new Ꮥ("MISF"),new Ꮥ("SAWG"),new Ꮥ("PLSG"),new Ꮥ("PLSF"),new Ꮥ("BFGG"),new Ꮥ("BFGF"),new Ꮥ(
"BLUD"),new Ꮥ("PUFF"),new Ꮥ("BAL1"),new Ꮥ("BAL2"),new Ꮥ("PLSS"),new Ꮥ("PLSE"),new Ꮥ("MISL"),new Ꮥ("BFS1"),new Ꮥ("BFE1"),new Ꮥ(
"BFE2"),new Ꮥ("TFOG"),new Ꮥ("IFOG"),new Ꮥ("PLAY"),new Ꮥ("POSS"),new Ꮥ("SPOS"),new Ꮥ("VILE"),new Ꮥ("FIRE"),new Ꮥ("FATB"),new Ꮥ(
"FBXP"),new Ꮥ("SKEL"),new Ꮥ("MANF"),new Ꮥ("FATT"),new Ꮥ("CPOS"),new Ꮥ("SARG"),new Ꮥ("HEAD"),new Ꮥ("BAL7"),new Ꮥ("BOSS"),new Ꮥ(
"BOS2"),new Ꮥ("SKUL"),new Ꮥ("SPID"),new Ꮥ("BSPI"),new Ꮥ("APLS"),new Ꮥ("APBX"),new Ꮥ("CYBR"),new Ꮥ("PAIN"),new Ꮥ("SSWV"),new Ꮥ(
"KEEN"),new Ꮥ("BBRN"),new Ꮥ("BOSF"),new Ꮥ("ARM1"),new Ꮥ("ARM2"),new Ꮥ("BAR1"),new Ꮥ("BEXP"),new Ꮥ("FCAN"),new Ꮥ("BON1"),new Ꮥ(
"BON2"),new Ꮥ("BKEY"),new Ꮥ("RKEY"),new Ꮥ("YKEY"),new Ꮥ("BSKU"),new Ꮥ("RSKU"),new Ꮥ("YSKU"),new Ꮥ("STIM"),new Ꮥ("MEDI"),new Ꮥ(
"SOUL"),new Ꮥ("PINV"),new Ꮥ("PSTR"),new Ꮥ("PINS"),new Ꮥ("MEGA"),new Ꮥ("SUIT"),new Ꮥ("PMAP"),new Ꮥ("PVIS"),new Ꮥ("CLIP"),new Ꮥ(
"AMMO"),new Ꮥ("ROCK"),new Ꮥ("BROK"),new Ꮥ("CELL"),new Ꮥ("CELP"),new Ꮥ("SHEL"),new Ꮥ("SBOX"),new Ꮥ("BPAK"),new Ꮥ("BFUG"),new Ꮥ(
"MGUN"),new Ꮥ("CSAW"),new Ꮥ("LAUN"),new Ꮥ("PLAS"),new Ꮥ("SHOT"),new Ꮥ("SGN2"),new Ꮥ("COLU"),new Ꮥ("SMT2"),new Ꮥ("GOR1"),new Ꮥ(
"POL2"),new Ꮥ("POL5"),new Ꮥ("POL4"),new Ꮥ("POL3"),new Ꮥ("POL1"),new Ꮥ("POL6"),new Ꮥ("GOR2"),new Ꮥ("GOR3"),new Ꮥ("GOR4"),new Ꮥ(
"GOR5"),new Ꮥ("SMIT"),new Ꮥ("COL1"),new Ꮥ("COL2"),new Ꮥ("COL3"),new Ꮥ("COL4"),new Ꮥ("CAND"),new Ꮥ("CBRA"),new Ꮥ("COL6"),new Ꮥ(
"TRE1"),new Ꮥ("TRE2"),new Ꮥ("ELEC"),new Ꮥ("CEYE"),new Ꮥ("FSKU"),new Ꮥ("COL5"),new Ꮥ("TBLU"),new Ꮥ("TGRN"),new Ꮥ("TRED"),new Ꮥ(
"SMBT"),new Ꮥ("SMGT"),new Ꮥ("SMRT"),new Ꮥ("HDB1"),new Ꮥ("HDB2"),new Ꮥ("HDB3"),new Ꮥ("HDB4"),new Ꮥ("HDB5"),new Ꮥ("HDB6"),new Ꮥ(
"POB1"),new Ꮥ("POB2"),new Ꮥ("BRS1"),new Ꮥ("TLMP"),new Ꮥ("TLP2")};}static partial class ኑ{private static ኽ ጘ=new ኽ();private
static ᖁ ጕ=new ᖁ();public static Ш[]ጔ=new Ш[]{new Ш(0,э.བྷ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(1,э.བ,4,0,ጘ.ચ,null,ᚏ.ᚠ,0,0),new Ш(2,э.
ཕ,0,1,ጘ.ઝ,null,ᚏ.ધ,0,0),new Ш(3,э.ཕ,0,1,ጘ.ઞ,null,ᚏ.ᚺ,0,0),new Ш(4,э.ཕ,0,1,ጘ.દ,null,ᚏ.ᚻ,0,0),new Ш(5,э.ཕ,1,4,null,null,ᚏ.ᚽ
,0,0),new Ш(6,э.ཕ,2,4,ጘ.ધ,null,ᚏ.ᚾ,0,0),new Ш(7,э.ཕ,3,5,null,null,ᚏ.ᚿ,0,0),new Ш(8,э.ཕ,2,4,null,null,ᚏ.ᛀ,0,0),new Ш(9,э.ཕ
,1,5,ጘ.ણ,null,ᚏ.ધ,0,0),new Ш(10,э.པ,0,1,ጘ.ઝ,null,ᚏ.ਸ,0,0),new Ш(11,э.པ,0,1,ጘ.ઞ,null,ᚏ.ᛇ,0,0),new Ш(12,э.པ,0,1,ጘ.દ,null,ᚏ.
ᛁ,0,0),new Ш(13,э.པ,0,4,null,null,ᚏ.ᛃ,0,0),new Ш(14,э.པ,1,6,ጘ.ઌ,null,ᚏ.ᛄ,0,0),new Ш(15,э.པ,2,4,null,null,ᚏ.ᛅ,0,0),new Ш(
16,э.པ,1,5,ጘ.ણ,null,ᚏ.ਸ,0,0),new Ш(17,э.ན,32768,7,ጘ.ਤ,null,ᚏ.ᚨ,0,0),new Ш(18,э.བ,0,1,ጘ.ઝ,null,ᚏ.ᛈ,0,0),new Ш(19,э.བ,0,1,ጘ.
ઞ,null,ᚏ.ᚹ,0,0),new Ш(20,э.བ,0,1,ጘ.દ,null,ᚏ.ᚸ,0,0),new Ш(21,э.བ,0,3,null,null,ᚏ.ᚶ,0,0),new Ш(22,э.བ,0,7,ጘ.ਮ,null,ᚏ.ᚵ,0,0)
,new Ш(23,э.བ,1,5,null,null,ᚏ.ᚴ,0,0),new Ш(24,э.བ,2,5,null,null,ᚏ.ᚳ,0,0),new Ш(25,э.བ,3,4,null,null,ᚏ.ᚲ,0,0),new Ш(26,э.བ
,2,5,null,null,ᚏ.ᚱ,0,0),new Ш(27,э.བ,1,5,null,null,ᚏ.ᚰ,0,0),new Ш(28,э.བ,0,3,null,null,ᚏ.ᚯ,0,0),new Ш(29,э.བ,0,7,ጘ.ણ,null
,ᚏ.ᛈ,0,0),new Ш(30,э.དྷ,32768,4,ጘ.ਤ,null,ᚏ.ᚭ,0,0),new Ш(31,э.དྷ,32769,3,ጘ.ਯ,null,ᚏ.ᚨ,0,0),new Ш(32,э.ད,0,1,ጘ.ઝ,null,ᚏ.ᚬ,0,0
),new Ш(33,э.ད,0,1,ጘ.ઞ,null,ᚏ.ᚫ,0,0),new Ш(34,э.ད,0,1,ጘ.દ,null,ᚏ.ᚪ,0,0),new Ш(35,э.ད,0,3,null,null,ᚏ.ᚎ,0,0),new Ш(36,э.ད,
0,7,ጘ.ਲ,null,ᚏ.ᚍ,0,0),new Ш(37,э.ད,1,7,null,null,ᚏ.ᙋ,0,0),new Ш(38,э.ད,2,7,ጘ.ਭ,null,ᚏ.ᙚ,0,0),new Ш(39,э.ད,3,7,ጘ.ਬ,null,ᚏ.
ᙛ,0,0),new Ш(40,э.ད,4,7,null,null,ᚏ.ᙜ,0,0),new Ш(41,э.ད,5,7,ጘ.ਫ,null,ᚏ.ᙝ,0,0),new Ш(42,э.ད,6,6,null,null,ᚏ.ᙞ,0,0),new Ш(
43,э.ད,7,6,ጘ.ਪ,null,ᚏ.ᙟ,0,0),new Ш(44,э.ད,0,5,ጘ.ણ,null,ᚏ.ᚬ,0,0),new Ш(45,э.ད,1,7,null,null,ᚏ.ᙨ,0,0),new Ш(46,э.ད,0,3,null,
null,ᚏ.ᚫ,0,0),new Ш(47,э.ད,32776,5,ጘ.ਤ,null,ᚏ.ᙣ,0,0),new Ш(48,э.ད,32777,4,ጘ.ਯ,null,ᚏ.ᚨ,0,0),new Ш(49,э.ཐ,0,1,ጘ.ઝ,null,ᚏ.ᙤ,0,
0),new Ш(50,э.ཐ,0,1,ጘ.ઞ,null,ᚏ.ᙥ,0,0),new Ш(51,э.ཐ,0,1,ጘ.દ,null,ᚏ.ᙦ,0,0),new Ш(52,э.ཐ,0,4,ጘ.ਰ,null,ᚏ.ᙩ,0,0),new Ш(53,э.ཐ,
1,4,ጘ.ਰ,null,ᚏ.ᙙ,0,0),new Ш(54,э.ཐ,1,0,ጘ.ણ,null,ᚏ.ᙤ,0,0),new Ш(55,э.ཏ,32768,5,ጘ.ਤ,null,ᚏ.ᚨ,0,0),new Ш(56,э.ཏ,32769,5,ጘ.ਯ,
null,ᚏ.ᚨ,0,0),new Ш(57,э.ཎ,0,1,ጘ.ઝ,null,ᚏ.પ,0,0),new Ш(58,э.ཎ,0,1,ጘ.ઞ,null,ᚏ.ᙖ,0,0),new Ш(59,э.ཎ,0,1,ጘ.દ,null,ᚏ.ᙕ,0,0),new Ш
(60,э.ཎ,1,8,ጘ.ਨ,null,ᚏ.ᙓ,0,0),new Ш(61,э.ཎ,1,12,ጘ.ਧ,null,ᚏ.ᙒ,0,0),new Ш(62,э.ཎ,1,0,ጘ.ણ,null,ᚏ.પ,0,0),new Ш(63,э.ཌྷ,32768,3
,ጘ.ਤ,null,ᚏ.ᙐ,0,0),new Ш(64,э.ཌྷ,32769,4,null,null,ᚏ.ᙏ,0,0),new Ш(65,э.ཌྷ,32770,4,ጘ.ਯ,null,ᚏ.ᙎ,0,0),new Ш(66,э.ཌྷ,32771,4,ጘ.
ਯ,null,ᚏ.ᚨ,0,0),new Ш(67,э.ཌ,2,4,ጘ.ઝ,null,ᚏ.ᙍ,0,0),new Ш(68,э.ཌ,3,4,ጘ.ઝ,null,ᚏ.ન,0,0),new Ш(69,э.ཌ,2,1,ጘ.ઞ,null,ᚏ.ᙌ,0,0),
new Ш(70,э.ཌ,2,1,ጘ.દ,null,ᚏ.ᙠ,0,0),new Ш(71,э.ཌ,0,4,ጘ.ન,null,ᚏ.ᚄ,0,0),new Ш(72,э.ཌ,1,4,ጘ.ન,null,ᚏ.ᙽ,0,0),new Ш(73,э.ཌ,1,0,ጘ
.ણ,null,ᚏ.ન,0,0),new Ш(74,э.ཋ,0,1,ጘ.ઝ,null,ᚏ.Ϲ,0,0),new Ш(75,э.ཋ,0,1,ጘ.ઞ,null,ᚏ.ᙾ,0,0),new Ш(76,э.ཋ,0,1,ጘ.દ,null,ᚏ.ᙿ,0,0)
,new Ш(77,э.ཋ,0,3,ጘ.ਦ,null,ᚏ.ᚂ,0,0),new Ш(78,э.ཋ,1,20,ጘ.ણ,null,ᚏ.Ϲ,0,0),new Ш(79,э.ཊ,32768,4,ጘ.ਤ,null,ᚏ.ᚨ,0,0),new Ш(80,э
.ཊ,32769,4,ጘ.ਤ,null,ᚏ.ᚨ,0,0),new Ш(81,э.ཉ,0,1,ጘ.ઝ,null,ᚏ.Ϻ,0,0),new Ш(82,э.ཉ,0,1,ጘ.ઞ,null,ᚏ.ᚆ,0,0),new Ш(83,э.ཉ,0,1,ጘ.દ,
null,ᚏ.ᚇ,0,0),new Ш(84,э.ཉ,0,20,ጘ.ዘ,null,ᚏ.ᚉ,0,0),new Ш(85,э.ཉ,1,10,ጘ.ਨ,null,ᚏ.ᚊ,0,0),new Ш(86,э.ཉ,1,10,ጘ.ਵ,null,ᚏ.ᚋ,0,0),
new Ш(87,э.ཉ,1,20,ጘ.ણ,null,ᚏ.Ϻ,0,0),new Ш(88,э.ໆ,32768,11,ጘ.ਤ,null,ᚏ.ᙼ,0,0),new Ш(89,э.ໆ,32769,6,ጘ.ਯ,null,ᚏ.ᚨ,0,0),new Ш(90
,э.ໄ,2,8,null,null,ᚏ.ᙺ,0,0),new Ш(91,э.ໄ,1,8,null,null,ᚏ.ᙹ,0,0),new Ш(92,э.ໄ,0,8,null,null,ᚏ.ᚠ,0,0),new Ш(93,э.น,32768,4,
null,null,ᚏ.ᙷ,0,0),new Ш(94,э.น,1,4,null,null,ᚏ.ᙶ,0,0),new Ш(95,э.น,2,4,null,null,ᚏ.ᙵ,0,0),new Ш(96,э.น,3,4,null,null,ᚏ.ᚠ,0,
0),new Ш(97,э.ห,32768,4,null,null,ᚏ.ᙳ,0,0),new Ш(98,э.ห,32769,4,null,null,ᚏ.ᙴ,0,0),new Ш(99,э.ห,32770,6,null,null,ᚏ.ᙱ,0,0
),new Ш(100,э.ห,32771,6,null,null,ᚏ.ᙰ,0,0),new Ш(101,э.ห,32772,6,null,null,ᚏ.ᚠ,0,0),new Ш(102,э.ฬ,32768,4,null,null,ᚏ.ᙬ,0
,0),new Ш(103,э.ฬ,32769,4,null,null,ᚏ.ᙯ,0,0),new Ш(104,э.ฬ,32770,6,null,null,ᚏ.ᘲ,0,0),new Ш(105,э.ฬ,32771,6,null,null,ᚏ.ᝆ
,0,0),new Ш(106,э.ฬ,32772,6,null,null,ᚏ.ᚠ,0,0),new Ш(107,э.อ,32768,6,null,null,ᚏ.ᦕ,0,0),new Ш(108,э.อ,32769,6,null,null,ᚏ
.ᠦ,0,0),new Ш(109,э.ฮ,32768,4,null,null,ᚏ.ᦗ,0,0),new Ш(110,э.ฮ,32769,4,null,null,ᚏ.ᦘ,0,0),new Ш(111,э.ฮ,32770,4,null,null
,ᚏ.ᦙ,0,0),new Ш(112,э.ฮ,32771,4,null,null,ᚏ.ᦚ,0,0),new Ш(113,э.ฮ,32772,4,null,null,ᚏ.ᚠ,0,0),new Ш(114,э.ฯ,32768,1,null,
null,ᚏ.ϸ,0,0),new Ш(115,э.ะ,32768,4,null,null,ᚏ.ᦣ,0,0),new Ш(116,э.ะ,32769,4,null,null,ᚏ.ᦜ,0,0),new Ш(117,э.ำ,32768,8,null,
null,ᚏ.ᦞ,0,0),new Ш(118,э.ำ,32769,8,null,null,ᚏ.ᦟ,0,0),new Ш(119,э.ำ,32770,8,null,ጕ.ઈ,ᚏ.ᦠ,0,0),new Ш(120,э.ำ,32771,8,null,
null,ᚏ.ᦡ,0,0),new Ш(121,э.ำ,32772,8,null,null,ᚏ.ᦢ,0,0),new Ш(122,э.ำ,32773,8,null,null,ᚏ.ᚠ,0,0),new Ш(123,э.ๆ,32768,8,null,
null,ᚏ.ᦔ,0,0),new Ш(124,э.ๆ,32769,8,null,null,ᚏ.ᦓ,0,0),new Ш(125,э.ๆ,32770,8,null,null,ᚏ.ᦒ,0,0),new Ш(126,э.ๆ,32771,8,null,
null,ᚏ.ᚠ,0,0),new Ш(127,э.ฯ,32769,8,null,ጕ.ޏ,ᚏ.ᦐ,0,0),new Ш(128,э.ฯ,32770,6,null,null,ᚏ.ᦏ,0,0),new Ш(129,э.ฯ,32771,4,null,
null,ᚏ.ᚠ,0,0),new Ш(130,э.เ,32768,6,null,null,ᚏ.ᦎ,0,0),new Ш(131,э.เ,32769,6,null,null,ᚏ.ᦍ,0,0),new Ш(132,э.เ,32768,6,null,
null,ᚏ.ᦌ,0,0),new Ш(133,э.เ,32769,6,null,null,ᚏ.ᦋ,0,0),new Ш(134,э.เ,32770,6,null,null,ᚏ.ᦊ,0,0),new Ш(135,э.เ,32771,6,null,
null,ᚏ.ᦉ,0,0),new Ш(136,э.เ,32772,6,null,null,ᚏ.ᦈ,0,0),new Ш(137,э.เ,32773,6,null,null,ᚏ.ᦇ,0,0),new Ш(138,э.เ,32774,6,null,
null,ᚏ.ᦆ,0,0),new Ш(139,э.เ,32775,6,null,null,ᚏ.ᦛ,0,0),new Ш(140,э.เ,32776,6,null,null,ᚏ.ᦥ,0,0),new Ш(141,э.เ,32777,6,null,
null,ᚏ.ᚠ,0,0),new Ш(142,э.แ,32768,6,null,null,ᚏ.ᨄ,0,0),new Ш(143,э.แ,32769,6,null,null,ᚏ.ᨅ,0,0),new Ш(144,э.แ,32768,6,null,
null,ᚏ.ᨆ,0,0),new Ш(145,э.แ,32769,6,null,null,ᚏ.ᨇ,0,0),new Ш(146,э.แ,32770,6,null,null,ᚏ.ᨈ,0,0),new Ш(147,э.แ,32771,6,null,
null,ᚏ.ᨉ,0,0),new Ш(148,э.แ,32772,6,null,null,ᚏ.ᚠ,0,0),new Ш(149,э.โ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(150,э.โ,0,4,null,null,ᚏ.ᨒ
,0,0),new Ш(151,э.โ,1,4,null,null,ᚏ.ᨌ,0,0),new Ш(152,э.โ,2,4,null,null,ᚏ.ᨍ,0,0),new Ш(153,э.โ,3,4,null,null,ᚏ.ᨋ,0,0),new
Ш(154,э.โ,4,12,null,null,ᚏ.ᨊ,0,0),new Ш(155,э.โ,32773,6,null,null,ᚏ.ᨎ,0,0),new Ш(156,э.โ,6,4,null,null,ᚏ.ᨑ,0,0),new Ш(157
,э.โ,6,4,null,ጕ.Ϭ,ᚏ.ᨊ,0,0),new Ш(158,э.โ,7,10,null,null,ᚏ.ᨃ,0,0),new Ш(159,э.โ,8,10,null,ጕ.ŗ,ᚏ.ᨂ,0,0),new Ш(160,э.โ,9,10,
null,ጕ.ѱ,ᚏ.ᨁ,0,0),new Ш(161,э.โ,10,10,null,null,ᚏ.ᨀ,0,0),new Ш(162,э.โ,11,10,null,null,ᚏ.ᧇ,0,0),new Ш(163,э.โ,12,10,null,
null,ᚏ.ᧆ,0,0),new Ш(164,э.โ,13,-1,null,null,ᚏ.ᚠ,0,0),new Ш(165,э.โ,14,5,null,null,ᚏ.ᧄ,0,0),new Ш(166,э.โ,15,5,null,ጕ.Ѱ,ᚏ.ᧃ,0
,0),new Ш(167,э.โ,16,5,null,ጕ.ѱ,ᚏ.ᧂ,0,0),new Ш(168,э.โ,17,5,null,null,ᚏ.ᧁ,0,0),new Ш(169,э.โ,18,5,null,null,ᚏ.ᦫ,0,0),new
Ш(170,э.โ,19,5,null,null,ᚏ.ᦪ,0,0),new Ш(171,э.โ,20,5,null,null,ᚏ.ᦩ,0,0),new Ш(172,э.โ,21,5,null,null,ᚏ.ᦨ,0,0),new Ш(173,э
.โ,22,-1,null,null,ᚏ.ᚠ,0,0),new Ш(174,э.ใ,0,10,null,ጕ.Ҥ,ᚏ.ᦦ,0,0),new Ш(175,э.ใ,1,10,null,ጕ.Ҥ,ᚏ.ᦧ,0,0),new Ш(176,э.ใ,0,4,
null,ጕ.ј,ᚏ.ᦄ,0,0),new Ш(177,э.ใ,0,4,null,ጕ.ј,ᚏ.ᣵ,0,0),new Ш(178,э.ใ,1,4,null,ጕ.ј,ᚏ.ᤑ,0,0),new Ш(179,э.ใ,1,4,null,ጕ.ј,ᚏ.ᤒ,0,0
),new Ш(180,э.ใ,2,4,null,ጕ.ј,ᚏ.ᤓ,0,0),new Ш(181,э.ใ,2,4,null,ጕ.ј,ᚏ.ᤔ,0,0),new Ш(182,э.ใ,3,4,null,ጕ.ј,ᚏ.ᤕ,0,0),new Ш(183,э
.ใ,3,4,null,ጕ.ј,ᚏ.ᦅ,0,0),new Ш(184,э.ใ,4,10,null,ጕ.Ѳ,ᚏ.ᤘ,0,0),new Ш(185,э.ใ,5,8,null,ጕ.ѳ,ᚏ.ᥒ,0,0),new Ш(186,э.ใ,4,8,null,
null,ᚏ.ᦅ,0,0),new Ш(187,э.ใ,6,3,null,null,ᚏ.ᤚ,0,0),new Ш(188,э.ใ,6,3,null,ጕ.Ϭ,ᚏ.ᦅ,0,0),new Ш(189,э.ใ,7,5,null,null,ᚏ.ᤜ,0,0),
new Ш(190,э.ใ,8,5,null,ጕ.Ѥ,ᚏ.ᥐ,0,0),new Ш(191,э.ใ,9,5,null,ጕ.ѱ,ᚏ.ᥑ,0,0),new Ш(192,э.ใ,10,5,null,null,ᚏ.ᥓ,0,0),new Ш(193,э.ใ
,11,-1,null,null,ᚏ.ᚠ,0,0),new Ш(194,э.ใ,12,5,null,null,ᚏ.ᤎ,0,0),new Ш(195,э.ใ,13,5,null,ጕ.Ѱ,ᚏ.ᤍ,0,0),new Ш(196,э.ใ,14,5,
null,ጕ.ѱ,ᚏ.ᤌ,0,0),new Ш(197,э.ใ,15,5,null,null,ᚏ.ᤋ,0,0),new Ш(198,э.ใ,16,5,null,null,ᚏ.ᤊ,0,0),new Ш(199,э.ใ,17,5,null,null,ᚏ
.ᤉ,0,0),new Ш(200,э.ใ,18,5,null,null,ᚏ.ᤈ,0,0),new Ш(201,э.ใ,19,5,null,null,ᚏ.ᤇ,0,0),new Ш(202,э.ใ,20,-1,null,null,ᚏ.ᚠ,0,0
),new Ш(203,э.ใ,10,5,null,null,ᚏ.ᤅ,0,0),new Ш(204,э.ใ,9,5,null,null,ᚏ.ᤄ,0,0),new Ш(205,э.ใ,8,5,null,null,ᚏ.ᤃ,0,0),new Ш(
206,э.ใ,7,5,null,null,ᚏ.ᦅ,0,0),new Ш(207,э.ไ,0,10,null,ጕ.Ҥ,ᚏ.ᤁ,0,0),new Ш(208,э.ไ,1,10,null,ጕ.Ҥ,ᚏ.ᤂ,0,0),new Ш(209,э.ไ,0,3,
null,ጕ.ј,ᚏ.ᤗ,0,0),new Ш(210,э.ไ,0,3,null,ጕ.ј,ᚏ.ᥔ,0,0),new Ш(211,э.ไ,1,3,null,ጕ.ј,ᚏ.ᥭ,0,0),new Ш(212,э.ไ,1,3,null,ጕ.ј,ᚏ.ᥦ,0,0
),new Ш(213,э.ไ,2,3,null,ጕ.ј,ᚏ.ᥧ,0,0),new Ш(214,э.ไ,2,3,null,ጕ.ј,ᚏ.ᥨ,0,0),new Ш(215,э.ไ,3,3,null,ጕ.ј,ᚏ.ᥩ,0,0),new Ш(216,э
.ไ,3,3,null,ጕ.ј,ᚏ.ᤀ,0,0),new Ш(217,э.ไ,4,10,null,ጕ.Ѳ,ᚏ.ᥫ,0,0),new Ш(218,э.ไ,32773,10,null,ጕ.Ѵ,ᚏ.ᥬ,0,0),new Ш(219,э.ไ,4,10
,null,null,ᚏ.ᤀ,0,0),new Ш(220,э.ไ,6,3,null,null,ᚏ.ᦂ,0,0),new Ш(221,э.ไ,6,3,null,ጕ.Ϭ,ᚏ.ᤀ,0,0),new Ш(222,э.ไ,7,5,null,null,
ᚏ.ᥲ,0,0),new Ш(223,э.ไ,8,5,null,ጕ.Ѥ,ᚏ.ᥳ,0,0),new Ш(224,э.ไ,9,5,null,ጕ.ѱ,ᚏ.ᥴ,0,0),new Ш(225,э.ไ,10,5,null,null,ᚏ.ᦀ,0,0),
new Ш(226,э.ไ,11,-1,null,null,ᚏ.ᚠ,0,0),new Ш(227,э.ไ,12,5,null,null,ᚏ.ᦃ,0,0),new Ш(228,э.ไ,13,5,null,ጕ.Ѱ,ᚏ.ᥥ,0,0),new Ш(229
,э.ไ,14,5,null,ጕ.ѱ,ᚏ.ᥤ,0,0),new Ш(230,э.ไ,15,5,null,null,ᚏ.ᥣ,0,0),new Ш(231,э.ไ,16,5,null,null,ᚏ.ᥢ,0,0),new Ш(232,э.ไ,17,
5,null,null,ᚏ.ᥡ,0,0),new Ш(233,э.ไ,18,5,null,null,ᚏ.ᥠ,0,0),new Ш(234,э.ไ,19,5,null,null,ᚏ.ᥟ,0,0),new Ш(235,э.ไ,20,-1,null
,null,ᚏ.ᚠ,0,0),new Ш(236,э.ไ,11,5,null,null,ᚏ.ᥝ,0,0),new Ш(237,э.ไ,10,5,null,null,ᚏ.ᥜ,0,0),new Ш(238,э.ไ,9,5,null,null,ᚏ.
ᥛ,0,0),new Ш(239,э.ไ,8,5,null,null,ᚏ.ᥚ,0,0),new Ш(240,э.ไ,7,5,null,null,ᚏ.ᤀ,0,0),new Ш(241,э.ๅ,0,10,null,ጕ.Ҥ,ᚏ.ᥘ,0,0),new
Ш(242,э.ๅ,1,10,null,ጕ.Ҥ,ᚏ.ᥙ,0,0),new Ш(243,э.ๅ,0,2,null,ጕ.ހ,ᚏ.ᥖ,0,0),new Ш(244,э.ๅ,0,2,null,ጕ.ހ,ᚏ.ᤐ,0,0),new Ш(245,э.ๅ,1,
2,null,ጕ.ހ,ᚏ.ᨔ,0,0),new Ш(246,э.ๅ,1,2,null,ጕ.ހ,ᚏ.ᨯ,0,0),new Ш(247,э.ๅ,2,2,null,ጕ.ހ,ᚏ.ᬣ,0,0),new Ш(248,э.ๅ,2,2,null,ጕ.ހ,ᚏ.
ᬤ,0,0),new Ш(249,э.ๅ,3,2,null,ጕ.ހ,ᚏ.ᬥ,0,0),new Ш(250,э.ๅ,3,2,null,ጕ.ހ,ᚏ.ᬦ,0,0),new Ш(251,э.ๅ,4,2,null,ጕ.ހ,ᚏ.ᬧ,0,0),new Ш(
252,э.ๅ,4,2,null,ጕ.ހ,ᚏ.ᬨ,0,0),new Ш(253,э.ๅ,5,2,null,ጕ.ހ,ᚏ.ᬩ,0,0),new Ш(254,э.ๅ,5,2,null,ጕ.ހ,ᚏ.ᥗ,0,0),new Ш(255,э.ๅ,32774,0
,null,ጕ.ݥ,ᚏ.ᬲ,0,0),new Ш(256,э.ๅ,32774,10,null,ጕ.Ѳ,ᚏ.ᬬ,0,0),new Ш(257,э.ๅ,32775,8,null,ጕ.ݨ,ᚏ.ᬭ,0,0),new Ш(258,э.ๅ,32776,8
,null,ጕ.Ѳ,ᚏ.ᬮ,0,0),new Ш(259,э.ๅ,32777,8,null,ጕ.Ѳ,ᚏ.ᬯ,0,0),new Ш(260,э.ๅ,32778,8,null,ጕ.Ѳ,ᚏ.ᬰ,0,0),new Ш(261,э.ๅ,32779,8,
null,ጕ.Ѳ,ᚏ.ᬱ,0,0),new Ш(262,э.ๅ,32780,8,null,ጕ.Ѳ,ᚏ.ᬳ,0,0),new Ш(263,э.ๅ,32781,8,null,ጕ.Ѳ,ᚏ.ᬢ,0,0),new Ш(264,э.ๅ,32782,8,null
,ጕ.ݡ,ᚏ.ᬡ,0,0),new Ш(265,э.ๅ,32783,20,null,null,ᚏ.ᥗ,0,0),new Ш(266,э.ๅ,32794,10,null,null,ᚏ.ᬟ,0,0),new Ш(267,э.ๅ,32795,10,
null,null,ᚏ.ᬞ,0,0),new Ш(268,э.ๅ,32796,10,null,null,ᚏ.ᥗ,0,0),new Ш(269,э.ๅ,16,5,null,null,ᚏ.ᬜ,0,0),new Ш(270,э.ๅ,16,5,null,ጕ
.Ϭ,ᚏ.ᥗ,0,0),new Ш(271,э.ๅ,16,7,null,null,ᚏ.ᬚ,0,0),new Ш(272,э.ๅ,17,7,null,ጕ.Ѥ,ᚏ.ᬙ,0,0),new Ш(273,э.ๅ,18,7,null,ጕ.ѱ,ᚏ.ᬘ,0,
0),new Ш(274,э.ๅ,19,7,null,null,ᚏ.ᬗ,0,0),new Ш(275,э.ๅ,20,7,null,null,ᚏ.ᬖ,0,0),new Ш(276,э.ๅ,21,7,null,null,ᚏ.ᬕ,0,0),new
Ш(277,э.ๅ,22,7,null,null,ᚏ.ᬔ,0,0),new Ш(278,э.ๅ,23,5,null,null,ᚏ.ᬓ,0,0),new Ш(279,э.ๅ,24,5,null,null,ᚏ.ᬪ,0,0),new Ш(280,э
.ๅ,25,-1,null,null,ᚏ.ᚠ,0,0),new Ш(281,э.ກ,32768,2,null,ጕ.ݦ,ᚏ.ᮕ,0,0),new Ш(282,э.ກ,32769,2,null,ጕ.ъ,ᚏ.ᮎ,0,0),new Ш(283,э.ກ
,32768,2,null,ጕ.ъ,ᚏ.ᮏ,0,0),new Ш(284,э.ກ,32769,2,null,ጕ.ъ,ᚏ.ᮐ,0,0),new Ш(285,э.ກ,32770,2,null,ጕ.ݧ,ᚏ.ᮑ,0,0),new Ш(286,э.ກ,
32769,2,null,ጕ.ъ,ᚏ.ᮒ,0,0),new Ш(287,э.ກ,32770,2,null,ጕ.ъ,ᚏ.ᮓ,0,0),new Ш(288,э.ກ,32769,2,null,ጕ.ъ,ᚏ.ᮔ,0,0),new Ш(289,э.ກ,32770
,2,null,ጕ.ъ,ᚏ.ᮖ,0,0),new Ш(290,э.ກ,32771,2,null,ጕ.ъ,ᚏ.ᮝ,0,0),new Ш(291,э.ກ,32770,2,null,ጕ.ъ,ᚏ.ᮗ,0,0),new Ш(292,э.ກ,32771,
2,null,ጕ.ъ,ᚏ.ᮘ,0,0),new Ш(293,э.ກ,32770,2,null,ጕ.ъ,ᚏ.ᮙ,0,0),new Ш(294,э.ກ,32771,2,null,ጕ.ъ,ᚏ.ᮚ,0,0),new Ш(295,э.ກ,32772,2
,null,ጕ.ъ,ᚏ.ᮛ,0,0),new Ш(296,э.ກ,32771,2,null,ጕ.ъ,ᚏ.ᮜ,0,0),new Ш(297,э.ກ,32772,2,null,ጕ.ъ,ᚏ.ᮞ,0,0),new Ш(298,э.ກ,32771,2,
null,ጕ.ъ,ᚏ.ᮍ,0,0),new Ш(299,э.ກ,32772,2,null,ጕ.ݧ,ᚏ.ᮌ,0,0),new Ш(300,э.ກ,32773,2,null,ጕ.ъ,ᚏ.ᮋ,0,0),new Ш(301,э.ກ,32772,2,null
,ጕ.ъ,ᚏ.ᮊ,0,0),new Ш(302,э.ກ,32773,2,null,ጕ.ъ,ᚏ.ᮉ,0,0),new Ш(303,э.ກ,32772,2,null,ጕ.ъ,ᚏ.ᮈ,0,0),new Ш(304,э.ກ,32773,2,null,
ጕ.ъ,ᚏ.ᮇ,0,0),new Ш(305,э.ກ,32774,2,null,ጕ.ъ,ᚏ.ᮆ,0,0),new Ш(306,э.ກ,32775,2,null,ጕ.ъ,ᚏ.ᮅ,0,0),new Ш(307,э.ກ,32774,2,null,ጕ
.ъ,ᚏ.ᮄ,0,0),new Ш(308,э.ກ,32775,2,null,ጕ.ъ,ᚏ.ᮃ,0,0),new Ш(309,э.ກ,32774,2,null,ጕ.ъ,ᚏ.ᭋ,0,0),new Ш(310,э.ກ,32775,2,null,ጕ.
ъ,ᚏ.ᚠ,0,0),new Ш(311,э.น,1,4,null,null,ᚏ.ᭉ,0,0),new Ш(312,э.น,2,4,null,null,ᚏ.ᭈ,0,0),new Ш(313,э.น,1,4,null,null,ᚏ.ᭇ,0,0)
,new Ш(314,э.น,2,4,null,null,ᚏ.ᭆ,0,0),new Ш(315,э.น,3,4,null,null,ᚏ.ᚠ,0,0),new Ш(316,э.ษ,32768,2,null,ጕ.ш,ᚏ.ᬒ,0,0),new Ш(
317,э.ษ,32769,2,null,ጕ.ш,ᚏ.ш,0,0),new Ш(318,э.ศ,32768,8,null,null,ᚏ.ᨰ,0,0),new Ш(319,э.ศ,32769,6,null,null,ᚏ.ᨱ,0,0),new Ш(
320,э.ศ,32770,4,null,null,ᚏ.ᚠ,0,0),new Ш(321,э.ว,0,10,null,ጕ.Ҥ,ᚏ.ᨳ,0,0),new Ш(322,э.ว,1,10,null,ጕ.Ҥ,ᚏ.ᨲ,0,0),new Ш(323,э.ว,
0,2,null,ጕ.ј,ᚏ.ᨵ,0,0),new Ш(324,э.ว,0,2,null,ጕ.ј,ᚏ.ᨷ,0,0),new Ш(325,э.ว,1,2,null,ጕ.ј,ᚏ.ᨾ,0,0),new Ш(326,э.ว,1,2,null,ጕ.ј,
ᚏ.ᨸ,0,0),new Ш(327,э.ว,2,2,null,ጕ.ј,ᚏ.ᨹ,0,0),new Ш(328,э.ว,2,2,null,ጕ.ј,ᚏ.ᨺ,0,0),new Ш(329,э.ว,3,2,null,ጕ.ј,ᚏ.ᨻ,0,0),new
Ш(330,э.ว,3,2,null,ጕ.ј,ᚏ.ᨼ,0,0),new Ш(331,э.ว,4,2,null,ጕ.ј,ᚏ.ᨽ,0,0),new Ш(332,э.ว,4,2,null,ጕ.ј,ᚏ.ᨿ,0,0),new Ш(333,э.ว,5,2
,null,ጕ.ј,ᚏ.ᨮ,0,0),new Ш(334,э.ว,5,2,null,ጕ.ј,ᚏ.ᨴ,0,0),new Ш(335,э.ว,6,0,null,ጕ.Ѳ,ᚏ.ᨬ,0,0),new Ш(336,э.ว,6,6,null,ጕ.ݱ,ᚏ.ᨫ
,0,0),new Ш(337,э.ว,7,6,null,ጕ.Ѳ,ᚏ.ᨪ,0,0),new Ш(338,э.ว,8,6,null,ጕ.ݯ,ᚏ.ᨴ,0,0),new Ш(339,э.ว,32777,0,null,ጕ.Ѳ,ᚏ.ᨨ,0,0),new
Ш(340,э.ว,32777,10,null,ጕ.Ѳ,ᚏ.ᨧ,0,0),new Ш(341,э.ว,10,10,null,ጕ.ݟ,ᚏ.ᨦ,0,0),new Ш(342,э.ว,10,10,null,ጕ.Ѳ,ᚏ.ᨴ,0,0),new Ш(
343,э.ว,11,5,null,null,ᚏ.ᨤ,0,0),new Ш(344,э.ว,11,5,null,ጕ.Ϭ,ᚏ.ᨴ,0,0),new Ш(345,э.ว,11,7,null,null,ᚏ.ᨢ,0,0),new Ш(346,э.ว,12
,7,null,null,ᚏ.ᨡ,0,0),new Ш(347,э.ว,13,7,null,ጕ.Ѥ,ᚏ.ᨠ,0,0),new Ш(348,э.ว,14,7,null,ጕ.ѱ,ᚏ.ᨖ,0,0),new Ш(349,э.ว,15,7,null,
null,ᚏ.ᨶ,0,0),new Ш(350,э.ว,16,-1,null,null,ᚏ.ᚠ,0,0),new Ш(351,э.ว,16,5,null,null,ᚏ.ᬈ,0,0),new Ш(352,э.ว,15,5,null,null,ᚏ.ᩒ,
0,0),new Ш(353,э.ว,14,5,null,null,ᚏ.ᩓ,0,0),new Ш(354,э.ว,13,5,null,null,ᚏ.ᩔ,0,0),new Ш(355,э.ว,12,5,null,null,ᚏ.ᪧ,0,0),
new Ш(356,э.ว,11,5,null,null,ᚏ.ᨴ,0,0),new Ш(357,э.ฦ,32768,4,null,null,ᚏ.ᬆ,0,0),new Ш(358,э.ฦ,32769,4,null,null,ᚏ.ᬅ,0,0),new
Ш(359,э.ฯ,32769,8,null,null,ᚏ.ᬉ,0,0),new Ш(360,э.ฯ,32770,6,null,null,ᚏ.ᬐ,0,0),new Ш(361,э.ฯ,32771,4,null,null,ᚏ.ᚠ,0,0),
new Ш(362,э.ล,0,15,null,ጕ.Ҥ,ᚏ.ᬋ,0,0),new Ш(363,э.ล,1,15,null,ጕ.Ҥ,ᚏ.ᬊ,0,0),new Ш(364,э.ล,0,4,null,ጕ.ј,ᚏ.ᬍ,0,0),new Ш(365,э.ล
,0,4,null,ጕ.ј,ᚏ.ᬎ,0,0),new Ш(366,э.ล,1,4,null,ጕ.ј,ᚏ.ᬏ,0,0),new Ш(367,э.ล,1,4,null,ጕ.ј,ᚏ.ᬑ,0,0),new Ш(368,э.ล,2,4,null,ጕ.ј
,ᚏ.ᩑ,0,0),new Ш(369,э.ล,2,4,null,ጕ.ј,ᚏ.ᩐ,0,0),new Ш(370,э.ล,3,4,null,ጕ.ј,ᚏ.ᩏ,0,0),new Ш(371,э.ล,3,4,null,ጕ.ј,ᚏ.ᩎ,0,0),new
Ш(372,э.ล,4,4,null,ጕ.ј,ᚏ.ᩍ,0,0),new Ш(373,э.ล,4,4,null,ጕ.ј,ᚏ.ᩌ,0,0),new Ш(374,э.ล,5,4,null,ጕ.ј,ᚏ.ᩋ,0,0),new Ш(375,э.ล,5,4
,null,ጕ.ј,ᚏ.ᬌ,0,0),new Ш(376,э.ล,6,20,null,ጕ.ݾ,ᚏ.ᩉ,0,0),new Ш(377,э.ล,32775,10,null,ጕ.ݸ,ᚏ.ᩈ,0,0),new Ш(378,э.ล,8,5,null,ጕ
.Ѳ,ᚏ.ᩇ,0,0),new Ш(379,э.ล,6,5,null,ጕ.Ѳ,ᚏ.ᩆ,0,0),new Ш(380,э.ล,32775,10,null,ጕ.ݷ,ᚏ.ᩅ,0,0),new Ш(381,э.ล,8,5,null,ጕ.Ѳ,ᚏ.ᩄ,0
,0),new Ш(382,э.ล,6,5,null,ጕ.Ѳ,ᚏ.ᩃ,0,0),new Ш(383,э.ล,32775,10,null,ጕ.ݶ,ᚏ.ᩂ,0,0),new Ш(384,э.ล,8,5,null,ጕ.Ѳ,ᚏ.ᩁ,0,0),new
Ш(385,э.ล,6,5,null,ጕ.Ѳ,ᚏ.ᬌ,0,0),new Ш(386,э.ล,9,3,null,null,ᚏ.ᣴ,0,0),new Ш(387,э.ล,9,3,null,ጕ.Ϭ,ᚏ.ᬌ,0,0),new Ш(388,э.ล,10
,6,null,null,ᚏ.ᠧ,0,0),new Ш(389,э.ล,11,6,null,ጕ.Ѥ,ᚏ.ᠨ,0,0),new Ш(390,э.ล,12,6,null,ጕ.ѱ,ᚏ.ᠩ,0,0),new Ш(391,э.ล,13,6,null,
null,ᚏ.ᠪ,0,0),new Ш(392,э.ล,14,6,null,null,ᚏ.ᠫ,0,0),new Ш(393,э.ล,15,6,null,null,ᚏ.ᠬ,0,0),new Ш(394,э.ล,16,6,null,null,ᚏ.ᠮ,0
,0),new Ш(395,э.ล,17,6,null,null,ᚏ.ᠵ,0,0),new Ш(396,э.ล,18,6,null,null,ᚏ.ᠯ,0,0),new Ш(397,э.ล,19,-1,null,ጕ.ݪ,ᚏ.ᚠ,0,0),new
Ш(398,э.ล,17,5,null,null,ᚏ.ᠱ,0,0),new Ш(399,э.ล,16,5,null,null,ᚏ.ᠲ,0,0),new Ш(400,э.ล,15,5,null,null,ᚏ.ᠳ,0,0),new Ш(401,э
.ล,14,5,null,null,ᚏ.ᠴ,0,0),new Ш(402,э.ล,13,5,null,null,ᚏ.ᠶ,0,0),new Ш(403,э.ล,12,5,null,null,ᚏ.ᠥ,0,0),new Ш(404,э.ล,11,5
,null,null,ᚏ.ᠤ,0,0),new Ш(405,э.ล,10,5,null,null,ᚏ.ᬌ,0,0),new Ш(406,э.ฤ,0,10,null,ጕ.Ҥ,ᚏ.ᠢ,0,0),new Ш(407,э.ฤ,1,10,null,ጕ.
Ҥ,ᚏ.ᠣ,0,0),new Ш(408,э.ฤ,0,3,null,ጕ.ј,ᚏ.ᠠ,0,0),new Ш(409,э.ฤ,0,3,null,ጕ.ј,ᚏ.ៜ,0,0),new Ш(410,э.ฤ,1,3,null,ጕ.ј,ᚏ.ៗ,0,0),
new Ш(411,э.ฤ,1,3,null,ጕ.ј,ᚏ.ឳ,0,0),new Ш(412,э.ฤ,2,3,null,ጕ.ј,ᚏ.ឲ,0,0),new Ш(413,э.ฤ,2,3,null,ጕ.ј,ᚏ.ឱ,0,0),new Ш(414,э.ฤ,3
,3,null,ጕ.ј,ᚏ.ឰ,0,0),new Ш(415,э.ฤ,3,3,null,ጕ.ј,ᚏ.ᠡ,0,0),new Ш(416,э.ฤ,4,10,null,ጕ.Ѳ,ᚏ.ឮ,0,0),new Ш(417,э.ฤ,32773,4,null,
ጕ.ѯ,ᚏ.ឭ,0,0),new Ш(418,э.ฤ,32772,4,null,ጕ.ѯ,ᚏ.ឬ,0,0),new Ш(419,э.ฤ,5,1,null,ጕ.Ѭ,ᚏ.ឮ,0,0),new Ш(420,э.ฤ,6,3,null,null,ᚏ.ᠷ,
0,0),new Ш(421,э.ฤ,6,3,null,ጕ.Ϭ,ᚏ.ᠡ,0,0),new Ш(422,э.ฤ,7,5,null,null,ᚏ.ᡉ,0,0),new Ш(423,э.ฤ,8,5,null,ጕ.Ѥ,ᚏ.ᡊ,0,0),new Ш(
424,э.ฤ,9,5,null,ጕ.ѱ,ᚏ.ᡋ,0,0),new Ш(425,э.ฤ,10,5,null,null,ᚏ.ᡌ,0,0),new Ш(426,э.ฤ,11,5,null,null,ᚏ.ᡍ,0,0),new Ш(427,э.ฤ,12,
5,null,null,ᚏ.ᡎ,0,0),new Ш(428,э.ฤ,13,-1,null,null,ᚏ.ᚠ,0,0),new Ш(429,э.ฤ,14,5,null,null,ᚏ.ᡑ,0,0),new Ш(430,э.ฤ,15,5,null
,ጕ.Ѱ,ᚏ.ᡘ,0,0),new Ш(431,э.ฤ,16,5,null,ጕ.ѱ,ᚏ.ᡒ,0,0),new Ш(432,э.ฤ,17,5,null,null,ᚏ.ᡓ,0,0),new Ш(433,э.ฤ,18,5,null,null,ᚏ.ᡔ
,0,0),new Ш(434,э.ฤ,19,-1,null,null,ᚏ.ᚠ,0,0),new Ш(435,э.ฤ,13,5,null,null,ᚏ.ᡖ,0,0),new Ш(436,э.ฤ,12,5,null,null,ᚏ.ᡗ,0,0),
new Ш(437,э.ฤ,11,5,null,null,ᚏ.ᡙ,0,0),new Ш(438,э.ฤ,10,5,null,null,ᚏ.ᡈ,0,0),new Ш(439,э.ฤ,9,5,null,null,ᚏ.ᡇ,0,0),new Ш(440,
э.ฤ,8,5,null,null,ᚏ.ᡆ,0,0),new Ш(441,э.ฤ,7,5,null,null,ᚏ.ᠡ,0,0),new Ш(442,э.བྷ,0,10,null,ጕ.Ҥ,ᚏ.ᡄ,0,0),new Ш(443,э.བྷ,1,10,
null,ጕ.Ҥ,ᚏ.ᡅ,0,0),new Ш(444,э.བྷ,0,3,null,ጕ.ј,ᚏ.ᡂ,0,0),new Ш(445,э.བྷ,0,3,null,ጕ.ј,ᚏ.ᡁ,0,0),new Ш(446,э.བྷ,1,3,null,ጕ.ј,ᚏ.ᡀ,0,0
),new Ш(447,э.བྷ,1,3,null,ጕ.ј,ᚏ.ᠿ,0,0),new Ш(448,э.བྷ,2,3,null,ጕ.ј,ᚏ.ᠾ,0,0),new Ш(449,э.བྷ,2,3,null,ጕ.ј,ᚏ.ᠽ,0,0),new Ш(450,э
.བྷ,3,3,null,ጕ.ј,ᚏ.ᠼ,0,0),new Ш(451,э.བྷ,3,3,null,ጕ.ј,ᚏ.ᡃ,0,0),new Ш(452,э.བྷ,4,8,null,ጕ.Ѳ,ᚏ.ᠺ,0,0),new Ш(453,э.བྷ,5,8,null,ጕ
.Ѳ,ᚏ.ᠹ,0,0),new Ш(454,э.བྷ,6,6,null,ጕ.ѫ,ᚏ.ᡃ,0,0),new Ш(455,э.བྷ,7,2,null,null,ᚏ.ឫ,0,0),new Ш(456,э.བྷ,7,2,null,ጕ.Ϭ,ᚏ.ᡃ,0,0),
new Ш(457,э.བྷ,8,8,null,null,ᚏ.ᝇ,0,0),new Ш(458,э.བྷ,9,8,null,ጕ.Ѥ,ᚏ.ᝧ,0,0),new Ш(459,э.བྷ,10,6,null,null,ᚏ.ᝨ,0,0),new Ш(460,э.
བྷ,11,6,null,ጕ.ѱ,ᚏ.ᝩ,0,0),new Ш(461,э.བྷ,12,-1,null,null,ᚏ.ᚠ,0,0),new Ш(462,э.བྷ,13,5,null,null,ᚏ.ᝫ,0,0),new Ш(463,э.བྷ,14,5,
null,ጕ.Ѱ,ᚏ.ᝬ,0,0),new Ш(464,э.བྷ,15,5,null,null,ᚏ.ᝯ,0,0),new Ш(465,э.བྷ,16,5,null,ጕ.ѱ,ᚏ.ច,0,0),new Ш(466,э.བྷ,17,5,null,null,ᚏ.
ᝰ,0,0),new Ш(467,э.བྷ,18,5,null,null,ᚏ.ក,0,0),new Ш(468,э.བྷ,19,5,null,null,ᚏ.ខ,0,0),new Ш(469,э.བྷ,20,-1,null,null,ᚏ.ᚠ,0,0)
,new Ш(470,э.བྷ,12,8,null,null,ᚏ.ឃ,0,0),new Ш(471,э.བྷ,11,8,null,null,ᚏ.ង,0,0),new Ш(472,э.བྷ,10,6,null,null,ᚏ.ឆ,0,0),new Ш(
473,э.བྷ,9,6,null,null,ᚏ.ᝥ,0,0),new Ш(474,э.བྷ,8,6,null,null,ᚏ.ᡃ,0,0),new Ш(475,э.ร,0,10,null,ጕ.Ҥ,ᚏ.ᝣ,0,0),new Ш(476,э.ร,1,10
,null,ጕ.Ҥ,ᚏ.ᝤ,0,0),new Ш(477,э.ร,0,2,null,ጕ.ј,ᚏ.ᝡ,0,0),new Ш(478,э.ร,0,2,null,ጕ.ј,ᚏ.ᝠ,0,0),new Ш(479,э.ร,1,2,null,ጕ.ј,ᚏ.ᝑ
,0,0),new Ш(480,э.ร,1,2,null,ጕ.ј,ᚏ.ᝐ,0,0),new Ш(481,э.ร,2,2,null,ጕ.ј,ᚏ.ᝏ,0,0),new Ш(482,э.ร,2,2,null,ጕ.ј,ᚏ.ᝎ,0,0),new Ш(
483,э.ร,3,2,null,ጕ.ј,ᚏ.ᝍ,0,0),new Ш(484,э.ร,3,2,null,ጕ.ј,ᚏ.ᝢ,0,0),new Ш(485,э.ร,4,8,null,ጕ.Ѳ,ᚏ.ᝋ,0,0),new Ш(486,э.ร,5,8,
null,ጕ.Ѳ,ᚏ.ᝊ,0,0),new Ш(487,э.ร,6,8,null,ጕ.ٺ,ᚏ.ᝢ,0,0),new Ш(488,э.ร,7,2,null,null,ᚏ.ᝈ,0,0),new Ш(489,э.ร,7,2,null,ጕ.Ϭ,ᚏ.ᝢ,0,
0),new Ш(490,э.ร,8,8,null,null,ᚏ.ជ,0,0),new Ш(491,э.ร,9,8,null,ጕ.Ѥ,ᚏ.ហ,0,0),new Ш(492,э.ร,10,4,null,null,ᚏ.យ,0,0),new Ш(
493,э.ร,11,4,null,ጕ.ѱ,ᚏ.រ,0,0),new Ш(494,э.ร,12,4,null,null,ᚏ.ល,0,0),new Ш(495,э.ร,13,-1,null,null,ᚏ.ᚠ,0,0),new Ш(496,э.ร,
13,5,null,null,ᚏ.ឝ,0,0),new Ш(497,э.ร,12,5,null,null,ᚏ.ឞ,0,0),new Ш(498,э.ร,11,5,null,null,ᚏ.ស,0,0),new Ш(499,э.ร,10,5,
null,null,ᚏ.ឡ,0,0),new Ш(500,э.ร,9,5,null,null,ᚏ.ឨ,0,0),new Ш(501,э.ร,8,5,null,null,ᚏ.ᝢ,0,0),new Ш(502,э.ย,0,10,null,ጕ.Ҥ,ᚏ.អ
,0,0),new Ш(503,э.ย,0,3,null,ጕ.ј,ᚏ.ឣ,0,0),new Ш(504,э.ย,1,5,null,ጕ.Ѳ,ᚏ.ឥ,0,0),new Ш(505,э.ย,2,5,null,ጕ.Ѳ,ᚏ.ឦ,0,0),new Ш(
506,э.ย,32771,5,null,ጕ.ݺ,ᚏ.ឣ,0,0),new Ш(507,э.ย,4,3,null,null,ᚏ.ឩ,0,0),new Ш(508,э.ย,4,3,null,ጕ.Ϭ,ᚏ.ម,0,0),new Ш(509,э.ย,5,
6,null,null,ᚏ.ឣ,0,0),new Ш(510,э.ย,6,8,null,null,ᚏ.ព,0,0),new Ш(511,э.ย,7,8,null,ጕ.Ѥ,ᚏ.ផ,0,0),new Ш(512,э.ย,8,8,null,null
,ᚏ.ប,0,0),new Ш(513,э.ย,9,8,null,null,ᚏ.ន,0,0),new Ш(514,э.ย,10,8,null,ጕ.ѱ,ᚏ.ធ,0,0),new Ш(515,э.ย,11,-1,null,null,ᚏ.ᚠ,0,0
),new Ш(516,э.ย,11,8,null,null,ᚏ.ថ,0,0),new Ш(517,э.ย,10,8,null,null,ᚏ.ត,0,0),new Ш(518,э.ย,9,8,null,null,ᚏ.ណ,0,0),new Ш(
519,э.ย,8,8,null,null,ᚏ.ឍ,0,0),new Ш(520,э.ย,7,8,null,null,ᚏ.ឌ,0,0),new Ш(521,э.ย,6,8,null,null,ᚏ.ឣ,0,0),new Ш(522,э.ม,
32768,4,null,null,ᚏ.ដ,0,0),new Ш(523,э.ม,32769,4,null,null,ᚏ.ឋ,0,0),new Ш(524,э.ม,32770,6,null,null,ᚏ.ᝦ,0,0),new Ш(525,э.ม,
32771,6,null,null,ᚏ.ᡚ,0,0),new Ш(526,э.ม,32772,6,null,null,ᚏ.ᚠ,0,0),new Ш(527,э.ภ,0,10,null,ጕ.Ҥ,ᚏ.ᣀ,0,0),new Ш(528,э.ภ,1,10,
null,ጕ.Ҥ,ᚏ.ᡬ,0,0),new Ш(529,э.ภ,0,3,null,ጕ.ј,ᚏ.ᣂ,0,0),new Ш(530,э.ภ,0,3,null,ጕ.ј,ᚏ.ᣃ,0,0),new Ш(531,э.ภ,1,3,null,ጕ.ј,ᚏ.ᣄ,0,0
),new Ш(532,э.ภ,1,3,null,ጕ.ј,ᚏ.ᣅ,0,0),new Ш(533,э.ภ,2,3,null,ጕ.ј,ᚏ.ᣆ,0,0),new Ш(534,э.ภ,2,3,null,ጕ.ј,ᚏ.ᣈ,0,0),new Ш(535,э
.ภ,3,3,null,ጕ.ј,ᚏ.ᣏ,0,0),new Ш(536,э.ภ,3,3,null,ጕ.ј,ᚏ.ᣁ,0,0),new Ш(537,э.ภ,4,8,null,ጕ.Ѳ,ᚏ.ᣊ,0,0),new Ш(538,э.ภ,5,8,null,ጕ
.Ѳ,ᚏ.ᣋ,0,0),new Ш(539,э.ภ,6,8,null,ጕ.ݻ,ᚏ.ᣁ,0,0),new Ш(540,э.ภ,7,2,null,null,ᚏ.ᣍ,0,0),new Ш(541,э.ภ,7,2,null,ጕ.Ϭ,ᚏ.ᣁ,0,0),
new Ш(542,э.ภ,8,8,null,null,ᚏ.ᣐ,0,0),new Ш(543,э.ภ,9,8,null,ጕ.Ѥ,ᚏ.ᢿ,0,0),new Ш(544,э.ภ,10,8,null,null,ᚏ.ᢾ,0,0),new Ш(545,э.
ภ,11,8,null,ጕ.ѱ,ᚏ.ᢽ,0,0),new Ш(546,э.ภ,12,8,null,null,ᚏ.ᢼ,0,0),new Ш(547,э.ภ,13,8,null,null,ᚏ.ᢻ,0,0),new Ш(548,э.ภ,14,-1,
null,ጕ.ݪ,ᚏ.ᚠ,0,0),new Ш(549,э.ภ,14,8,null,null,ᚏ.ᢹ,0,0),new Ш(550,э.ภ,13,8,null,null,ᚏ.ᢸ,0,0),new Ш(551,э.ภ,12,8,null,null,ᚏ
.ᢷ,0,0),new Ш(552,э.ภ,11,8,null,null,ᚏ.ᢶ,0,0),new Ш(553,э.ภ,10,8,null,null,ᚏ.ᢵ,0,0),new Ш(554,э.ภ,9,8,null,null,ᚏ.ᢴ,0,0),
new Ш(555,э.ภ,8,8,null,null,ᚏ.ᣁ,0,0),new Ш(556,э.ฟ,0,10,null,ጕ.Ҥ,ᚏ.ᢲ,0,0),new Ш(557,э.ฟ,1,10,null,ጕ.Ҥ,ᚏ.ᢳ,0,0),new Ш(558,э.
ฟ,0,3,null,ጕ.ј,ᚏ.ᢰ,0,0),new Ш(559,э.ฟ,0,3,null,ጕ.ј,ᚏ.ᣇ,0,0),new Ш(560,э.ฟ,1,3,null,ጕ.ј,ᚏ.ᣑ,0,0),new Ш(561,э.ฟ,1,3,null,ጕ.
ј,ᚏ.ᣪ,0,0),new Ш(562,э.ฟ,2,3,null,ጕ.ј,ᚏ.ᣣ,0,0),new Ш(563,э.ฟ,2,3,null,ጕ.ј,ᚏ.ᣤ,0,0),new Ш(564,э.ฟ,3,3,null,ጕ.ј,ᚏ.ᣥ,0,0),
new Ш(565,э.ฟ,3,3,null,ጕ.ј,ᚏ.ᢱ,0,0),new Ш(566,э.ฟ,4,8,null,ጕ.Ѳ,ᚏ.ᣧ,0,0),new Ш(567,э.ฟ,5,8,null,ጕ.Ѳ,ᚏ.ᣨ,0,0),new Ш(568,э.ฟ,6
,8,null,ጕ.ݻ,ᚏ.ᢱ,0,0),new Ш(569,э.ฟ,7,2,null,null,ᚏ.ᣫ,0,0),new Ш(570,э.ฟ,7,2,null,ጕ.Ϭ,ᚏ.ᢱ,0,0),new Ш(571,э.ฟ,8,8,null,null
,ᚏ.ᣬ,0,0),new Ш(572,э.ฟ,9,8,null,ጕ.Ѥ,ᚏ.ᣭ,0,0),new Ш(573,э.ฟ,10,8,null,null,ᚏ.ᣮ,0,0),new Ш(574,э.ฟ,11,8,null,ጕ.ѱ,ᚏ.ᣯ,0,0),
new Ш(575,э.ฟ,12,8,null,null,ᚏ.ᣰ,0,0),new Ш(576,э.ฟ,13,8,null,null,ᚏ.ᣱ,0,0),new Ш(577,э.ฟ,14,-1,null,null,ᚏ.ᚠ,0,0),new Ш(
578,э.ฟ,14,8,null,null,ᚏ.ᣢ,0,0),new Ш(579,э.ฟ,13,8,null,null,ᚏ.ᣡ,0,0),new Ш(580,э.ฟ,12,8,null,null,ᚏ.ᣠ,0,0),new Ш(581,э.ฟ,
11,8,null,null,ᚏ.ᣟ,0,0),new Ш(582,э.ฟ,10,8,null,null,ᚏ.ᣞ,0,0),new Ш(583,э.ฟ,9,8,null,null,ᚏ.ᣝ,0,0),new Ш(584,э.ฟ,8,8,null,
null,ᚏ.ᢱ,0,0),new Ш(585,э.พ,32768,10,null,ጕ.Ҥ,ᚏ.ᣛ,0,0),new Ш(586,э.พ,32769,10,null,ጕ.Ҥ,ᚏ.ᣜ,0,0),new Ш(587,э.พ,32768,6,null,ጕ
.ј,ᚏ.ᣙ,0,0),new Ш(588,э.พ,32769,6,null,ጕ.ј,ᚏ.ᣚ,0,0),new Ш(589,э.พ,32770,10,null,ጕ.Ѳ,ᚏ.ᣗ,0,0),new Ш(590,э.พ,32771,4,null,ጕ
.ݽ,ᚏ.ᣖ,0,0),new Ш(591,э.พ,32770,4,null,null,ᚏ.ᣕ,0,0),new Ш(592,э.พ,32771,4,null,null,ᚏ.ᣖ,0,0),new Ш(593,э.พ,32772,3,null,
null,ᚏ.ᣓ,0,0),new Ш(594,э.พ,32772,3,null,ጕ.Ϭ,ᚏ.ᣚ,0,0),new Ш(595,э.พ,32773,6,null,null,ᚏ.ᢪ,0,0),new Ш(596,э.พ,32774,6,null,ጕ.
Ѥ,ᚏ.ᢨ,0,0),new Ш(597,э.พ,32775,6,null,null,ᚏ.ᡛ,0,0),new Ш(598,э.พ,32776,6,null,ጕ.ѱ,ᚏ.ᡭ,0,0),new Ш(599,э.พ,9,6,null,null,ᚏ
.ᡮ,0,0),new Ш(600,э.พ,10,6,null,null,ᚏ.ᚠ,0,0),new Ш(601,э.ฝ,0,10,null,ጕ.Ҥ,ᚏ.ᡰ,0,0),new Ш(602,э.ฝ,1,10,null,ጕ.Ҥ,ᚏ.ᡯ,0,0),
new Ш(603,э.ฝ,0,3,null,ጕ.ސ,ᚏ.ᡲ,0,0),new Ш(604,э.ฝ,0,3,null,ጕ.ј,ᚏ.ᡴ,0,0),new Ш(605,э.ฝ,1,3,null,ጕ.ј,ᚏ.ᢃ,0,0),new Ш(606,э.ฝ,1
,3,null,ጕ.ј,ᚏ.ᡵ,0,0),new Ш(607,э.ฝ,2,3,null,ጕ.ސ,ᚏ.ᡶ,0,0),new Ш(608,э.ฝ,2,3,null,ጕ.ј,ᚏ.ᡷ,0,0),new Ш(609,э.ฝ,3,3,null,ጕ.ј,ᚏ
.ᢀ,0,0),new Ш(610,э.ฝ,3,3,null,ጕ.ј,ᚏ.ᢁ,0,0),new Ш(611,э.ฝ,4,3,null,ጕ.ސ,ᚏ.ᢂ,0,0),new Ш(612,э.ฝ,4,3,null,ጕ.ј,ᚏ.ᢄ,0,0),new Ш
(613,э.ฝ,5,3,null,ጕ.ј,ᚏ.ᡫ,0,0),new Ш(614,э.ฝ,5,3,null,ጕ.ј,ᚏ.ᡱ,0,0),new Ш(615,э.ฝ,32768,20,null,ጕ.Ѳ,ᚏ.ᡩ,0,0),new Ш(616,э.ฝ
,32774,4,null,ጕ.Ѵ,ᚏ.ᡨ,0,0),new Ш(617,э.ฝ,32775,4,null,ጕ.Ѵ,ᚏ.ᡧ,0,0),new Ш(618,э.ฝ,32775,1,null,ጕ.ތ,ᚏ.ᡩ,0,0),new Ш(619,э.ฝ,
8,3,null,null,ᚏ.ᡥ,0,0),new Ш(620,э.ฝ,8,3,null,ጕ.Ϭ,ᚏ.ᡱ,0,0),new Ш(621,э.ฝ,9,20,null,ጕ.Ѥ,ᚏ.ᡣ,0,0),new Ш(622,э.ฝ,10,10,null,
ጕ.ѱ,ᚏ.ᡢ,0,0),new Ш(623,э.ฝ,11,10,null,null,ᚏ.ᡡ,0,0),new Ш(624,э.ฝ,12,10,null,null,ᚏ.ᡠ,0,0),new Ш(625,э.ฝ,13,10,null,null,
ᚏ.ᡟ,0,0),new Ш(626,э.ฝ,14,10,null,null,ᚏ.ᡞ,0,0),new Ш(627,э.ฝ,15,10,null,null,ᚏ.ᡝ,0,0),new Ш(628,э.ฝ,16,10,null,null,ᚏ.ᡜ,
0,0),new Ш(629,э.ฝ,17,10,null,null,ᚏ.ᡳ,0,0),new Ш(630,э.ฝ,18,30,null,null,ᚏ.ᢅ,0,0),new Ш(631,э.ฝ,18,-1,null,ጕ.ݪ,ᚏ.ᚠ,0,0),
new Ш(632,э.ผ,0,10,null,ጕ.Ҥ,ᚏ.ᢗ,0,0),new Ш(633,э.ผ,1,10,null,ጕ.Ҥ,ᚏ.ᢞ,0,0),new Ш(634,э.ผ,0,20,null,null,ᚏ.ᢙ,0,0),new Ш(635,э
.ผ,0,3,null,ጕ.ޑ,ᚏ.ᢚ,0,0),new Ш(636,э.ผ,0,3,null,ጕ.ј,ᚏ.ᢛ,0,0),new Ш(637,э.ผ,1,3,null,ጕ.ј,ᚏ.ᢜ,0,0),new Ш(638,э.ผ,1,3,null,ጕ
.ј,ᚏ.ᢝ,0,0),new Ш(639,э.ผ,2,3,null,ጕ.ј,ᚏ.ᢟ,0,0),new Ш(640,э.ผ,2,3,null,ጕ.ј,ᚏ.ᢦ,0,0),new Ш(641,э.ผ,3,3,null,ጕ.ޑ,ᚏ.ᢠ,0,0),
new Ш(642,э.ผ,3,3,null,ጕ.ј,ᚏ.ᢡ,0,0),new Ш(643,э.ผ,4,3,null,ጕ.ј,ᚏ.ᢢ,0,0),new Ш(644,э.ผ,4,3,null,ጕ.ј,ᚏ.ᢣ,0,0),new Ш(645,э.ผ,5
,3,null,ጕ.ј,ᚏ.ᢤ,0,0),new Ш(646,э.ผ,5,3,null,ጕ.ј,ᚏ.ᢙ,0,0),new Ш(647,э.ผ,32768,20,null,ጕ.Ѳ,ᚏ.ᢧ,0,0),new Ш(648,э.ผ,32774,4,
null,ጕ.ދ,ᚏ.ᢖ,0,0),new Ш(649,э.ผ,32775,4,null,null,ᚏ.ᢕ,0,0),new Ш(650,э.ผ,32775,1,null,ጕ.ތ,ᚏ.ᢧ,0,0),new Ш(651,э.ผ,8,3,null,
null,ᚏ.ᢓ,0,0),new Ш(652,э.ผ,8,3,null,ጕ.Ϭ,ᚏ.ᢙ,0,0),new Ш(653,э.ผ,9,20,null,ጕ.Ѥ,ᚏ.ᢑ,0,0),new Ш(654,э.ผ,10,7,null,ጕ.ѱ,ᚏ.ᢐ,0,0),
new Ш(655,э.ผ,11,7,null,null,ᚏ.ᢏ,0,0),new Ш(656,э.ผ,12,7,null,null,ᚏ.ᢎ,0,0),new Ш(657,э.ผ,13,7,null,null,ᚏ.ᢍ,0,0),new Ш(658
,э.ผ,14,7,null,null,ᚏ.ᢌ,0,0),new Ш(659,э.ผ,15,-1,null,ጕ.ݪ,ᚏ.ᚠ,0,0),new Ш(660,э.ผ,15,5,null,null,ᚏ.ᢊ,0,0),new Ш(661,э.ผ,14
,5,null,null,ᚏ.ᢉ,0,0),new Ш(662,э.ผ,13,5,null,null,ᚏ.ᢈ,0,0),new Ш(663,э.ผ,12,5,null,null,ᚏ.ᢇ,0,0),new Ш(664,э.ผ,11,5,null
,null,ᚏ.ᢆ,0,0),new Ш(665,э.ผ,10,5,null,null,ᚏ.ᆭ,0,0),new Ш(666,э.ผ,9,5,null,null,ᚏ.ᢙ,0,0),new Ш(667,э.ป,32768,5,null,null
,ᚏ.ᆚ,0,0),new Ш(668,э.ป,32769,5,null,null,ᚏ.ԍ,0,0),new Ш(669,э.บ,32768,5,null,null,ᚏ.ԏ,0,0),new Ш(670,э.บ,32769,5,null,
null,ᚏ.Ԑ,0,0),new Ш(671,э.บ,32770,5,null,null,ᚏ.ԑ,0,0),new Ш(672,э.บ,32771,5,null,null,ᚏ.Ԓ,0,0),new Ш(673,э.บ,32772,5,null,
null,ᚏ.ᚠ,0,0),new Ш(674,э.า,0,10,null,ጕ.Ҥ,ᚏ.ԕ,0,0),new Ш(675,э.า,1,10,null,ጕ.Ҥ,ᚏ.ԓ,0,0),new Ш(676,э.า,0,3,null,ጕ.ޒ,ᚏ.Ԗ,0,0),
new Ш(677,э.า,0,3,null,ጕ.ј,ᚏ.ԗ,0,0),new Ш(678,э.า,1,3,null,ጕ.ј,ᚏ.Ԙ,0,0),new Ш(679,э.า,1,3,null,ጕ.ј,ᚏ.ԙ,0,0),new Ш(680,э.า,2
,3,null,ጕ.ј,ᚏ.Ԛ,0,0),new Ш(681,э.า,2,3,null,ጕ.ј,ᚏ.ԛ,0,0),new Ш(682,э.า,3,3,null,ጕ.ސ,ᚏ.ԝ,0,0),new Ш(683,э.า,3,3,null,ጕ.ј,ᚏ
.Ԝ,0,0),new Ш(684,э.า,4,6,null,ጕ.Ѳ,ᚏ.ԋ,0,0),new Ш(685,э.า,5,12,null,ጕ.ގ,ᚏ.Ԋ,0,0),new Ш(686,э.า,4,12,null,ጕ.Ѳ,ᚏ.ԉ,0,0),new
Ш(687,э.า,5,12,null,ጕ.ގ,ᚏ.Ԉ,0,0),new Ш(688,э.า,4,12,null,ጕ.Ѳ,ᚏ.ԇ,0,0),new Ш(689,э.า,5,12,null,ጕ.ގ,ᚏ.Ԝ,0,0),new Ш(690,э.า,
6,10,null,ጕ.Ϭ,ᚏ.Ԝ,0,0),new Ш(691,э.า,7,10,null,null,ᚏ.Ԅ,0,0),new Ш(692,э.า,8,10,null,ጕ.Ѥ,ᚏ.ԃ,0,0),new Ш(693,э.า,9,10,null
,null,ᚏ.Ԃ,0,0),new Ш(694,э.า,10,10,null,null,ᚏ.ԁ,0,0),new Ш(695,э.า,11,10,null,null,ᚏ.Ԁ,0,0),new Ш(696,э.า,12,10,null,ጕ.ѱ
,ᚏ.ӿ,0,0),new Ш(697,э.า,13,10,null,null,ᚏ.Ӿ,0,0),new Ш(698,э.า,14,10,null,null,ᚏ.ӽ,0,0),new Ш(699,э.า,15,30,null,null,ᚏ.Ԕ
,0,0),new Ш(700,э.า,15,-1,null,ጕ.ݪ,ᚏ.ᚠ,0,0),new Ш(701,э.ຂ,0,10,null,ጕ.Ҥ,ᚏ.Ԟ,0,0),new Ш(702,э.ຂ,0,3,null,ጕ.ј,ᚏ.Թ,0,0),new
Ш(703,э.ຂ,0,3,null,ጕ.ј,ᚏ.Ժ,0,0),new Ш(704,э.ຂ,1,3,null,ጕ.ј,ᚏ.Ի,0,0),new Ш(705,э.ຂ,1,3,null,ጕ.ј,ᚏ.Լ,0,0),new Ш(706,э.ຂ,2,3
,null,ጕ.ј,ᚏ.Խ,0,0),new Ш(707,э.ຂ,2,3,null,ጕ.ј,ᚏ.Հ,0,0),new Ш(708,э.ຂ,3,5,null,ጕ.Ѳ,ᚏ.Կ,0,0),new Ш(709,э.ຂ,4,5,null,ጕ.Ѳ,ᚏ.Ձ
,0,0),new Ш(710,э.ຂ,32773,5,null,ጕ.Ѳ,ᚏ.Ո,0,0),new Ш(711,э.ຂ,32773,0,null,ጕ.ݵ,ᚏ.Հ,0,0),new Ш(712,э.ຂ,6,6,null,null,ᚏ.Ճ,0,0
),new Ш(713,э.ຂ,6,6,null,ጕ.Ϭ,ᚏ.Հ,0,0),new Ш(714,э.ຂ,32775,8,null,null,ᚏ.Յ,0,0),new Ш(715,э.ຂ,32776,8,null,ጕ.Ѥ,ᚏ.Ն,0,0),
new Ш(716,э.ຂ,32777,8,null,null,ᚏ.Շ,0,0),new Ш(717,э.ຂ,32778,8,null,null,ᚏ.Չ,0,0),new Ш(718,э.ຂ,32779,8,null,ጕ.ݮ,ᚏ.Ը,0,0),
new Ш(719,э.ຂ,32780,8,null,null,ᚏ.ᚠ,0,0),new Ш(720,э.ຂ,12,8,null,null,ᚏ.Զ,0,0),new Ш(721,э.ຂ,11,8,null,null,ᚏ.Ե,0,0),new Ш(
722,э.ຂ,10,8,null,null,ᚏ.Դ,0,0),new Ш(723,э.ຂ,9,8,null,null,ᚏ.Գ,0,0),new Ш(724,э.ຂ,8,8,null,null,ᚏ.Բ,0,0),new Ш(725,э.ຂ,7,8
,null,null,ᚏ.Հ,0,0),new Ш(726,э.ຮ,0,10,null,ጕ.Ҥ,ᚏ.ԧ,0,0),new Ш(727,э.ຮ,1,10,null,ጕ.Ҥ,ᚏ.Ա,0,0),new Ш(728,э.ຮ,0,3,null,ጕ.ј,
ᚏ.ԥ,0,0),new Ш(729,э.ຮ,0,3,null,ጕ.ј,ᚏ.Ԥ,0,0),new Ш(730,э.ຮ,1,3,null,ጕ.ј,ᚏ.ԣ,0,0),new Ш(731,э.ຮ,1,3,null,ጕ.ј,ᚏ.Ԣ,0,0),new
Ш(732,э.ຮ,2,3,null,ጕ.ј,ᚏ.ԡ,0,0),new Ш(733,э.ຮ,2,3,null,ጕ.ј,ᚏ.Ԡ,0,0),new Ш(734,э.ຮ,3,3,null,ጕ.ј,ᚏ.ԟ,0,0),new Ш(735,э.ຮ,3,3
,null,ጕ.ј,ᚏ.Ԧ,0,0),new Ш(736,э.ຮ,4,10,null,ጕ.Ѳ,ᚏ.ӻ,0,0),new Ш(737,э.ຮ,5,10,null,ጕ.Ѳ,ᚏ.Ҷ,0,0),new Ш(738,э.ຮ,32774,4,null,ጕ
.ѯ,ᚏ.ӈ,0,0),new Ш(739,э.ຮ,5,6,null,ጕ.Ѳ,ᚏ.Ӊ,0,0),new Ш(740,э.ຮ,32774,4,null,ጕ.ѯ,ᚏ.ӊ,0,0),new Ш(741,э.ຮ,5,1,null,ጕ.Ѭ,ᚏ.ӻ,0,
0),new Ш(742,э.ຮ,7,3,null,null,ᚏ.ӌ,0,0),new Ш(743,э.ຮ,7,3,null,ጕ.Ϭ,ᚏ.Ԧ,0,0),new Ш(744,э.ຮ,8,5,null,null,ᚏ.ӏ,0,0),new Ш(
745,э.ຮ,9,5,null,ጕ.Ѥ,ᚏ.Ӗ,0,0),new Ш(746,э.ຮ,10,5,null,ጕ.ѱ,ᚏ.Ӑ,0,0),new Ш(747,э.ຮ,11,5,null,null,ᚏ.ӑ,0,0),new Ш(748,э.ຮ,12,-
1,null,null,ᚏ.ᚠ,0,0),new Ш(749,э.ຮ,13,5,null,null,ᚏ.ӓ,0,0),new Ш(750,э.ຮ,14,5,null,ጕ.Ѱ,ᚏ.Ӕ,0,0),new Ш(751,э.ຮ,15,5,null,ጕ
.ѱ,ᚏ.ӕ,0,0),new Ш(752,э.ຮ,16,5,null,null,ᚏ.ӗ,0,0),new Ш(753,э.ຮ,17,5,null,null,ᚏ.ӆ,0,0),new Ш(754,э.ຮ,18,5,null,null,ᚏ.Ӆ,
0,0),new Ш(755,э.ຮ,19,5,null,null,ᚏ.ӄ,0,0),new Ш(756,э.ຮ,20,5,null,null,ᚏ.Ӄ,0,0),new Ш(757,э.ຮ,21,-1,null,null,ᚏ.ᚠ,0,0),
new Ш(758,э.ຮ,12,5,null,null,ᚏ.Ӂ,0,0),new Ш(759,э.ຮ,11,5,null,null,ᚏ.Ӏ,0,0),new Ш(760,э.ຮ,10,5,null,null,ᚏ.ҿ,0,0),new Ш(761
,э.ຮ,9,5,null,null,ᚏ.Ҿ,0,0),new Ш(762,э.ຮ,8,5,null,null,ᚏ.Ԧ,0,0),new Ш(763,э.ຢ,0,-1,null,null,ᚏ.ҽ,0,0),new Ш(764,э.ຢ,0,6,
null,null,ᚏ.һ,0,0),new Ш(765,э.ຢ,1,6,null,null,ᚏ.Һ,0,0),new Ш(766,э.ຢ,2,6,null,ጕ.Ѥ,ᚏ.ҹ,0,0),new Ш(767,э.ຢ,3,6,null,null,ᚏ.Ҹ,
0,0),new Ш(768,э.ຢ,4,6,null,null,ᚏ.ҷ,0,0),new Ш(769,э.ຢ,5,6,null,null,ᚏ.ӎ,0,0),new Ш(770,э.ຢ,6,6,null,null,ᚏ.Ә,0,0),new Ш
(771,э.ຢ,7,6,null,null,ᚏ.ӱ,0,0),new Ш(772,э.ຢ,8,6,null,null,ᚏ.Ӫ,0,0),new Ш(773,э.ຢ,9,6,null,null,ᚏ.ӫ,0,0),new Ш(774,э.ຢ,
10,6,null,ጕ.ݩ,ᚏ.Ӭ,0,0),new Ш(775,э.ຢ,11,-1,null,null,ᚏ.ᚠ,0,0),new Ш(776,э.ຢ,12,4,null,null,ᚏ.Ӯ,0,0),new Ш(777,э.ຢ,12,8,
null,ጕ.Ϭ,ᚏ.ҽ,0,0),new Ш(778,э.ຣ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(779,э.ຣ,1,36,null,ጕ.Ӱ,ᚏ.ӯ,0,0),new Ш(780,э.ຣ,0,100,null,ጕ.ߨ,ᚏ.
ӹ,0,0),new Ш(781,э.ຣ,0,10,null,null,ᚏ.ӳ,0,0),new Ш(782,э.ຣ,0,10,null,null,ᚏ.Ӵ,0,0),new Ш(783,э.ຣ,0,-1,null,ጕ.ߡ,ᚏ.ᚠ,0,0),
new Ш(784,э.ຮ,0,10,null,ጕ.Ҥ,ᚏ.ӵ,0,0),new Ш(785,э.ຮ,0,181,null,ጕ.ߧ,ᚏ.ӷ,0,0),new Ш(786,э.ຮ,0,150,null,ጕ.ߠ,ᚏ.ӷ,0,0),new Ш(787,
э.ລ,32768,3,null,ጕ.ߟ,ᚏ.Ӻ,0,0),new Ш(788,э.ລ,32769,3,null,ጕ.ߞ,ᚏ.ө,0,0),new Ш(789,э.ລ,32770,3,null,ጕ.ߞ,ᚏ.Ө,0,0),new Ш(790,э
.ລ,32771,3,null,ጕ.ߞ,ᚏ.Ӹ,0,0),new Ш(791,э.ກ,32768,4,null,ጕ.ъ,ᚏ.Ӧ,0,0),new Ш(792,э.ກ,32769,4,null,ጕ.ъ,ᚏ.ӥ,0,0),new Ш(793,э.
ກ,32770,4,null,ጕ.ъ,ᚏ.Ӥ,0,0),new Ш(794,э.ກ,32771,4,null,ጕ.ъ,ᚏ.ӣ,0,0),new Ш(795,э.ກ,32772,4,null,ጕ.ъ,ᚏ.Ӣ,0,0),new Ш(796,э.ກ
,32773,4,null,ጕ.ъ,ᚏ.ӡ,0,0),new Ш(797,э.ກ,32774,4,null,ጕ.ъ,ᚏ.Ӡ,0,0),new Ш(798,э.ກ,32775,4,null,ጕ.ъ,ᚏ.ᚠ,0,0),new Ш(799,э.ฯ,
32769,10,null,null,ᚏ.Ӟ,0,0),new Ш(800,э.ฯ,32770,10,null,null,ᚏ.ӝ,0,0),new Ш(801,э.ฯ,32771,10,null,ጕ.ߣ,ᚏ.ᚠ,0,0),new Ш(802,э.ວ,
0,6,null,null,ᚏ.ӛ,0,0),new Ш(803,э.ວ,32769,7,null,null,ᚏ.Ӝ,0,0),new Ш(804,э.ສ,0,6,null,null,ᚏ.Ӈ,0,0),new Ш(805,э.ສ,32769,
6,null,null,ᚏ.Ӛ,0,0),new Ш(806,э.ຫ,0,6,null,null,ᚏ.ե,0,0),new Ш(807,э.ຫ,1,6,null,null,ᚏ.Պ,0,0),new Ш(808,э.ອ,32768,5,null
,null,ᚏ.ئ,0,0),new Ш(809,э.ອ,32769,5,null,ጕ.Ѥ,ᚏ.ا,0,0),new Ш(810,э.ອ,32770,5,null,null,ᚏ.ب,0,0),new Ш(811,э.ອ,32771,10,
null,ጕ.ޏ,ᚏ.ة,0,0),new Ш(812,э.ອ,32772,10,null,null,ᚏ.ᚠ,0,0),new Ш(813,э.ຯ,32768,4,null,null,ᚏ.ث,0,0),new Ш(814,э.ຯ,32769,4,
null,null,ᚏ.ح,0,0),new Ш(815,э.ຯ,32770,4,null,null,ᚏ.ت,0,0),new Ш(816,э.ໂ,0,6,null,null,ᚏ.خ,0,0),new Ш(817,э.ໂ,1,6,null,null
,ᚏ.د,0,0),new Ш(818,э.ໂ,2,6,null,null,ᚏ.ذ,0,0),new Ш(819,э.ໂ,3,6,null,null,ᚏ.ر,0,0),new Ш(820,э.ໂ,2,6,null,null,ᚏ.ز,0,0),
new Ш(821,э.ໂ,1,6,null,null,ᚏ.ش,0,0),new Ш(822,э.ະ,0,6,null,null,ᚏ.ص,0,0),new Ш(823,э.ະ,1,6,null,null,ᚏ.ؤ,0,0),new Ш(824,э.
ະ,2,6,null,null,ᚏ.أ,0,0),new Ш(825,э.ະ,3,6,null,null,ᚏ.آ,0,0),new Ш(826,э.ະ,2,6,null,null,ᚏ.ء,0,0),new Ш(827,э.ະ,1,6,null
,null,ᚏ.س,0,0),new Ш(828,э.າ,0,10,null,null,ᚏ.ײ,0,0),new Ш(829,э.າ,32769,10,null,null,ᚏ.ؠ,0,0),new Ш(830,э.ຳ,0,10,null,
null,ᚏ.װ,0,0),new Ш(831,э.ຳ,32769,10,null,null,ᚏ.ױ,0,0),new Ш(832,э.ຽ,0,10,null,null,ᚏ.ש,0,0),new Ш(833,э.ຽ,32769,10,null,
null,ᚏ.ת,0,0),new Ш(834,э.ເ,0,10,null,null,ᚏ.ק,0,0),new Ш(835,э.ເ,32769,10,null,null,ᚏ.ר,0,0),new Ш(836,э.ແ,0,10,null,null,ᚏ
.ץ,0,0),new Ш(837,э.ແ,32769,10,null,null,ᚏ.צ,0,0),new Ш(838,э.ໃ,0,10,null,null,ᚏ.ף,0,0),new Ш(839,э.ໃ,32769,10,null,null,
ᚏ.פ,0,0),new Ш(840,э.ມ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(841,э.ຟ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(842,э.ພ,32768,6,null,null,ᚏ.ن,
0,0),new Ш(843,э.ພ,32769,6,null,null,ᚏ.ه,0,0),new Ш(844,э.ພ,32770,6,null,null,ᚏ.و,0,0),new Ш(845,э.ພ,32771,6,null,null,ᚏ.
ى,0,0),new Ш(846,э.ພ,32770,6,null,null,ᚏ.ي,0,0),new Ш(847,э.ພ,32769,6,null,null,ᚏ.ٱ,0,0),new Ш(848,э.ຝ,32768,6,null,null,
ᚏ.ٯ,0,0),new Ш(849,э.ຝ,32769,6,null,null,ᚏ.ٲ,0,0),new Ш(850,э.ຝ,32770,6,null,null,ᚏ.ٸ,0,0),new Ш(851,э.ຝ,32771,6,null,
null,ᚏ.ٮ,0,0),new Ш(852,э.ຜ,32768,-1,null,null,ᚏ.ᚠ,0,0),new Ш(853,э.ປ,32768,6,null,null,ᚏ.ٵ,0,0),new Ш(854,э.ປ,32769,6,null,
null,ᚏ.ٶ,0,0),new Ш(855,э.ປ,32770,6,null,null,ᚏ.ٷ,0,0),new Ш(856,э.ປ,32771,6,null,null,ᚏ.ٴ,0,0),new Ш(857,э.ບ,32768,6,null,
null,ᚏ.ٹ,0,0),new Ш(858,э.ບ,32769,6,null,null,ᚏ.م,0,0),new Ш(859,э.ບ,32770,6,null,null,ᚏ.ل,0,0),new Ш(860,э.ບ,32771,6,null,
null,ᚏ.Е,0,0),new Ш(861,э.ນ,32768,-1,null,null,ᚏ.ᚠ,0,0),new Ш(862,э.ທ,32768,6,null,null,ᚏ.ف,0,0),new Ш(863,э.ທ,32769,6,null,
null,ᚏ.ـ,0,0),new Ш(864,э.ທ,32770,6,null,null,ᚏ.ؿ,0,0),new Ш(865,э.ທ,32771,6,null,null,ᚏ.ؾ,0,0),new Ш(866,э.ທ,32770,6,null,
null,ᚏ.ؽ,0,0),new Ш(867,э.ທ,32769,6,null,null,ᚏ.ق,0,0),new Ш(868,э.ຖ,32768,6,null,null,ᚏ.ػ,0,0),new Ш(869,э.ຖ,1,6,null,null,
ᚏ.ؼ,0,0),new Ш(870,э.ຕ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(871,э.ດ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(872,э.ຍ,0,-1,null,null,ᚏ.ᚠ,0,0
),new Ш(873,э.ຊ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(874,э.ຈ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(875,э.ງ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш
(876,э.ส,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(877,э.ར,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(878,э.ཨ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(879,э.
သ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(880,э.ဟ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(881,э.ဠ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(882,э.အ,0,-1,
null,null,ᚏ.ᚠ,0,0),new Ш(883,э.ဢ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(884,э.ဣ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(885,э.ဤ,0,-1,null,null,
ᚏ.ᚠ,0,0),new Ш(886,э.ဦ,32768,-1,null,null,ᚏ.ᚠ,0,0),new Ш(887,э.ၑ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(888,э.ဧ,0,10,null,null,ᚏ.հ
,0,0),new Ш(889,э.ဧ,1,15,null,null,ᚏ.ձ,0,0),new Ш(890,э.ဧ,2,8,null,null,ᚏ.ղ,0,0),new Ш(891,э.ဧ,1,6,null,null,ᚏ.կ,0,0),new
Ш(892,э.โ,13,-1,null,null,ᚏ.ᚠ,0,0),new Ш(893,э.โ,18,-1,null,null,ᚏ.ᚠ,0,0),new Ш(894,э.ဨ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(895
,э.ဩ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(896,э.ဪ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(897,э.ဿ,32768,6,null,null,ᚏ.ՙ,0,0),new Ш(898,э.ဿ
,32769,6,null,null,ᚏ.ա,0,0),new Ш(899,э.ၐ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(900,э.ၒ,0,6,null,null,ᚏ.Ք,0,0),new Ш(901,э.ၒ,1,8,
null,null,ᚏ.Օ,0,0),new Ш(902,э.ဝ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(903,э.လ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(904,э.ရ,0,-1,null,null,
ᚏ.ᚠ,0,0),new Ш(905,э.ယ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(906,э.မ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(907,э.ဘ,0,-1,null,null,ᚏ.ᚠ,0,0
),new Ш(908,э.ဗ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(909,э.ဖ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(910,э.ပ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш
(911,э.န,32768,-1,null,null,ᚏ.ᚠ,0,0),new Ш(912,э.ဓ,32768,-1,null,null,ᚏ.ᚠ,0,0),new Ш(913,э.ဒ,0,-1,null,null,ᚏ.ᚠ,0,0),new
Ш(914,э.ထ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(915,э.တ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(916,э.ဏ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(917,э
.ဎ,32768,6,null,null,ᚏ.ו,0,0),new Ш(918,э.ဎ,32769,6,null,null,ᚏ.ז,0,0),new Ш(919,э.ဎ,32770,6,null,null,ᚏ.ט,0,0),new Ш(920
,э.ဎ,32769,6,null,null,ᚏ.ה,0,0),new Ш(921,э.ဥ,32768,6,null,null,ᚏ.י,0,0),new Ш(922,э.ဥ,32769,6,null,null,ᚏ.ך,0,0),new Ш(
923,э.ဥ,32770,6,null,null,ᚏ.ן,0,0),new Ш(924,э.ၓ,0,14,null,null,ᚏ.ל,0,0),new Ш(925,э.ၓ,1,14,null,null,ᚏ.כ,0,0),new Ш(926,э.
ၵ,32768,4,null,null,ᚏ.מ,0,0),new Ш(927,э.ၵ,32769,4,null,null,ᚏ.נ,0,0),new Ш(928,э.ၵ,32770,4,null,null,ᚏ.և,0,0),new Ш(929,
э.ၵ,32771,4,null,null,ᚏ.ם,0,0),new Ш(930,э.ၝ,32768,4,null,null,ᚏ.օ,0,0),new Ш(931,э.ၝ,32769,4,null,null,ᚏ.ք,0,0),new Ш(
932,э.ၝ,32770,4,null,null,ᚏ.փ,0,0),new Ш(933,э.ၝ,32771,4,null,null,ᚏ.ֆ,0,0),new Ш(934,э.ၡ,32768,4,null,null,ᚏ.ց,0,0),new Ш(
935,э.ၡ,32769,4,null,null,ᚏ.ր,0,0),new Ш(936,э.ၡ,32770,4,null,null,ᚏ.տ,0,0),new Ш(937,э.ၡ,32771,4,null,null,ᚏ.ւ,0,0),new Ш(
938,э.ၥ,32768,4,null,null,ᚏ.ս,0,0),new Ш(939,э.ၥ,32769,4,null,null,ᚏ.ռ,0,0),new Ш(940,э.ၥ,32770,4,null,null,ᚏ.ջ,0,0),new Ш(
941,э.ၥ,32771,4,null,null,ᚏ.վ,0,0),new Ш(942,э.ၦ,32768,4,null,null,ᚏ.չ,0,0),new Ш(943,э.ၦ,32769,4,null,null,ᚏ.ո,0,0),new Ш(
944,э.ၦ,32770,4,null,null,ᚏ.շ,0,0),new Ш(945,э.ၦ,32771,4,null,null,ᚏ.պ,0,0),new Ш(946,э.ၮ,32768,4,null,null,ᚏ.ҵ,0,0),new Ш(
947,э.ၮ,32769,4,null,null,ᚏ.Ͼ,0,0),new Ш(948,э.ၮ,32770,4,null,null,ᚏ.Э,0,0),new Ш(949,э.ၮ,32771,4,null,null,ᚏ.ә,0,0),new Ш(
950,э.ၯ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(951,э.ၰ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(952,э.ၶ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(953,э.ၽ,0
,-1,null,null,ᚏ.ᚠ,0,0),new Ш(954,э.ၷ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(955,э.ၸ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(956,э.ၹ,0,-1,
null,null,ᚏ.ᚠ,0,0),new Ш(957,э.ၺ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(958,э.ၻ,0,-1,null,null,ᚏ.ᚠ,0,0),new Ш(959,э.ၼ,32768,4,null,
null,ᚏ.з,0,0),new Ш(960,э.ၼ,32769,4,null,null,ᚏ.и,0,0),new Ш(961,э.ၼ,32770,4,null,null,ᚏ.й,0,0),new Ш(962,э.ၼ,32771,4,null,
null,ᚏ.ж,0,0),new Ш(963,э.ၾ,32768,4,null,null,ᚏ.Ы,0,0),new Ш(964,э.ၾ,32769,4,null,null,ᚏ.Ъ,0,0),new Ш(965,э.ၾ,32770,4,null,
null,ᚏ.Щ,0,0),new Ш(966,э.ၾ,32771,4,null,null,ᚏ.л,0,0)};}static partial class ኑ{public static class ጓ{public static Ꮥ ጒ=new
Ꮥ("PRESSKEY","press a key.");public static Ꮥ ጐ=new Ꮥ("PRESSYN","press y or n.");public static Ꮥ ጏ=new Ꮥ("QUITMSG",
"are you sure you want to\nquit this great game?");public static Ꮥ ጎ=new Ꮥ("LOADNET","you can't do load while in a net game!\n\n"+ጒ);public static Ꮥ ጜ=new Ꮥ("QLOADNET",
"you can't quickload during a netgame!\n\n"+ጒ);public static Ꮥ ጠ=new Ꮥ("QSAVESPOT","you haven't picked a quicksave slot yet!\n\n"+ጒ);public static Ꮥ ጹ=new Ꮥ(
"SAVEDEAD","you can't save if you aren't playing!\n\n"+ጒ);public static Ꮥ ጲ=new Ꮥ("QSPROMPT",
"quicksave over your game named\n\n'%s'?\n\n"+ጐ);public static Ꮥ ጳ=new Ꮥ("QLPROMPT","do you want to quickload the game named\n\n'%s'?\n\n"+ጐ);public static Ꮥ ጴ=new Ꮥ
("NEWGAME","you can't start a new game\n"+"while in a network game.\n\n"+ጒ);public static Ꮥ ጵ=new Ꮥ("NIGHTMARE",
"are you sure? this skill level\n"+"isn't even remotely fair.\n\n"+ጐ);public static Ꮥ ጶ=new Ꮥ("SWSTRING","this is the shareware version of doom.\n\n"+
"you need to order the entire trilogy.\n\n"+ጒ);public static Ꮥ ጷ=new Ꮥ("MSGOFF","Messages OFF");public static Ꮥ ጸ=new Ꮥ("MSGON","Messages ON");public static Ꮥ ጺ=
new Ꮥ("NETEND","you can't end a netgame!\n\n"+ጒ);public static Ꮥ ፁ=new Ꮥ("ENDGAME",
"are you sure you want to end the game?\n\n"+ጐ);public static Ꮥ ጻ=new Ꮥ("DOSY","(press y to quit)");public static Ꮥ ጼ=new Ꮥ("GAMMALVL0","Gamma correction OFF");
public static Ꮥ ጽ=new Ꮥ("GAMMALVL1","Gamma correction level 1");public static Ꮥ ጾ=new Ꮥ("GAMMALVL2","Gamma correction level 2"
);public static Ꮥ ጿ=new Ꮥ("GAMMALVL3","Gamma correction level 3");public static Ꮥ ፀ=new Ꮥ("GAMMALVL4",
"Gamma correction level 4");public static Ꮥ ፂ=new Ꮥ("EMPTYSTRING","empty slot");public static Ꮥ ጱ=new Ꮥ("GOTARMOR","Picked up the armor.");public
static Ꮥ ጰ=new Ꮥ("GOTMEGA","Picked up the MegaArmor!");public static Ꮥ ጯ=new Ꮥ("GOTHTHBONUS","Picked up a health bonus.");
public static Ꮥ ጮ=new Ꮥ("GOTARMBONUS","Picked up an armor bonus.");public static Ꮥ ጭ=new Ꮥ("GOTSTIM","Picked up a stimpack.");
public static Ꮥ ጬ=new Ꮥ("GOTMEDINEED","Picked up a medikit that you REALLY need!");public static Ꮥ ጫ=new Ꮥ("GOTMEDIKIT",
"Picked up a medikit.");public static Ꮥ ጪ=new Ꮥ("GOTSUPER","Supercharge!");public static Ꮥ ጩ=new Ꮥ("GOTBLUECARD","Picked up a blue keycard.");
public static Ꮥ ጨ=new Ꮥ("GOTYELWCARD","Picked up a yellow keycard.");public static Ꮥ ጧ=new Ꮥ("GOTREDCARD",
"Picked up a red keycard.");public static Ꮥ ጦ=new Ꮥ("GOTBLUESKUL","Picked up a blue skull key.");public static Ꮥ ጥ=new Ꮥ("GOTYELWSKUL",
"Picked up a yellow skull key.");public static Ꮥ ጤ=new Ꮥ("GOTREDSKULL","Picked up a red skull key.");public static Ꮥ ጣ=new Ꮥ("GOTINVUL",
"Invulnerability!");public static Ꮥ ጢ=new Ꮥ("GOTBERSERK","Berserk!");public static Ꮥ ጡ=new Ꮥ("GOTINVIS","Partial Invisibility");public
static Ꮥ ግ=new Ꮥ("GOTSUIT","Radiation Shielding Suit");public static Ꮥ ጌ=new Ꮥ("GOTMAP","Computer Area Map");public static Ꮥ ዄ
=new Ꮥ("GOTVISOR","Light Amplification Visor");public static Ꮥ ዙ=new Ꮥ("GOTMSPHERE","MegaSphere!");public static Ꮥ ዚ=new
Ꮥ("GOTCLIP","Picked up a clip.");public static Ꮥ ዛ=new Ꮥ("GOTCLIPBOX","Picked up a box of bullets.");public static Ꮥ ዜ=
new Ꮥ("GOTROCKET","Picked up a rocket.");public static Ꮥ ዝ=new Ꮥ("GOTROCKBOX","Picked up a box of rockets.");public static
Ꮥ ዞ=new Ꮥ("GOTCELL","Picked up an energy cell.");public static Ꮥ ዠ=new Ꮥ("GOTCELLBOX","Picked up an energy cell pack.");
public static Ꮥ ዧ=new Ꮥ("GOTSHELLS","Picked up 4 shotgun shells.");public static Ꮥ ዡ=new Ꮥ("GOTSHELLBOX",
"Picked up a box of shotgun shells.");public static Ꮥ ዢ=new Ꮥ("GOTBACKPACK","Picked up a backpack full of ammo!");public static Ꮥ ዣ=new Ꮥ("GOTBFG9000",
"You got the BFG9000!  Oh, yes.");public static Ꮥ ዤ=new Ꮥ("GOTCHAINGUN","You got the chaingun!");public static Ꮥ ዥ=new Ꮥ("GOTCHAINSAW",
"A chainsaw!  Find some meat!");public static Ꮥ ዦ=new Ꮥ("GOTLAUNCHER","You got the rocket launcher!");public static Ꮥ የ=new Ꮥ("GOTPLASMA",
"You got the plasma gun!");public static Ꮥ ዖ=new Ꮥ("GOTSHOTGUN","You got the shotgun!");public static Ꮥ ዕ=new Ꮥ("GOTSHOTGUN2",
"You got the super shotgun!");public static Ꮥ ዔ=new Ꮥ("PD_BLUEO","You need a blue key to activate this object");public static Ꮥ ዓ=new Ꮥ("PD_REDO",
"You need a red key to activate this object");public static Ꮥ ዒ=new Ꮥ("PD_YELLOWO","You need a yellow key to activate this object");public static Ꮥ ዑ=new Ꮥ(
"PD_BLUEK","You need a blue key to open this door");public static Ꮥ ዐ=new Ꮥ("PD_REDK","You need a red key to open this door");
public static Ꮥ ዏ=new Ꮥ("PD_YELLOWK","You need a yellow key to open this door");public static Ꮥ ዎ=new Ꮥ("GGSAVED",
"game saved.");public static Ꮥ ው=new Ꮥ("HUSTR_E1M1","E1M1: Hangar");public static Ꮥ ዌ=new Ꮥ("HUSTR_E1M2","E1M2: Nuclear Plant");
public static Ꮥ ዋ=new Ꮥ("HUSTR_E1M3","E1M3: Toxin Refinery");public static Ꮥ ዊ=new Ꮥ("HUSTR_E1M4","E1M4: Command Control");
public static Ꮥ ዉ=new Ꮥ("HUSTR_E1M5","E1M5: Phobos Lab");public static Ꮥ ወ=new Ꮥ("HUSTR_E1M6","E1M6: Central Processing");
public static Ꮥ ዅ=new Ꮥ("HUSTR_E1M7","E1M7: Computer Station");public static Ꮥ ዟ=new Ꮥ("HUSTR_E1M8","E1M8: Phobos Anomaly");
public static Ꮥ ዩ=new Ꮥ("HUSTR_E1M9","E1M9: Military Base");public static Ꮥ ጂ=new Ꮥ("HUSTR_E2M1","E2M1: Deimos Anomaly");
public static Ꮥ ዻ=new Ꮥ("HUSTR_E2M2","E2M2: Containment Area");public static Ꮥ ዼ=new Ꮥ("HUSTR_E2M3","E2M3: Refinery");public
static Ꮥ ዽ=new Ꮥ("HUSTR_E2M4","E2M4: Deimos Lab");public static Ꮥ ዾ=new Ꮥ("HUSTR_E2M5","E2M5: Command Center");public static Ꮥ
ዿ=new Ꮥ("HUSTR_E2M6","E2M6: Halls of the Damned");public static Ꮥ ጀ=new Ꮥ("HUSTR_E2M7","E2M7: Spawning Vats");public
static Ꮥ ጁ=new Ꮥ("HUSTR_E2M8","E2M8: Tower of Babel");public static Ꮥ ጃ=new Ꮥ("HUSTR_E2M9","E2M9: Fortress of Mystery");public
static Ꮥ ጊ=new Ꮥ("HUSTR_E3M1","E3M1: Hell Keep");public static Ꮥ ጄ=new Ꮥ("HUSTR_E3M2","E3M2: Slough of Despair");public static
Ꮥ ጅ=new Ꮥ("HUSTR_E3M3","E3M3: Pandemonium");public static Ꮥ ጆ=new Ꮥ("HUSTR_E3M4","E3M4: House of Pain");public static Ꮥ ጇ
=new Ꮥ("HUSTR_E3M5","E3M5: Unholy Cathedral");public static Ꮥ ገ=new Ꮥ("HUSTR_E3M6","E3M6: Mt. Erebus");public static Ꮥ ጉ=
new Ꮥ("HUSTR_E3M7","E3M7: Limbo");public static Ꮥ ጋ=new Ꮥ("HUSTR_E3M8","E3M8: Dis");public static Ꮥ ዺ=new Ꮥ("HUSTR_E3M9",
"E3M9: Warrens");public static Ꮥ ዹ=new Ꮥ("HUSTR_E4M1","E4M1: Hell Beneath");public static Ꮥ ዸ=new Ꮥ("HUSTR_E4M2","E4M2: Perfect Hatred"
);public static Ꮥ ዷ=new Ꮥ("HUSTR_E4M3","E4M3: Sever The Wicked");public static Ꮥ ዶ=new Ꮥ("HUSTR_E4M4","E4M4: Unruly Evil"
);public static Ꮥ ድ=new Ꮥ("HUSTR_E4M5","E4M5: They Will Repent");public static Ꮥ ዴ=new Ꮥ("HUSTR_E4M6",
"E4M6: Against Thee Wickedly");public static Ꮥ ዳ=new Ꮥ("HUSTR_E4M7","E4M7: And Hell Followed");public static Ꮥ ዲ=new Ꮥ("HUSTR_E4M8",
"E4M8: Unto The Cruel");public static Ꮥ ዱ=new Ꮥ("HUSTR_E4M9","E4M9: Fear");public static Ꮥ ደ=new Ꮥ("HUSTR_1","level 1: entryway");public
static Ꮥ ዯ=new Ꮥ("HUSTR_2","level 2: underhalls");public static Ꮥ ዮ=new Ꮥ("HUSTR_3","level 3: the gantlet");public static Ꮥ ይ=
new Ꮥ("HUSTR_4","level 4: the focus");public static Ꮥ ዬ=new Ꮥ("HUSTR_5","level 5: the waste tunnels");public static Ꮥ ያ=new
Ꮥ("HUSTR_6","level 6: the crusher");public static Ꮥ ዪ=new Ꮥ("HUSTR_7","level 7: dead simple");public static Ꮥ ኼ=new Ꮥ(
"HUSTR_8","level 8: tricks and traps");public static Ꮥ ኻ=new Ꮥ("HUSTR_9","level 9: the pit");public static Ꮥ ᇑ=new Ꮥ("HUSTR_10",
"level 10: refueling base");public static Ꮥ ህ=new Ꮥ("HUSTR_11","level 11: 'o' of destruction!");public static Ꮥ ሆ=new Ꮥ("HUSTR_12",
"level 12: the factory");public static Ꮥ ሇ=new Ꮥ("HUSTR_13","level 13: downtown");public static Ꮥ ለ=new Ꮥ("HUSTR_14",
"level 14: the inmost dens");public static Ꮥ ሉ=new Ꮥ("HUSTR_15","level 15: industrial zone");public static Ꮥ ሊ=new Ꮥ("HUSTR_16","level 16: suburbs"
);public static Ꮥ ሌ=new Ꮥ("HUSTR_17","level 17: tenements");public static Ꮥ ሓ=new Ꮥ("HUSTR_18","level 18: the courtyard")
;public static Ꮥ ል=new Ꮥ("HUSTR_19","level 19: the citadel");public static Ꮥ ሎ=new Ꮥ("HUSTR_20","level 20: gotcha!");
public static Ꮥ ሏ=new Ꮥ("HUSTR_21","level 21: nirvana");public static Ꮥ ሐ=new Ꮥ("HUSTR_22","level 22: the catacombs");public
static Ꮥ ሑ=new Ꮥ("HUSTR_23","level 23: barrels o' fun");public static Ꮥ ሒ=new Ꮥ("HUSTR_24","level 24: the chasm");public
static Ꮥ ሔ=new Ꮥ("HUSTR_25","level 25: bloodfalls");public static Ꮥ ሄ=new Ꮥ("HUSTR_26","level 26: the abandoned mines");public
static Ꮥ ሃ=new Ꮥ("HUSTR_27","level 27: monster condo");public static Ꮥ ሂ=new Ꮥ("HUSTR_28","level 28: the spirit world");public
static Ꮥ ሁ=new Ꮥ("HUSTR_29","level 29: the living end");public static Ꮥ ሀ=new Ꮥ("HUSTR_30","level 30: icon of sin");public
static Ꮥ ᇿ=new Ꮥ("HUSTR_31","level 31: wolfenstein");public static Ꮥ ᇾ=new Ꮥ("HUSTR_32","level 32: grosse");public static Ꮥ ᇽ=
new Ꮥ("PHUSTR_1","level 1: congo");public static Ꮥ ᇼ=new Ꮥ("PHUSTR_2","level 2: well of souls");public static Ꮥ ᇻ=new Ꮥ(
"PHUSTR_3","level 3: aztec");public static Ꮥ ᇺ=new Ꮥ("PHUSTR_4","level 4: caged");public static Ꮥ ᇹ=new Ꮥ("PHUSTR_5",
"level 5: ghost town");public static Ꮥ ᇸ=new Ꮥ("PHUSTR_6","level 6: baron's lair");public static Ꮥ ᇷ=new Ꮥ("PHUSTR_7","level 7: caughtyard");
public static Ꮥ ᇶ=new Ꮥ("PHUSTR_8","level 8: realm");public static Ꮥ ᇵ=new Ꮥ("PHUSTR_9","level 9: abattoire");public static Ꮥ
ላ=new Ꮥ("PHUSTR_10","level 10: onslaught");public static Ꮥ ሕ=new Ꮥ("PHUSTR_11","level 11: hunted");public static Ꮥ ሮ=new
Ꮥ("PHUSTR_12","level 12: speed");public static Ꮥ ሧ=new Ꮥ("PHUSTR_13","level 13: the crypt");public static Ꮥ ረ=new Ꮥ(
"PHUSTR_14","level 14: genesis");public static Ꮥ ሩ=new Ꮥ("PHUSTR_15","level 15: the twilight");public static Ꮥ ሪ=new Ꮥ("PHUSTR_16",
"level 16: the omen");public static Ꮥ ራ=new Ꮥ("PHUSTR_17","level 17: compound");public static Ꮥ ሬ=new Ꮥ("PHUSTR_18","level 18: neurosphere")
;public static Ꮥ ር=new Ꮥ("PHUSTR_19","level 19: nme");public static Ꮥ ሯ=new Ꮥ("PHUSTR_20","level 20: the death domain");
public static Ꮥ ሶ=new Ꮥ("PHUSTR_21","level 21: slayer");public static Ꮥ ሰ=new Ꮥ("PHUSTR_22","level 22: impossible mission");
public static Ꮥ ሱ=new Ꮥ("PHUSTR_23","level 23: tombstone");public static Ꮥ ሲ=new Ꮥ("PHUSTR_24","level 24: the final frontier")
;public static Ꮥ ሳ=new Ꮥ("PHUSTR_25","level 25: the temple of darkness");public static Ꮥ ሴ=new Ꮥ("PHUSTR_26",
"level 26: bunker");public static Ꮥ ስ=new Ꮥ("PHUSTR_27","level 27: anti-christ");public static Ꮥ ሷ=new Ꮥ("PHUSTR_28",
"level 28: the sewers");public static Ꮥ ሦ=new Ꮥ("PHUSTR_29","level 29: odyssey of noises");public static Ꮥ ሥ=new Ꮥ("PHUSTR_30",
"level 30: the gateway of hell");public static Ꮥ ሤ=new Ꮥ("PHUSTR_31","level 31: cyberden");public static Ꮥ ሣ=new Ꮥ("PHUSTR_32","level 32: go 2 it");
public static Ꮥ ሢ=new Ꮥ("THUSTR_1","level 1: system control");public static Ꮥ ሡ=new Ꮥ("THUSTR_2","level 2: human bbq");public
static Ꮥ ሠ=new Ꮥ("THUSTR_3","level 3: power control");public static Ꮥ ሟ=new Ꮥ("THUSTR_4","level 4: wormhole");public static Ꮥ
ሞ=new Ꮥ("THUSTR_5","level 5: hanger");public static Ꮥ ም=new Ꮥ("THUSTR_6","level 6: open season");public static Ꮥ ሜ=new Ꮥ(
"THUSTR_7","level 7: prison");public static Ꮥ ማ=new Ꮥ("THUSTR_8","level 8: metal");public static Ꮥ ሚ=new Ꮥ("THUSTR_9",
"level 9: stronghold");public static Ꮥ ሙ=new Ꮥ("THUSTR_10","level 10: redemption");public static Ꮥ መ=new Ꮥ("THUSTR_11",
"level 11: storage facility");public static Ꮥ ሗ=new Ꮥ("THUSTR_12","level 12: crater");public static Ꮥ ሖ=new Ꮥ("THUSTR_13",
"level 13: nukage processing");public static Ꮥ ᇴ=new Ꮥ("THUSTR_14","level 14: steel works");public static Ꮥ ᇳ=new Ꮥ("THUSTR_15","level 15: dead zone"
);public static Ꮥ ᆮ=new Ꮥ("THUSTR_16","level 16: deepest reaches");public static Ꮥ ᇀ=new Ꮥ("THUSTR_17",
"level 17: processing area");public static Ꮥ ᇁ=new Ꮥ("THUSTR_18","level 18: mill");public static Ꮥ ᇂ=new Ꮥ("THUSTR_19",
"level 19: shipping/respawning");public static Ꮥ ᇃ=new Ꮥ("THUSTR_20","level 20: central processing");public static Ꮥ ᇄ=new Ꮥ("THUSTR_21",
"level 21: administration center");public static Ꮥ ᇅ=new Ꮥ("THUSTR_22","level 22: habitat");public static Ꮥ ᇇ=new Ꮥ("THUSTR_23",
"level 23: lunar mining project");public static Ꮥ ᇎ=new Ꮥ("THUSTR_24","level 24: quarry");public static Ꮥ ᇈ=new Ꮥ("THUSTR_25","level 25: baron's den");
public static Ꮥ ᇉ=new Ꮥ("THUSTR_26","level 26: ballistyx");public static Ꮥ ᇊ=new Ꮥ("THUSTR_27","level 27: mount pain");public
static Ꮥ ᇋ=new Ꮥ("THUSTR_28","level 28: heck");public static Ꮥ ᇌ=new Ꮥ("THUSTR_29","level 29: river styx");public static Ꮥ ᇍ=
new Ꮥ("THUSTR_30","level 30: last call");public static Ꮥ ᇏ=new Ꮥ("THUSTR_31","level 31: pharaoh");public static Ꮥ ᆾ=new Ꮥ(
"THUSTR_32","level 32: caribbean");public static Ꮥ ᆽ=new Ꮥ("AMSTR_FOLLOWON","Follow Mode ON");public static Ꮥ ᆼ=new Ꮥ(
"AMSTR_FOLLOWOFF","Follow Mode OFF");public static Ꮥ ᆻ=new Ꮥ("AMSTR_GRIDON","Grid ON");public static Ꮥ ᆺ=new Ꮥ("AMSTR_GRIDOFF","Grid OFF"
);public static Ꮥ ᆹ=new Ꮥ("AMSTR_MARKEDSPOT","Marked Spot");public static Ꮥ ᆸ=new Ꮥ("AMSTR_MARKSCLEARED",
"All Marks Cleared");public static Ꮥ ᆷ=new Ꮥ("STSTR_MUS","Music Change");public static Ꮥ ᆶ=new Ꮥ("STSTR_NOMUS","IMPOSSIBLE SELECTION");
public static Ꮥ ᆵ=new Ꮥ("STSTR_DQDON","Degreelessness Mode On");public static Ꮥ ᆴ=new Ꮥ("STSTR_DQDOFF",
"Degreelessness Mode Off");public static Ꮥ ᆳ=new Ꮥ("STSTR_KFAADDED","Very Happy Ammo Added");public static Ꮥ ᆲ=new Ꮥ("STSTR_FAADDED",
"Ammo (no keys) Added");public static Ꮥ ᆱ=new Ꮥ("STSTR_NCON","No Clipping Mode ON");public static Ꮥ ᆰ=new Ꮥ("STSTR_NCOFF",
"No Clipping Mode OFF");public static Ꮥ ᆯ=new Ꮥ("STSTR_BEHOLD","inVuln, Str, Inviso, Rad, Allmap, or Lite-amp");public static Ꮥ ᇆ=new Ꮥ(
"STSTR_BEHOLDX","Power-up Toggled");public static Ꮥ ᇐ=new Ꮥ("STSTR_CHOPPERS","... doesn't suck - GM");public static Ꮥ ᇩ=new Ꮥ(
"STSTR_CLEV","Changing Level...");public static Ꮥ ᇢ=new Ꮥ("E1TEXT","Once you beat the big badasses and\n"+
"clean out the moon base you're supposed\n"+"to win, aren't you? Aren't you? Where's\n"+"your fat reward and ticket home? What\n"+
"the hell is this? It's not supposed to\n"+"end this way!\n"+"\n"+"It stinks like rotten meat, but looks\n"+"like the lost Deimos base.  Looks like\n"+
"you're stuck on The Shores of Hell.\n"+"The only way out is through.\n"+"\n"+"To continue the DOOM experience, play\n"+"The Shores of Hell and its amazing\n"+
"sequel, Inferno!\n");public static Ꮥ ᇣ=new Ꮥ("E2TEXT","You've done it! The hideous cyber-\n"+"demon lord that ruled the lost Deimos\n"+
"moon base has been slain and you\n"+"are triumphant! But ... where are\n"+"you? You clamber to the edge of the\n"+"moon and look down to see the awful\n"+
"truth.\n"+"\n"+"Deimos floats above Hell itself!\n"+"You've never heard of anyone escaping\n"+
"from Hell, but you'll make the bastards\n"+"sorry they ever heard of you! Quickly,\n"+"you rappel down to  the surface of\n"+"Hell.\n"+"\n"+
"Now, it's on to the final chapter of\n"+"DOOM! -- Inferno.");public static Ꮥ ᇤ=new Ꮥ("E3TEXT","The loathsome spiderdemon that\n"+
"masterminded the invasion of the moon\n"+"bases and caused so much death has had\n"+"its ass kicked for all time.\n"+"\n"+
"A hidden doorway opens and you enter.\n"+"You've proven too tough for Hell to\n"+"contain, and now Hell at last plays\n"+
"fair -- for you emerge from the door\n"+"to see the green fields of Earth!\n"+"Home at last.\n"+"\n"+"You wonder what's been happening on\n"+
"Earth while you were battling evil\n"+"unleashed. It's good that no Hell-\n"+"spawn could have come through that\n"+"door with you ...");public static Ꮥ ᇥ=
new Ꮥ("E4TEXT","the spider mastermind must have sent forth\n"+"its legions of hellspawn before your\n"+
"final confrontation with that terrible\n"+"beast from hell.  but you stepped forward\n"+"and brought forth eternal damnation and\n"+
"suffering upon the horde as a true hero\n"+"would in the face of something so evil.\n"+"\n"+"besides, someone was gonna pay for what\n"+
"happened to daisy, your pet rabbit.\n"+"\n"+"but now, you see spread before you more\n"+"potential pain and gibbitude as a nation\n"+
"of demons run amok among our cities.\n"+"\n"+"next stop, hell on earth!");public static Ꮥ ᇦ=new Ꮥ("C1TEXT","YOU HAVE ENTERED DEEPLY INTO THE INFESTED\n"+
"STARPORT. BUT SOMETHING IS WRONG. THE\n"+"MONSTERS HAVE BROUGHT THEIR OWN REALITY\n"+"WITH THEM, AND THE STARPORT'S TECHNOLOGY\n"+
"IS BEING SUBVERTED BY THEIR PRESENCE.\n"+"\n"+"AHEAD, YOU SEE AN OUTPOST OF HELL, A\n"+"FORTIFIED ZONE. IF YOU CAN GET PAST IT,\n"+
"YOU CAN PENETRATE INTO THE HAUNTED HEART\n"+"OF THE STARBASE AND FIND THE CONTROLLING\n"+"SWITCH WHICH HOLDS EARTH'S POPULATION\n"+"HOSTAGE.");public static Ꮥ ᇧ=
new Ꮥ("C2TEXT","YOU HAVE WON! YOUR VICTORY HAS ENABLED\n"+"HUMANKIND TO EVACUATE EARTH AND ESCAPE\n"+
"THE NIGHTMARE.  NOW YOU ARE THE ONLY\n"+"HUMAN LEFT ON THE FACE OF THE PLANET.\n"+"CANNIBAL MUTATIONS, CARNIVOROUS ALIENS,\n"+
"AND EVIL SPIRITS ARE YOUR ONLY NEIGHBORS.\n"+"YOU SIT BACK AND WAIT FOR DEATH, CONTENT\n"+"THAT YOU HAVE SAVED YOUR SPECIES.\n"+"\n"+
"BUT THEN, EARTH CONTROL BEAMS DOWN A\n"+"MESSAGE FROM SPACE: \"SENSORS HAVE LOCATED\n"+"THE SOURCE OF THE ALIEN INVASION. IF YOU\n"+
"GO THERE, YOU MAY BE ABLE TO BLOCK THEIR\n"+"ENTRY.  THE ALIEN BASE IS IN THE HEART OF\n"+"YOUR OWN HOME CITY, NOT FAR FROM THE\n"+
"STARPORT.\" SLOWLY AND PAINFULLY YOU GET\n"+"UP AND RETURN TO THE FRAY.");public static Ꮥ ᇨ=new Ꮥ("C3TEXT","YOU ARE AT THE CORRUPT HEART OF THE CITY,\n"+
"SURROUNDED BY THE CORPSES OF YOUR ENEMIES.\n"+"YOU SEE NO WAY TO DESTROY THE CREATURES'\n"+"ENTRYWAY ON THIS SIDE, SO YOU CLENCH YOUR\n"+
"TEETH AND PLUNGE THROUGH IT.\n"+"\n"+"THERE MUST BE A WAY TO CLOSE IT ON THE\n"+"OTHER SIDE. WHAT DO YOU CARE IF YOU'VE\n"+
"GOT TO GO THROUGH HELL TO GET TO IT?");public static Ꮥ ᇪ=new Ꮥ("C4TEXT","THE HORRENDOUS VISAGE OF THE BIGGEST\n"+"DEMON YOU'VE EVER SEEN CRUMBLES BEFORE\n"+
"YOU, AFTER YOU PUMP YOUR ROCKETS INTO\n"+"HIS EXPOSED BRAIN. THE MONSTER SHRIVELS\n"+"UP AND DIES, ITS THRASHING LIMBS\n"+"DEVASTATING UNTOLD MILES OF HELL'S\n"
+"SURFACE.\n"+"\n"+"YOU'VE DONE IT. THE INVASION IS OVER.\n"+"EARTH IS SAVED. HELL IS A WRECK. YOU\n"+
"WONDER WHERE BAD FOLKS WILL GO WHEN THEY\n"+"DIE, NOW. WIPING THE SWEAT FROM YOUR\n"+"FOREHEAD YOU BEGIN THE LONG TREK BACK\n"+
"HOME. REBUILDING EARTH OUGHT TO BE A\n"+"LOT MORE FUN THAN RUINING IT WAS.\n");public static Ꮥ ᇱ=new Ꮥ("C5TEXT","CONGRATULATIONS, YOU'VE FOUND THE SECRET\n"+
"LEVEL! LOOKS LIKE IT'S BEEN BUILT BY\n"+"HUMANS, RATHER THAN DEMONS. YOU WONDER\n"+"WHO THE INMATES OF THIS CORNER OF HELL\n"+"WILL BE.");public static Ꮥ ᇫ=new
Ꮥ("C6TEXT","CONGRATULATIONS, YOU'VE FOUND THE\n"+"SUPER SECRET LEVEL!  YOU'D BETTER\n"+"BLAZE THROUGH THIS ONE!\n");
public static Ꮥ ᇬ=new Ꮥ("P1TEXT","You gloat over the steaming carcass of the\n"+"Guardian.  With its death, you've wrested\n"+
"the Accelerator from the stinking claws\n"+"of Hell.  You relax and glance around the\n"+"room.  Damn!  There was supposed to be at\n"+
"least one working prototype, but you can't\n"+"see it. The demons must have taken it.\n"+"\n"+"You must find the prototype, or all your\n"+
"struggles will have been wasted. Keep\n"+"moving, keep fighting, keep killing.\n"+"Oh yes, keep living, too.");public static Ꮥ ᇭ=new Ꮥ("P2TEXT",
"Even the deadly Arch-Vile labyrinth could\n"+"not stop you, and you've gotten to the\n"+"prototype Accelerator which is soon\n"+
"efficiently and permanently deactivated.\n"+"\n"+"You're good at that kind of thing.");public static Ꮥ ᇮ=new Ꮥ("P3TEXT",
"You've bashed and battered your way into\n"+"the heart of the devil-hive.  Time for a\n"+"Search-and-Destroy mission, aimed at the\n"+
"Gatekeeper, whose foul offspring is\n"+"cascading to Earth.  Yeah, he's bad. But\n"+"you know who's worse!\n"+"\n"+
"Grinning evilly, you check your gear, and\n"+"get ready to give the bastard a little Hell\n"+"of your own making!");public static Ꮥ ᇯ=new Ꮥ("P4TEXT",
"The Gatekeeper's evil face is splattered\n"+"all over the place.  As its tattered corpse\n"+"collapses, an inverted Gate forms and\n"+
"sucks down the shards of the last\n"+"prototype Accelerator, not to mention the\n"+"few remaining demons.  You're done. Hell\n"+
"has gone back to pounding bad dead folks \n"+"instead of good live ones.  Remember to\n"+"tell your grandkids to put a rocket\n"+
"launcher in your coffin. If you go to Hell\n"+"when you die, you'll need it for some\n"+"final cleaning-up ...");public static Ꮥ ᇰ=new Ꮥ("P5TEXT",
"You've found the second-hardest level we\n"+"got. Hope you have a saved game a level or\n"+"two previous.  If not, be prepared to die\n"+
"aplenty. For master marines only.");public static Ꮥ ᇲ=new Ꮥ("P6TEXT","Betcha wondered just what WAS the hardest\n"+
"level we had ready for ya?  Now you know.\n"+"No one gets out alive.");public static Ꮥ ᇡ=new Ꮥ("T1TEXT","You've fought your way out of the infested\n"+
"experimental labs.   It seems that UAC has\n"+"once again gulped it down.  With their\n"+"high turnover, it must be hard for poor\n"+
"old UAC to buy corporate health insurance\n"+"nowadays..\n"+"\n"+"Ahead lies the military complex, now\n"+"swarming with diseased horrors hot to get\n"+
"their teeth into you. With luck, the\n"+"complex still has some warlike ordnance\n"+"laying around.");public static Ꮥ ᇠ=new Ꮥ("T2TEXT",
"You hear the grinding of heavy machinery\n"+"ahead.  You sure hope they're not stamping\n"+"out new hellspawn, but you're ready to\n"+
"ream out a whole herd if you have to.\n"+"They might be planning a blood feast, but\n"+"you feel about as mean as two thousand\n"+
"maniacs packed into one mad killer.\n"+"\n"+"You don't plan to go down easy.");public static Ꮥ ᇟ=new Ꮥ("T3TEXT","The vista opening ahead looks real damn\n"+
"familiar. Smells familiar, too -- like\n"+"fried excrement. You didn't like this\n"+"place before, and you sure as hell ain't\n"+
"planning to like it now. The more you\n"+"brood on it, the madder you get.\n"+"Hefting your gun, an evil grin trickles\n"+
"onto your face. Time to take some names.");public static Ꮥ ᇞ=new Ꮥ("T4TEXT","Suddenly, all is silent, from one horizon\n"+
"to the other. The agonizing echo of Hell\n"+"fades away, the nightmare sky turns to\n"+"blue, the heaps of monster corpses start \n"+
"to evaporate along with the evil stench \n"+"that filled the air. Jeeze, maybe you've\n"+"done it. Have you really won?\n"+"\n"+
"Something rumbles in the distance.\n"+"A blue light begins to glow inside the\n"+"ruined skull of the demon-spitter.");public static Ꮥ ᇝ=new Ꮥ("T5TEXT",
"What now? Looks totally different. Kind\n"+"of like King Tut's condo. Well,\n"+"whatever's here can't be any worse\n"+"than usual. Can it?  Or maybe it's best\n"+
"to let sleeping gods lie..");public static Ꮥ ᇜ=new Ꮥ("T6TEXT","Time for a vacation. You've burst the\n"+
"bowels of hell and by golly you're ready\n"+"for a break. You mutter to yourself,\n"+"Maybe someone else can kick Hell's ass\n"+
"next time around. Ahead lies a quiet town,\n"+"with peaceful flowing water, quaint\n"+"buildings, and presumably no Hellspawn.\n"+"\n"+
"As you step off the transport, you hear\n"+"the stomp of a cyberdemon's iron shoe.");public static Ꮥ ᇛ=new Ꮥ("CC_ZOMBIE","ZOMBIEMAN");public static Ꮥ ᇚ=new Ꮥ(
"CC_SHOTGUN","SHOTGUN GUY");public static Ꮥ ᇙ=new Ꮥ("CC_HEAVY","HEAVY WEAPON DUDE");public static Ꮥ ᇘ=new Ꮥ("CC_IMP","IMP");public
static Ꮥ ᇗ=new Ꮥ("CC_DEMON","DEMON");public static Ꮥ ᇖ=new Ꮥ("CC_LOST","LOST SOUL");public static Ꮥ ᇕ=new Ꮥ("CC_CACO",
"CACODEMON");public static Ꮥ ᇔ=new Ꮥ("CC_HELL","HELL KNIGHT");public static Ꮥ ᇓ=new Ꮥ("CC_BARON","BARON OF HELL");public static Ꮥ ᇒ
=new Ꮥ("CC_ARACH","ARACHNOTRON");public static Ꮥ ᆿ=new Ꮥ("CC_PAIN","PAIN ELEMENTAL");public static Ꮥ ሸ=new Ꮥ("CC_REVEN",
"REVENANT");public static Ꮥ ቇ=new Ꮥ("CC_MANCU","MANCUBUS");public static Ꮥ ኋ=new Ꮥ("CC_ARCH","ARCH-VILE");public static Ꮥ ኌ=new Ꮥ(
"CC_SPIDER","THE SPIDER MASTERMIND");public static Ꮥ ኍ=new Ꮥ("CC_CYBER","THE CYBERDEMON");public static Ꮥ ነ=new Ꮥ("CC_HERO",
"OUR HERO");}}static partial class ኑ{public static MyTuple<Ꮥ,Ꮥ>[]ኒ=new MyTuple<Ꮥ,Ꮥ>[]{MyTuple.Create(new Ꮥ("SW1BRCOM"),new Ꮥ(
"SW2BRCOM")),MyTuple.Create(new Ꮥ("SW1BRN1"),new Ꮥ("SW2BRN1")),MyTuple.Create(new Ꮥ("SW1BRN2"),new Ꮥ("SW2BRN2")),MyTuple.Create(
new Ꮥ("SW1BRNGN"),new Ꮥ("SW2BRNGN")),MyTuple.Create(new Ꮥ("SW1BROWN"),new Ꮥ("SW2BROWN")),MyTuple.Create(new Ꮥ("SW1COMM"),
new Ꮥ("SW2COMM")),MyTuple.Create(new Ꮥ("SW1COMP"),new Ꮥ("SW2COMP")),MyTuple.Create(new Ꮥ("SW1DIRT"),new Ꮥ("SW2DIRT")),
MyTuple.Create(new Ꮥ("SW1EXIT"),new Ꮥ("SW2EXIT")),MyTuple.Create(new Ꮥ("SW1GRAY"),new Ꮥ("SW2GRAY")),MyTuple.Create(new Ꮥ(
"SW1GRAY1"),new Ꮥ("SW2GRAY1")),MyTuple.Create(new Ꮥ("SW1METAL"),new Ꮥ("SW2METAL")),MyTuple.Create(new Ꮥ("SW1PIPE"),new Ꮥ("SW2PIPE"
)),MyTuple.Create(new Ꮥ("SW1SLAD"),new Ꮥ("SW2SLAD")),MyTuple.Create(new Ꮥ("SW1STARG"),new Ꮥ("SW2STARG")),MyTuple.Create(
new Ꮥ("SW1STON1"),new Ꮥ("SW2STON1")),MyTuple.Create(new Ꮥ("SW1STON2"),new Ꮥ("SW2STON2")),MyTuple.Create(new Ꮥ("SW1STONE"),
new Ꮥ("SW2STONE")),MyTuple.Create(new Ꮥ("SW1STRTN"),new Ꮥ("SW2STRTN")),MyTuple.Create(new Ꮥ("SW1BLUE"),new Ꮥ("SW2BLUE")),
MyTuple.Create(new Ꮥ("SW1CMT"),new Ꮥ("SW2CMT")),MyTuple.Create(new Ꮥ("SW1GARG"),new Ꮥ("SW2GARG")),MyTuple.Create(new Ꮥ(
"SW1GSTON"),new Ꮥ("SW2GSTON")),MyTuple.Create(new Ꮥ("SW1HOT"),new Ꮥ("SW2HOT")),MyTuple.Create(new Ꮥ("SW1LION"),new Ꮥ("SW2LION")),
MyTuple.Create(new Ꮥ("SW1SATYR"),new Ꮥ("SW2SATYR")),MyTuple.Create(new Ꮥ("SW1SKIN"),new Ꮥ("SW2SKIN")),MyTuple.Create(new Ꮥ(
"SW1VINE"),new Ꮥ("SW2VINE")),MyTuple.Create(new Ꮥ("SW1WOOD"),new Ꮥ("SW2WOOD")),MyTuple.Create(new Ꮥ("SW1PANEL"),new Ꮥ("SW2PANEL")
),MyTuple.Create(new Ꮥ("SW1ROCK"),new Ꮥ("SW2ROCK")),MyTuple.Create(new Ꮥ("SW1MET2"),new Ꮥ("SW2MET2")),MyTuple.Create(new
Ꮥ("SW1WDMET"),new Ꮥ("SW2WDMET")),MyTuple.Create(new Ꮥ("SW1BRIK"),new Ꮥ("SW2BRIK")),MyTuple.Create(new Ꮥ("SW1MOD1"),new Ꮥ(
"SW2MOD1")),MyTuple.Create(new Ꮥ("SW1ZIM"),new Ꮥ("SW2ZIM")),MyTuple.Create(new Ꮥ("SW1STON6"),new Ꮥ("SW2STON6")),MyTuple.Create(
new Ꮥ("SW1TEK"),new Ꮥ("SW2TEK")),MyTuple.Create(new Ꮥ("SW1MARB"),new Ꮥ("SW2MARB")),MyTuple.Create(new Ꮥ("SW1SKULL"),new Ꮥ(
"SW2SKULL"))};}static partial class ኑ{public static ᒻ[]ᅗ=new ᒻ[]{new ᒻ(false,"NUKAGE3","NUKAGE1",8),new ᒻ(false,"FWATER4",
"FWATER1",8),new ᒻ(false,"SWATER4","SWATER1",8),new ᒻ(false,"LAVA4","LAVA1",8),new ᒻ(false,"BLOOD3","BLOOD1",8),new ᒻ(false,
"RROCK08","RROCK05",8),new ᒻ(false,"SLIME04","SLIME01",8),new ᒻ(false,"SLIME08","SLIME05",8),new ᒻ(false,"SLIME12","SLIME09",8),
new ᒻ(true,"BLODGR4","BLODGR1",8),new ᒻ(true,"SLADRIP3","SLADRIP1",8),new ᒻ(true,"BLODRIP4","BLODRIP1",8),new ᒻ(true,
"FIREWALL","FIREWALA",8),new ᒻ(true,"GSTFONT3","GSTFONT1",8),new ᒻ(true,"FIRELAVA","FIRELAV3",8),new ᒻ(true,"FIREMAG3","FIREMAG1",
8),new ᒻ(true,"FIREBLU2","FIREBLU1",8),new ᒻ(true,"ROCKRED3","ROCKRED1",8),new ᒻ(true,"BFALL4","BFALL1",8),new ᒻ(true,
"SFALL4","SFALL1",8),new ᒻ(true,"WFALL4","WFALL1",8),new ᒻ(true,"DBRAIN4","DBRAIN1",8)};}static partial class ኑ{public static ઋ[
]ኔ=new ઋ[]{new ઋ(ᓩ.ᓫ,ᚏ.ᚻ,ᚏ.ᚺ,ᚏ.ધ,ᚏ.ᚼ,ᚏ.ᚠ),new ઋ(ᓩ.З,ᚏ.ᛁ,ᚏ.ᛇ,ᚏ.ਸ,ᚏ.ᛂ,ᚏ.ᛆ),new ઋ(ᓩ.ᓪ,ᚏ.ᚸ,ᚏ.ᚹ,ᚏ.ᛈ,ᚏ.ᚷ,ᚏ.ᚮ),new ઋ(ᓩ.З,ᚏ.ᙦ,ᚏ.ᙥ
,ᚏ.ᙤ,ᚏ.ᙧ,ᚏ.ᙘ),new ઋ(ᓩ.પ,ᚏ.ᙕ,ᚏ.ᙖ,ᚏ.પ,ᚏ.ᙔ,ᚏ.ᙑ),new ઋ(ᓩ.ظ,ᚏ.ᙿ,ᚏ.ᙾ,ᚏ.Ϲ,ᚏ.ᚁ,ᚏ.ᚃ),new ઋ(ᓩ.ظ,ᚏ.ᚇ,ᚏ.ᚆ,ᚏ.Ϻ,ᚏ.ᚈ,ᚏ.ᚌ),new ઋ(ᓩ.ᓫ,ᚏ.ᙠ,
ᚏ.ᙌ,ᚏ.ન,ᚏ.ᙪ,ᚏ.ᚠ),new ઋ(ᓩ.ᓪ,ᚏ.ᚪ,ᚏ.ᚫ,ᚏ.ᚬ,ᚏ.ᚩ,ᚏ.ᙢ)};}static class ን{public static string ቦ(byte[]f,int ù,int ኖ){var û=0;for(
var Ä=0;Ä<ኖ;Ä++){if(f[ù+Ä]==0){break;}û++;}var ኊ=new char[û];for(var Ä=0;Ä<ኊ.Length;Ä++){var Á=f[ù+Ä];if('a'<=Á&&Á<='z'){Á
-=0x20;}ኊ[Ä]=(char)Á;}return new string(ኊ);}}public enum ኈ{ኇ=-1,ኆ=0,ኅ,ኄ,ኃ,ኂ,ኁ,ኀ,ቿ,ቾ,ች,ና,ኗ,ኰ,ኩ,ኪ,ካ,ኬ,ክ,ኮ,ኯ,ኲ,ኹ,ኳ,ޚ,ޙ,ኴ,ኵ,ኸ,
ኺ,ከ,ኧ,ኦ,እ,ኤ,ኣ,ኢ,ኡ,አ,ኟ,ኞ,ኝ,ኜ,ኛ,ኚ,ኙ,ኘ,ቼ,ቻ,ሹ,ቈ,ቊ,ቋ,ቌ,ቍ,ቐ,ቒ,ቛ,ቓ,ቔ,ቕ,ቖ,ቘ,ቚ,Ǧ,ቆ,ቅ,ቄ,ᄩ,ቃ,ቂ,ቁ,ቀ,ሿ,ĳ,Ķ,ሾ,ሽ,ሼ,ሻ,ሺ,ቑ,ቜ,ቱ,ቪ,ቫ,ቬ,ቭ,ቮ,ቯ
,ተ,ቲ,ቹ,ታ,ቴ,ት,ቶ,ቷ,ቸ,ቺ,ቩ,ஓ,ŏ}static class ቨ{public static char ቧ(ኈ ብ){switch(ብ){case ኈ.ኆ:return'a';case ኈ.ኅ:return'b';case
ኈ.ኄ:return'c';case ኈ.ኃ:return'd';case ኈ.ኂ:return'e';case ኈ.ኁ:return'f';case ኈ.ኀ:return'g';case ኈ.ቿ:return'h';case ኈ.ቾ:
return'i';case ኈ.ች:return'j';case ኈ.ና:return'k';case ኈ.ኗ:return'l';case ኈ.ኰ:return'm';case ኈ.ኩ:return'n';case ኈ.ኪ:return'o';
case ኈ.ካ:return'p';case ኈ.ኬ:return'q';case ኈ.ክ:return'r';case ኈ.ኮ:return's';case ኈ.ኯ:return't';case ኈ.ኲ:return'u';case ኈ.ኹ:
return'v';case ኈ.ኳ:return'w';case ኈ.ޚ:return'x';case ኈ.ޙ:return'y';case ኈ.ኴ:return'z';case ኈ.ኵ:return'0';case ኈ.ኸ:return'1';
case ኈ.ኺ:return'2';case ኈ.ከ:return'3';case ኈ.ኧ:return'4';case ኈ.ኦ:return'5';case ኈ.እ:return'6';case ኈ.ኤ:return'7';case ኈ.ኣ:
return'8';case ኈ.ኢ:return'9';case ኈ.ቼ:return'[';case ኈ.ቻ:return']';case ኈ.ሹ:return';';case ኈ.ቈ:return',';case ኈ.ቊ:return'.';
case ኈ.ቋ:return'"';case ኈ.ቌ:return'/';case ኈ.ቍ:return'\\';case ኈ.ቒ:return'=';case ኈ.ቛ:return'-';case ኈ.ቓ:return' ';case ኈ.ᄩ:
return'+';case ኈ.ቃ:return'-';case ኈ.ቂ:return'*';case ኈ.ቁ:return'/';case ኈ.ሾ:return'0';case ኈ.ሽ:return'1';case ኈ.ሼ:return'2';
case ኈ.ሻ:return'3';case ኈ.ሺ:return'4';case ኈ.ቑ:return'5';case ኈ.ቜ:return'6';case ኈ.ቱ:return'7';case ኈ.ቪ:return'8';case ኈ.ቫ:
return'9';default:return'\0';}}public static string ቦ(ኈ ብ){switch(ብ){case ኈ.ኆ:return"a";case ኈ.ኅ:return"b";case ኈ.ኄ:return"c";
case ኈ.ኃ:return"d";case ኈ.ኂ:return"e";case ኈ.ኁ:return"f";case ኈ.ኀ:return"g";case ኈ.ቿ:return"h";case ኈ.ቾ:return"i";case ኈ.ች:
return"j";case ኈ.ና:return"k";case ኈ.ኗ:return"l";case ኈ.ኰ:return"m";case ኈ.ኩ:return"n";case ኈ.ኪ:return"o";case ኈ.ካ:return"p";
case ኈ.ኬ:return"q";case ኈ.ክ:return"r";case ኈ.ኮ:return"s";case ኈ.ኯ:return"t";case ኈ.ኲ:return"u";case ኈ.ኹ:return"v";case ኈ.ኳ:
return"w";case ኈ.ޚ:return"x";case ኈ.ޙ:return"y";case ኈ.ኴ:return"z";case ኈ.ኵ:return"num0";case ኈ.ኸ:return"num1";case ኈ.ኺ:return
"num2";case ኈ.ከ:return"num3";case ኈ.ኧ:return"num4";case ኈ.ኦ:return"num5";case ኈ.እ:return"num6";case ኈ.ኤ:return"num7";case ኈ.ኣ:
return"num8";case ኈ.ኢ:return"num9";case ኈ.ኡ:return"escape";case ኈ.አ:return"lcontrol";case ኈ.ኟ:return"lshift";case ኈ.ኞ:return
"lalt";case ኈ.ኝ:return"lsystem";case ኈ.ኜ:return"rcontrol";case ኈ.ኛ:return"rshift";case ኈ.ኚ:return"ralt";case ኈ.ኙ:return
"rsystem";case ኈ.ኘ:return"menu";case ኈ.ቼ:return"lbracket";case ኈ.ቻ:return"rbracket";case ኈ.ሹ:return"semicolon";case ኈ.ቈ:return
"comma";case ኈ.ቊ:return"period";case ኈ.ቋ:return"quote";case ኈ.ቌ:return"slash";case ኈ.ቍ:return"backslash";case ኈ.ቐ:return"tilde"
;case ኈ.ቒ:return"equal";case ኈ.ቛ:return"hyphen";case ኈ.ቓ:return"space";case ኈ.ቔ:return"enter";case ኈ.ቕ:return"backspace";
case ኈ.ቖ:return"tab";case ኈ.ቘ:return"pageup";case ኈ.ቚ:return"pagedown";case ኈ.Ǧ:return"end";case ኈ.ቆ:return"home";case ኈ.ቅ:
return"insert";case ኈ.ቄ:return"delete";case ኈ.ᄩ:return"add";case ኈ.ቃ:return"subtract";case ኈ.ቂ:return"multiply";case ኈ.ቁ:
return"divide";case ኈ.ቀ:return"left";case ኈ.ሿ:return"right";case ኈ.ĳ:return"up";case ኈ.Ķ:return"down";case ኈ.ሾ:return"numpad0"
;case ኈ.ሽ:return"numpad1";case ኈ.ሼ:return"numpad2";case ኈ.ሻ:return"numpad3";case ኈ.ሺ:return"numpad4";case ኈ.ቑ:return
"numpad5";case ኈ.ቜ:return"numpad6";case ኈ.ቱ:return"numpad7";case ኈ.ቪ:return"numpad8";case ኈ.ቫ:return"numpad9";case ኈ.ቬ:return"f1"
;case ኈ.ቭ:return"f2";case ኈ.ቮ:return"f3";case ኈ.ቯ:return"f4";case ኈ.ተ:return"f5";case ኈ.ቲ:return"f6";case ኈ.ቹ:return"f7";
case ኈ.ታ:return"f8";case ኈ.ቴ:return"f9";case ኈ.ት:return"f10";case ኈ.ቶ:return"f11";case ኈ.ቷ:return"f12";case ኈ.ቸ:return"f13";
case ኈ.ቺ:return"f14";case ኈ.ቩ:return"f15";case ኈ.ஓ:return"pause";default:return"unknown";}}public static ኈ ቤ(string u){
switch(u){case"a":return ኈ.ኆ;case"b":return ኈ.ኅ;case"c":return ኈ.ኄ;case"d":return ኈ.ኃ;case"e":return ኈ.ኂ;case"f":return ኈ.ኁ;
case"g":return ኈ.ኀ;case"h":return ኈ.ቿ;case"i":return ኈ.ቾ;case"j":return ኈ.ች;case"k":return ኈ.ና;case"l":return ኈ.ኗ;case"m":
return ኈ.ኰ;case"n":return ኈ.ኩ;case"o":return ኈ.ኪ;case"p":return ኈ.ካ;case"q":return ኈ.ኬ;case"r":return ኈ.ክ;case"s":return ኈ.ኮ;
case"t":return ኈ.ኯ;case"u":return ኈ.ኲ;case"v":return ኈ.ኹ;case"w":return ኈ.ኳ;case"x":return ኈ.ޚ;case"y":return ኈ.ޙ;case"z":
return ኈ.ኴ;case"num0":return ኈ.ኵ;case"num1":return ኈ.ኸ;case"num2":return ኈ.ኺ;case"num3":return ኈ.ከ;case"num4":return ኈ.ኧ;case
"num5":return ኈ.ኦ;case"num6":return ኈ.እ;case"num7":return ኈ.ኤ;case"num8":return ኈ.ኣ;case"num9":return ኈ.ኢ;case"escape":return
ኈ.ኡ;case"lcontrol":return ኈ.አ;case"lshift":return ኈ.ኟ;case"lalt":return ኈ.ኞ;case"lsystem":return ኈ.ኝ;case"rcontrol":
return ኈ.ኜ;case"rshift":return ኈ.ኛ;case"ralt":return ኈ.ኚ;case"rsystem":return ኈ.ኙ;case"menu":return ኈ.ኘ;case"lbracket":return
ኈ.ቼ;case"rbracket":return ኈ.ቻ;case"semicolon":return ኈ.ሹ;case"comma":return ኈ.ቈ;case"period":return ኈ.ቊ;case"quote":
return ኈ.ቋ;case"slash":return ኈ.ቌ;case"backslash":return ኈ.ቍ;case"tilde":return ኈ.ቐ;case"equal":return ኈ.ቒ;case"hyphen":return
ኈ.ቛ;case"space":return ኈ.ቓ;case"enter":return ኈ.ቔ;case"backspace":return ኈ.ቕ;case"tab":return ኈ.ቖ;case"pageup":return ኈ.ቘ
;case"pagedown":return ኈ.ቚ;case"end":return ኈ.Ǧ;case"home":return ኈ.ቆ;case"insert":return ኈ.ቅ;case"delete":return ኈ.ቄ;
case"add":return ኈ.ᄩ;case"subtract":return ኈ.ቃ;case"multiply":return ኈ.ቂ;case"divide":return ኈ.ቁ;case"left":return ኈ.ቀ;case
"right":return ኈ.ሿ;case"up":return ኈ.ĳ;case"down":return ኈ.Ķ;case"numpad0":return ኈ.ሾ;case"numpad1":return ኈ.ሽ;case"numpad2":
return ኈ.ሼ;case"numpad3":return ኈ.ሻ;case"numpad4":return ኈ.ሺ;case"numpad5":return ኈ.ቑ;case"numpad6":return ኈ.ቜ;case"numpad7":
return ኈ.ቱ;case"numpad8":return ኈ.ቪ;case"numpad9":return ኈ.ቫ;case"f1":return ኈ.ቬ;case"f2":return ኈ.ቭ;case"f3":return ኈ.ቮ;case
"f4":return ኈ.ቯ;case"f5":return ኈ.ተ;case"f6":return ኈ.ቲ;case"f7":return ኈ.ቹ;case"f8":return ኈ.ታ;case"f9":return ኈ.ቴ;case
"f10":return ኈ.ት;case"f11":return ኈ.ቶ;case"f12":return ኈ.ቷ;case"f13":return ኈ.ቸ;case"f14":return ኈ.ቺ;case"f15":return ኈ.ቩ;
case"pause":return ኈ.ஓ;default:return ኈ.ኇ;}}}sealed class ባ{private ጟ ǋ;private ɥ ቢ;private ɥ ቡ;private ɥ በ;private ɥ ቝ;
private ɥ ߛ;private ᶌ ፃ;private ĵ ᎏ;private ᯖ Ꮵ;private Ʒ Ꮶ;private Ʒ Ꮷ;private ଇ Ꮸ;private ଇ Ꮹ;private ǳ Ꮺ;private ᜮ Ġ;private
bool Ꮼ;private int Ƃ;private int Ꮽ;private ŉ Ꮾ;public ባ(ጟ ǋ){this.ǋ=ǋ;Ꮶ=new Ʒ(this,ኑ.ጓ.ጶ,null);Ꮷ=new Ʒ(this,ኑ.ጓ.ጹ,null);Ꮸ=
new ଇ(this,ኑ.ጓ.ጵ,()=>ǋ.ᔻ(ᵅ.ᵇ,Ꮽ,1));Ꮹ=new ଇ(this,ኑ.ጓ.ፁ,()=>ǋ.ᐐ());Ꮺ=new ǳ(this,ǋ);በ=new ɥ(this,"M_NEWG",96,14,"M_SKILL",54,
38,2,new ʉ("M_JKILL",16,58,48,63,()=>ǋ.ᔻ(ᵅ.Р,Ꮽ,1),null),new ʉ("M_ROUGH",16,74,48,79,()=>ǋ.ᔻ(ᵅ.ᅼ,Ꮽ,1),null),new ʉ("M_HURT",
16,90,48,95,()=>ǋ.ᔻ(ᵅ.ᵆ,Ꮽ,1),null),new ʉ("M_ULTRA",16,106,48,111,()=>ǋ.ᔻ(ᵅ.ᅽ,Ꮽ,1),null),new ʉ("M_NMARE",16,122,48,127,null
,Ꮸ));if(ǋ.હ.ಖ==ಖ.ᴈ){ቡ=new ɥ(this,"M_EPISOD",54,38,0,new ʉ("M_EPI1",16,58,48,63,()=>Ꮽ=1,በ),new ʉ("M_EPI2",16,74,48,79,()=>
Ꮽ=2,በ),new ʉ("M_EPI3",16,90,48,95,()=>Ꮽ=3,በ),new ʉ("M_EPI4",16,106,48,111,()=>Ꮽ=4,በ));}else{if(ǋ.હ.ಖ==ಖ.ᴋ){ቡ=new ɥ(this,
"M_EPISOD",54,38,0,new ʉ("M_EPI1",16,58,48,63,()=>Ꮽ=1,በ),new ʉ("M_EPI2",16,74,48,79,null,Ꮶ),new ʉ("M_EPI3",16,90,48,95,null,Ꮶ));}
else{ቡ=new ɥ(this,"M_EPISOD",54,38,0,new ʉ("M_EPI1",16,58,48,63,()=>Ꮽ=1,በ),new ʉ("M_EPI2",16,74,48,79,()=>Ꮽ=2,በ),new ʉ(
"M_EPI3",16,90,48,95,()=>Ꮽ=3,በ));}}var Ŗ=ǋ.હ.ᴽ;var Ꮿ=ǋ.હ.ᴾ;ߛ=new ɥ(this,"M_SVOL",60,38,0,new ʖ("M_SFXVOL",48,59,80,64,Ŗ.ᯊ+1,()=>
Ŗ.ᯋ,Ꮴ=>Ŗ.ᯋ=Ꮴ),new ʖ("M_MUSVOL",48,91,80,96,Ꮿ.ᯊ+1,()=>Ꮿ.ᯋ,Ꮴ=>Ꮿ.ᯋ=Ꮴ));var Ꮳ=ǋ.હ.ᴱ;var Ꮲ=ǋ.હ.ᵉ;ቝ=new ɥ(this,"M_OPTTTL",108,
15,0,new ʉ("M_ENDGAM",28,32,60,37,null,Ꮹ,()=>ǋ.Ŷ==Ꮣ.સ),new ஔ("M_MESSG",28,48,60,53,"M_MSGON","M_MSGOFF",180,()=>Ꮳ.Ǟ?0:1,u
=>Ꮳ.Ǟ=u==0),new ʖ("M_SCRNSZ",28,80-16,60,85-16,Ꮳ.ǜ+1,()=>Ꮳ.ǝ,ᄺ=>Ꮳ.ǝ=ᄺ),new ʖ("M_MSENS",28,112-16,60,117-16,Ꮲ.Ṁ+1,()=>Ꮲ.ḿ,Ꮱ
=>Ꮲ.ḿ=Ꮱ),new ʉ("M_SVOL",28,144-16,60,149-16,null,ߛ));ፃ=new ᶌ(this,"M_LOADG",72,28,0,new ท(48,49,72,61),new ท(48,65,72,77),
new ท(48,81,72,93),new ท(48,97,72,109),new ท(48,113,72,125),new ท(48,129,72,141));ᎏ=new ĵ(this,"M_SAVEG",72,28,0,new ท(48,
49,72,61),new ท(48,65,72,77),new ท(48,81,72,93),new ท(48,97,72,109),new ท(48,113,72,125),new ท(48,129,72,141));Ꮵ=new ᯖ(
this);if(ǋ.હ.ಖ==ಖ.ᴉ){ቢ=new ɥ(this,"M_DOOM",94,2,0,new ʉ("M_NGAME",65,67,97,72,null,በ),new ʉ("M_OPTION",65,83,97,88,null,ቝ),
new ʉ("M_LOADG",65,99,97,104,null,ፃ),new ʉ("M_SAVEG",65,115,97,120,null,ᎏ,()=>!(ǋ.Ŷ==Ꮣ.સ&&ǋ.સ.Ŷ!=ᵈ.ᔜ)),new ʉ("M_QUITG",65,
131,97,136,null,Ꮺ));}else{ቢ=new ɥ(this,"M_DOOM",94,2,0,new ʉ("M_NGAME",65,59,97,64,null,ቡ),new ʉ("M_OPTION",65,75,97,80,
null,ቝ),new ʉ("M_LOADG",65,91,97,96,null,ፃ),new ʉ("M_SAVEG",65,107,97,112,null,ᎏ,()=>!(ǋ.Ŷ==Ꮣ.સ&&ǋ.સ.Ŷ!=ᵈ.ᔜ)),new ʉ(
"M_RDTHIS",65,123,97,128,null,Ꮵ),new ʉ("M_QUITG",65,139,97,144,null,Ꮺ));}Ġ=ቢ;Ꮼ=false;Ƃ=0;Ꮽ=1;Ꮾ=new ŉ();}public bool ଅ(ᕨ Ň){if(Ꮼ){
if(Ġ.ଅ(Ň)){return true;}if(Ň.ᕤ==ኈ.ኡ&&Ň.ܫ==ፎ.ፏ){ಯ();}return true;}else{if(Ň.ᕤ==ኈ.ኡ&&Ň.ܫ==ፎ.ፏ){Ꮰ(ቢ);ರ();ૐ(ɴ.Ȭ);return true;}
if(Ň.ܫ==ፎ.ፏ&&ǋ.Ŷ==Ꮣ.Ꮤ){if(Ň.ᕤ==ኈ.ቔ||Ň.ᕤ==ኈ.ቓ||Ň.ᕤ==ኈ.አ||Ň.ᕤ==ኈ.ኜ||Ň.ᕤ==ኈ.ኡ){Ꮰ(ቢ);ರ();ૐ(ɴ.Ȭ);return true;}}return false;}}
public void ڞ(){Ƃ++;if(Ġ!=null){Ġ.ڞ();}if(Ꮼ&&!ǋ.હ.ᴶ){ǋ.ᕑ();}}public void Ꮰ(ᜮ ɾ){Ġ=ɾ;Ġ.ರ();}public void ರ(){Ꮼ=true;}public void
ಯ(){Ꮼ=false;if(!ǋ.હ.ᴶ){ǋ.ᕐ();}}public void ૐ(ɴ Ǯ){ǋ.હ.ᴽ.ૐ(Ǯ);}public void Ꮯ(){Ꮰ(Ꮷ);}public void Ꮻ(){Ꮰ(Ꮵ);ರ();ૐ(ɴ.Ȭ);}
public void Ᏸ(){Ꮰ(ᎏ);ರ();ૐ(ɴ.Ȭ);}public void ᐍ(){Ꮰ(ፃ);ರ();ૐ(ɴ.Ȭ);}public void ᐇ(){Ꮰ(ߛ);ರ();ૐ(ɴ.Ȭ);}public void ᐈ(){if(ᎏ.Ŏ==-1)
{Ᏸ();}else{var ᐉ=Ꮾ[ᎏ.Ŏ];var ᐊ=new ଇ(this,((string)ኑ.ጓ.ጲ).Replace("%s",ᐉ),()=>ᎏ.Ł(ᎏ.Ŏ));Ꮰ(ᐊ);ರ();ૐ(ɴ.Ȭ);}}public void ᐋ(){
if(ᎏ.Ŏ==-1){var ᐌ=new Ʒ(this,ኑ.ጓ.ጠ,null);Ꮰ(ᐌ);ರ();ૐ(ɴ.Ȭ);}else{var ᐉ=Ꮾ[ᎏ.Ŏ];var ᐊ=new ଇ(this,((string)ኑ.ጓ.ጳ).Replace("%s",
ᐉ),()=>ፃ.ᶝ(ᎏ.Ŏ));Ꮰ(ᐊ);ರ();ૐ(ɴ.Ȭ);}}public void ᐐ(){Ꮰ(Ꮹ);ರ();ૐ(ɴ.Ȭ);}public void ᐎ(){Ꮰ(Ꮺ);ರ();ૐ(ɴ.Ȭ);}public ጟ ጟ=>ǋ;public
ᴆ હ=>ǋ.હ;public ᜮ ᐏ=>Ġ;public bool ᄧ=>Ꮼ;public int ŵ=>Ƃ;public ŉ ŉ=>Ꮾ;}public enum ᐆ{ኇ=-1,ᐅ=0,ᐄ,ᐃ,ᐂ,ᐁ,ŏ}static class Ᏼ{
public static string ቦ(ᐆ Ᏻ){switch(Ᏻ){case ᐆ.ᐅ:return"mouse1";case ᐆ.ᐄ:return"mouse2";case ᐆ.ᐃ:return"mouse3";case ᐆ.ᐂ:return
"mouse4";case ᐆ.ᐁ:return"mouse5";default:return"unknown";}}public static ᐆ ቤ(string u){switch(u){case"mouse1":return ᐆ.ᐅ;case
"mouse2":return ᐆ.ᐄ;case"mouse3":return ᐆ.ᐃ;case"mouse4":return ᐆ.ᐂ;case"mouse5":return ᐆ.ᐁ;default:return ᐆ.ኇ;}}}sealed class Ᏺ
{private static int[]Ᏹ=new int[]{0,8,109,220,222,241,149,107,75,248,254,140,16,66,74,21,211,47,80,242,154,27,205,128,161,
89,77,36,95,110,85,48,212,140,211,249,22,79,200,50,28,188,52,140,202,120,68,145,62,70,184,190,91,197,152,224,149,104,25,
178,252,182,202,182,141,197,4,81,181,242,145,42,39,227,156,198,225,193,219,93,122,175,249,0,175,143,70,239,46,246,163,53,
163,109,168,135,2,235,25,92,20,145,138,77,69,166,78,176,173,212,166,113,94,161,41,50,239,49,111,164,70,60,2,37,171,75,136,
156,11,56,42,146,138,229,73,146,77,61,98,196,135,106,63,197,195,86,96,203,113,101,170,247,181,113,80,250,108,7,255,237,129,
226,79,107,112,166,103,241,24,223,239,120,198,58,60,82,128,3,184,66,143,224,145,224,81,206,163,45,63,90,168,114,59,33,159,
95,28,139,123,98,125,196,15,70,194,253,54,14,109,226,71,17,161,93,186,87,244,138,20,52,123,251,26,36,17,46,52,231,232,76,
31,221,84,37,216,165,212,106,197,242,98,43,39,175,254,145,190,84,118,222,187,136,120,163,236,249};private int ı;public Ᏺ()
{ı=0;}public Ᏺ(int Ꮮ){ı=Ꮮ&0xff;}public int ѐ(){ı=(ı+1)&0xff;return Ᏹ[ı];}public void ŷ(){ı=0;}}public enum Ꮣ{ந,Ꮤ,સ}sealed
class Ꮥ{private static Dictionary<string,Ꮥ>Ꮦ=new Dictionary<string,Ꮥ>();private static Dictionary<string,Ꮥ>Ꮧ=new Dictionary<
string,Ꮥ>();private string Ꮡ;private string Ꮠ;public Ꮥ(string Ꮡ){this.Ꮡ=Ꮡ;Ꮠ=Ꮡ;if(!Ꮦ.ContainsKey(Ꮡ)){Ꮦ.Add(Ꮡ,this);}}public Ꮥ(
string Ĭ,string Ꮡ):this(Ꮡ){Ꮧ.Add(Ĭ,this);}public override string ToString(){return Ꮠ;}public char this[int ı]{get{return Ꮠ[ı];
}}public static implicit operator string(Ꮥ Ꮞ){return Ꮞ.Ꮠ;}public static void Ꮢ(string Ꮡ,string Ꮠ){Ꮥ Ꮞ;if(Ꮦ.TryGetValue(Ꮡ,
out Ꮞ)){Ꮞ.Ꮠ=Ꮠ;}}public static void Ꮟ(string Ĭ,string u){Ꮥ Ꮞ;if(Ꮧ.TryGetValue(Ĭ,out Ꮞ)){Ꮞ.Ꮠ=u;}}}sealed class Ꮝ{private int
ځ;private int ˢ;private byte[]f;private ڦ[]ኊ;public Ꮝ(ಆ þ,int ځ,int ˢ){this.ځ=ځ;this.ˢ=ˢ;f=new byte[ځ*ˢ];ኊ=new ڦ[128];for
(var Ä=0;Ä<ኊ.Length;Ä++){var Ĭ="STCFN"+Ä.ToString("000");var ý=þ.ಞ(Ĭ);if(ý!=-1){ኊ[Ä]=ڦ.ċ(Ĭ,þ.ಠ(ý));}}}public void Ꮘ(ڦ ڌ,
int ǃ,int ǂ,int ǉ){var Ꮬ=ǃ-ǉ*ڌ.پ;var Ꮫ=ǂ-ǉ*ڌ.ٽ;var Ꮪ=ǉ*ڌ.Ǒ;var Ä=0;var ڂ=Ꮖ.Ꮒ/ǉ-Ꮖ.Ꮏ;var আ=Ꮖ.Ꮒ/ǉ;if(Ꮬ<0){var Ꮩ=-Ꮬ;ڂ+=Ꮩ*আ;Ä+=Ꮩ
;}if(Ꮬ+Ꮪ>ځ){var Ꮩ=Ꮬ+Ꮪ-ځ;Ꮪ-=Ꮩ;}for(;Ä<Ꮪ;Ä++){ࠨ(ڌ.ټ[ڂ.Ꮄ()],Ꮬ+Ä,Ꮫ,ǉ);ڂ+=আ;}}public void Ꮭ(ڦ ڌ,int ǃ,int ǂ,int ǉ){var Ꮬ=ǃ-ǉ*ڌ
.پ;var Ꮫ=ǂ-ǉ*ڌ.ٽ;var Ꮪ=ǉ*ڌ.Ǒ;var Ä=0;var ڂ=Ꮖ.Ꮒ/ǉ-Ꮖ.Ꮏ;var আ=Ꮖ.Ꮒ/ǉ;if(Ꮬ<0){var Ꮩ=-Ꮬ;ڂ+=Ꮩ*আ;Ä+=Ꮩ;}if(Ꮬ+Ꮪ>ځ){var Ꮩ=Ꮬ+Ꮪ-ځ;Ꮪ-=Ꮩ
;}for(;Ä<Ꮪ;Ä++){var ࡂ=ڌ.Ǒ-ڂ.Ꮄ()-1;ࠨ(ڌ.ټ[ࡂ],Ꮬ+Ä,Ꮫ,ǉ);ڂ+=আ;}}private void ࠨ(ᖒ[]ࠒ,int ǃ,int ǂ,int ǉ){var আ=Ꮖ.Ꮒ/ǉ;foreach(var
ࠐ in ࠒ){var ᐯ=ǉ*ࠐ.ᖔ;var ᐰ=ǉ*ࠐ.ᖏ;var ᅠ=ࠐ.ɟ;var Ꮫ=ǂ+ᐯ;var ᐱ=ᐰ;var Ä=0;var É=ˢ*ǃ+Ꮫ;var ڂ=Ꮖ.Ꮒ/ǉ-Ꮖ.Ꮏ;if(Ꮫ<0){var Ꮩ=-Ꮫ;É+=Ꮩ;ڂ+=
Ꮩ*আ;Ä+=Ꮩ;}if(Ꮫ+ᐱ>ˢ){var Ꮩ=Ꮫ+ᐱ-ˢ;ᐱ-=Ꮩ;}for(;Ä<ᐱ;Ä++){f[É]=ࠐ.Ꮁ[ᅠ+ڂ.Ꮄ()];É++;ڂ+=আ;}}}public void ᐭ(IReadOnlyList<char>Ǉ,int
ǃ,int ǂ,int ǉ){var Ꮬ=ǃ;var Ꮫ=ǂ-7*ǉ;foreach(var ම in Ǉ){if(ම>=ኊ.Length){continue;}if(ම==32){Ꮬ+=4*ǉ;continue;}var ı=(int)ම;
if('a'<=ı&&ı<='z'){ı=ı-'a'+'A';}var ڌ=ኊ[ı];if(ڌ==null){continue;}Ꮘ(ڌ,Ꮬ,Ꮫ,ǉ);Ꮬ+=ǉ*ڌ.Ǒ;}}public void ᐮ(char ම,int ǃ,int ǂ,
int ǉ){var Ꮬ=ǃ;var Ꮫ=ǂ-7*ǉ;if(ම>=ኊ.Length){return;}if(ම==32){return;}var ı=(int)ම;if('a'<=ı&&ı<='z'){ı=ı-'a'+'A';}var ڌ=ኊ[ı
];if(ڌ==null){return;}Ꮘ(ڌ,Ꮬ,Ꮫ,ǉ);}public void ᐭ(string Ǉ,int ǃ,int ǂ,int ǉ){var Ꮬ=ǃ;var Ꮫ=ǂ-7*ǉ;foreach(var ම in Ǉ){if(ම
>=ኊ.Length){continue;}if(ම==32){Ꮬ+=4*ǉ;continue;}var ı=(int)ම;if('a'<=ı&&ı<='z'){ı=ı-'a'+'A';}var ڌ=ኊ[ı];if(ڌ==null){
continue;}Ꮘ(ڌ,Ꮬ,Ꮫ,ǉ);Ꮬ+=ǉ*ڌ.Ǒ;}}public int ᐳ(char ම,int ǉ){if(ම>=ኊ.Length){return 0;}if(ම==32){return 4*ǉ;}var ı=(int)ම;if('a'<=
ı&&ı<='z'){ı=ı-'a'+'A';}var ڌ=ኊ[ı];if(ڌ==null){return 0;}return ǉ*ڌ.Ǒ;}public int ᐲ(IReadOnlyList<char>Ǉ,int ǉ){var ځ=0;
foreach(var ම in Ǉ){if(ම>=ኊ.Length){continue;}if(ම==32){ځ+=4*ǉ;continue;}var ı=(int)ම;if('a'<=ı&&ı<='z'){ı=ı-'a'+'A';}var ڌ=ኊ[ı
];if(ڌ==null){continue;}ځ+=ǉ*ڌ.Ǒ;}return ځ;}public int ᐲ(string Ǉ,int ǉ){var ځ=0;foreach(var ම in Ǉ){if(ම>=ኊ.Length){
continue;}if(ම==32){ځ+=4*ǉ;continue;}var ı=(int)ම;if('a'<=ı&&ı<='z'){ı=ı-'a'+'A';}var ڌ=ኊ[ı];if(ڌ==null){continue;}ځ+=ǉ*ڌ.Ǒ;}
return ځ;}public void অ(int ǃ,int ǂ,int ೡ,int ˡ,int ᐚ){var ǎ=ǃ;var ǐ=ǃ+ೡ;for(var Ꮬ=ǎ;Ꮬ<ǐ;Ꮬ++){var ࠆ=ˢ*Ꮬ+ǂ;for(var Ä=0;Ä<ˡ;Ä++)
{f[ࠆ]=(byte)ᐚ;ࠆ++;}}}[Flags]private enum ᐛ{ᐜ=0,ቀ=1,ሿ=2,Ꭿ=4,Ꮀ=8}private ᐛ ᐝ(float ǃ,float ǂ){var ᐞ=ᐛ.ᐜ;if(ǃ<0){ᐞ|=ᐛ.ቀ;}
else if(ǃ>ځ){ᐞ|=ᐛ.ሿ;}if(ǂ<0){ᐞ|=ᐛ.Ꭿ;}else if(ǂ>ˢ){ᐞ|=ᐛ.Ꮀ;}return ᐞ;}public void ᐟ(float ǎ,float ǆ,float ǐ,float ǅ,int ᐚ){var
ᐙ=ᐝ(ǎ,ǆ);var ᐘ=ᐝ(ǐ,ǅ);var ᐗ=false;while(true){if((ᐙ|ᐘ)==0){ᐗ=true;break;}else if((ᐙ&ᐘ)!=0){break;}else{var ǃ=0.0F;var ǂ=
0.0F;var ᐖ=ᐘ>ᐙ?ᐘ:ᐙ;if((ᐖ&ᐛ.Ꮀ)!=0){ǃ=ǎ+(ǐ-ǎ)*(ˢ-ǆ)/(ǅ-ǆ);ǂ=ˢ;}else if((ᐖ&ᐛ.Ꭿ)!=0){ǃ=ǎ+(ǐ-ǎ)*(0-ǆ)/(ǅ-ǆ);ǂ=0;}else if((ᐖ&ᐛ.ሿ)
!=0){ǂ=ǆ+(ǅ-ǆ)*(ځ-ǎ)/(ǐ-ǎ);ǃ=ځ;}else if((ᐖ&ᐛ.ቀ)!=0){ǂ=ǆ+(ǅ-ǆ)*(0-ǎ)/(ǐ-ǎ);ǃ=0;}if(ᐖ==ᐙ){ǎ=ǃ;ǆ=ǂ;ᐙ=ᐝ(ǎ,ǆ);}else{ǐ=ǃ;ǅ=ǂ;ᐘ=ᐝ
(ǐ,ǅ);}}}if(ᐗ){var ᐕ=ᕣ.ᕊ((int)ǎ,0,ځ-1);var ᐔ=ᕣ.ᕊ((int)ǆ,0,ˢ-1);var ᐓ=ᕣ.ᕊ((int)ǐ,0,ځ-1);var ᐒ=ᕣ.ᕊ((int)ǅ,0,ˢ-1);ᐑ(ᐕ,ᐔ,ᐓ,ᐒ,
ᐚ);}}private void ᐑ(int ǎ,int ǆ,int ǐ,int ǅ,int ᐚ){var ߝ=ǐ-ǎ;var ᐩ=2*(ߝ<0?-ߝ:ߝ);var ƃ=ߝ<0?-1:1;var Ǆ=ǅ-ǆ;var ᐧ=2*(Ǆ<0?-Ǆ:
Ǆ);var ƅ=Ǆ<0?-1:1;var ǃ=ǎ;var ǂ=ǆ;if(ᐩ>ᐧ){var ᐨ=ᐧ-ᐩ/2;while(true){f[ˢ*ǃ+ǂ]=(byte)ᐚ;if(ǃ==ǐ){return;}if(ᐨ>=0){ǂ+=ƅ;ᐨ-=ᐩ;}ǃ
+=ƃ;ᐨ+=ᐧ;}}else{var ᐨ=ᐩ-ᐧ/2;while(true){f[ˢ*ǃ+ǂ]=(byte)ᐚ;if(ǂ==ǅ){return;}if(ᐨ>=0){ǃ+=ƃ;ᐨ-=ᐧ;}ǂ+=ƅ;ᐨ+=ᐩ;}}}public int Ǒ=>ځ
;public int ǡ=>ˢ;public byte[]Ꮁ=>f;}static class ᐪ{private static ڦ ᐫ;public static ڦ ᐬ(){if(ᐫ!=null){return ᐫ;}else{var
ځ=64;var ˢ=128;var f=new byte[ˢ+32];for(var ǂ=0;ǂ<f.Length;ǂ++){f[ǂ]=ǂ/32%2==0?(byte)80:(byte)96;}var څ=new ᖒ[ځ][];var ᐦ=
new ᖒ[]{new ᖒ(0,f,0,ˢ)};var ᐥ=new ᖒ[]{new ᖒ(0,f,32,ˢ)};for(var ǃ=0;ǃ<ځ;ǃ++){څ[ǃ]=ǃ/32%2==0?ᐦ:ᐥ;}ᐫ=new ڦ("DUMMY",ځ,ˢ,32,128,
څ);return ᐫ;}}private static Dictionary<int,ථ>ᐤ=new Dictionary<int,ථ>();public static ථ ᐣ(int ˢ){if(ᐤ.ContainsKey(ˢ)){
return ᐤ[ˢ];}else{var ڌ=new ᅂ[]{new ᅂ(0,0,ᐬ())};ᐤ.Add(ˢ,new ථ("DUMMY",false,64,ˢ,ڌ));return ᐤ[ˢ];}}private static ᎂ ᐢ;public
static ᎂ ᐡ(){if(ᐢ!=null){return ᐢ;}else{var f=new byte[64*64];var ࠃ=0;for(var ǂ=0;ǂ<64;ǂ++){for(var ǃ=0;ǃ<64;ǃ++){f[ࠃ]=((ǃ/32)
^(ǂ/32))==0?(byte)80:(byte)96;ࠃ++;}}ᐢ=new ᎂ("DUMMY",f);return ᐢ;}}private static ᎂ ᐠ;public static ᎂ Ꮨ(){if(ᐠ!=null){
return ᐠ;}else{ᐠ=new ᎂ("DUMMY",ᐡ().Ꮁ);return ᐠ;}}}sealed class Ꮜ:ᮟ{private ᎂ[]ü;private Dictionary<string,ᎂ>Ꭰ;private
Dictionary<string,int>ᅩ;private int Ꭱ;private ᎂ Ꭲ;public Ꮜ(ಆ þ){var Ꭳ=þ.ಞ("F_START")+1;var Ꭴ=þ.ಞ("F_END")-1;var ú=Ꭴ-Ꭳ+1;ü=new ᎂ[ú]
;Ꭰ=new Dictionary<string,ᎂ>();ᅩ=new Dictionary<string,int>();for(var ý=Ꭳ;ý<=Ꭴ;ý++){if(þ.ಡ(ý)!=4096){continue;}var Ć=ý-Ꭳ;
var Ĭ=þ.ಘ[ý].Ń;var ࠉ=Ĭ!="F_SKY1"?ᐪ.ᐡ():ᐪ.Ꮨ();ü[Ć]=ࠉ;Ꭰ[Ĭ]=ࠉ;ᅩ[Ĭ]=Ć;}Ꭱ=ᅩ["F_SKY1"];Ꭲ=Ꭰ["F_SKY1"];}public int ᮾ(string Ĭ){if(ᅩ
.ContainsKey(Ĭ)){return ᅩ[Ĭ];}else{return-1;}}public IEnumerator<ᎂ>GetEnumerator(){return((IEnumerable<ᎂ>)ü).
GetEnumerator();}IEnumerator IEnumerable.GetEnumerator(){return ü.GetEnumerator();}public int Count=>ü.Length;public ᎂ this[int ܣ]=>ü
[ܣ];public ᎂ this[string Ĭ]=>Ꭰ[Ĭ];public int ᜅ=>Ꭱ;public ᎂ ᯇ=>Ꭲ;}sealed class ᎎ:Ḃ{private ၜ[]ဌ;public ᎎ(ಆ þ){var ў=new
Dictionary<string,List<ལ>>();for(var Ä=0;Ä<(int)э.ŏ;Ä++){ў.Add(ኑ.ጙ[Ä],new List<ལ>());}var چ=new Dictionary<int,ڦ>();foreach(var ý
in ས(þ)){var Ĭ=þ.ಘ[ý].Ń.Substring(0,4);if(!ў.ContainsKey(Ĭ)){continue;}var ǯ=ў[Ĭ];{var Ч=þ.ಘ[ý].Ń[4]-'A';var ཪ=þ.ಘ[ý].Ń[5]
-'0';while(ǯ.Count<Ч+1){ǯ.Add(new ལ());}if(ཪ==0){for(var Ä=0;Ä<8;Ä++){if(ǯ[Ч].ജ[Ä]==null){ǯ[Ч].ജ[Ä]=ᐪ.ᐬ();ǯ[Ч].ཀྵ[Ä]=false
;}}}else{if(ǯ[Ч].ജ[ཪ-1]==null){ǯ[Ч].ജ[ཪ-1]=ᐪ.ᐬ();ǯ[Ч].ཀྵ[ཪ-1]=false;}}}if(þ.ಘ[ý].Ń.Length==8){var Ч=þ.ಘ[ý].Ń[6]-'A';var ཪ=
þ.ಘ[ý].Ń[7]-'0';while(ǯ.Count<Ч+1){ǯ.Add(new ལ());}if(ཪ==0){for(var Ä=0;Ä<8;Ä++){if(ǯ[Ч].ജ[Ä]==null){ǯ[Ч].ജ[Ä]=ᐪ.ᐬ();ǯ[Ч]
.ཀྵ[Ä]=true;}}}else{if(ǯ[Ч].ജ[ཪ-1]==null){ǯ[Ч].ജ[ཪ-1]=ᐪ.ᐬ();ǯ[Ч].ཀྵ[ཪ-1]=true;}}}}ဌ=new ၜ[(int)э.ŏ];for(var Ä=0;Ä<ဌ.Length;
Ä++){var ǯ=ў[ኑ.ጙ[Ä]];var ཧ=new ၚ[ǯ.Count];for(var ͼ=0;ͼ<ཧ.Length;ͼ++){ǯ[ͼ].င();var Ч=new ၚ(ǯ[ͼ].က(),ǯ[ͼ].ജ,ǯ[ͼ].ཀྵ);ཧ[ͼ]=Ч
;}ဌ[Ä]=new ၜ(ཧ);}}private static IEnumerable<int>ས(ಆ þ){var ཥ=false;for(var ý=þ.ಘ.Count-1;ý>=0;ý--){var Ĭ=þ.ಘ[ý].Ń;if(Ĭ.
StartsWith("S")){if(Ĭ.EndsWith("_END")){ཥ=true;continue;}else if(Ĭ.EndsWith("_START")){ཥ=false;continue;}}if(ཥ){if(þ.ಘ[ý].ᶛ>0){
yield return ý;}}}}public ၜ this[э Т]{get{return ဌ[(int)Т];}}private class ལ{public ڦ[]ജ;public bool[]ཀྵ;public ལ(){ജ=new ڦ[8]
;ཀྵ=new bool[8];}public void င(){for(var Ä=0;Ä<ജ.Length;Ä++){if(ജ[Ä]==null){throw new Exception("Missing sprite!");}}}
public bool က(){for(var Ä=1;Ä<ജ.Length;Ä++){if(ജ[Ä]!=ജ[0]){return true;}}return false;}}}class Ꭵ:Ṇ{private List<ථ>ʽ;private
Dictionary<string,ථ>ᅪ;private Dictionary<string,int>ᅩ;private int[]ໝ;public Ꭵ(ಆ þ){ᅧ(þ);ᅒ();}private void ᅧ(ಆ þ){ʽ=new List<ථ>();ᅪ
=new Dictionary<string,ථ>();ᅩ=new Dictionary<string,int>();for(var ᅦ=1;ᅦ<=2;ᅦ++){var ಜ=þ.ಞ("TEXTURE"+ᅦ);if(ಜ==-1){break;}
var f=þ.ಠ(ಜ);var ú=BitConverter.ToInt32(f,0);for(var Ä=0;Ä<ú;Ä++){var ù=BitConverter.ToInt32(f,4+4*Ä);var Ĭ=ථ.ළ(f,ù);var ˢ=
ථ.ڎ(f,ù);var ϋ=ᐪ.ᐣ(ˢ);if(!ᅩ.ContainsKey(Ĭ)){ᅩ.Add(Ĭ,ʽ.Count);}ʽ.Add(ϋ);if(!ᅪ.ContainsKey(Ĭ)){ᅪ.Add(Ĭ,ϋ);}}}}private void
ᅒ(){var ǯ=new List<int>();foreach(var ᅃ in ኑ.ኒ){var ᅄ=ᮾ(ᅃ.Item1);var ᅅ=ᮾ(ᅃ.Item2);if(ᅄ!=-1&&ᅅ!=-1){ǯ.Add(ᅄ);ǯ.Add(ᅅ);}}ໝ=
ǯ.ToArray();}public int ᮾ(string Ĭ){if(Ĭ[0]=='-'){return 0;}int Ć;if(ᅩ.TryGetValue(Ĭ,out Ć)){return Ć;}else{return-1;}}
public IEnumerator<ථ>GetEnumerator(){return ʽ.GetEnumerator();}IEnumerator IEnumerable.GetEnumerator(){return ʽ.GetEnumerator(
);}public int Count=>ʽ.Count;public ථ this[int ܣ]=>ʽ[ܣ];public ථ this[string Ĭ]=>ᅪ[Ĭ];public int[]ṇ=>ໝ;}public enum ፎ{ፏ,ፐ
,ፑ,ፒ}sealed class ፓ{public static int ፔ=3;public static int ፌ=250;private ᴆ Å;private int ፋ;private int ú;private string
ࠉ;private string Ǉ;private int ፊ;private bool ፉ;private int ፈ;private ன ڟ;public ፓ(ᴆ Å){this.Å=Å;string ፇ;string ፆ;string
ፅ;string ፄ;string ፍ;string ፕ;switch(Å.ಕ){case ಕ.ᕩ:ፇ=ኑ.ጓ.ᇬ;ፆ=ኑ.ጓ.ᇭ;ፅ=ኑ.ጓ.ᇮ;ፄ=ኑ.ጓ.ᇯ;ፍ=ኑ.ጓ.ᇰ;ፕ=ኑ.ጓ.ᇲ;break;case ಕ.ᕹ:ፇ=ኑ.ጓ.ᇡ;
ፆ=ኑ.ጓ.ᇠ;ፅ=ኑ.ጓ.ᇟ;ፄ=ኑ.ጓ.ᇞ;ፍ=ኑ.ጓ.ᇝ;ፕ=ኑ.ጓ.ᇜ;break;default:ፇ=ኑ.ጓ.ᇦ;ፆ=ኑ.ጓ.ᇧ;ፅ=ኑ.ጓ.ᇨ;ፄ=ኑ.ጓ.ᇪ;ፍ=ኑ.ጓ.ᇱ;ፕ=ኑ.ጓ.ᇫ;break;}switch(Å.ಖ){
case ಖ.ᴋ:case ಖ.ᴊ:case ಖ.ᴈ:Å.ᴾ.ᯉ(ᑧ.ᑪ,true);switch(Å.ᰔ){case 1:ࠉ="FLOOR4_8";Ǉ=ኑ.ጓ.ᇢ;break;case 2:ࠉ="SFLR6_1";Ǉ=ኑ.ጓ.ᇣ;break;
case 3:ࠉ="MFLR8_4";Ǉ=ኑ.ጓ.ᇤ;break;case 4:ࠉ="MFLR8_3";Ǉ=ኑ.ጓ.ᇥ;break;default:break;}break;case ಖ.ᴉ:Å.ᴾ.ᯉ(ᑧ.ᐷ,true);switch(Å.શ){
case 6:ࠉ="SLIME16";Ǉ=ፇ;break;case 11:ࠉ="RROCK14";Ǉ=ፆ;break;case 20:ࠉ="RROCK07";Ǉ=ፅ;break;case 30:ࠉ="RROCK17";Ǉ=ፄ;break;case
15:ࠉ="RROCK13";Ǉ=ፍ;break;case 31:ࠉ="RROCK19";Ǉ=ፕ;break;default:break;}break;default:Å.ᴾ.ᯉ(ᑧ.ᐷ,true);ࠉ="F_SKY1";Ǉ=ኑ.ጓ.ᇦ;
break;}ፋ=0;ú=0;ፊ=0;ፉ=false;ፈ=0;}public ன ڞ(){ڟ=ன.ந;if(Å.ಖ==ಖ.ᴉ&&ú>50){int Ä;for(Ä=0;Ä<ђ.ۇ;Ä++){if(Å.ᰁ[Ä].ƕ.ఠ!=0){break;}}if(Ä
<ђ.ۇ&&ፋ!=2){if(Å.શ==30){ᎋ();}else{return ன.த;}}}ú++;if(ፋ==2){ᎌ();return ڟ;}if(Å.ಖ==ಖ.ᴉ){return ڟ;}if(ፋ==0&&ú>Ǉ.Length*ፔ+ፌ
){ú=0;ፋ=1;ڟ=ன.ண;if(Å.ᰔ==3){Å.ᴾ.ᯉ(ᑧ.ᑫ,true);}}if(ፋ==1&&Å.ᰔ==3){ᎃ();}return ڟ;}private void ᎃ(){ፊ=320-(ú-230)/2;if(ፊ>320){ፊ
=320;}if(ፊ<0){ፊ=0;}if(ú<1130){return;}ፉ=true;if(ú<1180){ፈ=0;return;}var ፋ=(ú-1180)/5;if(ፋ>6){ፋ=6;}if(ፋ>ፈ){ૐ(ɴ.ɲ);ፈ=ፋ;}}
private static ፖ[]ᎄ=new ፖ[]{new ፖ(ኑ.ጓ.ᇛ,ё.є),new ፖ(ኑ.ጓ.ᇚ,ё.ь),new ፖ(ኑ.ጓ.ᇙ,ё.ф),new ፖ(ኑ.ጓ.ᇘ,ё.у),new ፖ(ኑ.ጓ.ᇗ,ё.т),new ፖ(ኑ.ጓ.ᇖ,ё.
м),new ፖ(ኑ.ጓ.ᇕ,ё.р),new ፖ(ኑ.ጓ.ᇔ,ё.н),new ፖ(ኑ.ጓ.ᇓ,ё.п),new ፖ(ኑ.ጓ.ᇒ,ё.Р),new ፖ(ኑ.ጓ.ᆿ,ё.Ϭ),new ፖ(ኑ.ጓ.ሸ,ё.щ),new ፖ(ኑ.ጓ.ቇ,ё.ц)
,new ፖ(ኑ.ጓ.ኋ,ё.ы),new ፖ(ኑ.ጓ.ኌ,ё.С),new ፖ(ኑ.ጓ.ኍ,ё.Ϛ),new ፖ(ኑ.ጓ.ነ,ё.ђ)};private int ᎅ;private Ш ᎆ;private int ᎇ;private int
ᎈ;private bool ᎍ;private bool ᎉ;private bool ᎊ;private void ᎋ(){ፋ=2;ᎅ=0;ᎆ=ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚣ];ᎇ=ᎆ.ŵ;ᎈ=0;ᎍ=false;
ᎉ=false;ᎊ=false;ڟ=ன.ண;Å.ᴾ.ᯉ(ᑧ.ᐹ,true);}private void ᎌ(){if(--ᎇ>0){return;}if(ᎆ.ŵ==-1||ᎆ.ѐ==ᚏ.ᚠ){ᎅ++;ᎍ=false;if(ᎅ==ᎄ.
Length){ᎅ=0;}if(ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚤ!=0){ૐ(ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚤ);}ᎆ=ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚣ];ᎈ=0;}else{if(ᎆ==ኑ.ጔ[(int)ᚏ.ᨎ]){ᎊ=
false;ᎆ=ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚣ];ᎈ=0;goto stopAttack;}var ᆙ=ᎆ.ѐ;ᎆ=ኑ.ጔ[(int)ᆙ];ᎈ++;ɴ Ǯ;switch(ᆙ){case ᚏ.ᨎ:Ǯ=ɴ.ɯ;break;case
ᚏ.ᤘ:Ǯ=ɴ.ɲ;break;case ᚏ.ᥫ:Ǯ=ɴ.ɱ;break;case ᚏ.ᬲ:Ǯ=ɴ.Ɋ;break;case ᚏ.ᨬ:Ǯ=ɴ.ɍ;break;case ᚏ.ᨪ:Ǯ=ɴ.ɉ;break;case ᚏ.ᨨ:Ǯ=ɴ.ʣ;break;
case ᚏ.ᩃ:case ᚏ.ᩆ:case ᚏ.ᩉ:Ǯ=ɴ.ȥ;break;case ᚏ.ឮ:case ᚏ.ឭ:case ᚏ.ឬ:Ǯ=ɴ.ɱ;break;case ᚏ.ᠹ:Ǯ=ɴ.ɋ;break;case ᚏ.ᝋ:Ǯ=ɴ.Ɉ;break;case
ᚏ.ᣊ:case ᚏ.ᣧ:case ᚏ.ឥ:Ǯ=ɴ.ȥ;break;case ᚏ.ᣗ:Ǯ=ɴ.ɇ;break;case ᚏ.ᡩ:case ᚏ.ᡨ:Ǯ=ɴ.ɱ;break;case ᚏ.ᢧ:Ǯ=ɴ.ɫ;break;case ᚏ.ԋ:case ᚏ
.ԉ:case ᚏ.ԇ:Ǯ=ɴ.Ȓ;break;case ᚏ.Ձ:Ǯ=ɴ.ɇ;break;default:Ǯ=0;break;}if(Ǯ!=0){ૐ(Ǯ);}}if(ᎈ==12){ᎊ=true;if(ᎉ){ᎆ=ኑ.ጔ[(int)ኑ.ዃ[(
int)ᎄ[ᎅ].ܫ].ᚘ];}else{ᎆ=ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚗ];}ᎉ=!ᎉ;if(ᎆ==ኑ.ጔ[(int)ᚏ.ᚠ]){if(ᎉ){ᎆ=ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚘ];}else{ᎆ
=ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚗ];}}}if(ᎊ){if(ᎈ==24||ᎆ==ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚣ]){ᎊ=false;ᎆ=ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚣ];
ᎈ=0;}}stopAttack:ᎇ=ᎆ.ŵ;if(ᎇ==-1){ᎇ=15;}}public bool ଅ(ᕨ Ň){if(ፋ!=2){return false;}if(Ň.ܫ==ፎ.ፏ){if(ᎍ){return true;}ᎍ=true;
ᎆ=ኑ.ጔ[(int)ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚖ];ᎇ=ᎆ.ŵ;ᎈ=0;ᎊ=false;if(ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚔ!=0){ૐ(ኑ.ዃ[(int)ᎄ[ᎅ].ܫ].ᚔ);}return true;}return
false;}private void ૐ(ɴ Ǯ){Å.ᴽ.ૐ(Ǯ);}public ᴆ હ=>Å;public string ᎂ=>ࠉ;public string ǭ=>Ǉ;public int ŏ=>ú;public int ᎁ=>ፋ;
public string ᎀ=>ᎄ[ᎅ].Ń;public Ш ፚ=>ᎆ;public int ፙ=>ፊ;public int ፘ=>ፈ;public bool ፗ=>ፉ;private class ፖ{public string Ń;public
ё ܫ;public ፖ(string Ĭ,ё ˌ){Ń=Ĭ;ܫ=ˌ;}}}sealed class Ꮊ{private ಆ þ;private ᮟ ü;private Ḃ ᄣ;private Ꮝ ȋ;private int ǉ;
private ٻ چ;public Ꮊ(ᴍ Ǽ,Ꮝ ȋ){þ=Ǽ.ಆ;ü=Ǽ.ᛧ;ᄣ=Ǽ.ᴌ;this.ȋ=ȋ;ǉ=ȋ.Ǒ/320;چ=new ٻ(þ);}public void Ǌ(ፓ ȃ){if(ȃ.ᎁ==2){Ꮙ(ȃ);return;}if(ȃ.
ᎁ==0){Ꮋ(ȃ);}else{switch(ȃ.હ.ᰔ){case 1:Ꮘ("CREDIT",0,0);break;case 2:Ꮘ("VICTORY2",0,0);break;case 3:ᎃ(ȃ);break;case 4:Ꮘ(
"ENDPIC",0,0);break;}}}private void Ꮋ(ፓ ȃ){Ꮈ(ü[ȃ.ᎂ]);var Ꮌ=10*ǉ;var Ꮍ=17*ǉ;var ම=0;var ú=(ȃ.ŏ-10)/ፓ.ፔ;if(ú<0){ú=0;}for(;ú>0;ú--)
{if(ම==ȃ.ǭ.Length){break;}var Á=ȃ.ǭ[ම++];if(Á=='\n'){Ꮌ=10*ǉ;Ꮍ+=11*ǉ;continue;}ȋ.ᐮ(Á,Ꮌ,Ꮍ,ǉ);Ꮌ+=ȋ.ᐳ(Á,ǉ);}}private void ᎃ(ፓ
ȃ){var Ꮉ=320-ȃ.ፙ;Ꮘ("PFUB2",Ꮉ-320,0);Ꮘ("PFUB1",Ꮉ,0);if(ȃ.ፗ){string ڌ="END0";switch(ȃ.ፘ){case 1:ڌ="END1";break;case 2:ڌ=
"END2";break;case 3:ڌ="END3";break;case 4:ڌ="END4";break;case 5:ڌ="END5";break;case 6:ڌ="END6";break;}Ꮘ(ڌ,(320-13*8)/2,(240-8*
8)/2);}}private void Ꮈ(ᎂ ࠉ){var ट=ࠉ.Ꮁ;var ध=ȋ.Ꮁ;var ǉ=ȋ.Ǒ/320;var ࠅ=Ꮖ.Ꮒ/ǉ-Ꮖ.Ꮏ;var আ=Ꮖ.Ꮒ/ǉ;for(var ǃ=0;ǃ<ȋ.Ǒ;ǃ++){var ࠄ=Ꮖ.
Ꮒ/ǉ-Ꮖ.Ꮏ;var É=ȋ.ǡ*ǃ;for(var ǂ=0;ǂ<ȋ.ǡ;ǂ++){var Ꮚ=ࠅ.Ꮄ()&0x3F;var Ꮗ=ࠄ.Ꮄ()&0x3F;ध[É]=ट[(Ꮗ<<6)+Ꮚ];ࠄ+=আ;É++;}ࠅ+=আ;}}private
void Ꮘ(string Ĭ,int ǃ,int ǂ){var ǉ=ȋ.Ǒ/320;ȋ.Ꮘ(چ[Ĭ],ǉ*ǃ,ǉ*ǂ,ǉ);}private void Ꮙ(ፓ ȃ){Ꮘ("BOSSBACK",0,0);var Ч=ȃ.ፚ.ю&0x7fff;var
ڌ=ᄣ[ȃ.ፚ.э].ၛ[Ч].ജ[0];if(ᄣ[ȃ.ፚ.э].ၛ[Ч].ཀྵ[0]){ȋ.Ꮭ(ڌ,ȋ.Ǒ/2,ȋ.ǡ-ǉ*30,ǉ);}else{ȋ.Ꮘ(ڌ,ȋ.Ǒ/2,ȋ.ǡ-ǉ*30,ǉ);}var ځ=ȋ.ᐲ(ȃ.ᎀ,ǉ);ȋ.ᐭ(ȃ
.ᎀ,(ȋ.Ǒ-ځ)/2,ȋ.ǡ-ǉ*13,ǉ);}}sealed class Ꮛ:Ⴞ{private ଦ l;private Ŀ ô;private int ú;private int ฎ;private int ญ;public Ꮛ(ଦ
l){this.l=l;}public override void Ⴛ(){if(--ú>0){return;}var ˋ=(l.ષ.ѐ()&3)*16;if(ô.Ħ-ˋ<ญ){ô.Ħ=ญ;}else{ô.Ħ=ฎ-ˋ;}ú=4;}public
Ŀ Ŀ{get{return ô;}set{ô=value;}}public int ŏ{get{return ú;}set{ú=value;}}public int ฆ{get{return ฎ;}set{ฎ=value;}}public
int ง{get{return ญ;}set{ญ=value;}}}struct Ꮖ{public const int Ꮕ=16;public const int Ꮔ=1<<Ꮕ;public static Ꮖ Ꮓ=new Ꮖ(0);public
static Ꮖ Ꮒ=new Ꮖ(Ꮔ);public static Ꮖ Ꮑ=new Ꮖ(int.MaxValue);public static Ꮖ Ꮐ=new Ꮖ(int.MinValue);public static Ꮖ Ꮏ=new Ꮖ(1);
public static Ꮖ Ꮎ=new Ꮖ(Ꮔ+1);public static Ꮖ Ꮇ=new Ꮖ(Ꮔ-1);private int f;public Ꮖ(int f){this.f=f;}public static Ꮖ Ꭸ(int u){
return new Ꮖ(u<<Ꮕ);}public static Ꮖ Ꭹ(float u){return new Ꮖ((int)(Ꮔ*u));}public static Ꮖ Ꭺ(double u){return new Ꮖ((int)(Ꮔ*u));
}public float Ꭽ(){return(float)f/Ꮔ;}public double Ꭻ(){return(double)f/Ꮔ;}public static Ꮖ Ꭼ(Ꮖ Ã){if(Ã.f<0){return new Ꮖ(-Ã
.f);}else{return Ã;}}public static Ꮖ operator+(Ꮖ Ã){return Ã;}public static Ꮖ operator-(Ꮖ Ã){return new Ꮖ(-Ã.f);}public
static Ꮖ operator+(Ꮖ Ã,Ꮖ Â){return new Ꮖ(Ã.f+Â.f);}public static Ꮖ operator-(Ꮖ Ã,Ꮖ Â){return new Ꮖ(Ã.f-Â.f);}public static Ꮖ
operator*(Ꮖ Ã,Ꮖ Â){return new Ꮖ((int)(((long)Ã.f*(long)Â.f)>>Ꮕ));}public static Ꮖ operator*(int Ã,Ꮖ Â){return new Ꮖ(Ã*Â.f);}
public static Ꮖ operator*(Ꮖ Ã,int Â){return new Ꮖ(Ã.f*Â);}public static Ꮖ operator/(Ꮖ Ã,Ꮖ Â){if((Ꭷ(Ã.f)>>14)>=Ꭷ(Â.f)){return
new Ꮖ((Ã.f^Â.f)<0?int.MinValue:int.MaxValue);}return Ꭶ(Ã,Â);}private static int Ꭷ(int ᅦ){return ᅦ<0?-ᅦ:ᅦ;}private static Ꮖ
Ꭶ(Ꮖ Ã,Ꮖ Â){var Á=((double)Ã.f)/((double)Â.f)*Ꮔ;if(Á>=2147483648.0||Á<-2147483648.0){throw new DivideByZeroException();}
return new Ꮖ((int)Á);}public static Ꮖ operator/(int Ã,Ꮖ Â){return Ꮖ.Ꭸ(Ã)/Â;}public static Ꮖ operator/(Ꮖ Ã,int Â){return new Ꮖ(
Ã.f/Â);}public static Ꮖ operator<<(Ꮖ Ã,int Â){return new Ꮖ(Ã.f<<Â);}public static Ꮖ operator>>(Ꮖ Ã,int Â){return new Ꮖ(Ã.
f>>Â);}public static bool operator==(Ꮖ Ã,Ꮖ Â){return Ã.f==Â.f;}public static bool operator!=(Ꮖ Ã,Ꮖ Â){return Ã.f!=Â.f;}
public static bool operator<(Ꮖ Ã,Ꮖ Â){return Ã.f<Â.f;}public static bool operator>(Ꮖ Ã,Ꮖ Â){return Ã.f>Â.f;}public static bool
operator<=(Ꮖ Ã,Ꮖ Â){return Ã.f<=Â.f;}public static bool operator>=(Ꮖ Ã,Ꮖ Â){return Ã.f>=Â.f;}public static Ꮖ Ꮆ(Ꮖ Ã,Ꮖ Â){if(Ã<Â){
return Ã;}else{return Â;}}public static Ꮖ Ꮅ(Ꮖ Ã,Ꮖ Â){if(Ã<Â){return Â;}else{return Ã;}}public int Ꮄ(){return f>>Ꮕ;}public int
Ꮃ(){return(f+Ꮔ-1)>>Ꮕ;}public override bool Equals(object Ꮂ){throw new NotSupportedException();}public override int
GetHashCode(){return f.GetHashCode();}public override string ToString(){return((double)f/Ꮔ).ToString();}public int Ꮁ=>f;public
static Ꮖ Ꮀ(Ꮖ[]Ꭾ){return Ꭾ[ᒚ.Ꮀ];}public static Ꮖ Ꭿ(Ꮖ[]Ꭾ){return Ꭾ[ᒚ.Ꭿ];}public static Ꮖ ቀ(Ꮖ[]Ꭾ){return Ꭾ[ᒚ.ቀ];}public static Ꮖ
ሿ(Ꮖ[]Ꭾ){return Ꭾ[ᒚ.ሿ];}public static int Ꮀ(int[]Ꭾ){return Ꭾ[ᒚ.Ꮀ];}public static int Ꭿ(int[]Ꭾ){return Ꭾ[ᒚ.Ꭿ];}public
static int ቀ(int[]Ꭾ){return Ꭾ[ᒚ.ቀ];}public static int ሿ(int[]Ꭾ){return Ꭾ[ᒚ.ሿ];}}sealed class ᎂ{private string Ĭ;private byte[]
f;public ᎂ(string Ĭ,byte[]f){this.Ĭ=Ĭ;this.f=f;}public static ᎂ ċ(string Ĭ,byte[]f){return new ᎂ(Ĭ,f);}public override
string ToString(){return Ĭ;}public string Ń=>Ĭ;public byte[]Ꮁ=>f;}sealed class ᴕ:ᮟ{private ᎂ[]ü;private Dictionary<string,ᎂ>Ꭰ;
private Dictionary<string,int>ᅩ;private int Ꭱ;private ᎂ Ꭲ;public ᴕ(ಆ þ){var ᴔ=ᴙ(þ,"F_START");var ᴓ=ᴙ(þ,"F_END");var ᴒ=ᴙ(þ,
"FF_START");var ᴑ=ᴙ(þ,"FF_END");var ᴐ=ᴔ==1&&ᴓ==1&&ᴒ==0&&ᴑ==0;var ᴖ=ᴔ==1&&ᴓ>=2;var ᴗ=ᴔ+ᴒ>=2&&ᴓ+ᴑ>=2;if(ᴐ||ᴖ){ᴚ(þ);}else if(ᴗ){ᴞ(þ);
}else{throw new Exception("Failed to read flats.");}}private void ᴚ(ಆ þ){try{ᕣ.ᔵ.འ("Load flats: ");var Ꭳ=þ.ಞ("F_START")+1
;var Ꭴ=þ.ಞ("F_END")-1;var ú=Ꭴ-Ꭳ+1;ü=new ᎂ[ú];Ꭰ=new Dictionary<string,ᎂ>();ᅩ=new Dictionary<string,int>();for(var ý=Ꭳ;ý<=Ꭴ
;ý++){if(þ.ಡ(ý)!=4096){continue;}var Ć=ý-Ꭳ;var Ĭ=þ.ಘ[ý].Ń;var ࠉ=new ᎂ(Ĭ,þ.ಠ(ý));ü[Ć]=ࠉ;Ꭰ[Ĭ]=ࠉ;ᅩ[Ĭ]=Ć;}Ꭱ=ᅩ["F_SKY1"];Ꭲ=Ꭰ[
"F_SKY1"];ᕣ.ᔵ.འ("OK ("+Ꭰ.Count+" flats)");}catch(Exception e){ᕣ.ᔵ.འ("Failed");throw e;}}private void ᴞ(ಆ þ){try{ᕣ.ᔵ.འ(
"Load flats: ");var ᴛ=new List<int>();var ᴜ=false;for(var ý=0;ý<þ.ಘ.Count;ý++){var Ĭ=þ.ಘ[ý].Ń;if(ᴜ){if(Ĭ=="F_END"||Ĭ=="FF_END"){ᴜ=
false;}else{ᴛ.Add(ý);}}else{if(Ĭ=="F_START"||Ĭ=="FF_START"){ᴜ=true;}}}ᴛ.Reverse();var ᴝ=new HashSet<string>();var ᴟ=new List<
int>();foreach(var ý in ᴛ){if(!ᴝ.Contains(þ.ಘ[ý].Ń)){ᴟ.Add(ý);ᴝ.Add(þ.ಘ[ý].Ń);}}ᴟ.Reverse();ü=new ᎂ[ᴟ.Count];Ꭰ=new
Dictionary<string,ᎂ>();ᅩ=new Dictionary<string,int>();for(var Ć=0;Ć<ü.Length;Ć++){var ý=ᴟ[Ć];if(þ.ಡ(ý)!=4096){continue;}var Ĭ=þ.ಘ[
ý].Ń;var ࠉ=new ᎂ(Ĭ,þ.ಠ(ý));ü[Ć]=ࠉ;Ꭰ[Ĭ]=ࠉ;ᅩ[Ĭ]=Ć;}Ꭱ=ᅩ["F_SKY1"];Ꭲ=Ꭰ["F_SKY1"];ᕣ.ᔵ.འ("OK ("+Ꭰ.Count+" flats)");}catch(
Exception e){ᕣ.ᔵ.འ("Failed");throw e;}}public int ᮾ(string Ĭ){if(ᅩ.ContainsKey(Ĭ)){return ᅩ[Ĭ];}else{return-1;}}public
IEnumerator<ᎂ>GetEnumerator(){return((IEnumerable<ᎂ>)ü).GetEnumerator();}IEnumerator IEnumerable.GetEnumerator(){return ü.
GetEnumerator();}private static int ᴙ(ಆ þ,string Ĭ){var ú=0;foreach(var ý in þ.ಘ){if(ý.Ń==Ĭ){ú++;}}return ú;}public int Count=>ü.
Length;public ᎂ this[int ܣ]=>ü[ܣ];public ᎂ this[string Ĭ]=>Ꭰ[Ĭ];public int ᜅ=>Ꭱ;public ᎂ ᯇ=>Ꭲ;}sealed class ᴘ:Ⴞ{private ଦ l;
private ᴀ ˌ;private bool Δ;private Ŀ ô;private int Β;private Ϋ ᱸ;private int ϋ;private Ꮖ ᴁ;private Ꮖ Ζ;public ᴘ(ଦ l){this.l=l;}
public override void Ⴛ(){ί ŀ;var Ü=l.Đ;ŀ=Ü.Ξ(ô,Ζ,ᴁ,Δ,0,Β);if(((l.ଖ+ô.ġ)&7)==0){l.ૐ(ô.ĕ,ɴ.Ȳ,ʡ.ʝ);}if(ŀ==ί.ά){ô.Ē=null;if(Β==1){
switch(ˌ){case ᴀ.ᱼ:ô.Ě=ᱸ;ô.Ĥ=ϋ;break;}}else if(Β==-1){switch(ˌ){case ᴀ.ᳬ:ô.Ě=ᱸ;ô.Ĥ=ϋ;break;}}l.વ.ᄪ(this);ô.Ĝ();l.ૐ(ô.ĕ,ɴ.Ȩ,ʡ.ʝ
);}}public ᴀ ܫ{get{return ˌ;}set{ˌ=value;}}public bool ܬ{get{return Δ;}set{Δ=value;}}public Ŀ Ŀ{get{return ô;}set{ô=value
;}}public int ಣ{get{return Β;}set{Β=value;}}public Ϋ ᴃ{get{return ᱸ;}set{ᱸ=value;}}public int ථ{get{return ϋ;}set{ϋ=value
;}}public Ꮖ ᴄ{get{return ᴁ;}set{ᴁ=value;}}public Ꮖ ݏ{get{return Ζ;}set{Ζ=value;}}}public enum ᴀ{ᳶ,ᳵ,ᳱ,ᳰ,ᳯ,ᳮ,ᳬ,ᳫ,ᳪ,ᳩ,ᱽ,ᱼ,ᱻ
}static class ᱺ{public static int ᱹ=35;public static Ꮖ ᴂ=Ꮖ.Ꭸ(32);public static int ᴅ=0x32;}sealed class ᴍ{private ಆ þ;
private ڗ ǌ;private Ƭ ۼ;private Ṇ ʽ;private ᮟ ü;private Ḃ ᄣ;private ᅗ ᛜ;private ᴍ(){}public ᴍ(ᖎ ᕜ){þ=new ಆ(ᗄ.ᗋ(ᕜ));ǌ=new ڗ(þ);ۼ
=new Ƭ(þ);ʽ=new ᅰ(þ);ü=new ᴕ(þ);ᄣ=new ဍ(þ);ᛜ=new ᅗ(ʽ,ü);}public static ᴍ ᴎ(params string[]ᗌ){var ᴏ=new ᴍ();ᴏ.þ=new ಆ(ᗌ);ᴏ
.ǌ=new ڗ(ᴏ.þ);ᴏ.ۼ=new Ƭ(ᴏ.þ);ᴏ.ʽ=new Ꭵ(ᴏ.þ);ᴏ.ü=new Ꮜ(ᴏ.þ);ᴏ.ᄣ=new ᎎ(ᴏ.þ);ᴏ.ᛜ=new ᅗ(ᴏ.ʽ,ᴏ.ü);return ᴏ;}public ಆ ಆ=>þ;
public ڗ ڗ=>ǌ;public Ƭ Ƭ=>ۼ;public Ṇ ᜁ=>ʽ;public ᮟ ᛧ=>ü;public Ḃ ᴌ=>ᄣ;public ᅗ ᒳ=>ᛜ;}public enum ಖ{ᴋ,ᴊ,ᴉ,ᴈ,ᴇ}sealed class ᴆ{
private ಗ ౙ;private ಖ ౘ;private ಕ ఽ;private ђ[]Ë;private int Ƹ;private int ᔾ;private int ࠎ;private ᵅ ᔽ;private bool ᴸ;private
int ᖉ;private bool ᴹ;private bool ᴺ;private bool ᴻ;private ᰋ ᴼ;private Ᏺ ǵ;private Ḿ Ꮳ;private ᶫ Ŗ;private ᯈ Ꮿ;private Ṉ Ꮲ;
public ᴆ(){ౙ=ಗ.ᵊ;ౘ=ಖ.ᴉ;ఽ=ಕ.ኾ;Ë=new ђ[ђ.ۇ];for(var Ä=0;Ä<ђ.ۇ;Ä++){Ë[Ä]=new ђ(Ä);}Ë[0].Ÿ=true;Ƹ=0;ᔾ=1;ࠎ=1;ᔽ=ᵅ.ᵆ;ᴸ=false;ᖉ=0;ᴹ=
false;ᴺ=false;ᴻ=false;ᴼ=new ᰋ();ǵ=new Ᏺ();Ꮳ=ߖ.ߏ();Ŗ=ߘ.ߏ();Ꮿ=ޔ.ߏ();Ꮲ=ߜ.ߏ();}public ᴆ(ᖎ ᕜ,ᴍ Ǽ):this(){if(ᕜ.ᖄ.ᗪ){ᴸ=true;}ౙ=Ǽ.ಆ.ಗ
;ౘ=Ǽ.ಆ.ಖ;ఽ=Ǽ.ಆ.ಕ;}public ಗ ಗ{get{return ౙ;}set{ౙ=value;}}public ಖ ಖ{get{return ౘ;}set{ౘ=value;}}public ಕ ಕ{get{return ఽ;}
set{ఽ=value;}}public ђ[]ᰁ{get{return Ë;}}public int ଙ{get{return Ƹ;}set{Ƹ=value;}}public int ᰔ{get{return ᔾ;}set{ᔾ=value;}}
public int શ{get{return ࠎ;}set{ࠎ=value;}}public ᵅ ᴷ{get{return ᔽ;}set{ᔽ=value;}}public bool ᴶ{get{return ᴸ;}set{ᴸ=value;}}
public int ᴵ{get{return ᖉ;}set{ᖉ=value;}}public bool ᴴ{get{return ᴹ;}set{ᴹ=value;}}public bool ᴳ{get{return ᴺ;}set{ᴺ=value;}}
public bool ᴲ{get{return ᴻ;}set{ᴻ=value;}}public ᰋ ᰋ{get{return ᴼ;}}public Ᏺ ષ{get{return ǵ;}}public Ḿ ᴱ{get{return Ꮳ;}set{Ꮳ=
value;}}public ᶫ ᴽ{get{return Ŗ;}set{Ŗ=value;}}public ᯈ ᴾ{get{return Ꮿ;}set{Ꮿ=value;}}public Ṉ ᵉ{get{return Ꮲ;}set{Ꮲ=value;}}
}public enum ᵅ{Р,ᅼ,ᵆ,ᅽ,ᵇ}public enum ᵈ{ᔜ,ᕅ,ፓ}public enum ಗ{ᵊ,ᵋ,ᵌ,ᵍ}static class ᵎ{private const int ᵄ=2048;private const
int ᵃ=11;private const int ᵂ=Ꮖ.Ꮕ-ᵃ;private static uint ᵁ(Ꮖ ܣ,Ꮖ ܢ){if((uint)ܢ.Ꮁ<512){return ᵄ;}var ᵀ=((uint)ܣ.Ꮁ<<3)/((uint)ܢ
.Ꮁ>>8);return ᵀ<=ᵄ?ᵀ:ᵄ;}public static Ꮖ ᴿ(Ꮖ ᴪ,Ꮖ ᴦ,Ꮖ ᴧ,Ꮖ ᴨ){var ߝ=Ꮖ.Ꭼ(ᴧ-ᴪ);var Ǆ=Ꮖ.Ꭼ(ᴨ-ᴦ);if(Ǆ>ߝ){var ў=ߝ;ߝ=Ǆ;Ǆ=ў;}Ꮖ ڂ;if(
ߝ!=Ꮖ.Ꮓ){ڂ=Ǆ/ߝ;}else{ڂ=Ꮖ.Ꮓ;}var Ş=(ஜ.అ((uint)ڂ.Ꮁ>>ᵂ)+ɡ.ᓮ);var њ=ߝ/ஜ.ஸ(Ş);return њ;}public static int ᴰ(Ꮖ ǃ,Ꮖ ǂ,ߵ ޛ){if(ޛ.ޘ
==Ꮖ.Ꮓ){if(ǃ<=ޛ.ޚ){return ޛ.ޗ>Ꮖ.Ꮓ?1:0;}else{return ޛ.ޗ<Ꮖ.Ꮓ?1:0;}}if(ޛ.ޗ==Ꮖ.Ꮓ){if(ǂ<=ޛ.ޙ){return ޛ.ޘ<Ꮖ.Ꮓ?1:0;}else{return ޛ.
ޘ>Ꮖ.Ꮓ?1:0;}}var ߝ=(ǃ-ޛ.ޚ);var Ǆ=(ǂ-ޛ.ޙ);if(((ޛ.ޗ.Ꮁ^ޛ.ޘ.Ꮁ^ߝ.Ꮁ^Ǆ.Ꮁ)&0x80000000)!=0){if(((ޛ.ޗ.Ꮁ^ߝ.Ꮁ)&0x80000000)!=0){return
1;}return 0;}var ල=new Ꮖ(ޛ.ޗ.Ꮁ>>Ꮖ.Ꮕ)*ߝ;var ර=Ǆ*new Ꮖ(ޛ.ޘ.Ꮁ>>Ꮖ.Ꮕ);if(ර<ල){return 0;}else{return 1;}}public static ɡ ᴥ(Ꮖ ᴪ,
Ꮖ ᴦ,Ꮖ ᴧ,Ꮖ ᴨ){var ǃ=ᴧ-ᴪ;var ǂ=ᴨ-ᴦ;if(ǃ==Ꮖ.Ꮓ&&ǂ==Ꮖ.Ꮓ){return ɡ.ᓬ;}if(ǃ>=Ꮖ.Ꮓ){if(ǂ>=Ꮖ.Ꮓ){if(ǃ>ǂ){return ஜ.అ(ᵁ(ǂ,ǃ));}else{
return new ɡ(ɡ.ᓮ.Ꮁ-1)-ஜ.అ(ᵁ(ǃ,ǂ));}}else{ǂ=-ǂ;if(ǃ>ǂ){return-ஜ.అ(ᵁ(ǂ,ǃ));}else{return ɡ.ᓢ+ஜ.అ(ᵁ(ǃ,ǂ));}}}else{ǃ=-ǃ;if(ǂ>=Ꮖ.Ꮓ){
if(ǃ>ǂ){return new ɡ(ɡ.ᓣ.Ꮁ-1)-ஜ.అ(ᵁ(ǂ,ǃ));}else{return ɡ.ᓮ+ஜ.అ(ᵁ(ǃ,ǂ));}}else{ǂ=-ǂ;if(ǃ>ǂ){return ɡ.ᓣ+ஜ.అ(ᵁ(ǂ,ǃ));}else{
return new ɡ(ɡ.ᓢ.Ꮁ-1)-ஜ.అ(ᵁ(ǃ,ǂ));}}}}public static ฃ ᴩ(Ꮖ ǃ,Ꮖ ǂ,શ ࠎ){if(ࠎ.ᜆ.Length==0){return ࠎ.ᜂ[0];}var ಎ=ࠎ.ᜆ.Length-1;while
(!ߵ.ޝ(ಎ)){var ޛ=ࠎ.ᜆ[ಎ];var ñ=ᴰ(ǃ,ǂ,ޛ);ಎ=ޛ.ޕ[ñ];}return ࠎ.ᜂ[ߵ.ޜ(ಎ)];}public static int ᴤ(Ꮖ ǃ,Ꮖ ǂ,Ω ò){var ᴣ=ò.ɝ.ޚ;var ᴢ=ò.
ɝ.ޙ;var ᴡ=ò.ɞ.ޚ-ᴣ;var ᴠ=ò.ɞ.ޙ-ᴢ;if(ᴡ==Ꮖ.Ꮓ){if(ǃ<=ᴣ){return ᴠ>Ꮖ.Ꮓ?1:0;}else{return ᴠ<Ꮖ.Ꮓ?1:0;}}if(ᴠ==Ꮖ.Ꮓ){if(ǂ<=ᴢ){return
ᴡ<Ꮖ.Ꮓ?1:0;}else{return ᴡ>Ꮖ.Ꮓ?1:0;}}var ߝ=(ǃ-ᴣ);var Ǆ=(ǂ-ᴢ);if(((ᴠ.Ꮁ^ᴡ.Ꮁ^ߝ.Ꮁ^Ǆ.Ꮁ)&0x80000000)!=0){if(((ᴠ.Ꮁ^ߝ.Ꮁ)&0x80000000
)!=0){return 1;}else{return 0;}}var ල=new Ꮖ(ᴠ.Ꮁ>>Ꮖ.Ꮕ)*ߝ;var ර=Ǆ*new Ꮖ(ᴡ.Ꮁ>>Ꮖ.Ꮕ);if(ර<ල){return 0;}else{return 1;}}public
static int ᴫ(Ꮖ ǃ,Ꮖ ǂ,ɢ ò){if(ò.ޘ==Ꮖ.Ꮓ){if(ǃ<=ò.ɝ.ޚ){return ò.ޗ>Ꮖ.Ꮓ?1:0;}else{return ò.ޗ<Ꮖ.Ꮓ?1:0;}}if(ò.ޗ==Ꮖ.Ꮓ){if(ǂ<=ò.ɝ.ޙ){
return ò.ޘ<Ꮖ.Ꮓ?1:0;}else{return ò.ޘ>Ꮖ.Ꮓ?1:0;}}var ߝ=(ǃ-ò.ɝ.ޚ);var Ǆ=(ǂ-ò.ɝ.ޙ);var ල=new Ꮖ(ò.ޗ.Ꮁ>>Ꮖ.Ꮕ)*ߝ;var ර=Ǆ*new Ꮖ(ò.ޘ.Ꮁ>>Ꮖ
.Ꮕ);if(ර<ල){return 0;}else{return 1;}}public static int ᴮ(Ꮖ[]Ꭾ,ɢ ò){int ࠌ;int ࠇ;switch(ò.ʐ){case ʐ.ʏ:ࠌ=Ꭾ[ᒚ.Ꮀ]>ò.ɝ.ޙ?1:0;ࠇ
=Ꭾ[ᒚ.Ꭿ]>ò.ɝ.ޙ?1:0;if(ò.ޘ<Ꮖ.Ꮓ){ࠌ^=1;ࠇ^=1;}break;case ʐ.ʎ:ࠌ=Ꭾ[ᒚ.ሿ]<ò.ɝ.ޚ?1:0;ࠇ=Ꭾ[ᒚ.ቀ]<ò.ɝ.ޚ?1:0;if(ò.ޗ<Ꮖ.Ꮓ){ࠌ^=1;ࠇ^=1;}
break;case ʐ.ʍ:ࠌ=ᴫ(Ꭾ[ᒚ.ቀ],Ꭾ[ᒚ.Ꮀ],ò);ࠇ=ᴫ(Ꭾ[ᒚ.ሿ],Ꭾ[ᒚ.Ꭿ],ò);break;case ʐ.ʌ:ࠌ=ᴫ(Ꭾ[ᒚ.ሿ],Ꭾ[ᒚ.Ꮀ],ò);ࠇ=ᴫ(Ꭾ[ᒚ.ቀ],Ꭾ[ᒚ.Ꭿ],ò);break;
default:throw new Exception("Invalid SlopeType.");}if(ࠌ==ࠇ){return ࠌ;}else{return-1;}}public static int ᴯ(Ꮖ ǃ,Ꮖ ǂ,ᗕ ò){if(ò.ޘ==
Ꮖ.Ꮓ){if(ǃ<=ò.ޚ){return ò.ޗ>Ꮖ.Ꮓ?1:0;}else{return ò.ޗ<Ꮖ.Ꮓ?1:0;}}if(ò.ޗ==Ꮖ.Ꮓ){if(ǂ<=ò.ޙ){return ò.ޘ<Ꮖ.Ꮓ?1:0;}else{return ò.ޘ
>Ꮖ.Ꮓ?1:0;}}var ߝ=(ǃ-ò.ޚ);var Ǆ=(ǂ-ò.ޙ);if(((ò.ޗ.Ꮁ^ò.ޘ.Ꮁ^ߝ.Ꮁ^Ǆ.Ꮁ)&0x80000000)!=0){if(((ò.ޗ.Ꮁ^ߝ.Ꮁ)&0x80000000)!=0){return 1
;}else{return 0;}}var ල=new Ꮖ(ò.ޗ.Ꮁ>>8)*new Ꮖ(ߝ.Ꮁ>>8);var ර=new Ꮖ(Ǆ.Ꮁ>>8)*new Ꮖ(ò.ޘ.Ꮁ>>8);if(ර<ල){return 0;}else{return 1
;}}public static Ꮖ ᴭ(Ꮖ ߝ,Ꮖ Ǆ){ߝ=Ꮖ.Ꭼ(ߝ);Ǆ=Ꮖ.Ꭼ(Ǆ);if(ߝ<Ǆ){return ߝ+Ǆ-(ߝ>>1);}else{return ߝ+Ǆ-(Ǆ>>1);}}public static int ᴬ(Ꮖ
ǃ,Ꮖ ǂ,ᗕ ò){if(ò.ޘ==Ꮖ.Ꮓ){if(ǃ==ò.ޚ){return 2;}if(ǃ<=ò.ޚ){return ò.ޗ>Ꮖ.Ꮓ?1:0;}return ò.ޗ<Ꮖ.Ꮓ?1:0;}if(ò.ޗ==Ꮖ.Ꮓ){if(ǃ==ò.ޙ){
return 2;}if(ǂ<=ò.ޙ){return ò.ޘ<Ꮖ.Ꮓ?1:0;}return ò.ޘ>Ꮖ.Ꮓ?1:0;}var ߝ=(ǃ-ò.ޚ);var Ǆ=(ǂ-ò.ޙ);var ල=new Ꮖ((ò.ޗ.Ꮁ>>Ꮖ.Ꮕ)*(ߝ.Ꮁ>>Ꮖ.Ꮕ));
var ර=new Ꮖ((Ǆ.Ꮁ>>Ꮖ.Ꮕ)*(ò.ޘ.Ꮁ>>Ꮖ.Ꮕ));if(ර<ල){return 0;}if(ල==ර){return 2;}else{return 1;}}public static int ᴬ(Ꮖ ǃ,Ꮖ ǂ,ߵ ޛ){
if(ޛ.ޘ==Ꮖ.Ꮓ){if(ǃ==ޛ.ޚ){return 2;}if(ǃ<=ޛ.ޚ){return ޛ.ޗ>Ꮖ.Ꮓ?1:0;}return ޛ.ޗ<Ꮖ.Ꮓ?1:0;}if(ޛ.ޗ==Ꮖ.Ꮓ){if(ǃ==ޛ.ޙ){return 2;}if(
ǂ<=ޛ.ޙ){return ޛ.ޘ<Ꮖ.Ꮓ?1:0;}return ޛ.ޘ>Ꮖ.Ꮓ?1:0;}var ߝ=(ǃ-ޛ.ޚ);var Ǆ=(ǂ-ޛ.ޙ);var ල=new Ꮖ((ޛ.ޗ.Ꮁ>>Ꮖ.Ꮕ)*(ߝ.Ꮁ>>Ꮖ.Ꮕ));var ර=
new Ꮖ((Ǆ.Ꮁ>>Ꮖ.Ꮕ)*(ޛ.ޘ.Ꮁ>>Ꮖ.Ꮕ));if(ර<ල){return 0;}if(ල==ර){return 2;}else{return 1;}}}sealed class ᯗ:Ⴞ{private static int ᯘ=
8;private ଦ l;private Ŀ ô;private int ญ;private int ฎ;private int Β;public ᯗ(ଦ l){this.l=l;}public override void Ⴛ(){
switch(Β){case-1:ô.Ħ-=ᯘ;if(ô.Ħ<=ญ){ô.Ħ+=ᯘ;Β=1;}break;case 1:ô.Ħ+=ᯘ;if(ô.Ħ>=ฎ){ô.Ħ-=ᯘ;Β=-1;}break;}}public Ŀ Ŀ{get{return ô;}
set{ô=value;}}public int ง{get{return ญ;}set{ญ=value;}}public int ฆ{get{return ฎ;}set{ฎ=value;}}public int ಣ{get{return Β;}
set{Β=value;}}}sealed class ᯖ:ᜮ{private int ᯕ;private int ᯔ;public ᯖ(ባ ĭ):base(ĭ){if(ĭ.હ.ಖ==ಖ.ᴋ){ᯕ=2;}else{ᯕ=1;}}public
override void ರ(){ᯔ=ᯕ-1;}public override bool ଅ(ᕨ Ň){if(Ň.ܫ!=ፎ.ፏ){return true;}if(Ň.ᕤ==ኈ.ቔ||Ň.ᕤ==ኈ.ቓ||Ň.ᕤ==ኈ.አ||Ň.ᕤ==ኈ.ኜ){ᯔ--;if
(ᯔ==-1){ኘ.ಯ();}ኘ.ૐ(ɴ.ɲ);}if(Ň.ᕤ==ኈ.ኡ){ኘ.ಯ();ኘ.ૐ(ɴ.ȭ);}return true;}public int ᯓ=>ᯔ;}sealed class ભ{private ଦ l;public ભ(ଦ
l){this.l=l;ᯒ=ᯝ;ᯑ=ᯟ;}private Func<ᯍ,bool>ᯒ;private Func<ᯍ,bool>ᯑ;private Ɠ ᯐ;private Ɠ ᯏ;private Ꮖ ᯎ;private Ꮖ ᯙ;private
Ꮖ ᯚ;private int ᯞ;private Ꮖ ಸ;private Ꮖ ಷ;private bool ᯝ(ᯍ ܩ){if(ܩ.ᒭ!=null){var ò=ܩ.ᒭ;if((ò.ᘾ&ᶘ.ᶔ)==0){return false;}var
ઑ=l.ય;ઑ.ᛣ(ò);if(ઑ.ᛡ>=ઑ.ᛢ){return false;}var њ=ᯙ*ܩ.ᯆ;if(ò.ɤ==null||ò.ɣ.Ģ!=ò.ɤ.Ģ){var ѭ=(ઑ.ᛡ-ᯎ)/њ;if(ѭ>ಷ){ಷ=ѭ;}}if(ò.ɤ==
null||ò.ɣ.ģ!=ò.ɤ.ģ){var ѭ=(ઑ.ᛢ-ᯎ)/њ;if(ѭ<ಸ){ಸ=ѭ;}}if(ಸ<=ಷ){return false;}return true;}var ğ=ܩ.ᯅ;if(ğ==ᯏ){return true;}{if((ğ
.ᘾ&ళ.ᘐ)==0){return true;}var њ=ᯙ*ܩ.ᯆ;var ᯜ=(ğ.ኴ+ğ.ǡ-ᯎ)/њ;if(ᯜ<ಷ){return true;}var ᯛ=(ğ.ኴ-ᯎ)/њ;if(ᯛ>ಸ){return true;}if(ᯜ>ಸ
){ᯜ=ಸ;}if(ᯛ<ಷ){ᯛ=ಷ;}ᯚ=(ᯜ+ᯛ)/2;ᯐ=ğ;return false;}}private bool ᯟ(ᯍ ܩ){var ട=l.મ;var Ⴃ=l.ڏ;if(ܩ.ᒭ!=null){var ò=ܩ.ᒭ;if(ò.Ě!=
0){ട.ᛏ(ᯏ,ò);}if((ò.ᘾ&ᶘ.ᶔ)==0){goto hitLine;}var ઑ=l.ય;ઑ.ᛣ(ò);var њ=ᯙ*ܩ.ᯆ;if(ò.ɤ==null){{var ѭ=(ઑ.ᛡ-ᯎ)/њ;if(ѭ>ᯚ){goto
hitLine;}}{var ѭ=(ઑ.ᛢ-ᯎ)/њ;if(ѭ<ᯚ){goto hitLine;}}}else{if(ò.ɣ.Ģ!=ò.ɤ.Ģ){var ѭ=(ઑ.ᛡ-ᯎ)/њ;if(ѭ>ᯚ){goto hitLine;}}if(ò.ɣ.ģ!=ò.ɤ.ģ
){var ѭ=(ઑ.ᛢ-ᯎ)/њ;if(ѭ<ᯚ){goto hitLine;}}}return true;hitLine:var ڂ=ܩ.ᯆ-Ꮖ.Ꭸ(4)/ᯙ;var ǃ=Ⴃ.ݕ.ޚ+Ⴃ.ݕ.ޘ*ڂ;var ǂ=Ⴃ.ݕ.ޙ+Ⴃ.ݕ.ޗ*ڂ;
var ݳ=ᯎ+ᯚ*(ڂ*ᯙ);if(ò.ɣ.ĥ==l.શ.ᜅ){if(ݳ>ò.ɣ.ģ){return false;}if(ò.ɤ!=null&&ò.ɤ.ĥ==l.શ.ᜅ){return false;}}ᯀ(ǃ,ǂ,ݳ);return false
;}{var ğ=ܩ.ᯅ;if(ğ==ᯏ){return true;}if((ğ.ᘾ&ళ.ᘐ)==0){return true;}var њ=ᯙ*ܩ.ᯆ;var ᯜ=(ğ.ኴ+ğ.ǡ-ᯎ)/њ;if(ᯜ<ᯚ){return true;}var
ᯛ=(ğ.ኴ-ᯎ)/њ;if(ᯛ>ᯚ){return true;}var ڂ=ܩ.ᯆ-Ꮖ.Ꭸ(10)/ᯙ;var ǃ=Ⴃ.ݕ.ޚ+Ⴃ.ݕ.ޘ*ڂ;var ǂ=Ⴃ.ݕ.ޙ+Ⴃ.ݕ.ޗ*ڂ;var ݳ=ᯎ+ᯚ*(ڂ*ᯙ);if((ܩ.ᯅ.ᘾ&ళ.
ᘊ)!=0){ᯀ(ǃ,ǂ,ݳ);}else{ᮺ(ǃ,ǂ,ݳ,ᯞ);}if(ᯞ!=0){l.ર.ᅹ(ğ,ᯏ,ᯏ,ᯞ);}return false;}}public Ꮖ ᮼ(Ɠ ᮽ,ɡ Ş,Ꮖ র){ᮽ=l.ઽ(ᮽ);ᯏ=ᮽ;ᯎ=ᮽ.ኴ+(ᮽ.ǡ
>>1)+Ꮖ.Ꭸ(8);ᯙ=র;var ಹ=ᮽ.ޚ+র.Ꮄ()*ஜ.ௐ(Ş);var ಽ=ᮽ.ޙ+র.Ꮄ()*ஜ.ஸ(Ş);ಸ=Ꮖ.Ꭸ(100)/160;ಷ=Ꮖ.Ꭸ(-100)/160;ᯐ=null;l.ڏ.ܨ(ᮽ.ޚ,ᮽ.ޙ,ಹ,ಽ,ݐ.ݑ|
ݐ.ݒ,ᯒ);if(ᯐ!=null){return ᯚ;}return Ꮖ.Ꮓ;}public void ᮿ(Ɠ ᮽ,ɡ Ş,Ꮖ র,Ꮖ ѭ,int Ь){ᯏ=ᮽ;ᯎ=ᮽ.ኴ+(ᮽ.ǡ>>1)+Ꮖ.Ꭸ(8);ᯙ=র;ᯚ=ѭ;ᯞ=Ь;var ಹ
=ᮽ.ޚ+র.Ꮄ()*ஜ.ௐ(Ş);var ಽ=ᮽ.ޙ+র.Ꮄ()*ஜ.ஸ(Ş);l.ڏ.ܨ(ᮽ.ޚ,ᮽ.ޙ,ಹ,ಽ,ݐ.ݑ|ݐ.ݒ,ᯑ);}public void ᯀ(Ꮖ ǃ,Ꮖ ǂ,Ꮖ ݳ){var ǵ=l.ષ;ݳ+=new Ꮖ((ǵ.ѐ
()-ǵ.ѐ())<<10);var ğ=l.ળ.ᆕ(ǃ,ǂ,ݳ,ё.Ϫ);ğ.ᙀ=Ꮖ.Ꮒ;ğ.ŵ-=ǵ.ѐ()&3;if(ğ.ŵ<1){ğ.ŵ=1;}if(ᯙ==ଊ.ಔ){ğ.ᘸ(ᚏ.ᙶ);}}public void ᮺ(Ꮖ ǃ,Ꮖ ǂ,Ꮖ
ݳ,int Ь){var ǵ=l.ષ;ݳ+=new Ꮖ((ǵ.ѐ()-ǵ.ѐ())<<10);var ğ=l.ળ.ᆕ(ǃ,ǂ,ݳ,ё.ϩ);ğ.ᙀ=Ꮖ.Ꭸ(2);ğ.ŵ-=ǵ.ѐ()&3;if(ğ.ŵ<1){ğ.ŵ=1;}if(Ь<=12&&
Ь>=9){ğ.ᘸ(ᚏ.ᙺ);}else if(Ь<9){ğ.ᘸ(ᚏ.ᙹ);}}public Ɠ ᮯ=>ᯐ;public Ꮖ ᮮ=>ಷ;public Ꮖ ᮠ=>ಸ;}interface ᮟ:IReadOnlyList<ᎂ>{int ᮾ(
string Ĭ);ᎂ this[string Ĭ]{get;}int ᜅ{get;}ᎂ ᯇ{get;}}interface ᯈ{void ᯉ(ᑧ ߚ,bool ߗ);int ᯊ{get;}int ᯋ{get;set;}}sealed class ᯍ{
private Ꮖ ڂ;private Ɠ ğ;private ɢ ò;public void ᯌ(Ꮖ ڂ,Ɠ ğ){this.ڂ=ڂ;this.ğ=ğ;this.ò=null;}public void ᯌ(Ꮖ ڂ,ɢ ò){this.ڂ=ڂ;this.
ğ=null;this.ò=ò;}public Ꮖ ᯆ{get{return ڂ;}set{ڂ=value;}}public Ɠ ᯅ=>ğ;public ɢ ᒭ=>ò;}sealed class ᕅ{private ᴆ Å;private ᰋ
ݤ;private ŕ[]ᯄ;private bool ᯃ;private ᶴ Í;private int[]œ;private int[]Œ;private int[]š;private int[]ᯂ;private int ᯁ;
private int ᮻ;private int ᯠ;private int ᰉ;private int ᱥ;private bool ᱦ;private int ᱧ;private int[][]ᱨ;private int[]ᱩ;private Ᏺ
ǵ;private ᒳ[]ཙ;private bool ᱪ;private int ú;private int ᒾ;private bool ஆ;public ᕅ(ᴆ Å,ᰋ ݤ){this.Å=Å;this.ݤ=ݤ;ᯄ=ݤ.ᰁ;œ=new
int[ђ.ۇ];Œ=new int[ђ.ۇ];š=new int[ђ.ۇ];ᯂ=new int[ђ.ۇ];ᱨ=new int[ђ.ۇ][];for(var Ä=0;Ä<ђ.ۇ;Ä++){ᱨ[Ä]=new int[ђ.ۇ];}ᱩ=new int[
ђ.ۇ];if(Å.ᴵ!=0){ᱣ();}else if(Å.ᴶ){ᱤ();}else{ᱫ();}ஆ=false;}private void ᱫ(){Í=ᶴ.ᶭ;ᯃ=false;ᰉ=1;œ[0]=Œ[0]=š[0]=-1;ᯁ=ᮻ=-1;ᯠ=ᱺ
.ᱹ;ᱟ();}private void ᱤ(){Í=ᶴ.ᶭ;ᯃ=false;ᱥ=1;ᯠ=ᱺ.ᱹ;var ž=0;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(!Å.ᰁ[Ä].Ÿ){continue;}œ[Ä]=Œ[Ä]=š[Ä]=ᯂ[
Ä]=0;ž+=ᱱ(Ä);}ᱦ=ž>0;ᱟ();}private void ᱣ(){Í=ᶴ.ᶭ;ᯃ=false;ᱧ=1;ᯠ=ᱺ.ᱹ;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Å.ᰁ[Ä].Ÿ){for(var ͼ=0;ͼ<ђ.ۇ;ͼ
++){if(Å.ᰁ[ͼ].Ÿ){ᱨ[Ä][ͼ]=0;}}ᱩ[Ä]=0;}}ᱟ();}private void ᱢ(){Í=ᶴ.ᶮ;ᯃ=false;ú=10;}private static int ᱡ=4;private void ᱠ(){Í=
ᶴ.ᶬ;ᯃ=false;ú=ᱡ*ᱺ.ᱹ;ᱟ();}private void ᱟ(){if(Å.ಖ==ಖ.ᴉ){return;}if(ݤ.ᰔ>2){return;}if(ཙ==null){ཙ=new ᒳ[ᒶ.ᔟ[ݤ.ᰔ].Count];for(
var Ä=0;Ä<ཙ.Length;Ä++){ཙ[Ä]=new ᒳ(this,ᒶ.ᔟ[ݤ.ᰔ][Ä],Ä);}ǵ=new Ᏺ();}foreach(var ᛜ in ཙ){ᛜ.ʕ(ᒾ);}}public ன ڞ(){ᒾ++;ᱷ();if(ᒾ==
1){if(Å.ಖ==ಖ.ᴉ){Å.ᴾ.ᯉ(ᑧ.ᐵ,true);}else{Å.ᴾ.ᯉ(ᑧ.ᑭ,true);}}switch(Í){case ᶴ.ᶭ:if(Å.ᴵ!=0){ᱲ();}else if(Å.ᴶ){ᱝ();}else{ᱞ();}
break;case ᶴ.ᶬ:ᱴ();break;case ᶴ.ᶮ:ᱵ();break;}if(ஆ){return ன.த;}else{if(ᒾ==1){return ன.ண;}else{return ன.ந;}}}private void ᱞ(){
ᱶ();if(ᯃ&&ᰉ!=10){ᯃ=false;œ[0]=(ᯄ[0].Ź*100)/ݤ.ᰆ;Œ[0]=(ᯄ[0].ź*100)/ݤ.ᰅ;š[0]=(ᯄ[0].Ż*100)/ݤ.ᰄ;ᯁ=ᯄ[0].ż/ᱺ.ᱹ;ᮻ=ݤ.ᰂ/ᱺ.ᱹ;ૐ(ɴ.ɺ);
ᰉ=10;}if(ᰉ==2){œ[0]+=2;if((ᒾ&3)==0){ૐ(ɴ.ɲ);}if(œ[0]>=(ᯄ[0].Ź*100)/ݤ.ᰆ){œ[0]=(ᯄ[0].Ź*100)/ݤ.ᰆ;ૐ(ɴ.ɺ);ᰉ++;}}else if(ᰉ==4){Œ
[0]+=2;if((ᒾ&3)==0){ૐ(ɴ.ɲ);}if(Œ[0]>=(ᯄ[0].ź*100)/ݤ.ᰅ){Œ[0]=(ᯄ[0].ź*100)/ݤ.ᰅ;ૐ(ɴ.ɺ);ᰉ++;}}else if(ᰉ==6){š[0]+=2;if((ᒾ&3)
==0){ૐ(ɴ.ɲ);}if(š[0]>=(ᯄ[0].Ż*100)/ݤ.ᰄ){š[0]=(ᯄ[0].Ż*100)/ݤ.ᰄ;ૐ(ɴ.ɺ);ᰉ++;}}else if(ᰉ==8){if((ᒾ&3)==0){ૐ(ɴ.ɲ);}ᯁ+=3;if(ᯁ>=ᯄ
[0].ż/ᱺ.ᱹ){ᯁ=ᯄ[0].ż/ᱺ.ᱹ;}ᮻ+=3;if(ᮻ>=ݤ.ᰂ/ᱺ.ᱹ){ᮻ=ݤ.ᰂ/ᱺ.ᱹ;if(ᯁ>=ᯄ[0].ż/ᱺ.ᱹ){ૐ(ɴ.ɺ);ᰉ++;}}}else if(ᰉ==10){if(ᯃ){ૐ(ɴ.ɰ);if(Å.ಖ
==ಖ.ᴉ){ᱢ();}else{ᱠ();}}}else if((ᰉ&1)!=0){if(--ᯠ==0){ᰉ++;ᯠ=ᱺ.ᱹ;}}}private void ᱝ(){ᱶ();bool ᱜ;if(ᯃ&&ᱥ!=10){ᯃ=false;for(var
Ä=0;Ä<ђ.ۇ;Ä++){if(!Å.ᰁ[Ä].Ÿ){continue;}œ[Ä]=(ᯄ[Ä].Ź*100)/ݤ.ᰆ;Œ[Ä]=(ᯄ[Ä].ź*100)/ݤ.ᰅ;š[Ä]=(ᯄ[Ä].Ż*100)/ݤ.ᰄ;}ૐ(ɴ.ɺ);ᱥ=10;}if
(ᱥ==2){if((ᒾ&3)==0){ૐ(ɴ.ɲ);}ᱜ=false;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(!Å.ᰁ[Ä].Ÿ){continue;}œ[Ä]+=2;if(œ[Ä]>=(ᯄ[Ä].Ź*100)/ݤ.ᰆ){œ[Ä
]=(ᯄ[Ä].Ź*100)/ݤ.ᰆ;}else{ᱜ=true;}}if(!ᱜ){ૐ(ɴ.ɺ);ᱥ++;}}else if(ᱥ==4){if((ᒾ&3)==0){ૐ(ɴ.ɲ);}ᱜ=false;for(var Ä=0;Ä<ђ.ۇ;Ä++){
if(!Å.ᰁ[Ä].Ÿ){continue;}Œ[Ä]+=2;if(Œ[Ä]>=(ᯄ[Ä].ź*100)/ݤ.ᰅ){Œ[Ä]=(ᯄ[Ä].ź*100)/ݤ.ᰅ;}else{ᱜ=true;}}if(!ᱜ){ૐ(ɴ.ɺ);ᱥ++;}}else
if(ᱥ==6){if((ᒾ&3)==0){ૐ(ɴ.ɲ);}ᱜ=false;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(!Å.ᰁ[Ä].Ÿ){continue;}š[Ä]+=2;if(š[Ä]>=(ᯄ[Ä].Ż*100)/ݤ.ᰄ){š[
Ä]=(ᯄ[Ä].Ż*100)/ݤ.ᰄ;}else{ᱜ=true;}}if(!ᱜ){ૐ(ɴ.ɺ);if(ᱦ){ᱥ++;}else{ᱥ+=3;}}}else if(ᱥ==8){if((ᒾ&3)==0){ૐ(ɴ.ɲ);}ᱜ=false;for(
var Ä=0;Ä<ђ.ۇ;Ä++){if(!Å.ᰁ[Ä].Ÿ){continue;}ᯂ[Ä]+=1;var ആ=ᱱ(Ä);if(ᯂ[Ä]>=ആ){ᯂ[Ä]=ആ;}else{ᱜ=true;}}if(!ᱜ){ૐ(ɴ.ɔ);ᱥ++;}}else if
(ᱥ==10){if(ᯃ){ૐ(ɴ.ɰ);if(Å.ಖ==ಖ.ᴉ){ᱢ();}else{ᱠ();}}}else if((ᱥ&1)!=0){if(--ᯠ==0){ᱥ++;ᯠ=ᱺ.ᱹ;}}}private void ᱲ(){ᱶ();bool ᱳ;
if(ᯃ&&ᱧ!=4){ᯃ=false;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Å.ᰁ[Ä].Ÿ){for(var ͼ=0;ͼ<ђ.ۇ;ͼ++){if(Å.ᰁ[ͼ].Ÿ){ᱨ[Ä][ͼ]=ᯄ[Ä].Ž[ͼ];}}ᱩ[Ä]=ᱱ(Ä);
}}ૐ(ɴ.ɺ);ᱧ=4;}if(ᱧ==2){if((ᒾ&3)==0){ૐ(ɴ.ɲ);}ᱳ=false;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Å.ᰁ[Ä].Ÿ){for(var ͼ=0;ͼ<ђ.ۇ;ͼ++){if(Å.ᰁ[ͼ].
Ÿ&&ᱨ[Ä][ͼ]!=ᯄ[Ä].Ž[ͼ]){if(ᯄ[Ä].Ž[ͼ]<0){ᱨ[Ä][ͼ]--;}else{ᱨ[Ä][ͼ]++;}if(ᱨ[Ä][ͼ]>99){ᱨ[Ä][ͼ]=99;}if(ᱨ[Ä][ͼ]<-99){ᱨ[Ä][ͼ]=-99;
}ᱳ=true;}}ᱩ[Ä]=ᱱ(Ä);if(ᱩ[Ä]>99){ᱩ[Ä]=99;}if(ᱩ[Ä]<-99){ᱩ[Ä]=-99;}}}if(!ᱳ){ૐ(ɴ.ɺ);ᱧ++;}}else if(ᱧ==4){if(ᯃ){ૐ(ɴ.ȡ);if(Å.ಖ==
ಖ.ᴉ){ᱢ();}else{ᱠ();}}}else if((ᱧ&1)!=0){if(--ᯠ==0){ᱧ++;ᯠ=ᱺ.ᱹ;}}}private void ᱴ(){ᱶ();if(--ú==0||ᯃ){ᱢ();}else{ᱪ=(ú&31)<20;
}}private void ᱵ(){ᱶ();if(--ú==0){ஆ=true;}}private void ᱶ(){if(Å.ಖ==ಖ.ᴉ){return;}if(ݤ.ᰔ>2){return;}foreach(var Ã in ཙ){Ã.
ڞ(ᒾ);}}private void ᱷ(){for(var Ä=0;Ä<ђ.ۇ;Ä++){var Ð=Å.ᰁ[Ä];if(Ð.Ÿ){if((Ð.ƕ.ఠ&ట.ఞ)!=0){if(!Ð.Ɗ){ᯃ=true;}Ð.Ɗ=true;}else{Ð.
Ɗ=false;}if((Ð.ƕ.ఠ&ట.ఆ)!=0){if(!Ð.Ɖ){ᯃ=true;}Ð.Ɖ=true;}else{Ð.Ɖ=false;}}}}private int ᱱ(int ä){var ž=0;for(var Ä=0;Ä<ђ.ۇ;
Ä++){if(Å.ᰁ[Ä].Ÿ&&Ä!=ä){ž+=ᯄ[ä].Ž[Ä];}}ž-=ᯄ[ä].Ž[ä];return ž;}private void ૐ(ɴ Ǯ){Å.ᴽ.ૐ(Ǯ);}public ᴆ હ=>Å;public ᰋ ᘿ=>ݤ;
public ᶴ Ŷ=>Í;public IReadOnlyList<int>Ź=>œ;public IReadOnlyList<int>ź=>Œ;public IReadOnlyList<int>Ż=>š;public IReadOnlyList<
int>ᱰ=>ᯂ;public int ᱯ=>ᯁ;public int ᱮ=>ᮻ;public int[][]ᱭ=>ᱨ;public int[]ᱬ=>ᱩ;public bool ᱛ=>ᱦ;public Ᏺ ષ=>ǵ;public ᒳ[]ᅓ=>ཙ;
public bool ᰊ=>ᱪ;}class ᰋ{private int ᔾ;private bool ܒ;private int ᰌ;private int ᰍ;private int ᰏ;private int ᰓ;private int ᰐ;
private int ᰑ;private int ᰒ;private ŕ[]Ë;public ᰋ(){Ë=new ŕ[ђ.ۇ];for(var Ä=0;Ä<ђ.ۇ;Ä++){Ë[Ä]=new ŕ();}}public int ᰔ{get{return
ᔾ;}set{ᔾ=value;}}public bool Ʈ{get{return ܒ;}set{ܒ=value;}}public int ᰈ{get{return ᰌ;}set{ᰌ=value;}}public int ᰇ{get{
return ᰍ;}set{ᰍ=value;}}public int ᰆ{get{return Math.Max(ᰏ,1);}set{ᰏ=value;}}public int ᰅ{get{return Math.Max(ᰓ,1);}set{ᰓ=
value;}}public int ᰄ{get{return Math.Max(ᰐ,1);}set{ᰐ=value;}}public int ᰃ{get{return Math.Max(ᰑ,1);}set{ᰑ=value;}}public int
ᰂ{get{return ᰒ;}set{ᰒ=value;}}public ŕ[]ᰁ{get{return Ë;}}}sealed class ᰀ{private static int Ī=2;private static int ᯥ=33;
private static int ᯤ=50;private static int ᯣ=50;private static int ᯢ=16;private static int ᯡ=200-32;private static int ᰎ=50;
private static int ᰕ=64;private static int ᰡ=42;private static int ᰚ=68;private static int ᰛ=40;private static int ᰜ=269;
private static int ᰝ=10;private static int ᰞ=100;private static int ᰟ=5;private static int ᰠ=50;private static string[]ᰢ=new
string[]{"WIMAP0","WIMAP1","WIMAP2"};private static string[]ᱚ=new string[]{"STPB0","STPB1","STPB2","STPB3"};private static
string[]ᰣ=new string[]{"WIURH0","WIURH1"};private static string[][]ᱍ;private static string[]ᱎ;static ᰀ(){ᱍ=new string[4][];for
(var Ň=0;Ň<4;Ň++){ᱍ[Ň]=new string[9];for(var ᱏ=0;ᱏ<9;ᱏ++){ᱍ[Ň][ᱏ]="WILV"+Ň+ᱏ;}}ᱎ=new string[32];for(var ᱏ=0;ᱏ<32;ᱏ++){ᱎ[ᱏ
]="CWILV"+ᱏ.ToString("00");}}private ಆ þ;private Ꮝ ȋ;private ٻ چ;private ڦ ᰙ;private ڦ[]ᰘ;private ڦ ᰗ;private ڦ ᰖ;private
int ǉ;public ᰀ(ಆ þ,Ꮝ ȋ){this.þ=þ;this.ȋ=ȋ;چ=new ٻ(þ);ᰙ=ڦ.ÿ(þ,"WIMINUS");ᰘ=new ڦ[10];for(var Ä=0;Ä<10;Ä++){ᰘ[Ä]=ڦ.ÿ(þ,
"WINUM"+Ä);}ᰗ=ڦ.ÿ(þ,"WIPCNT");ᰖ=ڦ.ÿ(þ,"WICOLON");ǉ=ȋ.Ǒ/320;}private void Ꮘ(ڦ ڌ,int ǃ,int ǂ){ȋ.Ꮘ(ڌ,ǉ*ǃ,ǉ*ǂ,ǉ);}private void Ꮘ(
string Ĭ,int ǃ,int ǂ){var ǉ=ȋ.Ǒ/320;ȋ.Ꮘ(چ[Ĭ],ǉ*ǃ,ǉ*ǂ,ǉ);}private int ڍ(string Ĭ){return چ.ڍ(Ĭ);}private int ڎ(string Ĭ){return
چ.ڎ(Ĭ);}public void Ǌ(ᕅ ᒲ){switch(ᒲ.Ŷ){case ᶴ.ᶭ:if(ᒲ.હ.ᴵ!=0){ḇ(ᒲ);}else if(ᒲ.હ.ᴶ){Ḍ(ᒲ);}else{Ḋ(ᒲ);}break;case ᶴ.ᶬ:ḅ(ᒲ);
break;case ᶴ.ᶮ:Ḇ(ᒲ);break;}}private void ḍ(ᕅ ᒲ){if(ᒲ.હ.ಖ==ಖ.ᴉ){Ꮘ("INTERPIC",0,0);}else{var Ň=ᒲ.હ.ᰔ-1;if(Ň<ᰢ.Length){Ꮘ(ᰢ[Ň],0,
0);}else{Ꮘ("INTERPIC",0,0);}}}private void Ḋ(ᕅ ᒲ){ḍ(ᒲ);ᶨ(ᒲ);Ḑ(ᒲ);var ḋ=(3*ᰘ[0].ǡ)/2;Ꮘ("WIOSTK",ᯤ,ᯣ);ഡ(320-ᯤ,ᯣ,ᒲ.Ź[0]);Ꮘ(
"WIOSTI",ᯤ,ᯣ+ḋ);ഡ(320-ᯤ,ᯣ+ḋ,ᒲ.ź[0]);Ꮘ("WISCRT2",ᯤ,ᯣ+2*ḋ);ഡ(320-ᯤ,ᯣ+2*ḋ,ᒲ.Ż[0]);Ꮘ("WITIME",ᯢ,ᯡ);Ḏ(320/2-ᯢ,ᯡ,ᒲ.ᱯ);if(ᒲ.ᘿ.ᰔ<3){Ꮘ(
"WIPAR",320/2+ᯢ,ᯡ);Ḏ(320-ᯢ,ᯡ,ᒲ.ᱮ);}}private void Ḍ(ᕅ ᒲ){ḍ(ᒲ);ᶨ(ᒲ);Ḑ(ᒲ);var Ḉ=32+ڍ("STFST01")/2;if(!ᒲ.ᱛ){Ḉ+=32;}Ꮘ("WIOSTK",Ḉ+ᰕ-ڍ
("WIOSTK"),ᰎ);Ꮘ("WIOSTI",Ḉ+2*ᰕ-ڍ("WIOSTI"),ᰎ);Ꮘ("WIOSTS",Ḉ+3*ᰕ-ڍ("WIOSTS"),ᰎ);if(ᒲ.ᱛ){Ꮘ("WIFRGS",Ḉ+4*ᰕ-ڍ("WIFRGS"),ᰎ);}
var ǂ=ᰎ+ڎ("WIOSTK");for(var Ä=0;Ä<ђ.ۇ;Ä++){if(!ᒲ.હ.ᰁ[Ä].Ÿ){continue;}var ǃ=Ḉ;Ꮘ(ᱚ[Ä],ǃ-ڍ(ᱚ[Ä]),ǂ);if(Ä==ᒲ.હ.ଙ){Ꮘ("STFST01",ǃ
-ڍ(ᱚ[Ä]),ǂ);}ǃ+=ᰕ;ഡ(ǃ-ᰗ.Ǒ,ǂ+10,ᒲ.Ź[Ä]);ǃ+=ᰕ;ഡ(ǃ-ᰗ.Ǒ,ǂ+10,ᒲ.ź[Ä]);ǃ+=ᰕ;ഡ(ǃ-ᰗ.Ǒ,ǂ+10,ᒲ.Ż[Ä]);ǃ+=ᰕ;if(ᒲ.ᱛ){അ(ǃ,ǂ+10,ᒲ.ᱰ[Ä],-
1);}ǂ+=ᯥ;}}private void ḇ(ᕅ ᒲ){ḍ(ᒲ);ᶨ(ᒲ);Ḑ(ᒲ);Ꮘ("WIMSTT",ᰜ-ڍ("WIMSTT")/2,ᰚ-ᯥ+10);Ꮘ("WIKILRS",ᰝ,ᰞ);Ꮘ("WIVCTMS",ᰟ,ᰠ);var ǃ=
ᰡ+ᰛ;var ǂ=ᰚ;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(ᒲ.હ.ᰁ[Ä].Ÿ){Ꮘ(ᱚ[Ä],ǃ-ڍ(ᱚ[Ä])/2,ᰚ-ᯥ);Ꮘ(ᱚ[Ä],ᰡ-ڍ(ᱚ[Ä])/2,ǂ);if(Ä==ᒲ.હ.ଙ){Ꮘ("STFDEAD0"
,ǃ-ڍ(ᱚ[Ä])/2,ᰚ-ᯥ);Ꮘ("STFST01",ᰡ-ڍ(ᱚ[Ä])/2,ǂ);}}else{}ǃ+=ᰛ;ǂ+=ᯥ;}ǂ=ᰚ+10;var ೡ=ᰘ[0].Ǒ;for(var Ä=0;Ä<ђ.ۇ;Ä++){ǃ=ᰡ+ᰛ;if(ᒲ.હ.ᰁ
[Ä].Ÿ){for(var ͼ=0;ͼ<ђ.ۇ;ͼ++){if(ᒲ.હ.ᰁ[ͼ].Ÿ){അ(ǃ+ೡ,ǂ,ᒲ.ᱭ[Ä][ͼ],2);}ǃ+=ᰛ;}അ(ᰜ+ೡ,ǂ,ᒲ.ᱬ[Ä],2);}ǂ+=ᯥ;}}private void Ḇ(ᕅ ᒲ){ḅ(
ᒲ);}private void ḅ(ᕅ ᒲ){ḍ(ᒲ);ᶨ(ᒲ);if(ᒲ.હ.ಖ!=ಖ.ᴉ){if(ᒲ.ᘿ.ᰔ>2){ḓ(ᒲ);return;}var ḉ=(ᒲ.ᘿ.ᰈ==8)?ᒲ.ᘿ.ᰇ-1:ᒲ.ᘿ.ᰈ;for(var Ä=0;Ä<=ḉ
;Ä++){var ǃ=ଝ.ଉ[ᒲ.ᘿ.ᰔ][Ä].ޚ;var ǂ=ଝ.ଉ[ᒲ.ᘿ.ᰔ][Ä].ޙ;Ꮘ("WISPLAT",ǃ,ǂ);}if(ᒲ.ᘿ.Ʈ){var ǃ=ଝ.ଉ[ᒲ.ᘿ.ᰔ][8].ޚ;var ǂ=ଝ.ଉ[ᒲ.ᘿ.ᰔ][8].ޙ
;Ꮘ("WISPLAT",ǃ,ǂ);}if(ᒲ.ᰊ){var ǃ=ଝ.ଉ[ᒲ.ᘿ.ᰔ][ᒲ.ᘿ.ᰇ].ޚ;var ǂ=ଝ.ଉ[ᒲ.ᘿ.ᰔ][ᒲ.ᘿ.ᰇ].ޙ;ᶯ(ᰣ,ǃ,ǂ);}}if((ᒲ.હ.ಖ!=ಖ.ᴉ)||ᒲ.ᘿ.ᰇ!=30){ḓ(ᒲ
);}}private void Ḑ(ᕅ Ȏ){var ḑ=Ȏ.ᘿ;var ǂ=Ī;string Ḓ;if(Ȏ.હ.ಖ!=ಖ.ᴉ){var Ň=Ȏ.હ.ᰔ-1;Ḓ=ᱍ[Ň][ḑ.ᰈ];}else{Ḓ=ᱎ[ḑ.ᰈ];}Ꮘ(Ḓ,(320-ڍ(Ḓ)
)/2,ǂ);ǂ+=(5*ڎ(Ḓ))/4;Ꮘ("WIF",(320-ڍ("WIF"))/2,ǂ);}private void ḓ(ᕅ ᒲ){var ḑ=ᒲ.ᘿ;int ǂ=Ī;string Ḓ;if(ᒲ.હ.ಖ!=ಖ.ᴉ){var Ň=ᒲ.હ
.ᰔ-1;Ḓ=ᱍ[Ň][ḑ.ᰇ];}else{Ḓ=ᱎ[ḑ.ᰇ];}Ꮘ("WIENTER",(320-ڍ("WIENTER"))/2,ǂ);ǂ+=(5*ڎ(Ḓ))/4;Ꮘ(Ḓ,(320-ڍ(Ḓ))/2,ǂ);}private int അ(int
ǃ,int ǂ,int ᅦ,int ೱ){if(ೱ<0){if(ᅦ==0){ೱ=1;}else{ೱ=0;var ў=ᅦ;while(ў!=0){ў/=10;ೱ++;}}}var ഏ=ᅦ<0;if(ഏ){ᅦ=-ᅦ;}if(ᅦ==1994){
return 0;}var ḏ=ᰘ[0].Ǒ;while(ೱ--!=0){ǃ-=ḏ;Ꮘ(ᰘ[ᅦ%10],ǃ,ǂ);ᅦ/=10;}if(ഏ){Ꮘ(ᰙ,ǃ-=8,ǂ);}return ǃ;}private void ഡ(int ǃ,int ǂ,int É)
{if(É<0){return;}Ꮘ(ᰗ,ǃ,ǂ);അ(ǃ,ǂ,É,-1);}private void Ḏ(int ǃ,int ǂ,int ჹ){if(ჹ<0){return;}if(ჹ<=61*59){var Ḅ=1;do{var ᅦ=(ჹ
/Ḅ)%60;ǃ=അ(ǃ,ǂ,ᅦ,2)-ᰖ.Ǒ;Ḅ*=60;if(Ḅ==60||ჹ/Ḅ!=0){Ꮘ(ᰖ,ǃ,ǂ);}}while(ჹ/Ḅ!=0);}else{Ꮘ("WISUCKS",ǃ-ڍ("WISUCKS"),ǂ);}}private
void ᶨ(ᕅ ᒲ){if(ᒲ.હ.ಖ==ಖ.ᴉ){return;}if(ᒲ.ᘿ.ᰔ>2){return;}for(var Ä=0;Ä<ᒲ.ᅓ.Length;Ä++){var Ã=ᒲ.ᅓ[Ä];if(Ã.ᓁ>=0){Ꮘ(Ã.ജ[Ã.ᓁ],Ã.ᒿ,
Ã.ᓀ);}}}private void ᶯ(IReadOnlyList<string>ᶰ,int ǃ,int ǂ){var ᶳ=false;var Ä=0;do{var ڌ=چ[ᶰ[Ä]];var ල=ǃ-ڌ.پ;var ᶱ=ǂ-ڌ.ٽ;
var ර=ල+ڌ.Ǒ;var ᶲ=ᶱ+ڌ.ǡ;if(ල>=0&&ර<320&&ᶱ>=0&&ᶲ<320){ᶳ=true;}else{Ä++;}}while(!ᶳ&&Ä!=2);if(ᶳ&&Ä<2){Ꮘ(ᶰ[Ä],ǃ,ǂ);}else{throw
new Exception("Could not place patch!");}}}public enum ᶴ{ᶮ=-1,ᶭ,ᶬ}interface ᶫ{void ᶪ(Ɠ ᶩ);void ڞ();void ૐ(ɴ Ǯ);void ૐ(Ɠ ã,ɴ
Ǯ,ʡ ˌ);void ૐ(Ɠ ã,ɴ Ǯ,ʡ ˌ,int ߛ);void ૠ(Ɠ ã);void ʕ();void ஓ();void ḁ();int ᯊ{get;}int ᯋ{get;set;}}interface Ḃ{ၜ this[э Т
]{get;}}sealed class ଔ{private ଦ l;public ଔ(ଦ l){this.l=l;}public bool ḃ(ђ Ð,ᓩ ۅ,int ˋ){if(ۅ==ᓩ.ᓫ){return false;}if(ۅ<0||
(int)ۅ>(int)ᓩ.ŏ){throw new Exception("Bad ammo type: "+ۅ);}if(Ð.ƌ[(int)ۅ]==Ð.Ƌ[(int)ۅ]){return false;}if(ˋ!=0){ˋ*=ኑ.ᕸ.З[(
int)ۅ];}else{ˋ=ኑ.ᕸ.З[(int)ۅ]/2;}if(l.હ.ᴷ==ᵅ.Р||l.હ.ᴷ==ᵅ.ᵇ){ˋ<<=1;}var Ḁ=Ð.ƌ[(int)ۅ];Ð.ƌ[(int)ۅ]+=ˋ;if(Ð.ƌ[(int)ۅ]>Ð.Ƌ[(int)
ۅ]){Ð.ƌ[(int)ۅ]=Ð.Ƌ[(int)ۅ];}if(Ḁ!=0){return true;}switch(ۅ){case ᓩ.З:if(Ð.Ə==ਖ਼.ਹ){if(Ð.ƍ[(int)ਖ਼.Ѝ]){Ð.Ǝ=ਖ਼.Ѝ;}else{Ð.Ǝ=ਖ਼.
ਸ;}}break;case ᓩ.ᓪ:if(Ð.Ə==ਖ਼.ਹ||Ð.Ə==ਖ਼.ਸ){if(Ð.ƍ[(int)ਖ਼.Љ]){Ð.Ǝ=ਖ਼.Љ;}}break;case ᓩ.ظ:if(Ð.Ə==ਖ਼.ਹ||Ð.Ə==ਖ਼.ਸ){if(Ð.ƍ[(int)ਖ਼
.Ϲ]){Ð.Ǝ=ਖ਼.Ϲ;}}break;case ᓩ.પ:if(Ð.Ə==ਖ਼.ਹ){if(Ð.ƍ[(int)ਖ਼.પ]){Ð.Ǝ=ਖ਼.પ;}}break;default:break;}return true;}private static
int ᶿ=6;public bool ᶾ(ђ Ð,ਖ਼ ᶽ,bool ᶼ){if(l.હ.ᴶ&&(l.હ.ᴵ!=2)&&!ᶼ){if(Ð.ƍ[(int)ᶽ]){return false;}Ð.ư+=ᶿ;Ð.ƍ[(int)ᶽ]=true;if(l.
હ.ᴵ!=0){ḃ(Ð,ኑ.ኔ[(int)ᶽ].ƌ,5);}else{ḃ(Ð,ኑ.ኔ[(int)ᶽ].ƌ,2);}Ð.Ǝ=ᶽ;if(Ð==l.ଙ){l.ૐ(Ð.Ɠ,ɴ.ȟ,ʡ.ʝ);}return false;}bool ᶻ;if(ኑ.ኔ[(
int)ᶽ].ƌ!=ᓩ.ᓫ){if(ᶼ){ᶻ=ḃ(Ð,ኑ.ኔ[(int)ᶽ].ƌ,1);}else{ᶻ=ḃ(Ð,ኑ.ኔ[(int)ᶽ].ƌ,2);}}else{ᶻ=false;}bool ᶺ;if(Ð.ƍ[(int)ᶽ]){ᶺ=false;}
else{ᶺ=true;Ð.ƍ[(int)ᶽ]=true;Ð.Ǝ=ᶽ;}return(ᶺ||ᶻ);}private bool ᶹ(ђ Ð,int ˋ){if(Ð.ƙ>=ኑ.ᕺ.ᕻ){return false;}Ð.ƙ+=ˋ;if(Ð.ƙ>ኑ.ᕺ.ᕻ
){Ð.ƙ=ኑ.ᕺ.ᕻ;}Ð.Ɠ.ƙ=Ð.ƙ;return true;}private bool ᶸ(ђ Ð,int ˌ){var ᶷ=ˌ*100;if(Ð.ƚ>=ᶷ){return false;}Ð.ƛ=ˌ;Ð.ƚ=ᶷ;return
true;}private void ᶶ(ђ Ð,ᒨ ᶵ){if(Ð.Ƒ[(int)ᶵ]){return;}Ð.ư=ᶿ;Ð.Ƒ[(int)ᶵ]=true;}private bool Ḕ(ђ Ð,Ů ˌ){if(ˌ==Ů.ŭ){Ð.Ɲ[(int)ˌ]
=ኑ.ጞ.ŭ;return true;}if(ˌ==Ů.ū){Ð.Ɲ[(int)ˌ]=ኑ.ጞ.ū;Ð.Ɠ.ᘾ|=ళ.ᘋ;return true;}if(ˌ==Ů.Ũ){Ð.Ɲ[(int)ˌ]=ኑ.ጞ.Ũ;return true;}if(ˌ==
Ů.Ū){Ð.Ɲ[(int)ˌ]=ኑ.ጞ.Ū;return true;}if(ˌ==Ů.Ŭ){ᶹ(Ð,100);Ð.Ɲ[(int)ˌ]=1;return true;}if(Ð.Ɲ[(int)ˌ]!=0){return false;}Ð.Ɲ[(
int)ˌ]=1;return true;}public void Ṅ(Ɠ ā,Ɠ ṅ){var ş=ā.ኴ-ṅ.ኴ;if(ş>ṅ.ǡ||ş<Ꮖ.Ꭸ(-8)){return;}var Ŗ=ɴ.Ƞ;var Ð=ṅ.ђ;if(ṅ.ƙ<=0){
return;}switch(ā.э){case э.ວ:if(!ᶸ(Ð,ኑ.ᕺ.ᖀ)){return;}Ð.ۍ(ኑ.ጓ.ጱ);break;case э.ສ:if(!ᶸ(Ð,ኑ.ᕺ.ᕵ)){return;}Ð.ۍ(ኑ.ጓ.ጰ);break;case э
.ໂ:Ð.ƙ++;if(Ð.ƙ>ኑ.ᕺ.ᕽ){Ð.ƙ=ኑ.ᕺ.ᕽ;}Ð.Ɠ.ƙ=Ð.ƙ;Ð.ۍ(ኑ.ጓ.ጯ);break;case э.ະ:Ð.ƚ++;if(Ð.ƚ>ኑ.ᕺ.ᕾ){Ð.ƚ=ኑ.ᕺ.ᕾ;}if(Ð.ƛ==0){Ð.ƛ=ኑ.ᕺ.ᖀ
;}Ð.ۍ(ኑ.ጓ.ጮ);break;case э.ພ:Ð.ƙ+=ኑ.ᕺ.ᕳ;if(Ð.ƙ>ኑ.ᕺ.ᕴ){Ð.ƙ=ኑ.ᕺ.ᕴ;}Ð.Ɠ.ƙ=Ð.ƙ;Ð.ۍ(ኑ.ጓ.ጪ);Ŗ=ɴ.ʳ;break;case э.ບ:if(l.હ.ಖ!=ಖ.ᴉ){
return;}Ð.ƙ=ኑ.ᕺ.ᕲ;Ð.Ɠ.ƙ=Ð.ƙ;ᶸ(Ð,ኑ.ᕺ.ᕵ);Ð.ۍ(ኑ.ጓ.ዙ);Ŗ=ɴ.ʳ;break;case э.າ:if(!Ð.Ƒ[(int)ᒨ.ᒧ]){Ð.ۍ(ኑ.ጓ.ጩ);}ᶶ(Ð,ᒨ.ᒧ);if(!l.હ.ᴶ){
break;}return;case э.ຽ:if(!Ð.Ƒ[(int)ᒨ.ᒦ]){Ð.ۍ(ኑ.ጓ.ጨ);}ᶶ(Ð,ᒨ.ᒦ);if(!l.હ.ᴶ){break;}return;case э.ຳ:if(!Ð.Ƒ[(int)ᒨ.ᒥ]){Ð.ۍ(ኑ.ጓ.ጧ
);}ᶶ(Ð,ᒨ.ᒥ);if(!l.હ.ᴶ){break;}return;case э.ເ:if(!Ð.Ƒ[(int)ᒨ.ᒤ]){Ð.ۍ(ኑ.ጓ.ጦ);}ᶶ(Ð,ᒨ.ᒤ);if(!l.હ.ᴶ){break;}return;case э.ໃ:
if(!Ð.Ƒ[(int)ᒨ.ᒣ]){Ð.ۍ(ኑ.ጓ.ጥ);}ᶶ(Ð,ᒨ.ᒣ);if(!l.હ.ᴶ){break;}return;case э.ແ:if(!Ð.Ƒ[(int)ᒨ.ᒢ]){Ð.ۍ(ኑ.ጓ.ጤ);}ᶶ(Ð,ᒨ.ᒢ);if(!l.હ.
ᴶ){break;}return;case э.ມ:if(!ᶹ(Ð,10)){return;}Ð.ۍ(ኑ.ጓ.ጭ);break;case э.ຟ:if(!ᶹ(Ð,25)){return;}if(Ð.ƙ<25){Ð.ۍ(ኑ.ጓ.ጬ);}else
{Ð.ۍ(ኑ.ጓ.ጫ);}break;case э.ຝ:if(!Ḕ(Ð,Ů.ŭ)){return;}Ð.ۍ(ኑ.ጓ.ጣ);Ŗ=ɴ.ʳ;break;case э.ຜ:if(!Ḕ(Ð,Ů.Ŭ)){return;}Ð.ۍ(ኑ.ጓ.ጢ);if(Ð.Ə
!=ਖ਼.ਹ){Ð.Ǝ=ਖ਼.ਹ;}Ŗ=ɴ.ʳ;break;case э.ປ:if(!Ḕ(Ð,Ů.ū)){return;}Ð.ۍ(ኑ.ጓ.ጡ);Ŗ=ɴ.ʳ;break;case э.ນ:if(!Ḕ(Ð,Ů.Ū)){return;}Ð.ۍ(ኑ.ጓ.ግ
);Ŗ=ɴ.ʳ;break;case э.ທ:if(!Ḕ(Ð,Ů.ũ)){return;}Ð.ۍ(ኑ.ጓ.ጌ);Ŗ=ɴ.ʳ;break;case э.ຖ:if(!Ḕ(Ð,Ů.Ũ)){return;}Ð.ۍ(ኑ.ጓ.ዄ);Ŗ=ɴ.ʳ;break
;case э.ຕ:if((ā.ᘾ&ళ.ᘌ)!=0){if(!ḃ(Ð,ᓩ.З,0)){return;}}else{if(!ḃ(Ð,ᓩ.З,1)){return;}}Ð.ۍ(ኑ.ጓ.ዚ);break;case э.ດ:if(!ḃ(Ð,ᓩ.З,5
)){return;}Ð.ۍ(ኑ.ጓ.ዛ);break;case э.ຍ:if(!ḃ(Ð,ᓩ.પ,1)){return;}Ð.ۍ(ኑ.ጓ.ዜ);break;case э.ຊ:if(!ḃ(Ð,ᓩ.પ,5)){return;}Ð.ۍ(ኑ.ጓ.ዝ)
;break;case э.ຈ:if(!ḃ(Ð,ᓩ.ظ,1)){return;}Ð.ۍ(ኑ.ጓ.ዞ);break;case э.ງ:if(!ḃ(Ð,ᓩ.ظ,5)){return;}Ð.ۍ(ኑ.ጓ.ዠ);break;case э.ส:if(!ḃ
(Ð,ᓩ.ᓪ,1)){return;}Ð.ۍ(ኑ.ጓ.ዧ);break;case э.ར:if(!ḃ(Ð,ᓩ.ᓪ,5)){return;}Ð.ۍ(ኑ.ጓ.ዡ);break;case э.ཨ:if(!Ð.Ɛ){for(var Ä=0;Ä<(
int)ᓩ.ŏ;Ä++){Ð.Ƌ[Ä]*=2;}Ð.Ɛ=true;}for(var Ä=0;Ä<(int)ᓩ.ŏ;Ä++){ḃ(Ð,(ᓩ)Ä,1);}Ð.ۍ(ኑ.ጓ.ዢ);break;case э.သ:if(!ᶾ(Ð,ਖ਼.Ϻ,false)){
return;}Ð.ۍ(ኑ.ጓ.ዣ);Ŗ=ɴ.ȟ;break;case э.ဟ:if(!ᶾ(Ð,ਖ਼.Ѝ,(ā.ᘾ&ళ.ᘌ)!=0)){return;}Ð.ۍ(ኑ.ጓ.ዤ);Ŗ=ɴ.ȟ;break;case э.ဠ:if(!ᶾ(Ð,ਖ਼.ନ,false))
{return;}Ð.ۍ(ኑ.ጓ.ዥ);Ŗ=ɴ.ȟ;break;case э.အ:if(!ᶾ(Ð,ਖ਼.પ,false)){return;}Ð.ۍ(ኑ.ጓ.ዦ);Ŗ=ɴ.ȟ;break;case э.ဢ:if(!ᶾ(Ð,ਖ਼.Ϲ,false)){
return;}Ð.ۍ(ኑ.ጓ.የ);Ŗ=ɴ.ȟ;break;case э.ဣ:if(!ᶾ(Ð,ਖ਼.Љ,(ā.ᘾ&ళ.ᘌ)!=0)){return;}Ð.ۍ(ኑ.ጓ.ዖ);Ŗ=ɴ.ȟ;break;case э.ဤ:if(!ᶾ(Ð,ਖ਼.ପ,(ā.ᘾ&ళ.
ᘌ)!=0)){return;}Ð.ۍ(ኑ.ጓ.ዕ);Ŗ=ɴ.ȟ;break;default:throw new Exception("Unknown gettable thing!");}if((ā.ᘾ&ళ.ᘆ)!=0){Ð.ź++;}l.
ળ.ᆖ(ā);Ð.ư+=ᶿ;if(Ð==l.ଙ){l.ૐ(Ð.Ɠ,Ŗ,ʡ.ʝ);}}}interface Ṇ:IReadOnlyList<ථ>{int ᮾ(string Ĭ);ථ this[string Ĭ]{get;}int[]ṇ{get;
}}interface Ṉ{void ṃ(థ Ƥ);void ʕ();void Ṃ();void ṁ();int Ṁ{get;}int ḿ{get;set;}}interface Ḿ{void Ǌ(ጟ ǋ,Ꮖ č);void ƾ();bool
ḽ();int ǜ{get;}int ǝ{get;set;}bool Ǟ{get;set;}int ǟ{get;}int Ǡ{get;set;}int ǚ{get;}int Ǜ{get;}}class Ṑ{MyGridProgram ᵽ=
null;ḛ ᓐ=null;bool ṑ=false;public Ṑ(MyGridProgram ᵮ,ḛ ᵭ,bool ṕ){ᵽ=ᵮ;ᓐ=ᵭ;}public List<IMyTerminalBlock>Ṓ(String Ā){List<
IMyTerminalBlock>ṓ=new List<IMyTerminalBlock>();ᵽ.GridTerminalSystem.GetBlocksOfType(ṓ,(IMyTerminalBlock ǃ)=>((ǃ.CustomName!=null)&&(ǃ.
CustomName.ToUpper().IndexOf("["+Ā.ToUpper()+"]")>=0)&&(ǃ is IMyShipController)));ᓐ.Ḗ("Found "+ṓ.Count+" controllers with tag "+Ā)
;return ṓ;}public bool Ṕ(IMyShipController ḕ){return ḕ.IsUnderControl;}public bool Ṗ(IMyShipController ḕ,bool ṏ){bool ṋ=
false;Vector3 Ḝ=ḕ.MoveIndicator;if(Ḝ.X!=0||(ṏ&&Ḝ.Y!=0)||Ḝ.Z!=0){ṋ=true;}return ṋ;}public bool ṉ(IMyShipController ḕ){Vector3
Ḝ=ḕ.MoveIndicator;if(ṑ&&Ḝ.X<0&&Ḝ.Y==0&&Ḝ.Z==0)return true;else if(!ṑ&&Ḝ.X<0)return true;return false;}public bool Ṋ(
IMyShipController ḕ){Vector3 Ḝ=ḕ.MoveIndicator;if(ṑ&&Ḝ.X>0&&Ḝ.Y==0&&Ḝ.Z==0)return true;else if(!ṑ&&Ḝ.X>0)return true;return false;}public
bool Ṏ(IMyShipController ḕ){Vector3 Ḝ=ḕ.MoveIndicator;if(ṑ&&Ḝ.X==0&&Ḝ.Y==0&&Ḝ.Z<0)return true;else if(!ṑ&&Ḝ.Z<0)return true;
return false;}public bool Ṍ(IMyShipController ḕ){Vector3 Ḝ=ḕ.MoveIndicator;if(ṑ&&Ḝ.X==0&&Ḝ.Y==0&&Ḝ.Z>0)return true;else if(!ṑ
&&Ḝ.Z>0)return true;return false;}public bool ṍ(IMyShipController ḕ){Vector3 Ḝ=ḕ.MoveIndicator;if(ṑ&&Ḝ.X==0&&Ḝ.Y>0&&Ḝ.Z==0
)return true;else if(!ṑ&&Ḝ.Y>0)return true;return false;}public bool Ḟ(IMyShipController ḕ){Vector3 Ḝ=ḕ.MoveIndicator;if(
ṑ&&Ḝ.X==0&&Ḝ.Y<0&&Ḝ.Z==0)return true;else if(!ṑ&&Ḝ.Y<0)return true;return false;}public bool ḟ(IMyShipController ḕ){float
Ḝ=ḕ.RollIndicator;if(Ḝ<0.0)return true;return false;}public bool Ḡ(IMyShipController ḕ){float Ḝ=ḕ.RollIndicator;if(Ḝ>0.0)
return true;return false;}public bool ḣ(IMyShipController ḕ){Vector2 Ḝ=ḕ.RotationIndicator;if(ṑ&&Ḝ.X==0&&Ḝ.Y<0)return true;
else if(!ṑ&&Ḝ.Y<0)return true;return false;}public bool ḡ(IMyShipController ḕ){Vector2 Ḝ=ḕ.RotationIndicator;if(ṑ&&Ḝ.X==0&&Ḝ
.Y>0)return true;else if(!ṑ&&Ḝ.Y>0)return true;return false;}public bool Ḣ(IMyShipController ḕ){Vector2 Ḝ=ḕ.
RotationIndicator;if(ṑ&&Ḝ.X>0&&Ḝ.Y==0)return true;else if(!ṑ&&Ḝ.X>0)return true;return false;}public bool ḝ(IMyShipController ḕ){Vector2
Ḝ=ḕ.RotationIndicator;if(ṑ&&Ḝ.X<0&&Ḝ.Y==0)return true;else if(!ṑ&&Ḝ.X<0)return true;return false;}}class ḛ{public bool ᓎ=
false;private MyGridProgram ᵽ=null;private ᵼ ᓑ=null;private bool Ḛ=false;private static List<IMyTerminalBlock>ḙ=null;public ḛ
(MyGridProgram ᵮ,bool Ḙ){ᵽ=ᵮ;ᓎ=Ḙ;ᓑ=new ᵼ(ᵮ,this,false);}public void ḗ(string མ){ᵽ.Echo("JDBG: "+མ);}public void Ḗ(String
མ){Ḗ(མ,true);}public void Ḗ(String མ,bool ḱ){if(ᓎ&&!Ḛ){Ḛ=true;if(ḙ==null){ḗ("First run - working out debug panels");ḳ();Ḵ
();}ḗ("D:"+མ);ᓑ.ᵛ(ḙ,མ+"\n",true);Ḛ=false;}}public void Ḳ(String མ){ḗ(མ);Ḗ(མ,false);}private void ḳ(){Ḛ=true;ḙ=ᓑ.ᵻ("DEBUG"
);ᓑ.ᶅ(ḙ,TextAlignment.LEFT);Ḛ=false;}public void Ḵ(){if(ᓎ){if(ḙ==null){ḗ("First runC - working out debug panels");ḳ();}ᓑ.
ᵛ(ḙ,"",false);}}public void ḵ(String Ḷ,String ᶋ,String ḻ,String ḷ){List<IMyTerminalBlock>Ḹ=new List<IMyTerminalBlock>();ᵽ
.GridTerminalSystem.GetBlocksOfType(Ḹ,(IMyTerminalBlock ǃ)=>((ǃ.CustomName!=null)&&(ǃ.CustomName.IndexOf("["+ḻ+"]")>=0)&&
(ǃ is IMyTextSurfaceProvider)));Ḳ("Found "+Ḹ.Count+" lcds with '"+ḻ+"' to alert to");String ḹ=ᵼ.ᵹ[ᶋ]+" "+DateTime.Now.
ToShortTimeString()+":"+ḷ+" "+Ḷ+"\n";Ḳ("ALERT: "+Ḷ);if(Ḹ.Count>0){ᓑ.ᵛ(Ḹ,ḹ,true);}}public void Ḻ(String Ā){ḗ(Ā+" instruction count: "+ᵽ.
Runtime.CurrentInstructionCount+","+ᵽ.Runtime.CurrentCallChainDepth);}public void Ḽ(){ḗ("Max instruction count: "+ᵽ.Runtime.
MaxInstructionCount+","+ᵽ.Runtime.MaxCallChainDepth);}}class Ḱ{private ḛ ᓐ=null;public enum ḯ{Ḯ,ḭ,ດ,Ḭ,ḫ,Ḫ};Dictionary<String,String>ḩ=new
Dictionary<String,String>{{"MyObjectBuilder_Ore/Cobalt","MyObjectBuilder_Ingot/Cobalt"},{"MyObjectBuilder_Ore/Gold",
"MyObjectBuilder_Ingot/Gold"},{"MyObjectBuilder_Ore/Stone","MyObjectBuilder_Ingot/Stone"},{"MyObjectBuilder_Ore/Iron","MyObjectBuilder_Ingot/Iron"},
{"MyObjectBuilder_Ore/Magnesium","MyObjectBuilder_Ingot/Magnesium"},{"MyObjectBuilder_Ore/Nickel",
"MyObjectBuilder_Ingot/Nickel"},{"MyObjectBuilder_Ore/Platinum","MyObjectBuilder_Ingot/Platinum"},{"MyObjectBuilder_Ore/Silicon",
"MyObjectBuilder_Ingot/Silicon"},{"MyObjectBuilder_Ore/Silver","MyObjectBuilder_Ingot/Silver"},{"MyObjectBuilder_Ore/Uranium",
"MyObjectBuilder_Ingot/Uranium"},};Dictionary<String,String>Ḩ=new Dictionary<String,String>{{"MyObjectBuilder_BlueprintDefinition/Position0040_Datapad"
,"MyObjectBuilder_Datapad/Datapad"},};Dictionary<String,String>ḧ=new Dictionary<String,String>{{
"MyObjectBuilder_PhysicalGunObject/AngleGrinderItem","MyObjectBuilder_BlueprintDefinition/Position0010_AngleGrinder"},{"MyObjectBuilder_PhysicalGunObject/AngleGrinder2Item"
,"MyObjectBuilder_BlueprintDefinition/Position0020_AngleGrinder2"},{"MyObjectBuilder_PhysicalGunObject/AngleGrinder3Item"
,"MyObjectBuilder_BlueprintDefinition/Position0030_AngleGrinder3"},{"MyObjectBuilder_PhysicalGunObject/AngleGrinder4Item"
,"MyObjectBuilder_BlueprintDefinition/Position0040_AngleGrinder4"},{"MyObjectBuilder_PhysicalGunObject/WelderItem",
"MyObjectBuilder_BlueprintDefinition/Position0090_Welder"},{"MyObjectBuilder_PhysicalGunObject/Welder2Item","MyObjectBuilder_BlueprintDefinition/Position0100_Welder2"},{
"MyObjectBuilder_PhysicalGunObject/Welder3Item","MyObjectBuilder_BlueprintDefinition/Position0110_Welder3"},{"MyObjectBuilder_PhysicalGunObject/Welder4Item",
"MyObjectBuilder_BlueprintDefinition/Position0120_Welder4"},{"MyObjectBuilder_PhysicalGunObject/HandDrillItem","MyObjectBuilder_BlueprintDefinition/Position0050_HandDrill"},{
"MyObjectBuilder_PhysicalGunObject/HandDrill2Item","MyObjectBuilder_BlueprintDefinition/Position0060_HandDrill2"},{"MyObjectBuilder_PhysicalGunObject/HandDrill3Item",
"MyObjectBuilder_BlueprintDefinition/Position0070_HandDrill3"},{"MyObjectBuilder_PhysicalGunObject/HandDrill4Item","MyObjectBuilder_BlueprintDefinition/Position0080_HandDrill4"},};
Dictionary<String,String>Ḧ=new Dictionary<String,String>{{"MyObjectBuilder_GasContainerObject/HydrogenBottle",
"MyObjectBuilder_BlueprintDefinition/Position0020_HydrogenBottle"},{"MyObjectBuilder_OxygenContainerObject/OxygenBottle","MyObjectBuilder_BlueprintDefinition/HydrogenBottlesRefill"},};
Dictionary<String,String>ḥ=new Dictionary<String,String>{{"myobjectbuilder_component/bulletproofglass",
"myobjectbuilder_blueprintdefinition/bulletproofglass"},{"myobjectbuilder_component/canvas","myobjectbuilder_blueprintdefinition/position0030_canvas"},{
"myobjectbuilder_component/computer","myobjectbuilder_blueprintdefinition/computercomponent"},{"myobjectbuilder_component/construction",
"myobjectbuilder_blueprintdefinition/constructioncomponent"},{"myobjectbuilder_component/detector","myobjectbuilder_blueprintdefinition/detectorcomponent"},{
"myobjectbuilder_component/display","myobjectbuilder_blueprintdefinition/display"},{"myobjectbuilder_component/explosives",
"myobjectbuilder_blueprintdefinition/explosivescomponent"},{"myobjectbuilder_component/girder","myobjectbuilder_blueprintdefinition/girdercomponent"},{
"myobjectbuilder_component/gravitygenerator","myobjectbuilder_blueprintdefinition/gravitygeneratorcomponent"},{"myobjectbuilder_component/interiorplate",
"myobjectbuilder_blueprintdefinition/interiorplate"},{"myobjectbuilder_component/largetube","myobjectbuilder_blueprintdefinition/largetube"},{
"myobjectbuilder_component/medical","myobjectbuilder_blueprintdefinition/medicalcomponent"},{"myobjectbuilder_component/metalgrid",
"myobjectbuilder_blueprintdefinition/metalgrid"},{"myobjectbuilder_component/motor","myobjectbuilder_blueprintdefinition/motorcomponent"},{
"myobjectbuilder_component/powercell","myobjectbuilder_blueprintdefinition/powercell"},{"myobjectbuilder_component/reactor",
"myobjectbuilder_blueprintdefinition/reactorcomponent"},{"myobjectbuilder_component/radiocommunication","myobjectbuilder_blueprintdefinition/radiocommunicationcomponent"},{
"myobjectbuilder_component/smalltube","myobjectbuilder_blueprintdefinition/smalltube"},{"myobjectbuilder_component/solarcell",
"myobjectbuilder_blueprintdefinition/solarcell"},{"myobjectbuilder_component/steelplate","myobjectbuilder_blueprintdefinition/steelplate"},{
"myobjectbuilder_component/superconductor","myobjectbuilder_blueprintdefinition/superconductor"},{"myobjectbuilder_component/thrust",
"myobjectbuilder_blueprintdefinition/thrustcomponent"},};Dictionary<String,String>Ḥ=new Dictionary<String,String>{{"MyObjectBuilder_AmmoMagazine/NATO_25x184mm",
"MyObjectBuilder_BlueprintDefinition/Position0080_NATO_25x184mmMagazine"},{"MyObjectBuilder_AmmoMagazine/AutocannonClip","MyObjectBuilder_BlueprintDefinition/Position0090_AutocannonClip"},{
"MyObjectBuilder_AmmoMagazine/Missile200mm","MyObjectBuilder_BlueprintDefinition/Position0100_Missile200mm"},{"MyObjectBuilder_AmmoMagazine/MediumCalibreAmmo",
"MyObjectBuilder_BlueprintDefinition/Position0110_MediumCalibreAmmo"},{"MyObjectBuilder_AmmoMagazine/LargeCalibreAmmo","MyObjectBuilder_BlueprintDefinition/Position0120_LargeCalibreAmmo"},
{"MyObjectBuilder_AmmoMagazine/SmallRailgunAmmo","MyObjectBuilder_BlueprintDefinition/Position0130_SmallRailgunAmmo"},{
"MyObjectBuilder_AmmoMagazine/LargeRailgunAmmo","MyObjectBuilder_BlueprintDefinition/Position0140_LargeRailgunAmmo"},{
"MyObjectBuilder_AmmoMagazine/SemiAutoPistolMagazine","MyObjectBuilder_BlueprintDefinition/Position0010_SemiAutoPistolMagazine"},{
"MyObjectBuilder_AmmoMagazine/ElitePistolMagazine","MyObjectBuilder_BlueprintDefinition/Position0030_ElitePistolMagazine"},{
"MyObjectBuilder_AmmoMagazine/FullAutoPistolMagazine","MyObjectBuilder_BlueprintDefinition/Position0020_FullAutoPistolMagazine"},{
"MyObjectBuilder_AmmoMagazine/AutomaticRifleGun_Mag_20rd","MyObjectBuilder_BlueprintDefinition/Position0040_AutomaticRifleGun_Mag_20rd"},{
"MyObjectBuilder_AmmoMagazine/UltimateAutomaticRifleGun_Mag_30rd","MyObjectBuilder_BlueprintDefinition/Position0070_UltimateAutomaticRifleGun_Mag_30rd"},{
"MyObjectBuilder_AmmoMagazine/RapidFireAutomaticRifleGun_Mag_50rd","MyObjectBuilder_BlueprintDefinition/Position0050_RapidFireAutomaticRifleGun_Mag_50rd"},{
"MyObjectBuilder_AmmoMagazine/PreciseAutomaticRifleGun_Mag_5rd","MyObjectBuilder_BlueprintDefinition/Position0060_PreciseAutomaticRifleGun_Mag_5rd"},{
"MyObjectBuilder_AmmoMagazine/NATO_5p56x45mm",null},};public Ḱ(ḛ ᵭ){ᓐ=ᵭ;}public void ᶧ(ḯ ᵺ,ref Dictionary<String,String>ᵡ){switch(ᵺ){case ḯ.Ḯ:ᵡ=ᵡ.Concat(Ḧ).
ToDictionary(ǃ=>ǃ.Key,ǃ=>ǃ.Value);break;case ḯ.ḭ:ᵡ=ᵡ.Concat(ḥ).ToDictionary(ǃ=>ǃ.Key,ǃ=>ǃ.Value);break;case ḯ.ດ:ᵡ=ᵡ.Concat(Ḥ).
ToDictionary(ǃ=>ǃ.Key,ǃ=>ǃ.Value);break;case ḯ.Ḭ:ᵡ=ᵡ.Concat(ḧ).ToDictionary(ǃ=>ǃ.Key,ǃ=>ǃ.Value);break;case ḯ.ḫ:ᵡ=ᵡ.Concat(Ḩ).
ToDictionary(ǃ=>ǃ.Key,ǃ=>ǃ.Value);break;case ḯ.Ḫ:ᵡ=ᵡ.Concat(ḩ).ToDictionary(ǃ=>ǃ.Key,ǃ=>ǃ.Value);break;}}}class ᵼ{public
MyGridProgram ᵽ=null;public ḛ ᓐ=null;bool ᵬ=false;public static Dictionary<String,char>ᵹ=new Dictionary<String,char>{{"YELLOW",''},{
"RED",''},{"ORANGE",''},{"GREEN",''},{"CYAN",''},{"PURPLE",''},{"BLUE",''},{"WHITE",''},{"BLACK",''},{"GREY",''}};
public static char ᵸ='';public static char ᵷ='';public static char ᵶ='';public static char ᵵ='';public static char ᵴ='';
public static char ᵳ='';public static char ᵲ='';public static char ᵱ='';public static char ᵰ='';public static char ᵯ='';
public ᵼ(MyGridProgram ᵮ,ḛ ᵭ,bool ᵬ){this.ᵽ=ᵮ;this.ᓐ=ᵭ;this.ᵬ=ᵬ;}public List<IMyTerminalBlock>ᵻ(String Ā){List<
IMyTerminalBlock>ᵠ=new List<IMyTerminalBlock>();ᵽ.GridTerminalSystem.GetBlocksOfType(ᵠ,(IMyTerminalBlock ǃ)=>((ǃ.CustomName!=null)&&(ǃ.
CustomName.ToUpper().IndexOf("["+Ā.ToUpper()+"]")>=0)&&(ǃ is IMyTextSurfaceProvider)));ᓐ.Ḗ("Found "+ᵠ.Count+
" lcds to update with tag "+Ā);return ᵠ;}public void ᶅ(List<IMyTerminalBlock>ᵠ,TextAlignment ᶆ){foreach(var ᵝ in ᵠ){ᓐ.Ḗ("Setting up the font for "+
ᵝ.CustomName);IMyTextSurface ᵞ=((IMyTextSurfaceProvider)ᵝ).GetSurface(0);if(ᵞ==null)continue;ᵞ.Font="Monospace";ᵞ.
ContentType=ContentType.TEXT_AND_IMAGE;ᵞ.BackgroundColor=Color.Black;ᵞ.Alignment=ᶆ;}}public void ᶇ(List<IMyTerminalBlock>ᵠ,Color ᶋ)
{foreach(var ᵝ in ᵠ){if(ᵝ is IMyTextPanel){ᓐ.Ḗ("Setting up the color for "+ᵝ.CustomName);((IMyTextPanel)ᵝ).FontColor=ᶋ;}}
}public void ᶈ(List<IMyTerminalBlock>ᵠ,float ᶉ){foreach(var ᵝ in ᵠ){if(ᵝ is IMyTextPanel){ᓐ.Ḗ(
"Setting up the rotation for "+ᵝ.CustomName);ᵝ.SetValueFloat("Rotate",ᶉ);}}}public void ᶊ(List<IMyTerminalBlock>ᵠ,int ᶂ,int ᶁ,bool ᶀ){ᶃ(ᵠ,ᶂ,ᶁ,ᶀ,0.05F,
0.05F);}public void ᶄ(List<IMyTerminalBlock>ᵠ,int ᶂ,int ᶁ,bool ᶀ,float ᄺ,float ᵕ){ᶃ(ᵠ,ᶂ,ᶁ,ᶀ,ᄺ,ᵕ);}public void ᶃ(List<
IMyTerminalBlock>ᵠ,int ᶂ,int ᶁ,bool ᶀ,float ᵿ,float ᵾ){foreach(var ᵝ in ᵠ){ᓐ.Ḗ("Setting up font on screen: "+ᵝ.CustomName+" ("+ᶂ+" x "+ᶁ
+")");IMyTextSurface ᵞ=((IMyTextSurfaceProvider)ᵝ).GetSurface(0);if(ᵞ==null)continue;float ᄺ=ᵿ;float ᵕ=ᵾ;StringBuilder ᵖ=
new StringBuilder("".PadRight(ᶁ,(ᶀ?ᵹ["BLACK"]:'W')));Vector2 ᵗ=ᵞ.TextureSize;while(true){ᵞ.FontSize=ᄺ;Vector2 ᵘ=ᵞ.
TextureSize;Vector2 ᵙ=ᵞ.MeasureStringInPixels(ᵖ,ᵞ.Font,ᄺ);int ᵚ=(int)Math.Floor(ᵗ.Y/ᵙ.Y);if((ᵙ.X<ᵘ.X)&&(ᵚ>ᶂ)){ᄺ+=ᵕ;}else{break;}}ᵞ.
FontSize=ᄺ-ᵕ;ᓐ.Ḗ("Calc size of "+ᵞ.FontSize);if(ᵝ.DefinitionDisplayNameText.Contains("Corner LCD")){ᓐ.Ḗ(
"INFO: Avoiding bug, multiplying by 4: "+ᵝ.DefinitionDisplayNameText);ᵞ.FontSize*=4;}}}public void ᵛ(List<IMyTerminalBlock>ᵠ,String ᕕ,bool ᵜ){foreach(var ᵝ in ᵠ
){if(!this.ᵬ)ᓐ.Ḗ("Writing to display "+ᵝ.CustomName);IMyTextSurface ᵞ=((IMyTextSurfaceProvider)ᵝ).GetSurface(0);if(ᵞ==
null)continue;ᵞ.WriteText(ᕕ,ᵜ);}}public char ᵟ(byte ڬ,byte ڭ,byte Â){const double ᵓ=255.0/7.0;return(char)(0xe100+((int)Math
.Round(ڬ/ᵓ)<<6)+((int)Math.Round(ڭ/ᵓ)<<3)+(int)Math.Round(Â/ᵓ));}}sealed class ᵒ{public static ᵒ ᓍ=new ᵒ();private ኈ[]എ;
private ᐆ[]ᵑ;private ᵒ(){എ=Array.Empty<ኈ>();ᵑ=Array.Empty<ᐆ>();}public ᵒ(IReadOnlyList<ኈ>എ){this.എ=എ.ToArray();this.ᵑ=Array.
Empty<ᐆ>();}public ᵒ(IReadOnlyList<ኈ>എ,IReadOnlyList<ᐆ>ᵑ){this.എ=എ.ToArray();this.ᵑ=ᵑ.ToArray();}public override string
ToString(){var ᵐ=എ.Select(ብ=>ቨ.ቦ(ብ));var ᵏ=ᵑ.Select(Ᏻ=>Ᏼ.ቦ(Ᏻ));var ᖙ=ᵐ.Concat(ᵏ).ToArray();if(ᖙ.Length>0){return string.Join(
", ",ᖙ);}else{return"none";}}public static ᵒ ቤ(string u){if(u=="none"){return ᓍ;}var എ=new List<ኈ>();var ᵑ=new List<ᐆ>();var
ᗾ=u.Split(',').Select(ǃ=>ǃ.Trim());foreach(var ᵧ in ᗾ){var ብ=ቨ.ቤ(ᵧ);if(ብ!=ኈ.ኇ){എ.Add(ብ);continue;}var ᵨ=Ᏼ.ቤ(ᵧ);if(ᵨ!=ᐆ.ኇ)
{ᵑ.Add(ᵨ);}}return new ᵒ(എ,ᵑ);}public IReadOnlyList<ኈ>ഖ=>എ;public IReadOnlyList<ᐆ>ᵪ=>ᵑ;}sealed class ᵩ:Ⴞ{private ଦ l;
private Ŀ ô;private int ú;private int ฎ;private int ญ;private int ᵫ;private int ᵦ;public ᵩ(ଦ l){this.l=l;}public override void
Ⴛ(){if(--ú>0){return;}if(ô.Ħ==ฎ){ô.Ħ=ญ;ú=(l.ષ.ѐ()&ᵦ)+1;}else{ô.Ħ=ฎ;ú=(l.ષ.ѐ()&ᵫ)+1;}}public Ŀ Ŀ{get{return ô;}set{ô=value
;}}public int ŏ{get{return ú;}set{ú=value;}}public int ฆ{get{return ฎ;}set{ฎ=value;}}public int ง{get{return ญ;}set{ญ=
value;}}public int ᵥ{get{return ᵫ;}set{ᵫ=value;}}public int ᵤ{get{return ᵦ;}set{ᵦ=value;}}}sealed class ଋ{private ଦ l;public
ଋ(ଦ l){this.l=l;}public void ᵣ(Ŀ ô){ô.Ě=0;var ᵢ=new Ꮛ(l);l.વ.ᄩ(ᵢ);ᵢ.Ŀ=ô;ᵢ.ฆ=ô.Ħ;ᵢ.ง=ᶤ(ô,ô.Ħ)+16;ᵢ.ŏ=4;}public void ᵔ(Ŀ ô)
{ô.Ě=0;var ᶕ=new ᵩ(l);l.વ.ᄩ(ᶕ);ᶕ.Ŀ=ô;ᶕ.ฆ=ô.Ħ;ᶕ.ง=ᶤ(ô,ô.Ħ);ᶕ.ᵥ=64;ᶕ.ᵤ=7;ᶕ.ŏ=(l.ષ.ѐ()&ᶕ.ᵥ)+1;}public void ᶠ(Ŀ ô,int Ŧ,bool
ᶡ){var Ô=new ฉ(l);l.વ.ᄩ(Ô);Ô.Ŀ=ô;Ô.ฅ=Ŧ;Ô.ค=ฉ.ช;Ô.ฆ=ô.Ħ;Ô.ง=ᶤ(ô,ô.Ħ);if(Ô.ง==Ô.ฆ){Ô.ง=0;}ô.Ě=0;if(ᶡ){Ô.ŏ=1;}else{Ô.ŏ=(l.ષ.
ѐ()&7)+1;}}public void ᶢ(Ŀ ô){var ᶣ=new ᯗ(l);l.વ.ᄩ(ᶣ);ᶣ.Ŀ=ô;ᶣ.ง=ᶤ(ô,ô.Ħ);ᶣ.ฆ=ô.Ħ;ᶣ.ಣ=-1;ô.Ě=0;}private int ᶤ(Ŀ ô,int ᕋ){
var ː=ᕋ;for(var Ä=0;Ä<ô.đ.Length;Ä++){var ò=ô.đ[Ä];var ˠ=ΐ(ò,ô);if(ˠ==null){continue;}if(ˠ.Ħ<ː){ː=ˠ.Ħ;}}return ː;}private Ŀ
ΐ(ɢ ò,Ŀ ô){if((ò.ᘾ&ᶘ.ᶔ)==0){return null;}if(ò.ɣ==ô){return ò.ɤ;}return ò.ɣ;}}sealed class ɢ{private static int ľ=14;
private ட ζ;private ட θ;private Ꮖ ߝ;private Ꮖ Ǆ;private ᶘ ܠ;private ᶍ ā;private short Ā;private ɦ ʹ;private ɦ ͳ;private Ꮖ[]ߺ;
private ʐ ᶟ;private Ŀ π;private Ŀ ρ;private int Ĺ;private Ⴞ ķ;private Ɠ ĺ;public ɢ(ட ζ,ட θ,ᶘ ܠ,ᶍ ā,short Ā,ɦ ʹ,ɦ ͳ){this.ζ=ζ;
this.θ=θ;this.ܠ=ܠ;this.ā=ā;this.Ā=Ā;this.ʹ=ʹ;this.ͳ=ͳ;ߝ=θ.ޚ-ζ.ޚ;Ǆ=θ.ޙ-ζ.ޙ;if(ߝ==Ꮖ.Ꮓ){ᶟ=ʐ.ʎ;}else if(Ǆ==Ꮖ.Ꮓ){ᶟ=ʐ.ʏ;}else{if(Ǆ
/ߝ>Ꮖ.Ꮓ){ᶟ=ʐ.ʍ;}else{ᶟ=ʐ.ʌ;}}ߺ=new Ꮖ[4];ߺ[ᒚ.Ꮀ]=Ꮖ.Ꮅ(ζ.ޙ,θ.ޙ);ߺ[ᒚ.Ꭿ]=Ꮖ.Ꮆ(ζ.ޙ,θ.ޙ);ߺ[ᒚ.ቀ]=Ꮖ.Ꮆ(ζ.ޚ,θ.ޚ);ߺ[ᒚ.ሿ]=Ꮖ.Ꮅ(ζ.ޚ,θ.ޚ);π=
ʹ?.Ŀ;ρ=ͳ?.Ŀ;}public static ɢ ċ(byte[]f,int ù,ட[]ι,ɦ[]ʼ){var ξ=BitConverter.ToInt16(f,ù);var ν=BitConverter.ToInt16(f,ù+2)
;var ܠ=BitConverter.ToInt16(f,ù+4);var ā=BitConverter.ToInt16(f,ù+6);var Ā=BitConverter.ToInt16(f,ù+8);var ᶦ=BitConverter
.ToInt16(f,ù+10);var ᶥ=BitConverter.ToInt16(f,ù+12);return new ɢ(ι[ξ],ι[ν],(ᶘ)ܠ,(ᶍ)ā,Ā,ʼ[ᶦ],ᶥ!=-1?ʼ[ᶥ]:null);}public
static ɢ[]ÿ(ಆ þ,int ý,ட[]ι,ɦ[]ʼ){var û=þ.ಡ(ý);if(û%ľ!=0){throw new Exception();}var f=þ.ಠ(ý);var ú=û/ľ;var ß=new ɢ[ú];;for(var
Ä=0;Ä<ú;Ä++){var ù=14*Ä;ß[Ä]=ċ(f,ù,ι,ʼ);}return ß;}public ட ɝ=>ζ;public ட ɞ=>θ;public Ꮖ ޘ=>ߝ;public Ꮖ ޗ=>Ǆ;public ᶘ ᘾ{get
{return ܠ;}set{ܠ=value;}}public ᶍ Ě{get{return ā;}set{ā=value;}}public short ę{get{return Ā;}set{Ā=value;}}public ɦ ᶖ=>ʹ;
public ɦ ᶗ=>ͳ;public Ꮖ[]ޖ=>ߺ;public ʐ ʐ=>ᶟ;public Ŀ ɣ=>π;public Ŀ ɤ=>ρ;public int Ĕ{get{return Ĺ;}set{Ĺ=value;}}public Ⴞ Ē{get
{return ķ;}set{ķ=value;}}public Ɠ ĕ{get{return ĺ;}set{ĺ=value;}}}[Flags]public enum ᶘ{ᶙ=1,ᶚ=2,ᶔ=4,ᶓ=8,ᶒ=16,ᶑ=32,ᶐ=64,ᶏ=
128,ᶎ=256}public enum ᶍ{Ϊ=0}sealed class ᶌ:ᜮ{private string[]Ĭ;private int[]ī;private int[]Ī;private ท[]Ĩ;private int ı;
private ท İ;public ᶌ(ባ ĭ,string Ĭ,int ī,int Ī,int ĩ,params ท[]Ĩ):base(ĭ){this.Ĭ=new[]{Ĭ};this.ī=new[]{ī};this.Ī=new[]{Ī};this.Ĩ
=Ĩ;ı=ĩ;İ=Ĩ[ı];}public override void ರ(){for(var Ä=0;Ä<Ĩ.Length;Ä++){Ĩ[Ä].ฑ(ኘ.ŉ[Ä]);}}private void ĳ(){ı--;if(ı<0){ı=Ĩ.
Length-1;}İ=Ĩ[ı];}private void Ķ(){ı++;if(ı>=Ĩ.Length){ı=0;}İ=Ĩ[ı];}public override bool ଅ(ᕨ Ň){if(Ň.ܫ!=ፎ.ፏ){return true;}if(Ň
.ᕤ==ኈ.ĳ){ĳ();ኘ.ૐ(ɴ.Ȩ);}if(Ň.ᕤ==ኈ.Ķ){Ķ();ኘ.ૐ(ɴ.Ȩ);}if(Ň.ᕤ==ኈ.ቔ){if(ᶝ(ı)){ኘ.ಯ();}ኘ.ૐ(ɴ.ɲ);}if(Ň.ᕤ==ኈ.ኡ){ኘ.ಯ();ኘ.ૐ(ɴ.ȭ);}
return true;}public bool ᶝ(int ł){if(ኘ.ŉ[ł]!=null){ኘ.ጟ.Î(ł);return true;}else{return false;}}public IReadOnlyList<string>Ń=>Ĭ;
public IReadOnlyList<int>ń=>ī;public IReadOnlyList<int>Ņ=>Ī;public IReadOnlyList<ᜯ>ņ=>Ĩ;public ᜯ ň=>İ;}sealed class ᶞ{public
const int ᅁ=16;private string Ĭ;public byte[]ᶜ;private int ť;private int ᄺ;public ᶞ(string Ĭ,byte[]f,int ť,int ᄺ){this.Ĭ=Ĭ;
this.ᶜ=f;this.ť=ť;this.ᄺ=ᄺ;}public string Ń=>Ĭ;public int ᒪ=>ť;public int ᶛ=>ᄺ;}sealed class શ{private Ṇ ʽ;private ᮟ ü;
private ᅗ ᛜ;private ଦ l;private ட[]ι;private Ŀ[]Þ;private ɦ[]ʼ;private ɢ[]ß;private Ω[]ɜ;private ฃ[]ߍ;private ߵ[]ޞ;private ᛔ[]ᛓ
;private ᑋ ᛝ;private Ǭ ᛠ;private ථ ᛞ;private string ᛟ;public શ(ᴍ ଳ,ଦ l):this(ଳ.ಆ,ଳ.ᜁ,ଳ.ᛧ,ଳ.ᒳ,l){}public શ(ಆ þ,Ṇ ʽ,ᮟ ü,ᅗ ᛜ
,ଦ l){try{this.ʽ=ʽ;this.ü=ü;this.ᛜ=ᛜ;this.l=l;var Å=l.હ;string Ĭ;if(þ.ಖ==ಖ.ᴉ){Ĭ="MAP"+Å.શ.ToString("00");}else{Ĭ="E"+Å.ᰔ+
"M"+Å.શ;}ᕣ.ᔵ.འ("Load map '"+Ĭ+"': ");var ࠎ=þ.ಞ(Ĭ);if(ࠎ==-1){throw new Exception("Map '"+Ĭ+"' was not found!");}ι=ட.ÿ(þ,ࠎ+4)
;Þ=Ŀ.ÿ(þ,ࠎ+8,ü);ʼ=ɦ.ÿ(þ,ࠎ+3,ʽ,Þ);ß=ɢ.ÿ(þ,ࠎ+2,ι,ʼ);ɜ=Ω.ÿ(þ,ࠎ+5,ι,ß);ߍ=ฃ.ÿ(þ,ࠎ+6,ɜ);ޞ=ߵ.ÿ(þ,ࠎ+7,ߍ);ᛓ=ᛔ.ÿ(þ,ࠎ+1);ᛝ=ᑋ.ÿ(þ,ࠎ+
10,ß);ᛠ=Ǭ.ÿ(þ,ࠎ+9,Þ);ᛛ();ᛞ=ᛗ(Ĭ);if(Å.ಖ==ಖ.ᴉ){switch(Å.ಕ){case ಕ.ᕩ:ᛟ=ኑ.ᕪ.ᕩ[Å.શ-1];break;case ಕ.ᕹ:ᛟ=ኑ.ᕪ.ᕹ[Å.શ-1];break;
default:ᛟ=ኑ.ᕪ.ኾ[Å.શ-1];break;}}else{ᛟ=ኑ.ᕪ.ጟ[Å.ᰔ-1][Å.શ-1];}ᕣ.ᔵ.འ("OK");}catch(Exception e){ᕣ.ᔵ.འ("Failed");throw e;}}private
void ᛛ(){var ᛚ=new List<ɢ>();var ߺ=new Ꮖ[4];foreach(var ò in ß){if(ò.Ě!=0){var ᛙ=new Ɠ(l);ᛙ.ޚ=(ò.ɝ.ޚ+ò.ɞ.ޚ)/2;ᛙ.ޙ=(ò.ɝ.ޙ+ò.ɞ
.ޙ)/2;ò.ĕ=ᛙ;}}foreach(var ô in Þ){ᛚ.Clear();ᒚ.ŷ(ߺ);foreach(var ò in ß){if(ò.ɣ==ô||ò.ɤ==ô){ᛚ.Add(ò);ᒚ.ᒞ(ߺ,ò.ɝ.ޚ,ò.ɝ.ޙ);ᒚ.ᒞ
(ߺ,ò.ɞ.ޚ,ò.ɞ.ޙ);}}ô.đ=ᛚ.ToArray();ô.ĕ=new Ɠ(l);ô.ĕ.ޚ=(ߺ[ᒚ.ሿ]+ߺ[ᒚ.ቀ])/2;ô.ĕ.ޙ=(ߺ[ᒚ.Ꮀ]+ߺ[ᒚ.Ꭿ])/2;ô.Ė=new int[4];int ᛘ;ᛘ=(ߺ[
ᒚ.Ꮀ]-ᛝ.ᅍ+ᱺ.ᴂ).Ꮁ>>ᑋ.ᑙ;ᛘ=ᛘ>=ᛝ.ǡ?ᛝ.ǡ-1:ᛘ;ô.Ė[ᒚ.Ꮀ]=ᛘ;ᛘ=(ߺ[ᒚ.Ꭿ]-ᛝ.ᅍ-ᱺ.ᴂ).Ꮁ>>ᑋ.ᑙ;ᛘ=ᛘ<0?0:ᛘ;ô.Ė[ᒚ.Ꭿ]=ᛘ;ᛘ=(ߺ[ᒚ.ሿ]-ᛝ.ᅌ+ᱺ.ᴂ).Ꮁ>>ᑋ.ᑙ
;ᛘ=ᛘ>=ᛝ.Ǒ?ᛝ.Ǒ-1:ᛘ;ô.Ė[ᒚ.ሿ]=ᛘ;ᛘ=(ߺ[ᒚ.ቀ]-ᛝ.ᅌ-ᱺ.ᴂ).Ꮁ>>ᑋ.ᑙ;ᛘ=ᛘ<0?0:ᛘ;ô.Ė[ᒚ.ቀ]=ᛘ;}}private ථ ᛗ(string Ĭ){if(Ĭ.Length==4){
switch(Ĭ[1]){case'1':return ʽ["SKY1"];case'2':return ʽ["SKY2"];case'3':return ʽ["SKY3"];default:return ʽ["SKY4"];}}else{var Ć=
int.Parse(Ĭ.Substring(3));if(Ć<=11){return ʽ["SKY1"];}else if(Ć<=21){return ʽ["SKY2"];}else{return ʽ["SKY3"];}}}public Ṇ ᜁ
=>ʽ;public ᮟ ᛧ=>ü;public ᅗ ᒳ=>ᛜ;public ட[]ᛨ=>ι;public Ŀ[]ᛩ=>Þ;public ɦ[]ᛪ=>ʼ;public ɢ[]đ=>ß;public Ω[]ᜀ=>ɜ;public ฃ[]ᜂ=>ߍ;
public ߵ[]ᜆ=>ޞ;public ᛔ[]ᜃ=>ᛓ;public ᑋ ᑋ=>ᛝ;public Ǭ Ǭ=>ᛠ;public ථ ᜄ=>ᛞ;public int ᜅ=>ü.ᜅ;public string ښ=>ᛟ;private static ᑧ[
]ᜇ=new ᑧ[]{ᑧ.ᑳ,ᑧ.ᑵ,ᑧ.ᑴ,ᑧ.ᑽ,ᑧ.ᒈ,ᑧ.ᒄ,ᑧ.ᒆ,ᑧ.ᒅ,ᑧ.ᒇ};public static ᑧ ᛦ(ᴆ Å){ᑧ ߚ;if(Å.ಖ==ಖ.ᴉ){ߚ=ᑧ.ᑨ+Å.શ-1;}else{if(Å.ᰔ<4){ߚ=ᑧ.ᑹ
+(Å.ᰔ-1)*9+Å.શ-1;}else{ߚ=ᜇ[Å.શ-1];}}return ߚ;}}sealed class ય{private ଦ l;private Ꮖ ಉ;private Ꮖ ಊ;private Ꮖ ᛥ;private Ꮖ ᛤ
;public ય(ଦ l){this.l=l;}public void ᛣ(ɢ ò){if(ò.ᶗ==null){ᛥ=Ꮖ.Ꮓ;return;}var ಇ=ò.ɣ;var ಈ=ò.ɤ;if(ಇ.ģ<ಈ.ģ){ಉ=ಇ.ģ;}else{ಉ=ಈ.ģ
;}if(ಇ.Ģ>ಈ.Ģ){ಊ=ಇ.Ģ;ᛤ=ಈ.Ģ;}else{ಊ=ಈ.Ģ;ᛤ=ಇ.Ģ;}ᛥ=ಉ-ಊ;}public Ꮖ ᛢ=>ಉ;public Ꮖ ᛡ=>ಊ;public Ꮖ ᛖ=>ᛥ;public Ꮖ ᛕ=>ᛤ;}sealed class
મ{private static Ꮖ ᛌ=Ꮖ.Ꭸ(64);private ଦ l;public મ(ଦ l){this.l=l;ᛐ();}private Ɠ ᛍ;private Func<ᯍ,bool>ᛎ;private void ᛐ(){ᛎ
=ᛒ;}private bool ᛒ(ᯍ ܩ){var ઑ=l.ય;if(ܩ.ᒭ.Ě==0){ઑ.ᛣ(ܩ.ᒭ);if(ઑ.ᛖ<=Ꮖ.Ꮓ){l.ૐ(ᛍ,ɴ.ȣ,ʡ.ʟ);return false;}return true;}var ñ=0;if
(ᵎ.ᴫ(ᛍ.ޚ,ᛍ.ޙ,ܩ.ᒭ)==1){ñ=1;}ᛋ(ᛍ,ܩ.ᒭ,ñ);return false;}public void ᛑ(ђ Ð){var Ⴃ=l.ڏ;ᛍ=Ð.Ɠ;var Ş=Ð.Ɠ.ɡ;var ǎ=Ð.Ɠ.ޚ;var ǆ=Ð.Ɠ.
ޙ;var ǐ=ǎ+ᛌ.Ꮄ()*ஜ.ௐ(Ş);var ǅ=ǆ+ᛌ.Ꮄ()*ஜ.ஸ(Ş);Ⴃ.ܨ(ǎ,ǆ,ǐ,ǅ,ݐ.ݑ,ᛎ);}public bool ᛋ(Ɠ ğ,ɢ ò,int ñ){var ଥ=l.ƒ;var Ü=l.Đ;if(ñ!=0)
{switch((int)ò.Ě){case 124:break;default:return false;}}if(ğ.ђ==null){if((ò.ᘾ&ᶘ.ᶑ)!=0){return false;}switch((int)ò.Ě){
case 1:case 32:case 33:case 34:break;default:return false;}}switch((int)ò.Ě){case 1:case 26:case 27:case 28:case 31:case 32:
case 33:case 34:case 117:case 118:Ü.Ο(ò,ğ);break;case 7:if(Ü.Ͳ(ò,ခ.ဂ)){ଥ.ໟ(ò,false);}break;case 9:if(Ü.ε(ò)){ଥ.ໟ(ò,false);}
break;case 11:ଥ.ໟ(ò,false);l.ଞ();break;case 14:if(Ü.ˍ(ò,ۀ.ۃ,32)){ଥ.ໟ(ò,false);}break;case 15:if(Ü.ˍ(ò,ۀ.ۃ,24)){ଥ.ໟ(ò,false);}
break;case 18:if(Ü.Ί(ò,ᴀ.ᳯ)){ଥ.ໟ(ò,false);}break;case 20:if(Ü.ˍ(ò,ۀ.ۄ,0)){ଥ.ໟ(ò,false);}break;case 21:if(Ü.ˍ(ò,ۀ.ۂ,0)){ଥ.ໟ(ò,
false);}break;case 23:if(Ü.Ί(ò,ᴀ.ᳵ)){ଥ.ໟ(ò,false);}break;case 29:if(Ü.Ό(ò,ಫ.Ϊ)){ଥ.ໟ(ò,false);}break;case 41:if(Ü.ώ(ò,ᒓ.ᒔ)){ଥ.
ໟ(ò,false);}break;case 71:if(Ü.Ί(ò,ᴀ.ᳱ)){ଥ.ໟ(ò,false);}break;case 49:if(Ü.ώ(ò,ᒓ.ᒍ)){ଥ.ໟ(ò,false);}break;case 50:if(Ü.Ό(ò,
ಫ.ಯ)){ଥ.ໟ(ò,false);}break;case 51:ଥ.ໟ(ò,false);l.ફ();break;case 55:if(Ü.Ί(ò,ᴀ.ᳩ)){ଥ.ໟ(ò,false);}break;case 101:if(Ü.Ί(ò,ᴀ
.ᳰ)){ଥ.ໟ(ò,false);}break;case 102:if(Ü.Ί(ò,ᴀ.ᳶ)){ଥ.ໟ(ò,false);}break;case 103:if(Ü.Ό(ò,ಫ.ರ)){ଥ.ໟ(ò,false);}break;case 111
:if(Ü.Ό(ò,ಫ.ಲ)){ଥ.ໟ(ò,false);}break;case 112:if(Ü.Ό(ò,ಫ.ಳ)){ଥ.ໟ(ò,false);}break;case 113:if(Ü.Ό(ò,ಫ.ವ)){ଥ.ໟ(ò,false);}
break;case 122:if(Ü.ˍ(ò,ۀ.ۆ,0)){ଥ.ໟ(ò,false);}break;case 127:if(Ü.Ͳ(ò,ခ.ဃ)){ଥ.ໟ(ò,false);}break;case 131:if(Ü.Ί(ò,ᴀ.ᱽ)){ଥ.ໟ(ò
,false);}break;case 133:case 135:case 137:if(Ü.ˬ(ò,ಫ.ಳ,ğ)){ଥ.ໟ(ò,false);}break;case 140:if(Ü.Ί(ò,ᴀ.ᱻ)){ଥ.ໟ(ò,false);}
break;case 42:if(Ü.Ό(ò,ಫ.ಯ)){ଥ.ໟ(ò,true);}break;case 43:if(Ü.ώ(ò,ᒓ.ᒔ)){ଥ.ໟ(ò,true);}break;case 45:if(Ü.Ί(ò,ᴀ.ᳶ)){ଥ.ໟ(ò,true);
}break;case 60:if(Ü.Ί(ò,ᴀ.ᳵ)){ଥ.ໟ(ò,true);}break;case 61:if(Ü.Ό(ò,ಫ.ರ)){ଥ.ໟ(ò,true);}break;case 62:if(Ü.ˍ(ò,ۀ.ۂ,1)){ଥ.ໟ(ò
,true);}break;case 63:if(Ü.Ό(ò,ಫ.Ϊ)){ଥ.ໟ(ò,true);}break;case 64:if(Ü.Ί(ò,ᴀ.ᳰ)){ଥ.ໟ(ò,true);}break;case 66:if(Ü.ˍ(ò,ۀ.ۃ,24
)){ଥ.ໟ(ò,true);}break;case 67:if(Ü.ˍ(ò,ۀ.ۃ,32)){ଥ.ໟ(ò,true);}break;case 65:if(Ü.Ί(ò,ᴀ.ᳩ)){ଥ.ໟ(ò,true);}break;case 68:if(Ü
.ˍ(ò,ۀ.ۄ,0)){ଥ.ໟ(ò,true);}break;case 69:if(Ü.Ί(ò,ᴀ.ᳯ)){ଥ.ໟ(ò,true);}break;case 70:if(Ü.Ί(ò,ᴀ.ᳱ)){ଥ.ໟ(ò,true);}break;case
114:if(Ü.Ό(ò,ಫ.ಲ)){ଥ.ໟ(ò,true);}break;case 115:if(Ü.Ό(ò,ಫ.ಳ)){ଥ.ໟ(ò,true);}break;case 116:if(Ü.Ό(ò,ಫ.ವ)){ଥ.ໟ(ò,true);}break
;case 123:if(Ü.ˍ(ò,ۀ.ۆ,0)){ଥ.ໟ(ò,true);}break;case 132:if(Ü.Ί(ò,ᴀ.ᱽ)){ଥ.ໟ(ò,true);}break;case 99:case 134:case 136:if(Ü.ˬ
(ò,ಫ.ಳ,ğ)){ଥ.ໟ(ò,true);}break;case 138:Ü.ϐ(ò,255);ଥ.ໟ(ò,true);break;case 139:Ü.ϐ(ò,35);ଥ.ໟ(ò,true);break;}return true;}
public void ᛊ(ɢ ò,int ñ,Ɠ ğ){if(ğ.ђ==null){switch(ğ.ܫ){case ё.ϸ:case ё.Ϲ:case ё.Ϻ:case ё.ϵ:case ё.Ϸ:case ё.о:return;default:
break;}var ό=false;switch((int)ò.Ě){case 39:case 97:case 125:case 126:case 4:case 10:case 88:ό=true;break;}if(!ό){return;}}
var Ü=l.Đ;switch((int)ò.Ě){case 2:Ü.Ό(ò,ಫ.ರ);ò.Ě=0;break;case 3:Ü.Ό(ò,ಫ.ಯ);ò.Ě=0;break;case 4:Ü.Ό(ò,ಫ.Ϊ);ò.Ě=0;break;case 5
:Ü.Ί(ò,ᴀ.ᳰ);ò.Ě=0;break;case 6:Ü.ώ(ò,ᒓ.ᒌ);ò.Ě=0;break;case 8:Ü.Ͳ(ò,ခ.ဂ);ò.Ě=0;break;case 10:Ü.ˍ(ò,ۀ.ۂ,0);ò.Ě=0;break;case
12:Ü.ϐ(ò,0);ò.Ě=0;break;case 13:Ü.ϐ(ò,255);ò.Ě=0;break;case 16:Ü.Ό(ò,ಫ.ಮ);ò.Ě=0;break;case 17:Ü.δ(ò);ò.Ě=0;break;case 19:Ü
.Ί(ò,ᴀ.ᳶ);ò.Ě=0;break;case 22:Ü.ˍ(ò,ۀ.ۄ,0);ò.Ě=0;break;case 25:Ü.ώ(ò,ᒓ.ᒍ);ò.Ě=0;break;case 30:Ü.Ί(ò,ᴀ.ᳮ);ò.Ě=0;break;case
35:Ü.ϐ(ò,35);ò.Ě=0;break;case 36:Ü.Ί(ò,ᴀ.ᳱ);ò.Ě=0;break;case 37:Ü.Ί(ò,ᴀ.ᳬ);ò.Ě=0;break;case 38:Ü.Ί(ò,ᴀ.ᳵ);ò.Ě=0;break;case
39:Ü.ϔ(ò,ñ,ğ);ò.Ě=0;break;case 40:Ü.ώ(ò,ᒓ.ᒏ);Ü.Ί(ò,ᴀ.ᳵ);ò.Ě=0;break;case 44:Ü.ώ(ò,ᒓ.ᒎ);ò.Ě=0;break;case 52:l.ଞ();break;
case 53:Ü.ˍ(ò,ۀ.ہ,0);ò.Ě=0;break;case 54:Ü.ͻ(ò);ò.Ě=0;break;case 56:Ü.Ί(ò,ᴀ.ᳩ);ò.Ě=0;break;case 57:Ü.ϕ(ò);ò.Ě=0;break;case
58:Ü.Ί(ò,ᴀ.ᳫ);ò.Ě=0;break;case 59:Ü.Ί(ò,ᴀ.ᳪ);ò.Ě=0;break;case 104:Ü.ϑ(ò);ò.Ě=0;break;case 108:Ü.Ό(ò,ಫ.ಲ);ò.Ě=0;break;case
109:Ü.Ό(ò,ಫ.ಳ);ò.Ě=0;break;case 100:Ü.Ͳ(ò,ခ.ဃ);ò.Ě=0;break;case 110:Ü.Ό(ò,ಫ.ವ);ò.Ě=0;break;case 119:Ü.Ί(ò,ᴀ.ᳯ);ò.Ě=0;break;
case 121:Ü.ˍ(ò,ۀ.ۆ,0);ò.Ě=0;break;case 124:l.ફ();break;case 125:if(ğ.ђ==null){Ü.ϔ(ò,ñ,ğ);ò.Ě=0;}break;case 130:Ü.Ί(ò,ᴀ.ᱽ);ò.
Ě=0;break;case 141:Ü.ώ(ò,ᒓ.ᒋ);ò.Ě=0;break;case 72:Ü.ώ(ò,ᒓ.ᒎ);break;case 73:Ü.ώ(ò,ᒓ.ᒍ);break;case 74:Ü.ϕ(ò);break;case 75:
Ü.Ό(ò,ಫ.ಯ);break;case 76:Ü.Ό(ò,ಫ.ಮ);break;case 77:Ü.ώ(ò,ᒓ.ᒌ);break;case 79:Ü.ϐ(ò,35);break;case 80:Ü.ϐ(ò,0);break;case 81
:Ü.ϐ(ò,255);break;case 82:Ü.Ί(ò,ᴀ.ᳵ);break;case 83:Ü.Ί(ò,ᴀ.ᳶ);break;case 84:Ü.Ί(ò,ᴀ.ᳬ);break;case 86:Ü.Ό(ò,ಫ.ರ);break;
case 87:Ü.ˍ(ò,ۀ.ہ,0);break;case 88:Ü.ˍ(ò,ۀ.ۂ,0);break;case 89:Ü.ͻ(ò);break;case 90:Ü.Ό(ò,ಫ.Ϊ);break;case 91:Ü.Ί(ò,ᴀ.ᳰ);break
;case 92:Ü.Ί(ò,ᴀ.ᳫ);break;case 93:Ü.Ί(ò,ᴀ.ᳪ);break;case 94:Ü.Ί(ò,ᴀ.ᳩ);break;case 95:Ü.ˍ(ò,ۀ.ۄ,0);break;case 96:Ü.Ί(ò,ᴀ.ᳮ)
;break;case 97:Ü.ϔ(ò,ñ,ğ);break;case 98:Ü.Ί(ò,ᴀ.ᳱ);break;case 105:Ü.Ό(ò,ಫ.ಲ);break;case 106:Ü.Ό(ò,ಫ.ಳ);break;case 107:Ü.Ό
(ò,ಫ.ವ);break;case 120:Ü.ˍ(ò,ۀ.ۆ,0);break;case 126:if(ğ.ђ==null){Ü.ϔ(ò,ñ,ğ);}break;case 128:Ü.Ί(ò,ᴀ.ᳯ);break;case 129:Ü.Ί
(ò,ᴀ.ᱽ);break;}}public void ᛏ(Ɠ ğ,ɢ ò){bool ό;if(ğ.ђ==null){ό=false;switch((int)ò.Ě){case 46:ό=true;break;}if(!ό){return;
}}var Ü=l.Đ;var ଥ=l.ƒ;switch((int)ò.Ě){case 24:Ü.Ί(ò,ᴀ.ᳰ);ଥ.ໟ(ò,false);break;case 46:Ü.Ό(ò,ಫ.ರ);ଥ.ໟ(ò,true);break;case 47
:Ü.ˍ(ò,ۀ.ۄ,0);ଥ.ໟ(ò,false);break;}}}sealed class ᛔ{private static int ľ=10;public static ᛔ ᓍ=new ᛔ(Ꮖ.Ꮓ,Ꮖ.Ꮓ,ɡ.ᓬ,0,0);
private Ꮖ ǃ;private Ꮖ ǂ;private ɡ Ş;private int ˌ;private ᅻ ܠ;public ᛔ(Ꮖ ǃ,Ꮖ ǂ,ɡ Ş,int ˌ,ᅻ ܠ){this.ǃ=ǃ;this.ǂ=ǂ;this.Ş=Ş;this.ˌ
=ˌ;this.ܠ=ܠ;}public static ᛔ ċ(byte[]f,int ù){var ǃ=BitConverter.ToInt16(f,ù);var ǂ=BitConverter.ToInt16(f,ù+2);var Ş=
BitConverter.ToInt16(f,ù+4);var ˌ=BitConverter.ToInt16(f,ù+6);var ܠ=BitConverter.ToInt16(f,ù+8);return new ᛔ(Ꮖ.Ꭸ(ǃ),Ꮖ.Ꭸ(ǂ),new ɡ(ɡ.ᓭ
.Ꮁ*(uint)(Ş/45)),ˌ,(ᅻ)ܠ);}public static ᛔ[]ÿ(ಆ þ,int ý){var û=þ.ಡ(ý);if(û%ľ!=0){throw new Exception();}var f=þ.ಠ(ý);var ú
=û/ľ;var ᛓ=new ᛔ[ú];for(var Ä=0;Ä<ú;Ä++){var ù=ľ*Ä;ᛓ[Ä]=ċ(f,ù);}return ᛓ;}public Ꮖ ޚ=>ǃ;public Ꮖ ޙ=>ǂ;public ɡ ɡ=>Ş;
public int ܫ{get{return ˌ;}set{ˌ=value;}}public ᅻ ᘾ=>ܠ;}abstract class ᜮ{private ባ ĭ;public ᜮ(ባ ĭ){this.ĭ=ĭ;}public virtual
void ರ(){}public virtual void ڞ(){}public abstract bool ଅ(ᕨ Ň);public ባ ኘ=>ĭ;}abstract class ᜯ{private int ʂ;private int ʁ;
private ᜮ ɾ;private ᜯ(){}public ᜯ(int ʂ,int ʁ,ᜮ ɾ){this.ʂ=ʂ;this.ʁ=ʁ;this.ɾ=ɾ;}public int ᜭ=>ʂ;public int ᜬ=>ʁ;public ᜮ ѐ=>ɾ;}
sealed class ᜫ{private static char[]ᜪ={'_'};private ಆ þ;private Ꮝ ȋ;private ٻ چ;public ᜫ(ಆ þ,Ꮝ ȋ){this.þ=þ;this.ȋ=ȋ;چ=new ٻ(þ)
;}public void Ǌ(ባ ĭ){var ɽ=ĭ.ᐏ as ɥ;if(ɽ!=null){ᝄ(ɽ);}var ᎏ=ĭ.ᐏ as ĵ;if(ᎏ!=null){ᝅ(ᎏ);}var ፃ=ĭ.ᐏ as ᶌ;if(ፃ!=null){ᝁ(ፃ);}
var ᝂ=ĭ.ᐏ as ଇ;if(ᝂ!=null){ᐭ(ᝂ.ǭ);}var ᝃ=ĭ.ᐏ as Ʒ;if(ᝃ!=null){ᐭ(ᝃ.ǭ);}var ᕀ=ĭ.ᐏ as ǳ;if(ᕀ!=null){ᐭ(ᕀ.ǭ);}var Ꮵ=ĭ.ᐏ as ᯖ;if(
Ꮵ!=null){ᜐ(Ꮵ);}}private void ᝄ(ɥ ɽ){for(var Ä=0;Ä<ɽ.Ń.Count;Ä++){ᜰ(ɽ.Ń[Ä],ɽ.ń[Ä],ɽ.Ņ[Ä]);}foreach(var ɵ in ɽ.ņ){ᝀ(ɽ.ኘ,ɵ);
}var İ=ɽ.ň;var ݴ=ɽ.ኘ.ŵ/8%2==0?"M_SKULL1":"M_SKULL2";ᜰ(ݴ,İ.ᜭ,İ.ᜬ);}private void ᝅ(ĵ ᎏ){for(var Ä=0;Ä<ᎏ.Ń.Count;Ä++){ᜰ(ᎏ.Ń[
Ä],ᎏ.ń[Ä],ᎏ.Ņ[Ä]);}foreach(var ɵ in ᎏ.ņ){ᝀ(ᎏ.ኘ,ɵ);}var İ=ᎏ.ň;var ݴ=ᎏ.ኘ.ŵ/8%2==0?"M_SKULL1":"M_SKULL2";ᜰ(ݴ,İ.ᜭ,İ.ᜬ);}
private void ᝁ(ᶌ ፃ){for(var Ä=0;Ä<ፃ.Ń.Count;Ä++){ᜰ(ፃ.Ń[Ä],ፃ.ń[Ä],ፃ.Ņ[Ä]);}foreach(var ɵ in ፃ.ņ){ᝀ(ፃ.ኘ,ɵ);}var İ=ፃ.ň;var ݴ=ፃ.ኘ.ŵ
/8%2==0?"M_SKULL1":"M_SKULL2";ᜰ(ݴ,İ.ᜭ,İ.ᜬ);}private void ᝀ(ባ ĭ,ᜯ ɵ){var ɹ=ɵ as ʉ;if(ɹ!=null){ᜌ(ɹ);}var ɶ=ɵ as ஔ;if(ɶ!=
null){ᜎ(ɶ);}var ɷ=ɵ as ʖ;if(ɷ!=null){ᜏ(ɷ);}var ᜱ=ɵ as ท;if(ᜱ!=null){ᜉ(ᜱ,ĭ.ŵ);}}private void ᜰ(string Ĭ,int ǃ,int ǂ){var ǉ=ȋ.
Ǒ/320;ȋ.Ꮘ(چ[Ĭ],ǉ*ǃ,ǉ*ǂ,ǉ);}private void ᜋ(IReadOnlyList<char>Ǉ,int ǃ,int ǂ){var ǉ=ȋ.Ǒ/320;ȋ.ᐭ(Ǉ,ǉ*ǃ,ǉ*ǂ,ǉ);}private void
ᜌ(ʉ ɵ){if(!ɵ.Ń.Equals("M_EPI4")){ᜰ(ɵ.Ń,ɵ.ɼ,ɵ.ʈ);}}private void ᜎ(ஔ ɵ){ᜰ(ɵ.Ń,ɵ.ɼ,ɵ.ʈ);ᜰ(ɵ.Ŷ,ɵ.க,ɵ.ʈ);}private void ᜏ(ʖ ɵ){
ᜰ(ɵ.Ń,ɵ.ɼ,ɵ.ʈ);ᜰ("M_THERML",ɵ.ʔ,ɵ.ʓ);for(var Ä=0;Ä<ɵ.ʒ;Ä++){var ǃ=ɵ.ʔ+8*(1+Ä);ᜰ("M_THERMM",ǃ,ɵ.ʓ);}var ය=ɵ.ʔ+8*(1+ɵ.ʒ);ᜰ(
"M_THERMR",ය,ɵ.ʓ);var ࠆ=ɵ.ʔ+8*(1+ɵ.ʑ);ᜰ("M_THERMO",ࠆ,ɵ.ʓ);}private char[]ᜊ="EMPTY SLOT".ToCharArray();private void ᜉ(ท ɵ,int Ƃ){
var û=24;ᜰ("M_LSLEFT",ɵ.ɼ,ɵ.ʈ);for(var Ä=0;Ä<û;Ä++){var ǃ=ɵ.ɼ+8*(1+Ä);ᜰ("M_LSCNTR",ǃ,ɵ.ʈ);}ᜰ("M_LSRGHT",ɵ.ɼ+8*(1+û),ɵ.ʈ);if
(!ɵ.ෆ){var Ǉ=ɵ.ǭ!=null?ɵ.ǭ:ᜊ;ᜋ(Ǉ,ɵ.ɼ+8,ɵ.ʈ);}else{ᜋ(ɵ.ǭ,ɵ.ɼ+8,ɵ.ʈ);if(Ƃ/3%2==0){var ᜈ=ȋ.ᐲ(ɵ.ǭ,1);ᜋ(ᜪ,ɵ.ɼ+8+ᜈ,ɵ.ʈ);}}}
private void ᐭ(IReadOnlyList<string>Ǉ){var ǉ=ȋ.Ǒ/320;var ˢ=7*ǉ*Ǉ.Count;for(var Ä=0;Ä<Ǉ.Count;Ä++){var ǃ=(ȋ.Ǒ-ȋ.ᐲ(Ǉ[Ä],ǉ))/2;var
ǂ=(ȋ.ǡ-ˢ)/2+7*ǉ*(Ä+1);ȋ.ᐭ(Ǉ[Ä],ǃ,ǂ,ǉ);}}private void ᜐ(ᯖ Ꮵ){var ݴ=Ꮵ.ኘ.ŵ/8%2==0?"M_SKULL1":"M_SKULL2";if(Ꮵ.ኘ.હ.ಖ==ಖ.ᴉ){ᜰ(
"HELP",0,0);ᜰ(ݴ,298,160);}else{if(Ꮵ.ᯓ==0){ᜰ("HELP1",0,0);ᜰ(ݴ,298,170);}else{ᜰ("HELP2",0,0);ᜰ(ݴ,248,180);}}}}public enum ಕ{ኾ,ᕩ,
ᕹ}class Ɠ:Ⴞ{public static Ꮖ ᜦ=Ꮖ.Ꮐ;public static Ꮖ ᜧ=Ꮖ.Ꮑ;private ଦ l;private Ꮖ ǃ;private Ꮖ ǂ;private Ꮖ ݳ;private Ɠ ᜨ;
private Ɠ ᜩ;private ɡ Ş;private э Т;private int Ч;private Ɠ ᜥ;private Ɠ ᜤ;private ฃ त;private Ꮖ ᜣ;private Ꮖ ᜢ;private Ꮖ ᘪ;
private Ꮖ ˢ;private Ꮖ ᜡ;private Ꮖ ᜠ;private Ꮖ ᜑ;private int Ĺ;private ё ˌ;private ᘁ ݤ;private int Ƃ;private Ш Í;private ళ ܠ;
private int ڹ;private ಣ ᛉ;private int ᘜ;private Ɠ Ψ;private int ᘣ;private int ᘳ;private ђ Ð;private int ᘴ;private ᛔ ᘵ;private Ɠ
ᘷ;private bool ۑ;private Ꮖ ϖ;private Ꮖ ϗ;private Ꮖ Ϙ;public Ɠ(ଦ l){this.l=l;}public override void Ⴛ(){if(ᜡ!=Ꮖ.Ꮓ||ᜠ!=Ꮖ.Ꮓ||
(ܠ&ళ.ᘅ)!=0){l.લ.უ(this);if(Ⴙ==Ⴙ.ᄦ){return;}}if((ݳ!=ᜣ)||ᜑ!=Ꮖ.Ꮓ){l.લ.პ(this);if(Ⴙ==Ⴙ.ᄦ){return;}}if(Ƃ!=-1){Ƃ--;if(Ƃ==0){if(
!ᘸ(Í.ѐ)){return;}}}else{if((ܠ&ళ.ᘇ)==0){return;}var Å=l.હ;if(!(Å.ᴷ==ᵅ.ᵇ||Å.ᴳ)){return;}ᘜ++;if(ᘜ<12*35){return;}if((l.ଖ&31)
!=0){return;}if(l.ષ.ѐ()>4){return;}ᘰ();}}public bool ᘸ(ᚏ Í){do{if(Í==ᚏ.ᚠ){this.Í=ኑ.ጔ[(int)ᚏ.ᚠ];l.ળ.ᆖ(this);return false;}
var ᆙ=ኑ.ጔ[(int)Í];this.Í=ᆙ;Ƃ=ᘱ(ᆙ);Т=ᆙ.э;Ч=ᆙ.ю;if(ᆙ.я!=null){ᆙ.я(l,this);}Í=ᆙ.ѐ;}while(Ƃ==0);return true;}private int ᘱ(Ш Í)
{var Å=l.હ;if(Å.ᴴ||Å.ᴷ==ᵅ.ᵇ){if((int)ᚏ.ᝢ<=Í.ġ&&Í.ġ<=(int)ᚏ.ᝈ){return Í.ŵ>>1;}else{return Í.ŵ;}}else{return Í.ŵ;}}private
void ᘰ(){ᛔ ᘯ;if(ᘵ!=null){ᘯ=ᘵ;}else{ᘯ=ᛔ.ᓍ;}if(!l.લ.ბ(this,ᘯ.ޚ,ᘯ.ޙ)){return;}var æ=l.ળ;var ϓ=æ.ᆕ(ǃ,ǂ,त.Ŀ.Ģ,ё.Ϩ);l.ૐ(ϓ,ɴ.ȝ,ʡ.ʝ)
;var Ⴤ=ᵎ.ᴩ(ᘯ.ޚ,ᘯ.ޙ,l.શ);var ϒ=æ.ᆕ(ᘯ.ޚ,ᘯ.ޙ,Ⴤ.Ŀ.Ģ,ё.Ϩ);l.ૐ(ϒ,ɴ.ȝ,ʡ.ʝ);Ꮖ ݳ;if((ݤ.ᘾ&ళ.ᘕ)!=0){ݳ=ᜧ;}else{ݳ=ᜦ;}var ã=æ.ᆕ(ᘯ.ޚ,ᘯ.ޙ
,ݳ,ˌ);ã.ᘬ=ᘵ;ã.ɡ=ᘯ.ɡ;if((ᘯ.ᘾ&ᅻ.ᅾ)!=0){ã.ᘾ|=ళ.ᅾ;}ã.ᘺ=18;l.ળ.ᆖ(this);}public override void Ċ(){ۑ=true;ϖ=ǃ;ϗ=ǂ;Ϙ=ݳ;}public
void Ĝ(){ۑ=false;}public Ꮖ ᘮ(Ꮖ č){if(ۑ){return ϖ+č*(ǃ-ϖ);}else{return ǃ;}}public Ꮖ ᘶ(Ꮖ č){if(ۑ){return ϗ+č*(ǂ-ϗ);}else{
return ǂ;}}public Ꮖ ᙅ(Ꮖ č){if(ۑ){return Ϙ+č*(ݳ-Ϙ);}else{return ݳ;}}public ଦ ଦ=>l;public Ꮖ ޚ{get{return ǃ;}set{ǃ=value;}}public
Ꮖ ޙ{get{return ǂ;}set{ǂ=value;}}public Ꮖ ኴ{get{return ݳ;}set{ݳ=value;}}public Ɠ ᙃ{get{return ᜨ;}set{ᜨ=value;}}public Ɠ ᙄ{
get{return ᜩ;}set{ᜩ=value;}}public ɡ ɡ{get{return Ş;}set{Ş=value;}}public э э{get{return Т;}set{Т=value;}}public int ю{get{
return Ч;}set{Ч=value;}}public Ɠ ᙆ{get{return ᜥ;}set{ᜥ=value;}}public Ɠ ᙇ{get{return ᜤ;}set{ᜤ=value;}}public ฃ ฃ{get{return त;
}set{त=value;}}public Ꮖ ᙈ{get{return ᜣ;}set{ᜣ=value;}}public Ꮖ ᙉ{get{return ᜢ;}set{ᜢ=value;}}public Ꮖ ᙊ{get{return ᘪ;}set
{ᘪ=value;}}public Ꮖ ǡ{get{return ˢ;}set{ˢ=value;}}public Ꮖ ᙂ{get{return ᜡ;}set{ᜡ=value;}}public Ꮖ ᙁ{get{return ᜠ;}set{ᜠ=
value;}}public Ꮖ ᙀ{get{return ᜑ;}set{ᜑ=value;}}public int Ĕ{get{return Ĺ;}set{Ĺ=value;}}public ё ܫ{get{return ˌ;}set{ˌ=value;
}}public ᘁ ᘿ{get{return ݤ;}set{ݤ=value;}}public int ŵ{get{return Ƃ;}set{Ƃ=value;}}public Ш Ŷ{get{return Í;}set{Í=value;}}
public ళ ᘾ{get{return ܠ;}set{ܠ=value;}}public int ƙ{get{return ڹ;}set{ڹ=value;}}public ಣ ᘽ{get{return ᛉ;}set{ᛉ=value;}}public
int ᘼ{get{return ᘜ;}set{ᘜ=value;}}public Ɠ ᘻ{get{return Ψ;}set{Ψ=value;}}public int ᘺ{get{return ᘣ;}set{ᘣ=value;}}public
int ᘹ{get{return ᘳ;}set{ᘳ=value;}}public ђ ђ{get{return Ð;}set{Ð=value;}}public int ᘭ{get{return ᘴ;}set{ᘴ=value;}}public ᛔ
ᘬ{get{return ᘵ;}set{ᘵ=value;}}public Ɠ ш{get{return ᘷ;}set{ᘷ=value;}}}[Flags]public enum ళ{Ě=1,ᘏ=2,ᘐ=4,ᘑ=8,ᘒ=16,ᅾ=32,ᘙ=64
,ᘔ=128,ᘕ=256,ᘖ=512,ᘗ=0x400,ᘘ=0x800,ᖮ=0x1000,ᘚ=0x2000,ᘍ=0x4000,ϔ=0x8000,પ=0x10000,ᘌ=0x20000,ᘋ=0x40000,ᘊ=0x80000,ᘉ=0x100000
,ᘈ=0x200000,ᘇ=0x400000,ᘆ=0x800000,ᘅ=0x1000000,ᘄ=0x2000000,ᘃ=0xc000000,ᘂ=26}sealed class ᘁ{private int ᘀ;private ᚏ ᘓ;
private int ᘛ;private ᚏ ᘥ;private ɴ ᘤ;private int ᘣ;private ɴ ᘢ;private ᚏ ᘡ;private int ᘠ;private ɴ ᘟ;private ᚏ ᘞ;private ᚏ ᘝ;
private ᚏ ᘎ;private ᚏ ᘨ;private ɴ ᘩ;private int Ζ;private Ꮖ ᘪ;private Ꮖ ˢ;private int ᘫ;private int Ь;private ɴ ᘧ;private ళ ܠ;
private ᚏ ᘦ;public ᘁ(int ᘀ,ᚏ ᘓ,int ᘛ,ᚏ ᘥ,ɴ ᘤ,int ᘣ,ɴ ᘢ,ᚏ ᘡ,int ᘠ,ɴ ᘟ,ᚏ ᘞ,ᚏ ᘝ,ᚏ ᘎ,ᚏ ᘨ,ɴ ᘩ,int Ζ,Ꮖ ᘪ,Ꮖ ˢ,int ᘫ,int Ь,ɴ ᘧ,ళ ܠ,ᚏ ᘦ)
{this.ᘀ=ᘀ;this.ᘓ=ᘓ;this.ᘛ=ᘛ;this.ᘥ=ᘥ;this.ᘤ=ᘤ;this.ᘣ=ᘣ;this.ᘢ=ᘢ;this.ᘡ=ᘡ;this.ᘠ=ᘠ;this.ᘟ=ᘟ;this.ᘞ=ᘞ;this.ᘝ=ᘝ;this.ᘎ=ᘎ;
this.ᘨ=ᘨ;this.ᘩ=ᘩ;this.Ζ=Ζ;this.ᘪ=ᘪ;this.ˢ=ˢ;this.ᘫ=ᘫ;this.Ь=Ь;this.ᘧ=ᘧ;this.ܠ=ܠ;this.ᘦ=ᘦ;}public int ᚦ{get{return ᘀ;}set{ᘀ=
value;}}public ᚏ ᚡ{get{return ᘓ;}set{ᘓ=value;}}public int ᚢ{get{return ᘛ;}set{ᘛ=value;}}public ᚏ ᚣ{get{return ᘥ;}set{ᘥ=value;
}}public ɴ ᚤ{get{return ᘤ;}set{ᘤ=value;}}public int ᘺ{get{return ᘣ;}set{ᘣ=value;}}public ɴ ᚥ{get{return ᘢ;}set{ᘢ=value;}}
public ᚏ ᚧ{get{return ᘡ;}set{ᘡ=value;}}public int ᚚ{get{return ᘠ;}set{ᘠ=value;}}public ɴ ᚙ{get{return ᘟ;}set{ᘟ=value;}}public
ᚏ ᚘ{get{return ᘞ;}set{ᘞ=value;}}public ᚏ ᚗ{get{return ᘝ;}set{ᘝ=value;}}public ᚏ ᚖ{get{return ᘎ;}set{ᘎ=value;}}public ᚏ ᚕ{
get{return ᘨ;}set{ᘨ=value;}}public ɴ ᚔ{get{return ᘩ;}set{ᘩ=value;}}public int ݏ{get{return Ζ;}set{Ζ=value;}}public Ꮖ ᙊ{get{
return ᘪ;}set{ᘪ=value;}}public Ꮖ ǡ{get{return ˢ;}set{ˢ=value;}}public int ᚓ{get{return ᘫ;}set{ᘫ=value;}}public int ᚒ{get{
return Ь;}set{Ь=value;}}public ɴ ᚑ{get{return ᘧ;}set{ᘧ=value;}}public ళ ᘾ{get{return ܠ;}set{ܠ=value;}}public ᚏ ᚐ{get{return ᘦ;
}set{ᘦ=value;}}}public enum ᚏ{ᚠ,ᚨ,ધ,ᚺ,ᚻ,ᚼ,ᚽ,ᚾ,ᚿ,ᛀ,ਸ,ᛇ,ᛁ,ᛂ,ᛃ,ᛄ,ᛅ,ᛆ,ᛈ,ᚹ,ᚸ,ᚷ,ᚶ,ᚵ,ᚴ,ᚳ,ᚲ,ᚱ,ᚰ,ᚯ,ᚮ,ᚭ,ᚬ,ᚫ,ᚪ,ᚩ,ᚎ,ᚍ,ᙋ,ᙚ,ᙛ,ᙜ,ᙝ,ᙞ,ᙟ,ᙡ
,ᙨ,ᙢ,ᙣ,ᙤ,ᙥ,ᙦ,ᙧ,ᙩ,ᙙ,ᙘ,ᙗ,પ,ᙖ,ᙕ,ᙔ,ᙓ,ᙒ,ᙑ,ᙐ,ᙏ,ᙎ,ન,ᙍ,ᙌ,ᙠ,ᙪ,ᚄ,ᙽ,Ϲ,ᙾ,ᙿ,ᚁ,ᚂ,ᚃ,ᚅ,Ϻ,ᚆ,ᚇ,ᚈ,ᚉ,ᚊ,ᚋ,ᚌ,ᙼ,ᙻ,ᙺ,ᙹ,ᙸ,ᙷ,ᙶ,ᙵ,ᙴ,ᙳ,ᙲ,ᙱ,ᙰ,ᙯ,ᙬ,ᙫ,ᘲ,
ᝆ,ᠦ,ᦕ,ᦖ,ᦗ,ᦘ,ᦙ,ᦚ,ϸ,ᦜ,ᦣ,ᦝ,ᦞ,ᦟ,ᦠ,ᦡ,ᦢ,ᦤ,ᦔ,ᦓ,ᦒ,ᦑ,ᦐ,ᦏ,Ϩ,ᦎ,ᦍ,ᦌ,ᦋ,ᦊ,ᦉ,ᦈ,ᦇ,ᦆ,ᦛ,ᦥ,ϧ,ᨄ,ᨅ,ᨆ,ᨇ,ᨈ,ᨉ,ᨊ,ᨋ,ᨒ,ᨌ,ᨍ,ᨎ,ᨏ,ᨐ,ᨑ,ᨓ,ᨃ,ᨂ,ᨁ,ᨀ,ᧇ,ᧆ,ᧅ,ᧄ
,ᧃ,ᧂ,ᧁ,ᦫ,ᦪ,ᦩ,ᦨ,ᦧ,ᦦ,ᦅ,ᦄ,ᣵ,ᤑ,ᤒ,ᤓ,ᤔ,ᤕ,ᤖ,ᤘ,ᥒ,ᤙ,ᤚ,ᤛ,ᤜ,ᥐ,ᥑ,ᥓ,ᤏ,ᤎ,ᤍ,ᤌ,ᤋ,ᤊ,ᤉ,ᤈ,ᤇ,ᤆ,ᤅ,ᤄ,ᤃ,ᤂ,ᤁ,ᤀ,ᤗ,ᥔ,ᥭ,ᥦ,ᥧ,ᥨ,ᥩ,ᥪ,ᥫ,ᥬ,ᥰ,ᦂ,ᥱ,ᥲ,ᥳ,ᥴ,ᦀ,
ᦁ,ᦃ,ᥥ,ᥤ,ᥣ,ᥢ,ᥡ,ᥠ,ᥟ,ᥞ,ᥝ,ᥜ,ᥛ,ᥚ,ᥙ,ᥘ,ᥗ,ᥖ,ᤐ,ᨔ,ᨯ,ᬣ,ᬤ,ᬥ,ᬦ,ᬧ,ᬨ,ᬩ,ᬫ,ᬲ,ᬬ,ᬭ,ᬮ,ᬯ,ᬰ,ᬱ,ᬳ,ᬢ,ᬡ,ᬠ,ᬟ,ᬞ,ᬝ,ᬜ,ᬛ,ᬚ,ᬙ,ᬘ,ᬗ,ᬖ,ᬕ,ᬔ,ᬓ,ᬪ,ᭅ,ᮕ,ᮎ,ᮏ,ᮐ,ᮑ,ᮒ
,ᮓ,ᮔ,ᮖ,ᮝ,ᮗ,ᮘ,ᮙ,ᮚ,ᮛ,ᮜ,ᮞ,ᮍ,ᮌ,ᮋ,ᮊ,ᮉ,ᮈ,ᮇ,ᮆ,ᮅ,ᮄ,ᮃ,ᭋ,ᭊ,ᭉ,ᭈ,ᭇ,ᭆ,ш,ᬒ,ᨕ,ᨰ,ᨱ,ᨲ,ᨳ,ᨴ,ᨵ,ᨷ,ᨾ,ᨸ,ᨹ,ᨺ,ᨻ,ᨼ,ᨽ,ᨿ,ᨮ,ᨭ,ᨬ,ᨫ,ᨪ,ᨩ,ᨨ,ᨧ,ᨦ,ᨥ,ᨤ,ᨣ,ᨢ,ᨡ,
ᨠ,ᨖ,ᨶ,ᩀ,ᬈ,ᩒ,ᩓ,ᩔ,ᪧ,ᬅ,ᬆ,ᬇ,ᬉ,ᬐ,ᬊ,ᬋ,ᬌ,ᬍ,ᬎ,ᬏ,ᬑ,ᩑ,ᩐ,ᩏ,ᩎ,ᩍ,ᩌ,ᩋ,ᩊ,ᩉ,ᩈ,ᩇ,ᩆ,ᩅ,ᩄ,ᩃ,ᩂ,ᩁ,ᥕ,ᣴ,ឈ,ᠧ,ᠨ,ᠩ,ᠪ,ᠫ,ᠬ,ᠮ,ᠵ,ᠯ,ᠰ,ᠱ,ᠲ,ᠳ,ᠴ,ᠶ,ᠥ,ᠤ,ᠣ,ᠢ,ᠡ
,ᠠ,ៜ,ៗ,ឳ,ឲ,ឱ,ឰ,ឯ,ឮ,ឭ,ឬ,ᠭ,ᠷ,ᡐ,ᡉ,ᡊ,ᡋ,ᡌ,ᡍ,ᡎ,ᡏ,ᡑ,ᡘ,ᡒ,ᡓ,ᡔ,ᡕ,ᡖ,ᡗ,ᡙ,ᡈ,ᡇ,ᡆ,ᡅ,ᡄ,ᡃ,ᡂ,ᡁ,ᡀ,ᠿ,ᠾ,ᠽ,ᠼ,ᠻ,ᠺ,ᠹ,ᠸ,ឫ,ឪ,ᝇ,ᝧ,ᝨ,ᝩ,ᝪ,ᝫ,ᝬ,ᝯ,ច,ᝰ,ក,
ខ,គ,ឃ,ង,ឆ,ᝥ,ᝤ,ᝣ,ᝢ,ᝡ,ᝠ,ᝑ,ᝐ,ᝏ,ᝎ,ᝍ,ᝌ,ᝋ,ᝊ,ᝉ,ᝈ,ᝮ,ជ,ហ,យ,រ,ល,វ,ឝ,ឞ,ស,ឡ,ឨ,អ,ឣ,ឤ,ឥ,ឦ,ឧ,ឩ,ម,ភ,ព,ផ,ប,ន,ធ,ទ,ថ,ត,ណ,ឍ,ឌ,ឋ,ដ,ញ,ᝦ,ᡚ,ᡬ,ᣀ,ᣁ
,ᣂ,ᣃ,ᣄ,ᣅ,ᣆ,ᣈ,ᣏ,ᣉ,ᣊ,ᣋ,ᣌ,ᣍ,ᣎ,ᣐ,ᢿ,ᢾ,ᢽ,ᢼ,ᢻ,ᢺ,ᢹ,ᢸ,ᢷ,ᢶ,ᢵ,ᢴ,ᢳ,ᢲ,ᢱ,ᢰ,ᣇ,ᣑ,ᣪ,ᣣ,ᣤ,ᣥ,ᣦ,ᣧ,ᣨ,ᣩ,ᣫ,ᣲ,ᣬ,ᣭ,ᣮ,ᣯ,ᣰ,ᣱ,ᣳ,ᣢ,ᣡ,ᣠ,ᣟ,ᣞ,ᣝ,ᣜ,ᣛ,ᣚ,ᣙ,ᣘ,
ᣗ,ᣖ,ᣕ,ᣔ,ᣓ,ᣒ,ᢪ,ᢨ,ᡛ,ᡭ,ᡮ,ᡯ,ᡰ,ᡱ,ᡲ,ᡴ,ᢃ,ᡵ,ᡶ,ᡷ,ᢀ,ᢁ,ᢂ,ᢄ,ᡫ,ᡪ,ᡩ,ᡨ,ᡧ,ᡦ,ᡥ,ᡤ,ᡣ,ᡢ,ᡡ,ᡠ,ᡟ,ᡞ,ᡝ,ᡜ,ᡳ,ᢅ,ᢞ,ᢗ,ᢘ,ᢙ,ᢚ,ᢛ,ᢜ,ᢝ,ᢟ,ᢦ,ᢠ,ᢡ,ᢢ,ᢣ,ᢤ,ᢥ,ᢧ,ᢖ,ᢕ
,ᢔ,ᢓ,ᢒ,ᢑ,ᢐ,ᢏ,ᢎ,ᢍ,ᢌ,ᢋ,ᢊ,ᢉ,ᢈ,ᢇ,ᢆ,ᆭ,ԍ,ᆚ,Ԏ,ԏ,Ԑ,ԑ,Ԓ,ԓ,ԕ,Ԝ,Ԗ,ԗ,Ԙ,ԙ,Ԛ,ԛ,ԝ,Ԍ,ԋ,Ԋ,ԉ,Ԉ,ԇ,Ԇ,ԅ,Ԅ,ԃ,Ԃ,ԁ,Ԁ,ӿ,Ӿ,ӽ,Ԕ,Ԟ,Հ,Թ,Ժ,Ի,Լ,Խ,Ծ,Կ,Ձ,
Ո,Ղ,Ճ,Մ,Յ,Ն,Շ,Չ,Ը,Է,Զ,Ե,Դ,Գ,Բ,Ա,ԧ,Ԧ,ԥ,Ԥ,ԣ,Ԣ,ԡ,Ԡ,ԟ,Ӽ,ӻ,Ҷ,ӈ,Ӊ,ӊ,Ӌ,ӌ,Ӎ,ӏ,Ӗ,Ӑ,ӑ,Ӓ,ӓ,Ӕ,ӕ,ӗ,ӆ,Ӆ,ӄ,Ӄ,ӂ,Ӂ,Ӏ,ҿ,Ҿ,ҽ,Ҽ,һ,Һ,ҹ,Ҹ,ҷ,ӎ,Ә
,ӱ,Ӫ,ӫ,Ӭ,ӭ,Ӯ,ӯ,Ӱ,Ӳ,ӹ,ӳ,Ӵ,ӵ,Ӷ,ӷ,Ӹ,Ӻ,ө,Ө,ӧ,Ӧ,ӥ,Ӥ,ӣ,Ӣ,ӡ,Ӡ,ӟ,Ӟ,ӝ,Ӝ,ӛ,Ӛ,Ӈ,Պ,ե,إ,ئ,ا,ب,ة,ت,ث,ح,ش,خ,د,ذ,ر,ز,س,ص,ؤ,أ,آ,ء,ؠ,ײ,ױ,װ,
ת,ש,ר,ק,צ,ץ,פ,ף,ج,ض,ٱ,ن,ه,و,ى,ي,ٮ,ٯ,ٲ,ٸ,ٳ,ٴ,ٵ,ٶ,ٷ,Е,ٹ,م,ل,ك,ق,ف,ـ,ؿ,ؾ,ؽ,ؼ,ػ,З,ƌ,غ,ع,ظ,ط,ע,ס,Ջ,զ,է,ը,թ,ժ,ի,խ,մ,ծ,կ,հ,ձ,ղ,ճ
,յ,դ,գ,բ,ա,ՙ,Ֆ,Օ,Ք,Փ,Ւ,Ց,Ր,Տ,Վ,Ս,Ռ,լ,ն,ח,א,ב,ג,ד,ה,ו,ז,ט,ן,י,ך,כ,ל,ם,מ,נ,և,ֆ,օ,ք,փ,ւ,ց,ր,տ,վ,ս,ռ,ջ,պ,չ,ո,շ,ә,ҵ,Ͼ,Э,Ю,Я,а,
б,в,г,к,д,е,ж,з,и,й,л,Ы,Ъ,Щ}class Ш{private int Ć;private э Т;private int Ч;private int Ƃ;private Action<ଦ,ђ,Ɓ>Ц;private
Action<ଦ,Ɠ>Х;private ᚏ ɾ;private int Ф;private int У;public Ш(int Ć,э Т,int Ч,int Ƃ,Action<ଦ,ђ,Ɓ>Ц,Action<ଦ,Ɠ>Х,ᚏ ɾ,int Ф,int
У){this.Ć=Ć;this.Т=Т;this.Ч=Ч;this.Ƃ=Ƃ;this.Ц=Ц;this.Х=Х;this.ɾ=ɾ;this.Ф=Ф;this.У=У;}public int ġ{get{return Ć;}set{Ć=
value;}}public э э{get{return Т;}set{Т=value;}}public int ю{get{return Ч;}set{Ч=value;}}public int ŵ{get{return Ƃ;}set{Ƃ=
value;}}public Action<ଦ,ђ,Ɓ>ѓ{get{return Ц;}set{Ц=value;}}public Action<ଦ,Ɠ>я{get{return Х;}set{Х=value;}}public ᚏ ѐ{get{
return ɾ;}set{ɾ=value;}}public int ϣ{get{return Ф;}set{Ф=value;}}public int Ϣ{get{return У;}set{У=value;}}}public enum ё{ђ,є,ь
,ы,ъ,щ,ш,ч,ц,х,ф,у,т,с,р,п,о,н,м,С,Р,Ϛ,Ϭ,ϭ,Ϯ,ϯ,ϰ,ϱ,ϳ,ϻ,ϴ,ϵ,Ϸ,ϸ,Ϲ,Ϻ,ϼ,Ϫ,ϩ,Ϩ,ϧ,Ϧ,ϥ,Ϥ,ϣ,Ϣ,ϡ,Ϡ,ϟ,Ϟ,ϝ,Ϝ,ϛ,ϲ,Ͻ,Ж,Џ,А,Б,В,Г,Д,Е,
З,О,И,Й,К,Л,М,Н,П,Ў,Ѝ,Ќ,Ћ,Њ,Љ,Ј,Ї,І,Ѕ,Є,Ѓ,Ђ,Ё,Ѐ,Ͽ,ϫ,ѕ,џ,Ҏ,ҏ,Ґ,ґ,Ғ,ғ,Ҕ,Җ,ҝ,җ,Ҙ,ҙ,Қ,қ,Ҝ,Ҟ,ҍ,Ҍ,ҋ,Ҋ,ҁ,Ҁ,ѿ,Ѿ,ѽ,Ѽ,ѻ,Ѻ,ѹ,Ѹ,ѷ,Ѷ,ҕ
,ҟ,Ү,ҧ,Ҩ,ҩ,Ҫ,ҫ,Ҭ,ҭ,ү,ҳ,Ұ,ұ}sealed class Ҳ{private ଦ l;public Ҳ(ଦ l){this.l=l;ރ();ݬ();ߦ();}private bool Ҵ(Ɠ ї,bool Ҧ){var
Ë=l.હ.ᰁ;var ú=0;var ҥ=(ї.ᘭ-1)&3;for(;;ї.ᘭ=(ї.ᘭ+1)&3){if(!Ë[ї.ᘭ].Ÿ){continue;}if(ú++==2||ї.ᘭ==ҥ){return false;}var Ð=Ë[ї.ᘭ
];if(Ð.ƙ<=0){continue;}if(!l.બ.ಏ(ї,Ð.Ɠ)){continue;}if(!Ҧ){var Ş=ᵎ.ᴥ(ї.ޚ,ї.ޙ,Ð.Ɠ.ޚ,Ð.Ɠ.ޙ)-ї.ɡ;if(Ş>ɡ.ᓮ&&Ş<ɡ.ᓢ){var њ=ᵎ.ᴭ(Ð
.Ɠ.ޚ-ї.ޚ,Ð.Ɠ.ޙ-ї.ޙ);if(њ>ଊ.ಔ){continue;}}}ї.ᘻ=Ð.Ɠ;return true;}}public void Ҥ(Ɠ ї){ї.ᘹ=0;var Ψ=ї.ฃ.Ŀ.ė;if(Ψ!=null&&(Ψ.ᘾ&ళ
.ᘐ)!=0){ї.ᘻ=Ψ;if((ї.ᘾ&ళ.ᅾ)!=0){if(l.બ.ಏ(ї,ї.ᘻ)){goto seeYou;}}else{goto seeYou;}}if(!Ҵ(ї,false)){return;}seeYou:if(ї.ᘿ.ᚤ
!=0){int Ŗ;switch(ї.ᘿ.ᚤ){case ɴ.Ȝ:case ɴ.ț:case ɴ.Ț:Ŗ=(int)ɴ.Ȝ+l.ષ.ѐ()%3;break;case ɴ.ș:case ɴ.Ș:Ŗ=(int)ɴ.ș+l.ષ.ѐ()%2;
break;default:Ŗ=(int)ї.ᘿ.ᚤ;break;}if(ї.ܫ==ё.С||ї.ܫ==ё.Ϛ){l.ૐ(ї,(ɴ)Ŗ,ʡ.ʠ);}else{l.ૐ(ї,(ɴ)Ŗ,ʡ.ʟ);}}ї.ᘸ(ї.ᘿ.ᚣ);}private static Ꮖ
[]ң={new Ꮖ(Ꮖ.Ꮔ),new Ꮖ(47000),new Ꮖ(0),new Ꮖ(-47000),new Ꮖ(-Ꮖ.Ꮔ),new Ꮖ(-47000),new Ꮖ(0),new Ꮖ(47000)};private static Ꮖ[]Ң=
{new Ꮖ(0),new Ꮖ(47000),new Ꮖ(Ꮖ.Ꮔ),new Ꮖ(47000),new Ꮖ(0),new Ꮖ(-47000),new Ꮖ(-Ꮖ.Ꮔ),new Ꮖ(-47000)};private bool ҡ(Ɠ ї){if(ї
.ᘽ==ಣ.ந){return false;}if((int)ї.ᘽ>=8){throw new Exception("Weird actor->movedir!");}var Ҡ=ї.ޚ+ї.ᘿ.ݏ*ң[(int)ї.ᘽ];var ѵ=ї.
ޙ+ї.ᘿ.ݏ*Ң[(int)ї.ᘽ];var Θ=l.લ;var і=Θ.შ(ї,Ҡ,ѵ);if(!і){if((ї.ᘾ&ళ.ᘍ)!=0&&Θ.Ⴉ){if(ї.ኴ<Θ.Ⴭ){ї.ኴ+=લ.ᆎ;}else{ї.ኴ-=લ.ᆎ;}ї.ᘾ|=ళ.ᘈ
;return true;}if(Θ.Ⴘ==0){return false;}ї.ᘽ=ಣ.ந;var Ѡ=false;while(Θ.Ⴘ-->0){var ò=Θ.ზ[Θ.Ⴘ];if(l.મ.ᛋ(ї,ò,0)){Ѡ=true;}}return
Ѡ;}else{ї.ᘾ&=~ళ.ᘈ;}if((ї.ᘾ&ళ.ᘍ)==0){ї.ኴ=ї.ᙈ;}return true;}private bool ѡ(Ɠ ї){if(!ҡ(ї)){return false;}ї.ᘼ=l.ષ.ѐ()&15;
return true;}private static ಣ[]Ѣ={ಣ.ᗍ,ಣ.ᗖ,ಣ.ᗓ,ಣ.ᗔ,ಣ.ᖿ,ಣ.ᖾ,ಣ.ᖽ,ಣ.ᗉ,ಣ.ந};private static ಣ[]ѣ={ಣ.ᗉ,ಣ.ᖾ,ಣ.ᗖ,ಣ.ᗔ};private ಣ[]ѥ=new
ಣ[3];private void Ѫ(Ɠ ї){if(ї.ᘻ==null){throw new Exception("Called with no target.");}var Ѧ=ї.ᘽ;var ѧ=Ѣ[(int)Ѧ];var Ѩ=ї.ᘻ
.ޚ-ї.ޚ;var ѩ=ї.ᘻ.ޙ-ї.ޙ;if(Ѩ>Ꮖ.Ꭸ(10)){ѥ[1]=ಣ.ᖿ;}else if(Ѩ<Ꮖ.Ꭸ(-10)){ѥ[1]=ಣ.ᗍ;}else{ѥ[1]=ಣ.ந;}if(ѩ<Ꮖ.Ꭸ(-10)){ѥ[2]=ಣ.ᗓ;}else
if(ѩ>Ꮖ.Ꭸ(10)){ѥ[2]=ಣ.ᖽ;}else{ѥ[2]=ಣ.ந;}if(ѥ[1]!=ಣ.ந&&ѥ[2]!=ಣ.ந){var Ã=(ѩ<Ꮖ.Ꮓ)?1:0;var Â=(Ѩ>Ꮖ.Ꮓ)?1:0;ї.ᘽ=ѣ[(Ã<<1)+Â];if(ї.ᘽ
!=ѧ&&ѡ(ї)){return;}}if(l.ષ.ѐ()>200||Ꮖ.Ꭼ(ѩ)>Ꮖ.Ꭼ(Ѩ)){var ў=ѥ[1];ѥ[1]=ѥ[2];ѥ[2]=ў;}if(ѥ[1]==ѧ){ѥ[1]=ಣ.ந;}if(ѥ[2]==ѧ){ѥ[2]=ಣ.ந
;}if(ѥ[1]!=ಣ.ந){ї.ᘽ=ѥ[1];if(ѡ(ї)){return;}}if(ѥ[2]!=ಣ.ந){ї.ᘽ=ѥ[2];if(ѡ(ї)){return;}}if(Ѧ!=ಣ.ந){ї.ᘽ=Ѧ;if(ѡ(ї)){return;}}if
((l.ષ.ѐ()&1)!=0){for(var ѝ=(int)ಣ.ᖿ;ѝ<=(int)ಣ.ᗔ;ѝ++){if((ಣ)ѝ!=ѧ){ї.ᘽ=(ಣ)ѝ;if(ѡ(ї)){return;}}}}else{for(var ѝ=(int)ಣ.ᗔ;ѝ!=
((int)ಣ.ᖿ-1);ѝ--){if((ಣ)ѝ!=ѧ){ї.ᘽ=(ಣ)ѝ;if(ѡ(ї)){return;}}}}if(ѧ!=ಣ.ந){ї.ᘽ=ѧ;if(ѡ(ї)){return;}}ї.ᘽ=ಣ.ந;}private bool ќ(Ɠ ї
){if(ї.ᘻ==null){return false;}var Ψ=ї.ᘻ;var њ=ᵎ.ᴭ(Ψ.ޚ-ї.ޚ,Ψ.ޙ-ї.ޙ);if(њ>=ଊ.ಔ-Ꮖ.Ꭸ(20)+Ψ.ᘿ.ᙊ){return false;}if(!l.બ.ಏ(ї,ї.ᘻ
)){return false;}return true;}private bool ћ(Ɠ ї){if(!l.બ.ಏ(ї,ї.ᘻ)){return false;}if((ї.ᘾ&ళ.ᘙ)!=0){ї.ᘾ&=~ళ.ᘙ;return true;
}if(ї.ᘺ>0){return false;}var њ=ᵎ.ᴭ(ї.ޚ-ї.ᘻ.ޚ,ї.ޙ-ї.ᘻ.ޙ)-Ꮖ.Ꭸ(64);if(ї.ᘿ.ᚘ==0){њ-=Ꮖ.Ꭸ(128);}var љ=њ.Ꮁ>>16;if(ї.ܫ==ё.ы){if(љ
>14*64){return false;}}if(ї.ܫ==ё.щ){if(љ<196){return false;}љ>>=1;}if(ї.ܫ==ё.Ϛ||ї.ܫ==ё.С||ї.ܫ==ё.м){љ>>=1;}if(љ>200){љ=
200;}if(ї.ܫ==ё.Ϛ&&љ>160){љ=160;}if(l.ષ.ѐ()<љ){return false;}return true;}public void ј(Ɠ ї){if(ї.ᘺ>0){ї.ᘺ--;}if(ї.ᘹ>0){if(ї
.ᘻ==null||ї.ᘻ.ƙ<=0){ї.ᘹ=0;}else{ї.ᘹ--;}}if((int)ї.ᘽ<8){ї.ɡ=new ɡ((int)ї.ɡ.Ꮁ&(7<<29));var ş=(int)(ї.ɡ-new ɡ((int)ї.ᘽ<<29))
.Ꮁ;if(ş>0){ї.ɡ-=new ɡ(ɡ.ᓮ.Ꮁ/2);}else if(ş<0){ї.ɡ+=new ɡ(ɡ.ᓮ.Ꮁ/2);}}if(ї.ᘻ==null||(ї.ᘻ.ᘾ&ళ.ᘐ)==0){if(Ҵ(ї,true)){return;}ї.
ᘸ(ї.ᘿ.ᚡ);return;}if((ї.ᘾ&ళ.ᘔ)!=0){ї.ᘾ&=~ళ.ᘔ;if(l.હ.ᴷ!=ᵅ.ᵇ&&!l.હ.ᴴ){Ѫ(ї);}return;}if(ї.ᘿ.ᚘ!=0&&ќ(ї)){if(ї.ᘿ.ᚥ!=0){l.ૐ(ї,ї.
ᘿ.ᚥ,ʡ.Ƅ);}ї.ᘸ(ї.ᘿ.ᚘ);return;}if(ї.ᘿ.ᚗ!=0){if(l.હ.ᴷ<ᵅ.ᵇ&&!l.હ.ᴴ&&ї.ᘼ!=0){goto noMissile;}if(!ћ(ї)){goto noMissile;}ї.ᘸ(ї.ᘿ
.ᚗ);ї.ᘾ|=ళ.ᘔ;return;}noMissile:if(l.હ.ᴶ&&ї.ᘹ==0&&!l.બ.ಏ(ї,ї.ᘻ)){if(Ҵ(ї,true)){return;}}if(--ї.ᘼ<0||!ҡ(ї)){Ѫ(ї);}if(ї.ᘿ.ᚑ
!=0&&l.ષ.ѐ()<3){l.ૐ(ї,ї.ᘿ.ᚑ,ʡ.ʟ);}}public void Ϭ(Ɠ ї){if(ї.ᘿ.ᚙ!=0){l.ૐ(ї,ї.ᘿ.ᚙ,ʡ.ʟ);}}public void Ѥ(Ɠ ї){int Ŗ;switch(ї.ᘿ.
ᚔ){case 0:return;case ɴ.ɏ:case ɴ.ɐ:case ɴ.ɑ:Ŗ=(int)ɴ.ɏ+l.ષ.ѐ()%3;break;case ɴ.ɒ:case ɴ.ɓ:Ŗ=(int)ɴ.ɒ+l.ષ.ѐ()%2;break;
default:Ŗ=(int)ї.ᘿ.ᚔ;break;}if(ї.ܫ==ё.С||ї.ܫ==ё.Ϛ){l.ૐ(ї,(ɴ)Ŗ,ʡ.ʠ);}else{l.ૐ(ї,(ɴ)Ŗ,ʡ.ʟ);}}public void Ѱ(Ɠ ї){l.ૐ(ї,ɴ.ȡ,ʡ.ʟ);}
public void ѱ(Ɠ ї){ї.ᘾ&=~ళ.ᘏ;}public void Ѳ(Ɠ ї){if(ї.ᘻ==null){return;}ї.ᘾ&=~ళ.ᅾ;ї.ɡ=ᵎ.ᴥ(ї.ޚ,ї.ޙ,ї.ᘻ.ޚ,ї.ᘻ.ޙ);var ǵ=l.ષ;if((ї.
ᘻ.ᘾ&ళ.ᘋ)!=0){ї.ɡ+=new ɡ((ǵ.ѐ()-ǵ.ѐ())<<21);}}public void ѳ(Ɠ ї){if(ї.ᘻ==null){return;}Ѳ(ї);var Ş=ї.ɡ;var ѭ=l.ભ.ᮼ(ї,Ş,ଊ.ಓ)
;l.ૐ(ї,ɴ.ɲ,ʡ.Ƅ);var ǵ=l.ષ;Ş+=new ɡ((ǵ.ѐ()-ǵ.ѐ())<<20);var Ь=((ǵ.ѐ()%5)+1)*3;l.ભ.ᮿ(ї,Ş,ଊ.ಓ,ѭ,Ь);}public void Ѵ(Ɠ ї){if(ї.ᘻ
==null){return;}l.ૐ(ї,ɴ.ɱ,ʡ.Ƅ);Ѳ(ї);var Ѯ=ї.ɡ;var ѭ=l.ભ.ᮼ(ї,Ѯ,ଊ.ಓ);var ǵ=l.ષ;for(var Ä=0;Ä<3;Ä++){var Ş=Ѯ+new ɡ((ǵ.ѐ()-ǵ.ѐ
())<<20);var Ь=((ǵ.ѐ()%5)+1)*3;l.ભ.ᮿ(ї,Ş,ଊ.ಓ,ѭ,Ь);}}public void ѯ(Ɠ ї){if(ї.ᘻ==null){return;}l.ૐ(ї,ɴ.ɱ,ʡ.Ƅ);Ѳ(ї);var Ѯ=ї.
ɡ;var ѭ=l.ભ.ᮼ(ї,Ѯ,ଊ.ಓ);var ǵ=l.ષ;var Ş=Ѯ+new ɡ((ǵ.ѐ()-ǵ.ѐ())<<20);var Ь=((ǵ.ѐ()%5)+1)*3;l.ભ.ᮿ(ї,Ş,ଊ.ಓ,ѭ,Ь);}public void Ѭ
(Ɠ ї){Ѳ(ї);if(l.ષ.ѐ()<40){return;}if(ї.ᘻ==null||ї.ᘻ.ƙ<=0||!l.બ.ಏ(ї,ї.ᘻ)){ї.ᘸ(ї.ᘿ.ᚣ);}}public void ѫ(Ɠ ї){if(ї.ᘻ==null){
return;}Ѳ(ї);if(ќ(ї)){l.ૐ(ї,ɴ.ɋ,ʡ.Ƅ);var Ь=(l.ષ.ѐ()%8+1)*3;l.ર.ᅹ(ї.ᘻ,ї,ї,Ь);return;}l.ળ.ᆓ(ї,ї.ᘻ,ё.ϵ);}public void ٺ(Ɠ ї){if(ї.
ᘻ==null){return;}Ѳ(ї);if(ќ(ї)){var Ь=((l.ષ.ѐ()%10)+1)*4;l.ર.ᅹ(ї.ᘻ,ї,ї,Ь);}}public void ݺ(Ɠ ї){if(ї.ᘻ==null){return;}Ѳ(ї);
if(ќ(ї)){var Ь=(l.ષ.ѐ()%6+1)*10;l.ર.ᅹ(ї.ᘻ,ї,ї,Ь);return;}l.ળ.ᆓ(ї,ї.ᘻ,ё.Ϸ);}public void ݻ(Ɠ ї){if(ї.ᘻ==null){return;}if(ќ(ї
)){l.ૐ(ї,ɴ.ɋ,ʡ.Ƅ);var Ь=(l.ષ.ѐ()%8+1)*10;l.ર.ᅹ(ї.ᘻ,ї,ї,Ь);return;}l.ળ.ᆓ(ї,ї.ᘻ,ё.о);}private static Ꮖ ݼ=Ꮖ.Ꭸ(20);public
void ݽ(Ɠ ї){if(ї.ᘻ==null){return;}var Ε=ї.ᘻ;ї.ᘾ|=ళ.ᘅ;l.ૐ(ї,ї.ᘿ.ᚥ,ʡ.ʟ);Ѳ(ї);var Ş=ї.ɡ;ї.ᙂ=ݼ*ஜ.ௐ(Ş);ї.ᙁ=ݼ*ஜ.ஸ(Ş);var њ=ᵎ.ᴭ(Ε.ޚ
-ї.ޚ,Ε.ޙ-ї.ޙ);var ܣ=(Ε.ኴ+(Ε.ǡ>>1)-ї.ኴ).Ꮁ;var ܢ=њ.Ꮁ/ݼ.Ꮁ;if(ܢ<1){ܢ=1;}ї.ᙀ=new Ꮖ(ܣ/ܢ);}public void ݾ(Ɠ ї){Ѳ(ї);l.ૐ(ї,ɴ.ʺ,ʡ.ʟ
);}private static ɡ ݹ=ɡ.ᓮ/8;public void ݸ(Ɠ ї){Ѳ(ї);var æ=l.ળ;ї.ɡ+=ݹ;var Ψ=l.ઽ(ї.ᘻ);æ.ᆓ(ї,Ψ,ё.х);var ݞ=æ.ᆓ(ї,Ψ,ё.х);ݞ.ɡ+=
ݹ;var Ş=ݞ.ɡ;ݞ.ᙂ=new Ꮖ(ݞ.ᘿ.ݏ)*ஜ.ௐ(Ş);ݞ.ᙁ=new Ꮖ(ݞ.ᘿ.ݏ)*ஜ.ஸ(Ş);}public void ݷ(Ɠ ї){Ѳ(ї);var æ=l.ળ;ї.ɡ-=ݹ;var Ψ=l.ઽ(ї.ᘻ);æ.ᆓ(
ї,Ψ,ё.х);var ݞ=æ.ᆓ(ї,Ψ,ё.х);ݞ.ɡ-=ݹ*2;var Ş=ݞ.ɡ;ݞ.ᙂ=new Ꮖ(ݞ.ᘿ.ݏ)*ஜ.ௐ(Ş);ݞ.ᙁ=new Ꮖ(ݞ.ᘿ.ݏ)*ஜ.ஸ(Ş);}public void ݶ(Ɠ ї){Ѳ(ї);
var æ=l.ળ;var Ψ=l.ઽ(ї.ᘻ);var ލ=æ.ᆓ(ї,Ψ,ё.х);ލ.ɡ-=ݹ/2;var ވ=ލ.ɡ;ލ.ᙂ=new Ꮖ(ލ.ᘿ.ݏ)*ஜ.ௐ(ވ);ލ.ᙁ=new Ꮖ(ލ.ᘿ.ݏ)*ஜ.ஸ(ވ);var މ=æ.ᆓ(ї,
Ψ,ё.х);މ.ɡ+=ݹ/2;var ފ=މ.ɡ;މ.ᙂ=new Ꮖ(މ.ᘿ.ݏ)*ஜ.ௐ(ފ);މ.ᙁ=new Ꮖ(މ.ᘿ.ݏ)*ஜ.ஸ(ފ);}public void ދ(Ɠ ї){if(ї.ᘻ==null){return;}Ѳ(ї);
l.ળ.ᆓ(ї,ї.ᘻ,ё.ϼ);}public void ތ(Ɠ ї){Ѳ(ї);if(l.ષ.ѐ()<10){return;}if(ї.ᘻ==null||ї.ᘻ.ƙ<=0||!l.બ.ಏ(ї,ї.ᘻ)){ї.ᘸ(ї.ᘿ.ᚣ);}}
public void ގ(Ɠ ї){if(ї.ᘻ==null){return;}Ѳ(ї);l.ળ.ᆓ(ї,ї.ᘻ,ё.ϸ);}public void ޏ(Ɠ ї){l.ર.ᆐ(ї,ї.ᘻ,128);}public void ސ(Ɠ ї){l.ૐ(ї,
ɴ.ʬ,ʡ.ʞ);ј(ї);}public void ޑ(Ɠ ї){l.ૐ(ї,ɴ.ȶ,ʡ.ʞ);ј(ї);}public void ޒ(Ɠ ї){l.ૐ(ї,ɴ.ʫ,ʡ.ʞ);ј(ї);}private Func<Ɠ,bool>އ;
private Ɠ ކ;private Ꮖ ޅ;private Ꮖ ބ;private void ރ(){އ=ނ;}private bool ނ(Ɠ ğ){if((ğ.ᘾ&ళ.ᘉ)==0){return true;}if(ğ.ŵ!=-1){return
true;}if(ğ.ᘿ.ᚐ==ᚏ.ᚠ){return true;}var ށ=ğ.ᘿ.ᙊ+ኑ.ዃ[(int)ё.ы].ᙊ;if(Ꮖ.Ꭼ(ğ.ޚ-ޅ)>ށ||Ꮖ.Ꭼ(ğ.ޙ-ބ)>ށ){return true;}ކ=ğ;ކ.ᙂ=ކ.ᙁ=Ꮖ.Ꮓ;ކ.
ǡ<<=2;var ˠ=l.લ.ბ(ކ,ކ.ޚ,ކ.ޙ);ކ.ǡ>>=2;if(!ˠ){return true;}return false;}public void ހ(Ɠ ї){if(ї.ᘽ!=ಣ.ந){ޅ=ї.ޚ+ї.ᘿ.ݏ*ң[(int
)ї.ᘽ];ބ=ї.ޙ+ї.ᘿ.ݏ*Ң[(int)ї.ᘽ];var Ν=l.શ.ᑋ;var ݿ=ᱺ.ᴂ*2;var ܞ=Ν.ᑗ(ޅ-ݿ);var ܜ=Ν.ᑗ(ޅ+ݿ);var ܝ=Ν.ᑖ(ބ-ݿ);var ܛ=Ν.ᑖ(ބ+ݿ);for(var
ܓ=ܞ;ܓ<=ܜ;ܓ++){for(var ܧ=ܝ;ܧ<=ܛ;ܧ++){if(!Ν.ᒟ(ܓ,ܧ,އ)){var ў=ї.ᘻ;ї.ᘻ=ކ;Ѳ(ї);ї.ᘻ=ў;ї.ᘸ(ᚏ.ᬠ);l.ૐ(ކ,ɴ.ȡ,ʡ.ʝ);var ݤ=ކ.ᘿ;ކ.ᘸ(ݤ.ᚐ)
;ކ.ǡ<<=2;ކ.ᘾ=ݤ.ᘾ;ކ.ƙ=ݤ.ᚢ;ކ.ᘻ=null;return;}}}}ј(ї);}public void ݥ(Ɠ ї){l.ૐ(ї,ɴ.Ɋ,ʡ.Ƅ);}public void ݦ(Ɠ ї){l.ૐ(ї,ɴ.ʹ,ʡ.Ƅ);ъ
(ї);}public void ݧ(Ɠ ї){l.ૐ(ї,ɴ.ʲ,ʡ.Ƅ);ъ(ї);}public void ъ(Ɠ ї){var Ε=ї.ш;if(Ε==null){return;}var Ψ=l.ઽ(ї.ᘻ);if(!l.બ.ಏ(Ψ,
Ε)){return;}l.લ.ნ(ї);var Ş=Ε.ɡ;ї.ޚ=Ε.ޚ+Ꮖ.Ꭸ(24)*ஜ.ௐ(Ş);ї.ޙ=Ε.ޙ+Ꮖ.Ꭸ(24)*ஜ.ஸ(Ş);ї.ኴ=Ε.ኴ;l.લ.ლ(ї);}public void ݨ(Ɠ ї){if(ї.ᘻ
==null){return;}Ѳ(ї);var ݢ=l.ળ.ᆕ(ї.ᘻ.ޚ,ї.ᘻ.ޚ,ї.ᘻ.ኴ,ё.ъ);ї.ш=ݢ;ݢ.ᘻ=ї;ݢ.ш=ї.ᘻ;ъ(ݢ);}public void ݡ(Ɠ ї){if(ї.ᘻ==null){return;
}Ѳ(ї);if(!l.બ.ಏ(ї,ї.ᘻ)){return;}l.ૐ(ї,ɴ.ɺ,ʡ.Ƅ);l.ર.ᅹ(ї.ᘻ,ї,ї,20);ї.ᘻ.ᙀ=Ꮖ.Ꭸ(1000)/ї.ᘻ.ᘿ.ᚓ;var ݠ=ї.ш;if(ݠ==null){return;}
var Ş=ї.ɡ;ݠ.ޚ=ї.ᘻ.ޚ-Ꮖ.Ꭸ(24)*ஜ.ௐ(Ş);ݠ.ޙ=ї.ᘻ.ޙ-Ꮖ.Ꭸ(24)*ஜ.ஸ(Ş);l.ર.ᆐ(ݠ,ї,70);}public void ݟ(Ɠ ї){if(ї.ᘻ==null){return;}Ѳ(ї);ї.
ኴ+=Ꮖ.Ꭸ(16);var ݞ=l.ળ.ᆓ(ї,ї.ᘻ,ё.ш);ї.ኴ-=Ꮖ.Ꭸ(16);ݞ.ޚ+=ݞ.ᙂ;ݞ.ޙ+=ݞ.ᙁ;ݞ.ш=ї.ᘻ;}private static ɡ ݝ=new ɡ(0xc000000);public void
ш(Ɠ ї){if((l.ଗ&3)!=0){return;}l.ભ.ᯀ(ї.ޚ,ї.ޙ,ї.ኴ);var ݜ=l.ળ.ᆕ(ї.ޚ-ї.ᙂ,ї.ޙ-ї.ᙁ,ї.ኴ,ё.ч);ݜ.ᙀ=Ꮖ.Ꮒ;ݜ.ŵ-=l.ષ.ѐ()&3;if(ݜ.ŵ<1){ݜ.
ŵ=1;}var Ε=ї.ш;if(Ε==null||Ε.ƙ<=0){return;}var ݛ=ᵎ.ᴥ(ї.ޚ,ї.ޙ,Ε.ޚ,Ε.ޙ);if(ݛ!=ї.ɡ){if(ݛ-ї.ɡ>ɡ.ᓣ){ї.ɡ-=ݝ;if(ݛ-ї.ɡ<ɡ.ᓣ){ї.ɡ=ݛ
;}}else{ї.ɡ+=ݝ;if(ݛ-ї.ɡ>ɡ.ᓣ){ї.ɡ=ݛ;}}}ݛ=ї.ɡ;ї.ᙂ=new Ꮖ(ї.ᘿ.ݏ)*ஜ.ௐ(ݛ);ї.ᙁ=new Ꮖ(ї.ᘿ.ݏ)*ஜ.ஸ(ݛ);var њ=ᵎ.ᴭ(Ε.ޚ-ї.ޚ,Ε.ޙ-ї.ޙ);
var ܣ=(Ε.ኴ+Ꮖ.Ꭸ(40)-ї.ኴ).Ꮁ;var ܢ=њ.Ꮁ/ї.ᘿ.ݏ;if(ܢ<1){ܢ=1;}var ѭ=new Ꮖ(ܣ/ܢ);if(ѭ<ї.ᙀ){ї.ᙀ-=Ꮖ.Ꮒ/8;}else{ї.ᙀ+=Ꮖ.Ꮒ/8;}}public void
ݱ(Ɠ ї){if(ї.ᘻ==null){return;}Ѳ(ї);l.ૐ(ї,ɴ.ɍ,ʡ.Ƅ);}public void ݯ(Ɠ ї){if(ї.ᘻ==null){return;}Ѳ(ї);if(ќ(ї)){var Ь=((l.ષ.ѐ()%
10)+1)*6;l.ૐ(ї,ɴ.ɉ,ʡ.Ƅ);l.ર.ᅹ(ї.ᘻ,ї,ї,Ь);}}private void ݰ(Ɠ ї,ɡ Ş){var ú=0;foreach(var â in l.વ){var ã=â as Ɠ;if(ã!=null&&
ã.ܫ==ё.м){ú++;}}if(ú>20){return;}var ݲ=Ꮖ.Ꭸ(4)+3*(ї.ᘿ.ᙊ+ኑ.ዃ[(int)ё.м].ᙊ)/2;var ǃ=ї.ޚ+ݲ*ஜ.ௐ(Ş);var ǂ=ї.ޙ+ݲ*ஜ.ஸ(Ş);var ݳ=ї.ኴ
+Ꮖ.Ꭸ(8);var ݴ=l.ળ.ᆕ(ǃ,ǂ,ݳ,ё.м);if(!l.લ.შ(ݴ,ݴ.ޚ,ݴ.ޙ)){l.ર.ᅹ(ݴ,ї,ї,10000);return;}ݴ.ᘻ=ї.ᘻ;ݽ(ݴ);}public void ݵ(Ɠ ї){if(ї.ᘻ==
null){return;}Ѳ(ї);ݰ(ї,ї.ɡ);}public void ݮ(Ɠ ї){ѱ(ї);ݰ(ї,ї.ɡ+ɡ.ᓮ);ݰ(ї,ї.ɡ+ɡ.ᓣ);ݰ(ї,ї.ɡ+ɡ.ᓢ);}private ɢ ݭ;private void ݬ(){
var ݫ=new ட(Ꮖ.Ꮓ,Ꮖ.Ꮓ);ݭ=new ɢ(ݫ,ݫ,0,0,0,null,null);}public void ݪ(Ɠ ї){var Å=l.હ;if(Å.ಖ==ಖ.ᴉ){if(Å.શ!=7){return;}if((ї.ܫ!=ё.
ц)&&(ї.ܫ!=ё.Р)){return;}}else{switch(Å.ᰔ){case 1:if(Å.શ!=8){return;}if(ї.ܫ!=ё.п){return;}break;case 2:if(Å.શ!=8){return;}
if(ї.ܫ!=ё.Ϛ){return;}break;case 3:if(Å.શ!=8){return;}if(ї.ܫ!=ё.С){return;}break;case 4:switch(Å.શ){case 6:if(ї.ܫ!=ё.Ϛ){
return;}break;case 8:if(ї.ܫ!=ё.С){return;}break;default:return;}break;default:if(Å.શ!=8){return;}break;}}var Ë=l.હ.ᰁ;int Ä;for
(Ä=0;Ä<ђ.ۇ;Ä++){if(Ë[Ä].Ÿ&&Ë[Ä].ƙ>0){break;}}if(Ä==ђ.ۇ){return;}foreach(var â in l.વ){var ݣ=â as Ɠ;if(ݣ==null){continue;}
if(ݣ!=ї&&ݣ.ܫ==ї.ܫ&&ݣ.ƙ>0){return;}}if(Å.ಖ==ಖ.ᴉ){if(Å.શ==7){if(ї.ܫ==ё.ц){ݭ.ę=666;l.Đ.Ί(ݭ,ᴀ.ᳵ);return;}if(ї.ܫ==ё.Р){ݭ.ę=667;
l.Đ.Ί(ݭ,ᴀ.ᳮ);return;}}}else{switch(Å.ᰔ){case 1:ݭ.ę=666;l.Đ.Ί(ݭ,ᴀ.ᳵ);return;case 4:switch(Å.શ){case 6:ݭ.ę=666;l.Đ.Ό(ݭ,ಫ.ಳ)
;return;case 8:ݭ.ę=666;l.Đ.Ί(ݭ,ᴀ.ᳵ);return;}break;}}l.ଞ();}public void ݩ(Ɠ ї){ѱ(ї);foreach(var â in l.વ){var ݣ=â as Ɠ;if(
ݣ==null){continue;}if(ݣ!=ї&&ݣ.ܫ==ї.ܫ&&ݣ.ƙ>0){return;}}ݭ.ę=666;l.Đ.Ό(ݭ,ಫ.ರ);}private Ɠ[]ޓ;private int ޟ;private int ߤ;
private bool ߥ;private void ߦ(){ޓ=new Ɠ[32];ޟ=0;ߤ=0;ߥ=false;}public void ߧ(Ɠ ї){ޟ=0;ߤ=0;foreach(var â in l.વ){var ã=â as Ɠ;if(ã
==null){continue;}if(ã.ܫ==ё.ϱ){ޓ[ޟ]=ã;ޟ++;}}l.ૐ(ї,ɴ.ʶ,ʡ.ʠ);}public void Ӱ(Ɠ ї){l.ૐ(ї,ɴ.ʷ,ʡ.ʠ);}public void ߨ(Ɠ ї){var ǵ=l.
ષ;for(var ǃ=ї.ޚ-Ꮖ.Ꭸ(196);ǃ<ї.ޚ+Ꮖ.Ꭸ(320);ǃ+=Ꮖ.Ꭸ(8)){var ǂ=ї.ޙ-Ꮖ.Ꭸ(320);var ݳ=new Ꮖ(128)+ǵ.ѐ()*Ꮖ.Ꭸ(2);var ߢ=l.ળ.ᆕ(ǃ,ǂ,ݳ,ё.ϸ
);ߢ.ᙀ=new Ꮖ(ǵ.ѐ()*512);ߢ.ᘸ(ᚏ.ӟ);ߢ.ŵ-=ǵ.ѐ()&7;if(ߢ.ŵ<1){ߢ.ŵ=1;}}l.ૐ(ї,ɴ.ʸ,ʡ.ʠ);}public void ߣ(Ɠ ї){var ǵ=l.ષ;var ǃ=ї.ޚ+new
Ꮖ((ǵ.ѐ()-ǵ.ѐ())*2048);var ǂ=ї.ޙ;var ݳ=new Ꮖ(128)+ǵ.ѐ()*Ꮖ.Ꭸ(2);var ߢ=l.ળ.ᆕ(ǃ,ǂ,ݳ,ё.ϸ);ߢ.ᙀ=new Ꮖ(ǵ.ѐ()*512);ߢ.ᘸ(ᚏ.ӟ);ߢ.ŵ-=ǵ
.ѐ()&7;if(ߢ.ŵ<1){ߢ.ŵ=1;}}public void ߡ(Ɠ ї){l.ଞ();}public void ߠ(Ɠ ї){ߥ=!ߥ;if(l.હ.ᴷ<=ᵅ.ᅼ&&(!ߥ)){return;}if(ޟ==0){ߧ(ї);}
var Ψ=ޓ[ߤ];ߤ=(ߤ+1)%ޟ;var ݞ=l.ળ.ᆓ(ї,Ψ,ё.ϳ);ݞ.ᘻ=Ψ;ݞ.ᘺ=((Ψ.ޙ-ї.ޙ).Ꮁ/ݞ.ᙁ.Ꮁ)/ݞ.Ŷ.ŵ;l.ૐ(ї,ɴ.ʴ,ʡ.ʠ);}public void ߟ(Ɠ ї){l.ૐ(ї,ɴ.ʵ,
ʡ.ʝ);ߞ(ї);}public void ߞ(Ɠ ї){if(--ї.ᘺ>0){return;}var Ψ=ї.ᘻ;if(Ψ==null){Ψ=ї;ї.ኴ=ї.ฃ.Ŀ.Ģ;}var æ=l.ળ;var ݢ=æ.ᆕ(Ψ.ޚ,Ψ.ޙ,Ψ.ኴ,
ё.ϻ);l.ૐ(ݢ,ɴ.ȝ,ʡ.ʝ);var ڬ=l.ષ.ѐ();ё ˌ;if(ڬ<50){ˌ=ё.у;}else if(ڬ<90){ˌ=ё.т;}else if(ڬ<120){ˌ=ё.с;}else if(ڬ<130){ˌ=ё.Ϭ;}
else if(ڬ<160){ˌ=ё.р;}else if(ڬ<162){ˌ=ё.ы;}else if(ڬ<172){ˌ=ё.щ;}else if(ڬ<192){ˌ=ё.Р;}else if(ڬ<222){ˌ=ё.ц;}else if(ڬ<246)
{ˌ=ё.н;}else{ˌ=ё.п;}var ߴ=æ.ᆕ(Ψ.ޚ,Ψ.ޙ,Ψ.ኴ,ˌ);if(Ҵ(ߴ,true)){ߴ.ᘸ(ߴ.ᘿ.ᚣ);}l.લ.Ⴣ(ߴ,ߴ.ޚ,ߴ.ޙ);l.ળ.ᆖ(ї);}}sealed class ߵ{private
static int ľ=28;private Ꮖ ǃ;private Ꮖ ǂ;private Ꮖ ߝ;private Ꮖ Ǆ;private Ꮖ[][]ߺ;private int[]ࠀ;public ߵ(Ꮖ ǃ,Ꮖ ǂ,Ꮖ ߝ,Ꮖ Ǆ,Ꮖ ޠ,Ꮖ ޡ
,Ꮖ ޢ,Ꮖ ޣ,Ꮖ ޤ,Ꮖ ޥ,Ꮖ ߊ,Ꮖ ߎ,int ߋ,int ߌ){this.ǃ=ǃ;this.ǂ=ǂ;this.ߝ=ߝ;this.Ǆ=Ǆ;var ߪ=new Ꮖ[4]{ޠ,ޡ,ޢ,ޣ};var ߩ=new Ꮖ[4]{ޤ,ޥ,ߊ,ߎ}
;ߺ=new Ꮖ[][]{ߪ,ߩ};ࠀ=new int[]{ߋ,ߌ};}public static ߵ ċ(byte[]f,int ù){var ǃ=BitConverter.ToInt16(f,ù);var ǂ=BitConverter.
ToInt16(f,ù+2);var ߝ=BitConverter.ToInt16(f,ù+4);var Ǆ=BitConverter.ToInt16(f,ù+6);var ޠ=BitConverter.ToInt16(f,ù+8);var ޡ=
BitConverter.ToInt16(f,ù+10);var ޢ=BitConverter.ToInt16(f,ù+12);var ޣ=BitConverter.ToInt16(f,ù+14);var ޤ=BitConverter.ToInt16(f,ù+16
);var ޥ=BitConverter.ToInt16(f,ù+18);var ߊ=BitConverter.ToInt16(f,ù+20);var ߎ=BitConverter.ToInt16(f,ù+22);var ߋ=
BitConverter.ToInt16(f,ù+24);var ߌ=BitConverter.ToInt16(f,ù+26);return new ߵ(Ꮖ.Ꭸ(ǃ),Ꮖ.Ꭸ(ǂ),Ꮖ.Ꭸ(ߝ),Ꮖ.Ꭸ(Ǆ),Ꮖ.Ꭸ(ޠ),Ꮖ.Ꭸ(ޡ),Ꮖ.Ꭸ(ޢ),Ꮖ.Ꭸ(ޣ)
,Ꮖ.Ꭸ(ޤ),Ꮖ.Ꭸ(ޥ),Ꮖ.Ꭸ(ߊ),Ꮖ.Ꭸ(ߎ),ߋ,ߌ);}public static ߵ[]ÿ(ಆ þ,int ý,ฃ[]ߍ){var û=þ.ಡ(ý);if(û%ߵ.ľ!=0){throw new Exception();}
var f=þ.ಠ(ý);var ú=û/ߵ.ľ;var ޞ=new ߵ[ú];for(var Ä=0;Ä<ú;Ä++){var ù=ߵ.ľ*Ä;ޞ[Ä]=ߵ.ċ(f,ù);}return ޞ;}public static bool ޝ(int
ޛ){return(ޛ&unchecked((int)0xFFFF8000))!=0;}public static int ޜ(int ޛ){return ޛ^unchecked((int)0xFFFF8000);}public Ꮖ ޚ=>ǃ
;public Ꮖ ޙ=>ǂ;public Ꮖ ޘ=>ߝ;public Ꮖ ޗ=>Ǆ;public Ꮖ[][]ޖ=>ߺ;public int[]ޕ=>ࠀ;}sealed class ޔ:ᯈ{private static ޔ ޱ;public
static ޔ ߏ(){if(ޱ==null){ޱ=new ޔ();}return ޱ;}public void ᯉ(ᑧ ߚ,bool ߗ){}public int ᯊ{get{return 15;}}public int ᯋ{get{return
0;}set{}}}sealed class ߘ:ᶫ{private static ߘ ޱ;public static ߘ ߏ(){if(ޱ==null){ޱ=new ߘ();}return ޱ;}public void ᶪ(Ɠ ߙ){}
public void ڞ(){}public void ૐ(ɴ Ǯ){}public void ૐ(Ɠ ã,ɴ Ǯ,ʡ ˌ){}public void ૐ(Ɠ ã,ɴ Ǯ,ʡ ˌ,int ߛ){}public void ૠ(Ɠ ã){}public
void ʕ(){}public void ஓ(){}public void ḁ(){}public int ᯊ{get{return 15;}}public int ᯋ{get{return 0;}set{}}}sealed class ߜ:Ṉ{
private static ߜ ޱ;public static ߜ ߏ(){if(ޱ==null){ޱ=new ߜ();}return ޱ;}public void ṃ(థ Ƥ){Ƥ.ŷ();}public void ʕ(){}public void
Ṃ(){}public void ṁ(){}public int Ṁ{get{return 9;}}public int ḿ{get{return 3;}set{}}}class ߖ:Ḿ{private static ߖ ޱ;public
static ߖ ߏ(){if(ޱ==null){ޱ=new ߖ();}return ޱ;}public void Ǌ(ጟ ǋ,Ꮖ č){}public void ƾ(){}public bool ḽ(){return true;}public int
ǜ=>ᄥ.ᄤ;public int ǝ{get{return 7;}set{}}public bool Ǟ{get{return true;}set{}}public int ǟ=>10;public int Ǡ{get{return 2;}
set{}}public int ǚ=>321;public int Ǜ=>200;}sealed class ߕ{private ᴍ Ǽ;private ᴆ Å;private ڛ Í;private int ߔ;private int ߓ;
private int ú;private int ߒ;private థ[]ߑ;private ژ ߐ;private ᕣ Æ;private bool ʙ;public ߕ(ᴍ Ǽ,ᴆ Å){this.Ǽ=Ǽ;this.Å=Å;ߑ=new థ[ђ.ۇ
];for(var Ä=0;Ä<ђ.ۇ;Ä++){ߑ[Ä]=new థ();}ߔ=0;ߓ=0;ʙ=false;ڡ();}public void ʕ(){ߔ=0;ߓ=0;ߐ=null;Æ=null;ʙ=true;ڡ();}public ன ڞ(
){var ڟ=ன.ந;if(ߓ!=ߔ){switch(ߓ){case 0:ڡ();break;case 1:ڢ("DEMO1");break;case 2:ڥ();break;case 3:ڢ("DEMO2");break;case 4:ڡ
();break;case 5:ڢ("DEMO3");break;case 6:ڥ();break;case 7:ڢ("DEMO4");break;}ߔ=ߓ;ڟ=ன.ண;}switch(ߔ){case 0:ú++;if(ú==ߒ){ߓ=1;}
break;case 1:if(!ߐ.ᗀ(ߑ)){ߓ=2;}else{Æ.ڞ(ߑ);}break;case 2:ú++;if(ú==ߒ){ߓ=3;}break;case 3:if(!ߐ.ᗀ(ߑ)){ߓ=4;}else{Æ.ڞ(ߑ);}break;
case 4:ú++;if(ú==ߒ){ߓ=5;}break;case 5:if(!ߐ.ᗀ(ߑ)){if(Ǽ.ಆ.ಞ("DEMO4")==-1){ߓ=0;}else{ߓ=6;}}else{Æ.ڞ(ߑ);}break;case 6:ú++;if(ú
==ߒ){ߓ=7;}break;case 7:if(!ߐ.ᗀ(ߑ)){ߓ=0;}else{Æ.ڞ(ߑ);}break;}if(Í==ڛ.ښ&&ú==1){if(Å.ಖ==ಖ.ᴉ){Å.ᴾ.ᯉ(ᑧ.ᐶ,false);}else{Å.ᴾ.ᯉ(ᑧ.ᑬ
,false);}}if(ʙ){ʙ=false;return ன.ண;}else{return ڟ;}}private void ڡ(){Í=ڛ.ښ;ú=0;if(Å.ಖ==ಖ.ᴉ){ߒ=35*11;}else{ߒ=170;}}private
void ڥ(){Í=ڛ.ڙ;ú=0;ߒ=200;}private void ڢ(string ý){Í=ڛ.ژ;ߐ=new ژ(Ǽ.ಆ.ಠ(ý));ߐ.હ.ಗ=Å.ಗ;ߐ.હ.ಖ=Å.ಖ;ߐ.હ.ಕ=Å.ಕ;ߐ.હ.ᴱ=Å.ᴱ;ߐ.હ.ᴽ=Å.ᴽ
;ߐ.હ.ᴾ=Å.ᴾ;Æ=new ᕣ(Ǽ,ߐ.હ);Æ.ᔼ();}public ڛ Ŷ=>Í;public ᕣ ڣ=>Æ;}class ڤ{private Ꮝ ȋ;private Ȉ ڝ;private ٻ چ;public ڤ(ಆ þ,Ꮝ
ȋ,Ȉ ڝ){this.ȋ=ȋ;this.ڝ=ڝ;چ=new ٻ(þ);}public void Ǌ(ߕ ڜ,Ꮖ č){var ǉ=ȋ.Ǒ/320;switch(ڜ.Ŷ){case ڛ.ښ:ȋ.Ꮘ(چ["TITLEPIC"],0,0,ǉ);
break;case ڛ.ژ:ڝ.ǹ(ڜ.ڣ,č);break;case ڛ.ڙ:ȋ.Ꮘ(چ["CREDIT"],0,0,ǉ);break;}}}public enum ڛ{ښ,ڙ,ژ}sealed class ڗ{public static int
ږ=1;public static int ƞ=8;public static int ڠ=9;public static int ư=4;public static int Ū=13;private byte[]f;private uint
[][]ڨ;public ڗ(ಆ þ){try{ᕣ.ᔵ.འ("Load palette: ");f=þ.ಠ("PLAYPAL");var ú=f.Length/(3*256);ڨ=new uint[ú][];for(var Ä=0;Ä<ڨ.
Length;Ä++){ڨ[Ä]=new uint[256];}ᕣ.ᔵ.འ("OK");}catch(Exception e){ᕣ.ᔵ.འ("Failed");throw e;}}public void ک(double É){for(var Ä=0;
Ä<ڨ.Length;Ä++){var ڪ=(3*256)*Ä;for(var ͼ=0;ͼ<256;ͼ++){var ګ=ڪ+3*ͼ;var ڬ=f[ګ];var ڭ=f[ګ+1];var Â=f[ګ+2];ڬ=(byte)Math.
Round(255*ڮ(ڬ/255.0,É));ڭ=(byte)Math.Round(255*ڮ(ڭ/255.0,É));Â=(byte)Math.Round(255*ڮ(Â/255.0,É));ڨ[Ä][ͼ]=(uint)((ڬ<<0)|(ڭ<<8
)|(Â<<16)|(255<<24));}}}private static double ڮ(double ǃ,double É){return Math.Pow(ǃ,É);}public uint[]this[int ڧ]{get{
return ڨ[ڧ];}}}sealed class ڦ{private string Ĭ;private int ځ;private int ˢ;private int ڃ;private int ڄ;private ᖒ[][]څ;public ڦ
(string Ĭ,int ځ,int ˢ,int ڃ,int ڄ,ᖒ[][]څ){this.Ĭ=Ĭ;this.ځ=ځ;this.ˢ=ˢ;this.ڃ=ڃ;this.ڄ=ڄ;this.څ=څ;}public static ڦ ċ(string
Ĭ,byte[]f){var ځ=BitConverter.ToInt16(f,0);var ˢ=BitConverter.ToInt16(f,2);var ڃ=BitConverter.ToInt16(f,4);var ڄ=
BitConverter.ToInt16(f,6);ڈ(ref f,ځ);var څ=new ᖒ[ځ][];for(var ǃ=0;ǃ<ځ;ǃ++){var ڇ=new List<ᖒ>();var É=BitConverter.ToInt32(f,8+4*ǃ);
while(true){var ٿ=f[É];if(ٿ==ᖒ.జ){break;}var û=f[É+1];var ù=É+3;ڇ.Add(new ᖒ(ٿ,f,ù,û));É+=û+4;}څ[ǃ]=ڇ.ToArray();}return new ڦ(
Ĭ,ځ,ˢ,ڃ,ڄ,څ);}public static ڦ ÿ(ಆ þ,string Ĭ){return ċ(Ĭ,þ.ಠ(Ĭ));}private static void ڈ(ref byte[]f,int ځ){var ڀ=0;for(
var ǃ=0;ǃ<ځ;ǃ++){var É=BitConverter.ToInt32(f,8+4*ǃ);while(true){var ٿ=f[É];if(ٿ==ᖒ.జ){break;}var û=f[É+1];var ù=É+3;ڀ=Math
.Max(ù+128,ڀ);É+=û+4;}}if(f.Length<ڀ){Array.Resize(ref f,ڀ);}}public override string ToString(){return Ĭ;}public string Ń
=>Ĭ;public int Ǒ=>ځ;public int ǡ=>ˢ;public int پ=>ڃ;public int ٽ=>ڄ;public ᖒ[][]ټ=>څ;}sealed class ٻ{private ಆ þ;private
Dictionary<string,ڦ>چ;public ٻ(ಆ þ){this.þ=þ;چ=new Dictionary<string,ڦ>();}public ڦ this[string Ĭ]{get{ڦ ڌ;if(!چ.TryGetValue(Ĭ,out
ڌ)){ڌ=ڦ.ÿ(þ,Ĭ);چ.Add(Ĭ,ڌ);}return ڌ;}}public int ڍ(string Ĭ){return this[Ĭ].Ǒ;}public int ڎ(string Ĭ){return this[Ĭ].ǡ;}}
sealed class ڏ{private ଦ l;private ᯍ[]ڕ;private int ڐ;private bool ڑ;private ᗕ Ψ;private ᗕ ڒ;private Func<ɢ,bool>ړ;private
Func<Ɠ,bool>ڔ;public ڏ(ଦ l){this.l=l;ڕ=new ᯍ[256];for(var Ä=0;Ä<ڕ.Length;Ä++){ڕ[Ä]=new ᯍ();}Ψ=new ᗕ();ڒ=new ᗕ();ړ=ڋ;ڔ=ڊ;}
private bool ڋ(ɢ ò){int Ȅ;int ȅ;if(ڒ.ޘ>Ꮖ.Ꭸ(16)||ڒ.ޗ>Ꮖ.Ꭸ(16)||ڒ.ޘ<-Ꮖ.Ꭸ(16)||ڒ.ޗ<-Ꮖ.Ꭸ(16)){Ȅ=ᵎ.ᴯ(ò.ɝ.ޚ,ò.ɝ.ޙ,ڒ);ȅ=ᵎ.ᴯ(ò.ɞ.ޚ,ò.ɞ.ޙ
,ڒ);}else{Ȅ=ᵎ.ᴫ(ڒ.ޚ,ڒ.ޙ,ò);ȅ=ᵎ.ᴫ(ڒ.ޚ+ڒ.ޘ,ڒ.ޙ+ڒ.ޗ,ò);}if(Ȅ==ȅ){return true;}Ψ.ᗗ(ò);var ڂ=گ(ڒ,Ψ);if(ڂ<Ꮖ.Ꮓ){return true;}if(
ڑ&&ڂ<Ꮖ.Ꮒ&&ò.ɤ==null){return false;}ڕ[ڐ].ᯌ(ڂ,ò);ڐ++;return true;}private bool ڊ(Ɠ ğ){var ډ=(ڒ.ޘ.Ꮁ^ڒ.ޗ.Ꮁ)>0;Ꮖ ǎ;Ꮖ ǆ;Ꮖ ǐ;Ꮖ ǅ
;if(ډ){ǎ=ğ.ޚ-ğ.ᙊ;ǆ=ğ.ޙ+ğ.ᙊ;ǐ=ğ.ޚ+ğ.ᙊ;ǅ=ğ.ޙ-ğ.ᙊ;}else{ǎ=ğ.ޚ-ğ.ᙊ;ǆ=ğ.ޙ-ğ.ᙊ;ǐ=ğ.ޚ+ğ.ᙊ;ǅ=ğ.ޙ+ğ.ᙊ;}var Ȅ=ᵎ.ᴯ(ǎ,ǆ,ڒ);var ȅ=ᵎ.ᴯ(
ǐ,ǅ,ڒ);if(Ȅ==ȅ){return true;}Ψ.ޚ=ǎ;Ψ.ޙ=ǆ;Ψ.ޘ=ǐ-ǎ;Ψ.ޗ=ǅ-ǆ;var ڂ=گ(ڒ,Ψ);if(ڂ<Ꮖ.Ꮓ){return true;}ڕ[ڐ].ᯌ(ڂ,ğ);ڐ++;return true;
}private Ꮖ گ(ᗕ ھ,ᗕ ܡ){var ܢ=(ܡ.ޗ>>8)*ھ.ޘ-(ܡ.ޘ>>8)*ھ.ޗ;if(ܢ==Ꮖ.Ꮓ){return Ꮖ.Ꮓ;}var ܣ=((ܡ.ޚ-ھ.ޚ)>>8)*ܡ.ޗ+((ھ.ޙ-ܡ.ޙ)>>8)*ܡ.ޘ;
var ڂ=ܣ/ܢ;return ڂ;}private bool ܤ(Func<ᯍ,bool>ܥ,Ꮖ ܦ){var ú=ڐ;ᯍ ܩ=null;while(ú-->0){var њ=Ꮖ.Ꮑ;for(var Ä=0;Ä<ڐ;Ä++){if(ڕ[Ä].
ᯆ<њ){њ=ڕ[Ä].ᯆ;ܩ=ڕ[Ä];}}if(њ>ܦ){return true;}if(!ܥ(ܩ)){return false;}ܩ.ᯆ=Ꮖ.Ꮑ;}return true;}public bool ܨ(Ꮖ ǎ,Ꮖ ǆ,Ꮖ ǐ,Ꮖ ǅ,ݐ
ܠ,Func<ᯍ,bool>ܟ){ڑ=(ܠ&ݐ.ݓ)!=0;var Ĺ=l.ૡ();var Ν=l.શ.ᑋ;ڐ=0;if(((ǎ-Ν.ᅌ).Ꮁ&(ᑋ.ᑛ.Ꮁ-1))==0){ǎ+=Ꮖ.Ꮒ;}if(((ǆ-Ν.ᅍ).Ꮁ&(ᑋ.ᑛ.Ꮁ-1))==
0){ǆ+=Ꮖ.Ꮒ;}ڒ.ޚ=ǎ;ڒ.ޙ=ǆ;ڒ.ޘ=ǐ-ǎ;ڒ.ޗ=ǅ-ǆ;ǎ-=Ν.ᅌ;ǆ-=Ν.ᅍ;var ܞ=ǎ.Ꮁ>>ᑋ.ᑙ;var ܝ=ǆ.Ꮁ>>ᑋ.ᑙ;ǐ-=Ν.ᅌ;ǅ-=Ν.ᅍ;var ܜ=ǐ.Ꮁ>>ᑋ.ᑙ;var ܛ=ǅ.Ꮁ
>>ᑋ.ᑙ;Ꮖ ܚ;Ꮖ ܙ;Ꮖ ܘ;int ܗ;int ܖ;if(ܜ>ܞ){ܗ=1;ܘ=new Ꮖ(Ꮖ.Ꮔ-((ǎ.Ꮁ>>ᑋ.ᑚ)&(Ꮖ.Ꮔ-1)));ܙ=(ǅ-ǆ)/Ꮖ.Ꭼ(ǐ-ǎ);}else if(ܜ<ܞ){ܗ=-1;ܘ=new Ꮖ((ǎ
.Ꮁ>>ᑋ.ᑚ)&(Ꮖ.Ꮔ-1));ܙ=(ǅ-ǆ)/Ꮖ.Ꭼ(ǐ-ǎ);}else{ܗ=0;ܘ=Ꮖ.Ꮒ;ܙ=Ꮖ.Ꭸ(256);}var ܕ=new Ꮖ(ǆ.Ꮁ>>ᑋ.ᑚ)+(ܘ*ܙ);if(ܛ>ܝ){ܖ=1;ܘ=new Ꮖ(Ꮖ.Ꮔ-((ǆ.Ꮁ
>>ᑋ.ᑚ)&(Ꮖ.Ꮔ-1)));ܚ=(ǐ-ǎ)/Ꮖ.Ꭼ(ǅ-ǆ);}else if(ܛ<ܝ){ܖ=-1;ܘ=new Ꮖ((ǆ.Ꮁ>>ᑋ.ᑚ)&(Ꮖ.Ꮔ-1));ܚ=(ǐ-ǎ)/Ꮖ.Ꭼ(ǅ-ǆ);}else{ܖ=0;ܘ=Ꮖ.Ꮒ;ܚ=Ꮖ.Ꭸ(
256);}var ܔ=new Ꮖ(ǎ.Ꮁ>>ᑋ.ᑚ)+(ܘ*ܚ);var ܓ=ܞ;var ܧ=ܝ;for(var ú=0;ú<64;ú++){if((ܠ&ݐ.ݑ)!=0){if(!Ν.ᒝ(ܓ,ܧ,ړ,Ĺ)){return false;}}if(
(ܠ&ݐ.ݒ)!=0){if(!Ν.ᒟ(ܓ,ܧ,ڔ)){return false;}}if(ܓ==ܜ&&ܧ==ܛ){break;}if((ܕ.Ꮄ())==ܧ){ܕ+=ܙ;ܓ+=ܗ;}else if((ܔ.Ꮄ())==ܓ){ܔ+=ܚ;ܧ+=ܖ;
}}return ܤ(ܟ,Ꮖ.Ꮒ);}public ᗕ ݕ=>ڒ;}[Flags]public enum ݐ{ݑ=1,ݒ=2,ݓ=4}sealed class ݔ:Ⴞ{private ଦ l;private Ŀ ô;private Ꮖ Ζ;
private Ꮖ ݚ;private Ꮖ ݖ;private int ݗ;private int ú;private ܪ ݘ;private ܪ ݙ;private bool Δ;private int Ā;private ۀ ˌ;public ݔ(ଦ
l){this.l=l;}public override void Ⴛ(){var Ü=l.Đ;ί ŀ;switch(ݘ){case ܪ.ĳ:ŀ=Ü.Ξ(ô,Ζ,ݖ,Δ,0,1);if(ˌ==ۀ.ۃ||ˌ==ۀ.ۄ){if(((l.ଖ+ô.ġ
)&7)==0){l.ૐ(ô.ĕ,ɴ.Ȳ,ʡ.ʝ);}}if(ŀ==ί.έ&&!Δ){ú=ݗ;ݘ=ܪ.Ķ;l.ૐ(ô.ĕ,ɴ.ȧ,ʡ.ʝ);}else{if(ŀ==ί.ά){ú=ݗ;ݘ=ܪ.ڰ;l.ૐ(ô.ĕ,ɴ.Ȩ,ʡ.ʝ);switch(
ˌ){case ۀ.ۆ:case ۀ.ۂ:Ü.Ά(this);ô.Ĝ();break;case ۀ.ۃ:case ۀ.ۄ:Ü.Ά(this);ô.Ĝ();break;default:break;}}}break;case ܪ.Ķ:ŀ=Ü.Ξ(
ô,Ζ,ݚ,false,0,-1);if(ŀ==ί.ά){ú=ݗ;ݘ=ܪ.ڰ;l.ૐ(ô.ĕ,ɴ.Ȩ,ʡ.ʝ);}break;case ܪ.ڰ:if(--ú==0){if(ô.Ģ==ݚ){ݘ=ܪ.ĳ;}else{ݘ=ܪ.Ķ;}l.ૐ(ô.ĕ,
ɴ.ȧ,ʡ.ʝ);}break;case ܪ.ڿ:break;}}public Ŀ Ŀ{get{return ô;}set{ô=value;}}public Ꮖ ݏ{get{return Ζ;}set{Ζ=value;}}public Ꮖ ݎ
{get{return ݚ;}set{ݚ=value;}}public Ꮖ ݍ{get{return ݖ;}set{ݖ=value;}}public int ܯ{get{return ݗ;}set{ݗ=value;}}public int ŏ
{get{return ú;}set{ú=value;}}public ܪ ܮ{get{return ݘ;}set{ݘ=value;}}public ܪ ܭ{get{return ݙ;}set{ݙ=value;}}public bool ܬ{
get{return Δ;}set{Δ=value;}}public int ę{get{return Ā;}set{Ā=value;}}public ۀ ܫ{get{return ˌ;}set{ˌ=value;}}}public enum ܪ{
ĳ,Ķ,ڰ,ڿ}public enum ۀ{ہ,ۂ,ۃ,ۄ,ۆ}sealed class ђ{public static int ۇ=4;public static Ꮖ ۈ=Ꮖ.Ꭸ(41);private static string[]ۉ=
new string[]{"Green","Indigo","Brown","Red"};private int Ć;private string Ĭ;private bool Ŕ;private Ɠ ã;private Ų ڽ;private
థ Ƥ;private Ꮖ ڼ;private Ꮖ ڻ;private Ꮖ ں;private Ꮖ Ƣ;private int ڹ;private int ڸ;private int ڷ;private int[]ڶ;private bool
[]ڵ;private bool ڴ;private int[]ž;private ਖ਼ ڳ;private ਖ਼ ڲ;private bool[]ڱ;private int[]ۅ;private int[]ۊ;private bool ۥ;
private bool ے;private ᖶ ۓ;private int ە;private int œ;private int Œ;private int š;private string ی;private int ۦ;private int ܐ
;private int ۮ;private Ɠ ۯ;private int ۺ;private int ۻ;private int ۼ;private Ɓ[]ۿ;private bool ܒ;private bool ۑ;private Ꮖ
ې;private ɡ ۏ;public ђ(int Ć){this.Ć=Ć;Ĭ=ۉ[Ć];Ƥ=new థ();ڶ=new int[(int)Ů.ŏ];ڵ=new bool[(int)ᒨ.ŏ];ž=new int[ۇ];ڱ=new bool[
(int)ਖ਼.ŏ];ۅ=new int[(int)ᓩ.ŏ];ۊ=new int[(int)ᓩ.ŏ];ۿ=new Ɓ[(int)ſ.ŏ];for(var Ä=0;Ä<ۿ.Length;Ä++){ۿ[Ä]=new Ɓ();}}public
void ŷ(){ã=null;ڽ=0;Ƥ.ŷ();ڼ=Ꮖ.Ꮓ;ڻ=Ꮖ.Ꮓ;ں=Ꮖ.Ꮓ;Ƣ=Ꮖ.Ꮓ;ڹ=0;ڸ=0;ڷ=0;Array.Clear(ڶ,0,ڶ.Length);Array.Clear(ڵ,0,ڵ.Length);ڴ=false;
Array.Clear(ž,0,ž.Length);ڳ=0;ڲ=0;Array.Clear(ڱ,0,ڱ.Length);Array.Clear(ۅ,0,ۅ.Length);Array.Clear(ۊ,0,ۊ.Length);ے=false;ۥ=
false;ۓ=0;ە=0;œ=0;Œ=0;š=0;ی=null;ۦ=0;ܐ=0;ۮ=0;ۯ=null;ۺ=0;ۻ=0;ۼ=0;foreach(var Ś in ۿ){Ś.ŷ();}ܒ=false;ۑ=false;ې=Ꮖ.Ꮓ;ۏ=ɡ.ᓬ;}
public void ů(){ã=null;ڽ=Ų.ű;Ƥ.ŷ();ڼ=Ꮖ.Ꮓ;ڻ=Ꮖ.Ꮓ;ں=Ꮖ.Ꮓ;Ƣ=Ꮖ.Ꮓ;ڹ=ኑ.ᕺ.ᕻ;ڸ=0;ڷ=0;Array.Clear(ڶ,0,ڶ.Length);Array.Clear(ڵ,0,ڵ.Length)
;ڴ=false;ڳ=ਖ਼.ਸ;ڲ=ਖ਼.ਸ;Array.Clear(ڱ,0,ڱ.Length);Array.Clear(ۅ,0,ۅ.Length);Array.Clear(ۊ,0,ۊ.Length);ڱ[(int)ਖ਼.ਹ]=true;ڱ[(
int)ਖ਼.ਸ]=true;ۅ[(int)ᓩ.З]=ኑ.ᕺ.ᕼ;for(var Ä=0;Ä<(int)ᓩ.ŏ;Ä++){ۊ[Ä]=ኑ.ᕸ.Ꮅ[Ä];}ے=true;ۥ=true;ۓ=0;ە=0;ی=null;ۦ=0;ܐ=0;ۮ=0;ۯ=null;
ۺ=0;ۻ=0;ۼ=0;foreach(var Ś in ۿ){Ś.ŷ();}ܒ=false;ۑ=false;ې=Ꮖ.Ꮓ;ۏ=ɡ.ᓬ;}public void ێ(){Array.Clear(ڶ,0,ڶ.Length);Array.Clear
(ڵ,0,ڵ.Length);ã.ᘾ&=~ళ.ᘋ;ۺ=0;ۻ=0;ܐ=0;ۮ=0;}public void ۍ(string ی){if(ReferenceEquals(this.ی,(string)ኑ.ጓ.ጷ)&&!
ReferenceEquals(ی,(string)ኑ.ጓ.ጸ)){return;}this.ی=ی;ۦ=4*ᱺ.ᱹ;}public void Ċ(){ۑ=true;ې=ڼ;ۏ=ã.ɡ;}public void Ĝ(){ۑ=false;}public Ꮖ ۋ(Ꮖ č){
if(ۑ&&ã.ଦ.ଖ>1){return ې+č*(ڼ-ې);}else{return ڼ;}}public ɡ ϙ(Ꮖ č){if(ۑ){var ş=ã.ɡ-ۏ;if(ş<ɡ.ᓣ){return ۏ+ɡ.ᓟ(č.Ꭻ()*ş.ᓜ());}
else{return ۏ-ɡ.ᓟ(č.Ꭻ()*(360.0-ş.ᓜ()));}}else{return ã.ɡ;}}public int ġ=>Ć;public string Ń=>Ĭ;public bool Ÿ{get{return Ŕ;}
set{Ŕ=value;}}public Ɠ Ɠ{get{return ã;}set{ã=value;}}public Ų Ų{get{return ڽ;}set{ڽ=value;}}public థ ƕ{get{return Ƥ;}}
public Ꮖ Ɯ{get{return ڼ;}set{ڼ=value;}}public Ꮖ Ɩ{get{return ڻ;}set{ڻ=value;}}public Ꮖ Ɨ{get{return ں;}set{ں=value;}}public Ꮖ
Ƙ{get{return Ƣ;}set{Ƣ=value;}}public int ƙ{get{return ڹ;}set{ڹ=value;}}public int ƚ{get{return ڸ;}set{ڸ=value;}}public
int ƛ{get{return ڷ;}set{ڷ=value;}}public int[]Ɲ{get{return ڶ;}}public bool[]Ƒ{get{return ڵ;}}public bool Ɛ{get{return ڴ;}
set{ڴ=value;}}public int[]Ž{get{return ž;}}public ਖ਼ Ə{get{return ڳ;}set{ڳ=value;}}public ਖ਼ Ǝ{get{return ڲ;}set{ڲ=value;}}
public bool[]ƍ{get{return ڱ;}}public int[]ƌ{get{return ۅ;}}public int[]Ƌ{get{return ۊ;}}public bool Ɗ{get{return ۥ;}set{ۥ=
value;}}public bool Ɖ{get{return ے;}set{ے=value;}}public ᖶ ƈ{get{return ۓ;}set{ۓ=value;}}public int Ƈ{get{return ە;}set{ە=
value;}}public int Ź{get{return œ;}set{œ=value;}}public int ź{get{return Œ;}set{Œ=value;}}public int Ż{get{return š;}set{š=
value;}}public string Ɔ{get{return ی;}set{ی=value;}}public int Ɣ{get{return ۦ;}set{ۦ=value;}}public int ƞ{get{return ܐ;}set{ܐ
=value;}}public int ư{get{return ۮ;}set{ۮ=value;}}public Ɠ Ʃ{get{return ۯ;}set{ۯ=value;}}public int ƪ{get{return ۺ;}set{ۺ
=value;}}public int ƫ{get{return ۻ;}set{ۻ=value;}}public int Ƭ{get{return ۼ;}set{ۼ=value;}}public Ɓ[]ƭ{get{return ۿ;}}
public bool Ʈ{get{return ܒ;}set{ܒ=value;}}}sealed class Ư{public static int[]Ʊ={0x19,0x32};public static int[]Ƶ={0x18,0x28};
public static int[]Ʋ={640,1280,320};public static int Ƴ=Ʊ[1];public static int ƴ=6;private ଦ l;public Ư(ଦ l){this.l=l;}public
void ƶ(ђ Ð){if(Ð.Ɣ>0){Ð.Ɣ--;}if((Ð.ƈ&ᖶ.ᖮ)!=0){Ð.Ɠ.ᘾ|=ళ.ᖮ;}else{Ð.Ɠ.ᘾ&=~ళ.ᖮ;}var Ƥ=Ð.ƕ;if((Ð.Ɠ.ᘾ&ళ.ᘔ)!=0){Ƥ.Ʋ=0;Ƥ.Ʊ=0xC800/
512;Ƥ.Ƶ=0;Ð.Ɠ.ᘾ&=~ళ.ᘔ;}if(Ð.Ų==Ų.Ű){ŝ(Ð);return;}if(Ð.Ɠ.ᘺ>0){Ð.Ɠ.ᘺ--;}else{ƥ(Ð);}ƣ(Ð);if(Ð.Ɠ.ฃ.Ŀ.Ě!=0){Ɵ(Ð);}if((Ƥ.ఠ&ట.Ě)!=
0){Ƥ.ఠ=0;}if((Ƥ.ఠ&ట.ஏ)!=0){var ƨ=(Ƥ.ఠ&ట.ஐ)>>ట.ஒ;if(ƨ==(int)ਖ਼.ਹ&&Ð.ƍ[(int)ਖ਼.ନ]&&!(Ð.Ə==ਖ਼.ନ&&Ð.Ɲ[(int)Ů.Ŭ]!=0)){ƨ=(int)ਖ਼.ନ;
}if((l.હ.ಖ==ಖ.ᴉ)&&ƨ==(int)ਖ਼.Љ&&Ð.ƍ[(int)ਖ਼.ପ]&&Ð.Ə!=ਖ਼.ପ){ƨ=(int)ਖ਼.ପ;}if(Ð.ƍ[ƨ]&&ƨ!=(int)Ð.Ə){if((ƨ!=(int)ਖ਼.Ϲ&&ƨ!=(int)ਖ਼.Ϻ)
||(l.હ.ಖ!=ಖ.ᴋ)){Ð.Ǝ=(ਖ਼)ƨ;}}}if((Ƥ.ఠ&ట.ఆ)!=0){if(!Ð.Ɖ){l.મ.ᛑ(Ð);Ð.Ɖ=true;}}else{Ð.Ɖ=false;}ś(Ð);if(Ð.Ɲ[(int)Ů.Ŭ]!=0){Ð.Ɲ[(
int)Ů.Ŭ]++;}if(Ð.Ɲ[(int)Ů.ŭ]>0){Ð.Ɲ[(int)Ů.ŭ]--;}if(Ð.Ɲ[(int)Ů.ū]>0){if(--Ð.Ɲ[(int)Ů.ū]==0){Ð.Ɠ.ᘾ&=~ళ.ᘋ;}}if(Ð.Ɲ[(int)Ů.Ũ]>
0){Ð.Ɲ[(int)Ů.Ũ]--;}if(Ð.Ɲ[(int)Ů.Ū]>0){Ð.Ɲ[(int)Ů.Ū]--;}if(Ð.ƞ>0){Ð.ƞ--;}if(Ð.ư>0){Ð.ư--;}if(Ð.Ɲ[(int)Ů.ŭ]>0){if(Ð.Ɲ[(
int)Ů.ŭ]>4*32||(Ð.Ɲ[(int)Ů.ŭ]&8)!=0){Ð.ƫ=Ƭ.ᖴ;}else{Ð.ƫ=0;}}else if(Ð.Ɲ[(int)Ů.Ũ]>0){if(Ð.Ɲ[(int)Ů.Ũ]>4*32||(Ð.Ɲ[(int)Ů.Ũ]&8
)!=0){Ð.ƫ=1;}else{Ð.ƫ=0;}}else{Ð.ƫ=0;}}private static Ꮖ Ƨ=new Ꮖ(0x100000);private bool Ʀ;public void ƥ(ђ Ð){var Ƥ=Ð.ƕ;Ð.Ɠ
.ɡ+=new ɡ(Ƥ.Ʋ<<16);Ʀ=(Ð.Ɠ.ኴ<=Ð.Ɠ.ᙈ);if(Ƥ.Ʊ!=0&&Ʀ){ơ(Ð,Ð.Ɠ.ɡ,new Ꮖ(Ƥ.Ʊ*2048));}if(Ƥ.Ƶ!=0&&Ʀ){ơ(Ð,Ð.Ɠ.ɡ-ɡ.ᓮ,new Ꮖ(Ƥ.Ƶ*2048)
);}if((Ƥ.Ʊ!=0||Ƥ.Ƶ!=0)&&Ð.Ɠ.Ŷ==ኑ.ጔ[(int)ᚏ.ᨊ]){Ð.Ɠ.ᘸ(ᚏ.ᨋ);}}public void ƣ(ђ Ð){Ð.Ƙ=Ð.Ɠ.ᙂ*Ð.Ɠ.ᙂ+Ð.Ɠ.ᙁ*Ð.Ɠ.ᙁ;Ð.Ƙ>>=2;if(Ð.Ƙ>
Ƨ){Ð.Ƙ=Ƨ;}if((Ð.ƈ&ᖶ.ᖵ)!=0||!Ʀ){Ð.Ɯ=Ð.Ɠ.ኴ+ђ.ۈ;if(Ð.Ɯ>Ð.Ɠ.ᙉ-Ꮖ.Ꭸ(4)){Ð.Ɯ=Ð.Ɠ.ᙉ-Ꮖ.Ꭸ(4);}Ð.Ɯ=Ð.Ɠ.ኴ+Ð.Ɩ;return;}var Ş=(ஜ.ஷ/20*l
.ଖ)&ஜ.ற;var Ƣ=(Ð.Ƙ/2)*ஜ.ஸ(Ş);if(Ð.Ų==Ų.ű){Ð.Ɩ+=Ð.Ɨ;if(Ð.Ɩ>ђ.ۈ){Ð.Ɩ=ђ.ۈ;Ð.Ɨ=Ꮖ.Ꮓ;}if(Ð.Ɩ<ђ.ۈ/2){Ð.Ɩ=ђ.ۈ/2;if(Ð.Ɨ<=Ꮖ.Ꮓ){Ð.Ɨ=
new Ꮖ(1);}}if(Ð.Ɨ!=Ꮖ.Ꮓ){Ð.Ɨ+=Ꮖ.Ꮒ/4;if(Ð.Ɨ==Ꮖ.Ꮓ){Ð.Ɨ=new Ꮖ(1);}}}Ð.Ɯ=Ð.Ɠ.ኴ+Ð.Ɩ+Ƣ;if(Ð.Ɯ>Ð.Ɠ.ᙉ-Ꮖ.Ꭸ(4)){Ð.Ɯ=Ð.Ɠ.ᙉ-Ꮖ.Ꭸ(4);}}
public void ơ(ђ Ð,ɡ Ş,Ꮖ Ơ){Ð.Ɠ.ᙂ+=Ơ*ஜ.ௐ(Ş);Ð.Ɠ.ᙁ+=Ơ*ஜ.ஸ(Ş);}private void Ɵ(ђ Ð){var ô=Ð.Ɠ.ฃ.Ŀ;if(Ð.Ɠ.ኴ!=ô.Ģ){return;}var ő=l.ર
;switch((int)ô.Ě){case 5:if(Ð.Ɲ[(int)Ů.Ū]==0){if((l.ଖ&0x1f)==0){ő.ᅹ(Ð.Ɠ,null,null,10);}}break;case 7:if(Ð.Ɲ[(int)Ů.Ū]==0)
{if((l.ଖ&0x1f)==0){ő.ᅹ(Ð.Ɠ,null,null,5);}}break;case 16:case 4:if(Ð.Ɲ[(int)Ů.Ū]==0||(l.ષ.ѐ()<5)){if((l.ଖ&0x1f)==0){ő.ᅹ(Ð.
Ɠ,null,null,20);}}break;case 9:Ð.Ż++;ô.Ě=0;break;case 11:Ð.ƈ&=~ᖶ.ᖱ;if((l.ଖ&0x1f)==0){ő.ᅹ(Ð.Ɠ,null,null,20);}if(Ð.ƙ<=10){l
.ଞ();}break;default:throw new Exception("Unknown sector special: "+(int)ô.Ě);}}private static ɡ Ŝ=new ɡ(ɡ.ᓮ.Ꮁ/18);private
void ŝ(ђ Ð){ś(Ð);if(Ð.Ɩ>Ꮖ.Ꭸ(6)){Ð.Ɩ-=Ꮖ.Ꮒ;}if(Ð.Ɩ<Ꮖ.Ꭸ(6)){Ð.Ɩ=Ꮖ.Ꭸ(6);}Ð.Ɨ=Ꮖ.Ꮓ;Ʀ=(Ð.Ɠ.ኴ<=Ð.Ɠ.ᙈ);ƣ(Ð);if(Ð.Ʃ!=null&&Ð.Ʃ!=Ð.Ɠ){
var Ş=ᵎ.ᴥ(Ð.Ɠ.ޚ,Ð.Ɠ.ޙ,Ð.Ʃ.ޚ,Ð.Ʃ.ޙ);var ş=Ş-Ð.Ɠ.ɡ;if(ş<Ŝ||ş.Ꮁ>(-Ŝ).Ꮁ){Ð.Ɠ.ɡ=Ş;if(Ð.ƞ>0){Ð.ƞ--;}}else if(ş<ɡ.ᓣ){Ð.Ɠ.ɡ+=Ŝ;}
else{Ð.Ɠ.ɡ-=Ŝ;}}else if(Ð.ƞ>0){Ð.ƞ--;}if((Ð.ƕ.ఠ&ట.ఆ)!=0){Ð.Ų=Ų.ů;}}public void Š(ђ Ð){for(var Ä=0;Ä<(int)ſ.ŏ;Ä++){Ð.ƭ[Ä].Ŷ=
null;}Ð.Ǝ=Ð.Ə;Ţ(Ð);}public void Ţ(ђ Ð){if(Ð.Ǝ==ਖ਼.ଫ){Ð.Ǝ=Ð.Ə;}if(Ð.Ǝ==ਖ਼.ନ){l.ૐ(Ð.Ɠ,ɴ.ɩ,ʡ.Ƅ);}var ţ=ኑ.ኔ[(int)Ð.Ǝ].ੲ;Ð.Ǝ=ਖ਼.ଫ;Ð.
ƭ[(int)ſ.Ƅ].ų=ଊ.இ;Ť(Ð,ſ.Ƅ,ţ);}public void Ť(ђ Ð,ſ ť,ᚏ Í){var Ś=Ð.ƭ[(int)ť];do{if(Í==ᚏ.ᚠ){Ś.Ŷ=null;break;}var ř=ኑ.ጔ[(int)Í
];Ś.Ŷ=ř;Ś.ŵ=ř.ŵ;if(ř.ϣ!=0){Ś.Ŵ=Ꮖ.Ꭸ(ř.ϣ);Ś.ų=Ꮖ.Ꭸ(ř.Ϣ);}if(ř.ѓ!=null){ř.ѓ(l,Ð,Ś);if(Ś.Ŷ==null){break;}}Í=Ś.Ŷ.ѐ;}while(Ś.ŵ==
0);}private void ś(ђ Ð){for(var Ä=0;Ä<(int)ſ.ŏ;Ä++){var Ś=Ð.ƭ[Ä];Ш ř;if((ř=Ś.Ŷ)!=null){if(Ś.ŵ!=-1){Ś.ŵ--;if(Ś.ŵ==0){Ť(Ð,(
ſ)Ä,Ś.Ŷ.ѐ);}}}}Ð.ƭ[(int)ſ.ƀ].Ŵ=Ð.ƭ[(int)ſ.Ƅ].Ŵ;Ð.ƭ[(int)ſ.ƀ].ų=Ð.ƭ[(int)ſ.Ƅ].ų;}public void Ř(ђ Ð){Ť(Ð,ſ.Ƅ,ኑ.ኔ[(int)Ð.Ə].
ਫ਼);}public void ŗ(Ɠ Ð){var Ŗ=ɴ.ɔ;if((l.હ.ಖ==ಖ.ᴉ)&&(Ð.ƙ<-50)){Ŗ=ɴ.Ɏ;}l.ૐ(Ð,Ŗ,ʡ.ʟ);}}class ŕ{private bool Ŕ;private int œ;
private int Œ;private int š;private int Ŧ;private int[]ž;public ŕ(){ž=new int[ђ.ۇ];}public bool Ÿ{get{return Ŕ;}set{Ŕ=value;}}
public int Ź{get{return œ;}set{œ=value;}}public int ź{get{return Œ;}set{Œ=value;}}public int Ż{get{return š;}set{š=value;}}
public int ż{get{return Ŧ;}set{Ŧ=value;}}public int[]Ž{get{return ž;}}}public enum ſ{Ƅ,ƀ,ŏ}sealed class Ɓ{private Ш Í;private
int Ƃ;private Ꮖ ƃ;private Ꮖ ƅ;public void ŷ(){Í=null;Ƃ=0;ƃ=Ꮖ.Ꮓ;ƅ=Ꮖ.Ꮓ;}public Ш Ŷ{get{return Í;}set{Í=value;}}public int ŵ{
get{return Ƃ;}set{Ƃ=value;}}public Ꮖ Ŵ{get{return ƃ;}set{ƃ=value;}}public Ꮖ ų{get{return ƅ;}set{ƅ=value;}}}public enum Ų{ű,
Ű,ů}public enum Ů{ŭ,Ŭ,ū,Ū,ũ,Ũ,ŏ}sealed class Ʒ:ᜮ{private string[]Ǉ;private Action Ǳ;public Ʒ(ባ ĭ,string Ǉ,Action Ǳ):base(
ĭ){this.Ǉ=Ǉ.Split('\n');this.Ǳ=Ǳ;}public override bool ଅ(ᕨ Ň){if(Ň.ܫ==ፎ.ፏ){if(Ǳ!=null){Ǳ();}ኘ.ಯ();ኘ.ૐ(ɴ.ȭ);return true;}
return true;}public IReadOnlyList<string>ǭ=>Ǉ;}sealed class ǳ:ᜮ{private static ɴ[]Ƿ=new ɴ[]{ɴ.ɔ,ɴ.ȯ,ɴ.Ȱ,ɴ.ȡ,ɴ.ȝ,ɴ.Ȝ,ɴ.Ț,ɴ.Ɉ};
private static ɴ[]Ǵ=new ɴ[]{ɴ.ȵ,ɴ.ʳ,ɴ.ʵ,ɴ.ȡ,ɴ.ɍ,ɴ.Ƚ,ɴ.ȷ,ɴ.Ɉ};private ጟ ǰ;private Ᏺ ǵ;private string[]Ǉ;private int Ƕ;public ǳ(ባ
ĭ,ጟ ǰ):base(ĭ){this.ǰ=ǰ;ǵ=new Ᏺ(DateTime.Now.Millisecond);Ƕ=-1;}public override void ರ(){IReadOnlyList<Ꮥ>ǯ;if(ǰ.હ.ಖ==ಖ.ᴉ)
{if(ǰ.હ.ಕ==ಕ.ኾ){ǯ=ኑ.ጝ.ኾ;}else{ǯ=ኑ.ጝ.ጛ;}}else{ǯ=ኑ.ጝ.ጟ;}Ǉ=(ǯ[ǵ.ѐ()%ǯ.Count]+"\n\n"+ኑ.ጓ.ጐ).Split('\n');}public override bool
ଅ(ᕨ Ň){if(Ƕ!=-1){return true;}if(Ň.ܫ!=ፎ.ፏ){return true;}if(Ň.ᕤ==ኈ.ޙ||Ň.ᕤ==ኈ.ቔ||Ň.ᕤ==ኈ.ቓ){Ƕ=0;ɴ Ǯ;if(ኘ.હ.ಖ==ಖ.ᴉ){Ǯ=Ǵ[ǵ.ѐ()
%Ǵ.Length];}else{Ǯ=Ƿ[ǵ.ѐ()%Ƿ.Length];}ኘ.ૐ(Ǯ);}if(Ň.ᕤ==ኈ.ኩ||Ň.ᕤ==ኈ.ኡ){ኘ.ಯ();ኘ.ૐ(ɴ.ȭ);}return true;}public override void ڞ(
){if(Ƕ!=-1){Ƕ++;}if(Ƕ==50){ǰ.ᐎ();}}public IReadOnlyList<string>ǭ=>Ǉ;}sealed class Ǭ{private byte[]f;private int ǫ;private
Ǭ(byte[]f,int ǫ){var Ǫ=(ǫ*ǫ+7)/8;if(f.Length<Ǫ){Array.Resize(ref f,Ǫ);}this.f=f;this.ǫ=ǫ;}public static Ǭ ÿ(ಆ þ,int ý,Ŀ[]
Þ){return new Ǭ(þ.ಠ(ý),Þ.Length);}public bool ǲ(Ŀ Ǹ,Ŀ Ȋ){var Ȅ=Ǹ.ġ;var ȅ=Ȋ.ġ;var É=Ȅ*ǫ+ȅ;var Ȇ=É>>3;var ȇ=1<<(É&7);return
(f[Ȇ]&ȇ)!=0;}}sealed class Ȉ{private static double[]ȉ=new double[]{1.00,0.95,0.90,0.85,0.80,0.75,0.70,0.65,0.60,0.55,0.50
};private ᗮ ǽ;private ڗ ǌ;private Ꮝ ȋ;private ᜫ ĭ;private ᄥ Ȍ;private ള ȍ;private ᰀ Ȏ;private ڤ ȏ;private ᔠ Ȑ;private Ꮊ ȃ
;private ڦ Ȃ;private int ȁ;private int Ȁ;private int ǿ;private byte[]Ǿ;public Ȉ(ᗮ ǽ,ᴍ Ǽ){this.ǽ=ǽ;ǌ=Ǽ.ڗ;if(ǽ.ᗜ){ȋ=new Ꮝ(Ǽ
.ಆ,640,400);}else{ȋ=new Ꮝ(Ǽ.ಆ,320,200);}ǽ.ᗚ=ᕣ.ᕊ(ǽ.ᗚ,0,ǜ);ǽ.ᗫ=ᕣ.ᕊ(ǽ.ᗫ,0,ǟ);ĭ=new ᜫ(Ǽ.ಆ,ȋ);Ȍ=new ᄥ(Ǽ,ȋ,ǽ.ᗚ);ȍ=new ള(Ǽ.ಆ,ȋ);
Ȏ=new ᰀ(Ǽ.ಆ,ȋ);ȏ=new ڤ(Ǽ.ಆ,ȋ,this);Ȑ=new ᔠ(Ǽ.ಆ,ȋ);ȃ=new Ꮊ(Ǽ,ȋ);Ȃ=ڦ.ÿ(Ǽ.ಆ,"M_PAUSE");var ǉ=ȋ.Ǒ/320;ȁ=2*ǉ;Ȁ=ȋ.Ǒ/ȁ+1;ǿ=ȋ.ǡ/ǉ
;Ǿ=new byte[ȋ.Ꮁ.Length];ǌ.ک(ȉ[ǽ.ᗫ]);}public void ǻ(ጟ ǋ,Ꮖ č){if(ǋ.Ŷ==Ꮣ.Ꮤ){ȏ.Ǌ(ǋ.Ꮤ,č);}else if(ǋ.Ŷ==Ꮣ.સ){ǹ(ǋ.સ,č);}if(!ǋ.ኘ.
ᄧ){if(ǋ.Ŷ==Ꮣ.સ&&ǋ.સ.Ŷ==ᵈ.ᔜ&&ǋ.સ.ᕄ){var ǉ=ȋ.Ǒ/320;ȋ.Ꮘ(Ȃ,(ȋ.Ǒ-ǉ*Ȃ.Ǒ)/2,4*ǉ,ǉ);}}}public void Ǻ(ጟ ǋ){if(ǋ.ኘ.ᄧ){ĭ.Ǌ(ǋ.ኘ);}}
public void ǹ(ᕣ Æ,Ꮖ č){if(Æ.ᕄ){č=Ꮖ.Ꮒ;}if(Æ.Ŷ==ᵈ.ᔜ){var Ƹ=Æ.ଦ.ଙ;var ǈ=Æ.ଦ.ଚ;if(Æ.ଦ.ଏ.ᔢ){Ȑ.Ǌ(Ƹ);ȍ.Ǌ(Ƹ,true);}else{Ȍ.Ǌ(ǈ,č);if(Ȍ.
ǝ<8){ȍ.Ǌ(Ƹ,true);}else if(Ȍ.ǝ==ᄥ.ᄤ){ȍ.Ǌ(Ƹ,false);}}if(ǽ.ᗛ||ReferenceEquals(Ƹ.Ɔ,(string)ኑ.ጓ.ጷ)){if(Ƹ.Ɣ>0){var ǉ=ȋ.Ǒ/320;ȋ.
ᐭ(Ƹ.Ɔ,0,7*ǉ,ǉ);}}}else if(Æ.Ŷ==ᵈ.ᕅ){Ȏ.Ǌ(Æ.ᕅ);}else if(Æ.Ŷ==ᵈ.ፓ){ȃ.Ǌ(Æ.ፓ);}}public void Ǌ(ጟ ǋ,byte[]ƻ,Ꮖ č){if(ǋ.ᕦ){Ǐ(ǋ,ƻ);
return;}ǻ(ǋ,č);Ǻ(ǋ);var Ƽ=ǌ[0];if(ǋ.Ŷ==Ꮣ.સ&&ǋ.સ.Ŷ==ᵈ.ᔜ){Ƽ=ǌ[ƺ(ǋ.સ.ଦ.ଙ)];}else if(ǋ.Ŷ==Ꮣ.Ꮤ&&ǋ.Ꮤ.Ŷ==ڛ.ژ&&ǋ.Ꮤ.ڣ.Ŷ==ᵈ.ᔜ){Ƽ=ǌ[ƺ(ǋ.Ꮤ
.ڣ.ଦ.ଙ)];}ƽ(Ƽ,ƻ);}private void Ǐ(ጟ ǋ,byte[]ƻ){ǻ(ǋ,Ꮖ.Ꮒ);var Ǎ=ǋ.ବ;var ǉ=ȋ.Ǒ/320;for(var Ä=0;Ä<Ȁ-1;Ä++){var ǎ=ȁ*Ä;var ǐ=ǎ+ȁ
;var ǆ=Math.Max(ǉ*Ǎ.ޙ[Ä],0);var ǅ=Math.Max(ǉ*Ǎ.ޙ[Ä+1],0);var Ǆ=(float)(ǅ-ǆ)/ȁ;for(var ǃ=ǎ;ǃ<ǐ;ǃ++){var ǂ=(int)Math.Round(
ǆ+Ǆ*((ǃ-ǎ)/2*2));var ǁ=ȋ.ǡ-ǂ;if(ǁ>0){var ǀ=ȋ.ǡ*ǃ;var ƿ=ȋ.ǡ*ǃ+ǂ;Array.Copy(Ǿ,ǀ,ȋ.Ꮁ,ƿ,ǁ);}}}Ǻ(ǋ);ƽ(ǌ[0],ƻ);}public void ƾ()
{Array.Copy(ȋ.Ꮁ,Ǿ,ȋ.Ꮁ.Length);}private void ƽ(uint[]Ƽ,byte[]ƻ){}private static int ƺ(ђ Ð){var ú=Ð.ƞ;if(Ð.Ɲ[(int)Ů.Ŭ]!=0){
var ƹ=12-(Ð.Ɲ[(int)Ů.Ŭ]>>6);if(ƹ>ú){ú=ƹ;}}int ǌ;if(ú!=0){ǌ=(ú+7)>>3;if(ǌ>=ڗ.ƞ){ǌ=ڗ.ƞ-1;}ǌ+=ڗ.ږ;}else if(Ð.ư!=0){ǌ=(Ð.ư+7)>>
3;if(ǌ>=ڗ.ư){ǌ=ڗ.ư-1;}ǌ+=ڗ.ڠ;}else if(Ð.Ɲ[(int)Ů.Ū]>4*32||(Ð.Ɲ[(int)Ů.Ū]&8)!=0){ǌ=ڗ.Ū;}else{ǌ=0;}return ǌ;}public int Ǒ=>
ȋ.Ǒ;public int ǡ=>ȋ.ǡ;public int ǚ=>Ȁ;public int Ǜ=>ǿ;public int ǜ{get{return ᄥ.ᄤ;}}public int ǝ{get{return Ȍ.ǝ;}set{ǽ.ᗚ=
value;Ȍ.ǝ=value;}}public bool Ǟ{get{return ǽ.ᗛ;}set{ǽ.ᗛ=value;}}public int ǟ{get{return ȉ.Length-1;}}public int Ǡ{get{return
ǽ.ᗫ;}set{ǽ.ᗫ=value;ǌ.ک(ȉ[ǽ.ᗫ]);}}}static class Ǣ{public static int Ǩ=24;private static int ǣ=16;private static int Ǥ=360*
1024;private enum ǥ{Ǧ,Ɠ}private enum ǧ{ǩ,Ǚ,ǘ,Ǘ,ƀ,ǖ,Ǖ,ǔ}public static void ï(ᕣ Æ,string ì,string ð){var Ǔ=new Ő(ì);Ǔ.ï(Æ,ð);}
public static void Ç(ᕣ Æ,string ð){var Å=Æ.હ;Æ.ᕌ(Å.ᴷ,Å.ᰔ,Å.શ);var ǒ=new byte[123];var ŧ=new Î(ǒ);ŧ.Ç(Æ);}private class Ő{
private byte[]f;private int Ï;public Ő(string ì){f=new byte[Ǥ];Ï=0;í(ì);î();}private void í(string ì){for(var Ä=0;Ä<ì.Length;Ä
++){f[Ä]=(byte)ì[Ä];}Ï+=Ǩ;}private void î(){var È="version 109";for(var Ä=0;Ä<È.Length;Ä++){f[Ï+Ä]=(byte)È[Ä];}Ï+=ǣ;}
public void ï(ᕣ Æ,string ð){var Å=Æ.ଦ.હ;f[Ï++]=(byte)Å.ᴷ;f[Ï++]=(byte)Å.ᰔ;f[Ï++]=(byte)Å.શ;for(var Ä=0;Ä<ђ.ۇ;Ä++){f[Ï++]=Å.ᰁ[Ä
].Ÿ?(byte)1:(byte)0;}f[Ï++]=(byte)(Æ.ଦ.ଖ>>16);f[Ï++]=(byte)(Æ.ଦ.ଖ>>8);f[Ï++]=(byte)(Æ.ଦ.ଖ);ê(Æ.ଦ);é(Æ.ଦ);è(Æ.ଦ);ç(Æ.ଦ);f[
Ï++]=0x1d;}private void º(){Ï+=(4-(Ï&3))&3;}private void ê(ଦ l){var Ë=l.હ.ᰁ;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(!Ë[Ä].Ÿ){continue;}
º();Ï=ö(Ë[Ä],f,Ï);}}private void é(ଦ l){var Þ=l.શ.ᛩ;for(var Ä=0;Ä<Þ.Length;Ä++){Ï=õ(Þ[Ä],f,Ï);}var ß=l.શ.đ;for(var Ä=0;Ä<
ß.Length;Ä++){Ï=ó(ß[Ä],f,Ï);}}private void è(ଦ l){var Ý=l.વ;foreach(var â in Ý){var ã=â as Ɠ;if(ã!=null){f[Ï++]=(byte)ǥ.Ɠ
;º();Ì(f,Ï+8,ã.Ⴙ);Ê(f,Ï+12,ã.ޚ.Ꮁ);Ê(f,Ï+16,ã.ޙ.Ꮁ);Ê(f,Ï+20,ã.ኴ.Ꮁ);Ê(f,Ï+32,ã.ɡ.Ꮁ);Ê(f,Ï+36,(int)ã.э);Ê(f,Ï+40,ã.ю);Ê(f,Ï+
56,ã.ᙈ.Ꮁ);Ê(f,Ï+60,ã.ᙉ.Ꮁ);Ê(f,Ï+64,ã.ᙊ.Ꮁ);Ê(f,Ï+68,ã.ǡ.Ꮁ);Ê(f,Ï+72,ã.ᙂ.Ꮁ);Ê(f,Ï+76,ã.ᙁ.Ꮁ);Ê(f,Ï+80,ã.ᙀ.Ꮁ);Ê(f,Ï+88,(int)ã.
ܫ);Ê(f,Ï+96,ã.ŵ);Ê(f,Ï+100,ã.Ŷ.ġ);Ê(f,Ï+104,(int)ã.ᘾ);Ê(f,Ï+108,ã.ƙ);Ê(f,Ï+112,(int)ã.ᘽ);Ê(f,Ï+116,ã.ᘼ);Ê(f,Ï+124,ã.ᘺ);Ê(
f,Ï+128,ã.ᘹ);if(ã.ђ==null){Ê(f,Ï+132,0);}else{Ê(f,Ï+132,ã.ђ.ġ+1);}Ê(f,Ï+136,ã.ᘭ);if(ã.ᘬ==null){Ê(f,Ï+140,(short)0);Ê(f,Ï+
142,(short)0);Ê(f,Ï+144,(short)0);Ê(f,Ï+146,(short)0);Ê(f,Ï+148,(short)0);}else{Ê(f,Ï+140,(short)ã.ᘬ.ޚ.Ꮄ());Ê(f,Ï+142,(
short)ã.ᘬ.ޙ.Ꮄ());Ê(f,Ï+144,(short)Math.Round(ã.ᘬ.ɡ.ᓜ()));Ê(f,Ï+146,(short)ã.ᘬ.ܫ);Ê(f,Ï+148,(short)ã.ᘬ.ᘾ);}Ï+=154;}}f[Ï++]=(
byte)ǥ.Ǧ;}private void ç(ଦ l){var Ý=l.વ;var Ü=l.Đ;foreach(var â in Ý){if(â.Ⴙ==Ⴙ.ڿ){var Ú=â as ᒡ;if(Ü.σ(Ú)){f[Ï++]=(byte)ǧ.ǩ;
º();Ì(f,Ï+8,Ú.Ⴙ);Ê(f,Ï+12,(int)Ú.ܫ);Ê(f,Ï+16,Ú.Ŀ.ġ);Ê(f,Ï+20,Ú.ᒑ.Ꮁ);Ê(f,Ï+24,Ú.ತ.Ꮁ);Ê(f,Ï+28,Ú.ݏ.Ꮁ);Ê(f,Ï+32,Ú.ܬ?1:0);Ê(f
,Ï+36,Ú.ಣ);Ê(f,Ï+40,Ú.ę);Ê(f,Ï+44,Ú.ᒒ);Ï+=48;}continue;}{var Ú=â as ᒡ;if(Ú!=null){f[Ï++]=(byte)ǧ.ǩ;º();Ì(f,Ï+8,Ú.Ⴙ);Ê(f,Ï
+12,(int)Ú.ܫ);Ê(f,Ï+16,Ú.Ŀ.ġ);Ê(f,Ï+20,Ú.ᒑ.Ꮁ);Ê(f,Ï+24,Ú.ತ.Ꮁ);Ê(f,Ï+28,Ú.ݏ.Ꮁ);Ê(f,Ï+32,Ú.ܬ?1:0);Ê(f,Ï+36,Ú.ಣ);Ê(f,Ï+40,Ú.
ę);Ê(f,Ï+44,Ú.ᒒ);Ï+=48;continue;}}{var Ù=â as ಪ;if(Ù!=null){f[Ï++]=(byte)ǧ.Ǚ;º();Ì(f,Ï+8,Ù.Ⴙ);Ê(f,Ï+12,(int)Ù.ܫ);Ê(f,Ï+16
,Ù.Ŀ.ġ);Ê(f,Ï+20,Ù.ತ.Ꮁ);Ê(f,Ï+24,Ù.ݏ.Ꮁ);Ê(f,Ï+28,Ù.ಣ);Ê(f,Ï+32,Ù.ಢ);Ê(f,Ï+36,Ù.ನ);Ï+=40;continue;}}{var Ø=â as ᴘ;if(Ø!=
null){f[Ï++]=(byte)ǧ.ǘ;º();Ì(f,Ï+8,Ø.Ⴙ);Ê(f,Ï+12,(int)Ø.ܫ);Ê(f,Ï+16,Ø.ܬ?1:0);Ê(f,Ï+20,Ø.Ŀ.ġ);Ê(f,Ï+24,Ø.ಣ);Ê(f,Ï+28,(int)Ø.ᴃ
);Ê(f,Ï+32,Ø.ථ);Ê(f,Ï+36,Ø.ᴄ.Ꮁ);Ê(f,Ï+40,Ø.ݏ.Ꮁ);Ï+=44;continue;}}{var Ö=â as ݔ;if(Ö!=null){f[Ï++]=(byte)ǧ.Ǘ;º();Ì(f,Ï+8,Ö
.Ⴙ);Ê(f,Ï+12,Ö.Ŀ.ġ);Ê(f,Ï+16,Ö.ݏ.Ꮁ);Ê(f,Ï+20,Ö.ݎ.Ꮁ);Ê(f,Ï+24,Ö.ݍ.Ꮁ);Ê(f,Ï+28,Ö.ܯ);Ê(f,Ï+32,Ö.ŏ);Ê(f,Ï+36,(int)Ö.ܮ);Ê(f,Ï+
40,(int)Ö.ܭ);Ê(f,Ï+44,Ö.ܬ?1:0);Ê(f,Ï+48,Ö.ę);Ê(f,Ï+52,(int)Ö.ܫ);Ï+=56;continue;}}{var Õ=â as ᵩ;if(Õ!=null){f[Ï++]=(byte)ǧ.
ƀ;º();Ì(f,Ï+8,Õ.Ⴙ);Ê(f,Ï+12,Õ.Ŀ.ġ);Ê(f,Ï+16,Õ.ŏ);Ê(f,Ï+20,Õ.ฆ);Ê(f,Ï+24,Õ.ง);Ê(f,Ï+28,Õ.ᵥ);Ê(f,Ï+32,Õ.ᵤ);Ï+=36;continue;}
}{var Ô=â as ฉ;if(Ô!=null){f[Ï++]=(byte)ǧ.ǖ;º();Ì(f,Ï+8,Ô.Ⴙ);Ê(f,Ï+12,Ô.Ŀ.ġ);Ê(f,Ï+16,Ô.ŏ);Ê(f,Ï+20,Ô.ง);Ê(f,Ï+24,Ô.ฆ);Ê(
f,Ï+28,Ô.ฅ);Ê(f,Ï+32,Ô.ค);Ï+=36;continue;}}{var Ó=â as ᯗ;if(Ó!=null){f[Ï++]=(byte)ǧ.Ǖ;º();Ì(f,Ï+8,Ó.Ⴙ);Ê(f,Ï+12,Ó.Ŀ.ġ);Ê(
f,Ï+16,Ó.ง);Ê(f,Ï+20,Ó.ฆ);Ê(f,Ï+24,Ó.ಣ);Ï+=28;continue;}}}f[Ï++]=(byte)ǧ.ǔ;}private static int ö(ђ Ð,byte[]f,int É){Ê(f,É
+4,(int)Ð.Ų);Ê(f,É+16,Ð.Ɯ.Ꮁ);Ê(f,É+20,Ð.Ɩ.Ꮁ);Ê(f,É+24,Ð.Ɨ.Ꮁ);Ê(f,É+28,Ð.Ƙ.Ꮁ);Ê(f,É+32,Ð.ƙ);Ê(f,É+36,Ð.ƚ);Ê(f,É+40,Ð.ƛ);
for(var Ä=0;Ä<(int)Ů.ŏ;Ä++){Ê(f,É+44+4*Ä,Ð.Ɲ[Ä]);}for(var Ä=0;Ä<(int)Ů.ŏ;Ä++){Ê(f,É+68+4*Ä,Ð.Ƒ[Ä]?1:0);}Ê(f,É+92,Ð.Ɛ?1:0);
for(var Ä=0;Ä<ђ.ۇ;Ä++){Ê(f,É+96+4*Ä,Ð.Ž[Ä]);}Ê(f,É+112,(int)Ð.Ə);Ê(f,É+116,(int)Ð.Ǝ);for(var Ä=0;Ä<(int)ਖ਼.ŏ;Ä++){Ê(f,É+120+
4*Ä,Ð.ƍ[Ä]?1:0);}for(var Ä=0;Ä<(int)ᓩ.ŏ;Ä++){Ê(f,É+156+4*Ä,Ð.ƌ[Ä]);}for(var Ä=0;Ä<(int)ᓩ.ŏ;Ä++){Ê(f,É+172+4*Ä,Ð.Ƌ[Ä]);}Ê(
f,É+188,Ð.Ɗ?1:0);Ê(f,É+192,Ð.Ɖ?1:0);Ê(f,É+196,(int)Ð.ƈ);Ê(f,É+200,Ð.Ƈ);Ê(f,É+204,Ð.Ź);Ê(f,É+208,Ð.ź);Ê(f,É+212,Ð.Ż);Ê(f,É
+220,Ð.ƞ);Ê(f,É+224,Ð.ư);Ê(f,É+232,Ð.ƪ);Ê(f,É+236,Ð.ƫ);Ê(f,É+240,Ð.Ƭ);for(var Ä=0;Ä<(int)ſ.ŏ;Ä++){if(Ð.ƭ[Ä].Ŷ==null){Ê(f,
É+244+16*Ä,0);}else{Ê(f,É+244+16*Ä,Ð.ƭ[Ä].Ŷ.ġ);}Ê(f,É+244+16*Ä+4,Ð.ƭ[Ä].ŵ);Ê(f,É+244+16*Ä+8,Ð.ƭ[Ä].Ŵ.Ꮁ);Ê(f,É+244+16*Ä+12
,Ð.ƭ[Ä].ų.Ꮁ);}Ê(f,É+276,Ð.Ʈ?1:0);return É+280;}private static int õ(Ŀ ô,byte[]f,int É){Ê(f,É,(short)(ô.Ģ.Ꮄ()));Ê(f,É+2,(
short)(ô.ģ.Ꮄ()));Ê(f,É+4,(short)ô.Ĥ);Ê(f,É+6,(short)ô.ĥ);Ê(f,É+8,(short)ô.Ħ);Ê(f,É+10,(short)ô.Ě);Ê(f,É+12,(short)ô.ę);return
É+14;}private static int ó(ɢ ò,byte[]f,int É){Ê(f,É,(short)ò.ᘾ);Ê(f,É+2,(short)ò.Ě);Ê(f,É+4,(short)ò.ę);É+=6;if(ò.ᶖ!=null
){var ñ=ò.ᶖ;Ê(f,É,(short)ñ.ʜ.Ꮄ());Ê(f,É+2,(short)ñ.ʛ.Ꮄ());Ê(f,É+4,(short)ñ.ɻ);Ê(f,É+6,(short)ñ.ʄ);Ê(f,É+8,(short)ñ.ʅ);É+=
10;}if(ò.ᶗ!=null){var ñ=ò.ᶗ;Ê(f,É,(short)ñ.ʜ.Ꮄ());Ê(f,É+2,(short)ñ.ʛ.Ꮄ());Ê(f,É+4,(short)ñ.ɻ);Ê(f,É+6,(short)ñ.ʄ);Ê(f,É+8,
(short)ñ.ʅ);É+=10;}return É;}private static void Ê(byte[]f,int É,int u){f[É]=(byte)u;f[É+1]=(byte)(u>>8);f[É+2]=(byte)(u
>>16);f[É+3]=(byte)(u>>24);}private static void Ê(byte[]f,int É,uint u){f[É]=(byte)u;f[É+1]=(byte)(u>>8);f[É+2]=(byte)(u>>
16);f[É+3]=(byte)(u>>24);}private static void Ê(byte[]f,int É,short u){f[É]=(byte)u;f[É+1]=(byte)(u>>8);}private static
void Ì(byte[]f,int É,Ⴙ Í){switch(Í){case Ⴙ.ڿ:Ê(f,É,0);break;default:Ê(f,É,1);break;}}}private class Î{private byte[]f;
private int Ï;public Î(byte[]f){this.f=f;Ï=0;µ();var È=ª();if(È!="VERSION 109"){throw new Exception("Unsupported version!");}}
public void Ç(ᕣ Æ){var Å=Æ.ଦ.હ;Å.ᴷ=(ᵅ)f[Ï++];Å.ᰔ=f[Ï++];Å.શ=f[Ï++];for(var Ä=0;Ä<ђ.ۇ;Ä++){Å.ᰁ[Ä].Ÿ=f[Ï++]!=0;}Æ.ᕌ(Å.ᴷ,Å.ᰔ,Å.શ)
;var Ã=f[Ï++];var Â=f[Ï++];var Á=f[Ï++];var À=(Ã<<16)+(Â<<8)+Á;o(Æ.ଦ);á(Æ.ଦ);à(Æ.ଦ);å(Æ.ଦ);if(f[Ï]!=0x1d){throw new
Exception("Bad savegame!");}Æ.ଦ.ଖ=À;Å.ᴽ.ᶪ(Æ.ଦ.ଙ.Ɠ);}private void º(){Ï+=(4-(Ï&3))&3;}private string µ(){var u=ን.ቦ(f,Ï,Ǩ);Ï+=Ǩ;
return u;}private string ª(){var u=ን.ቦ(f,Ï,ǣ);Ï+=ǣ;return u;}private void o(ଦ l){var Ë=l.હ.ᰁ;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(!Ë[Ä].Ÿ
){continue;}º();Ï=Ñ(Ë[Ä],f,Ï);}}private void á(ଦ l){var Þ=l.શ.ᛩ;for(var Ä=0;Ä<Þ.Length;Ä++){Ï=Ĳ(Þ[Ä],f,Ï);}var ß=l.શ.đ;
for(var Ä=0;Ä<ß.Length;Ä++){Ï=Ĵ(ß[Ä],f,Ï);}}private void à(ଦ l){var Ý=l.વ;var æ=l.ળ;foreach(var â in Ý){var ã=â as Ɠ;if(ã!=
null){æ.ᆖ(ã);}}Ý.ʕ();while(true){var Û=(ǥ)f[Ï++];switch(Û){case ǥ.Ǧ:return;case ǥ.Ɠ:º();var ã=new Ɠ(l);ã.Ⴙ=Ò(f,Ï+8);ã.ޚ=new
Ꮖ(BitConverter.ToInt32(f,Ï+12));ã.ޙ=new Ꮖ(BitConverter.ToInt32(f,Ï+16));ã.ኴ=new Ꮖ(BitConverter.ToInt32(f,Ï+20));ã.ɡ=new ɡ
(BitConverter.ToInt32(f,Ï+32));ã.э=(э)BitConverter.ToInt32(f,Ï+36);ã.ю=BitConverter.ToInt32(f,Ï+40);ã.ᙈ=new Ꮖ(
BitConverter.ToInt32(f,Ï+56));ã.ᙉ=new Ꮖ(BitConverter.ToInt32(f,Ï+60));ã.ᙊ=new Ꮖ(BitConverter.ToInt32(f,Ï+64));ã.ǡ=new Ꮖ(BitConverter
.ToInt32(f,Ï+68));ã.ᙂ=new Ꮖ(BitConverter.ToInt32(f,Ï+72));ã.ᙁ=new Ꮖ(BitConverter.ToInt32(f,Ï+76));ã.ᙀ=new Ꮖ(BitConverter.
ToInt32(f,Ï+80));ã.ܫ=(ё)BitConverter.ToInt32(f,Ï+88);ã.ᘿ=ኑ.ዃ[(int)ã.ܫ];ã.ŵ=BitConverter.ToInt32(f,Ï+96);ã.Ŷ=ኑ.ጔ[BitConverter.
ToInt32(f,Ï+100)];ã.ᘾ=(ళ)BitConverter.ToInt32(f,Ï+104);ã.ƙ=BitConverter.ToInt32(f,Ï+108);ã.ᘽ=(ಣ)BitConverter.ToInt32(f,Ï+112);ã
.ᘼ=BitConverter.ToInt32(f,Ï+116);ã.ᘺ=BitConverter.ToInt32(f,Ï+124);ã.ᘹ=BitConverter.ToInt32(f,Ï+128);var ä=BitConverter.
ToInt32(f,Ï+132);if(ä!=0){ã.ђ=l.હ.ᰁ[ä-1];ã.ђ.Ɠ=ã;}ã.ᘭ=BitConverter.ToInt32(f,Ï+136);ã.ᘬ=new ᛔ(Ꮖ.Ꭸ(BitConverter.ToInt16(f,Ï+140)
),Ꮖ.Ꭸ(BitConverter.ToInt16(f,Ï+142)),new ɡ(ɡ.ᓭ.Ꮁ*(uint)(BitConverter.ToInt16(f,Ï+144)/45)),BitConverter.ToInt16(f,Ï+146),
(ᅻ)BitConverter.ToInt16(f,Ï+148));Ï+=154;l.લ.ლ(ã);Ý.ᄩ(ã);break;default:throw new Exception(
"Unknown thinker class in savegame!");}}}private void å(ଦ l){var Ý=l.વ;var Ü=l.Đ;while(true){var Û=(ǧ)f[Ï++];switch(Û){case ǧ.ǔ:return;case ǧ.ǩ:º();var Ú=
new ᒡ(l);Ú.Ⴙ=Ò(f,Ï+8);Ú.ܫ=(ᒓ)BitConverter.ToInt32(f,Ï+12);Ú.Ŀ=l.શ.ᛩ[BitConverter.ToInt32(f,Ï+16)];Ú.Ŀ.Ē=Ú;Ú.ᒑ=new Ꮖ(
BitConverter.ToInt32(f,Ï+20));Ú.ತ=new Ꮖ(BitConverter.ToInt32(f,Ï+24));Ú.ݏ=new Ꮖ(BitConverter.ToInt32(f,Ï+28));Ú.ܬ=BitConverter.
ToInt32(f,Ï+32)!=0;Ú.ಣ=BitConverter.ToInt32(f,Ï+36);Ú.ę=BitConverter.ToInt32(f,Ï+40);Ú.ᒒ=BitConverter.ToInt32(f,Ï+44);Ï+=48;Ý.ᄩ
(Ú);Ü.υ(Ú);break;case ǧ.Ǚ:º();var Ù=new ಪ(l);Ù.Ⴙ=Ò(f,Ï+8);Ù.ܫ=(ಫ)BitConverter.ToInt32(f,Ï+12);Ù.Ŀ=l.શ.ᛩ[BitConverter.
ToInt32(f,Ï+16)];Ù.Ŀ.Ē=Ù;Ù.ತ=new Ꮖ(BitConverter.ToInt32(f,Ï+20));Ù.ݏ=new Ꮖ(BitConverter.ToInt32(f,Ï+24));Ù.ಣ=BitConverter.
ToInt32(f,Ï+28);Ù.ಢ=BitConverter.ToInt32(f,Ï+32);Ù.ನ=BitConverter.ToInt32(f,Ï+36);Ï+=40;Ý.ᄩ(Ù);break;case ǧ.ǘ:º();var Ø=new ᴘ(l
);Ø.Ⴙ=Ò(f,Ï+8);Ø.ܫ=(ᴀ)BitConverter.ToInt32(f,Ï+12);Ø.ܬ=BitConverter.ToInt32(f,Ï+16)!=0;Ø.Ŀ=l.શ.ᛩ[BitConverter.ToInt32(f,Ï
+20)];Ø.Ŀ.Ē=Ø;Ø.ಣ=BitConverter.ToInt32(f,Ï+24);Ø.ᴃ=(Ϋ)BitConverter.ToInt32(f,Ï+28);Ø.ථ=BitConverter.ToInt32(f,Ï+32);Ø.ᴄ=
new Ꮖ(BitConverter.ToInt32(f,Ï+36));Ø.ݏ=new Ꮖ(BitConverter.ToInt32(f,Ï+40));Ï+=44;Ý.ᄩ(Ø);break;case ǧ.Ǘ:º();var Ö=new ݔ(l);
Ö.Ⴙ=Ò(f,Ï+8);Ö.Ŀ=l.શ.ᛩ[BitConverter.ToInt32(f,Ï+12)];Ö.Ŀ.Ē=Ö;Ö.ݏ=new Ꮖ(BitConverter.ToInt32(f,Ï+16));Ö.ݎ=new Ꮖ(
BitConverter.ToInt32(f,Ï+20));Ö.ݍ=new Ꮖ(BitConverter.ToInt32(f,Ï+24));Ö.ܯ=BitConverter.ToInt32(f,Ï+28);Ö.ŏ=BitConverter.ToInt32(f,Ï+
32);Ö.ܮ=(ܪ)BitConverter.ToInt32(f,Ï+36);Ö.ܭ=(ܪ)BitConverter.ToInt32(f,Ï+40);Ö.ܬ=BitConverter.ToInt32(f,Ï+44)!=0;Ö.ę=
BitConverter.ToInt32(f,Ï+48);Ö.ܫ=(ۀ)BitConverter.ToInt32(f,Ï+52);Ï+=56;Ý.ᄩ(Ö);Ü.Ή(Ö);break;case ǧ.ƀ:º();var Õ=new ᵩ(l);Õ.Ⴙ=Ò(f,Ï+8);
Õ.Ŀ=l.શ.ᛩ[BitConverter.ToInt32(f,Ï+12)];Õ.ŏ=BitConverter.ToInt32(f,Ï+16);Õ.ฆ=BitConverter.ToInt32(f,Ï+20);Õ.ง=
BitConverter.ToInt32(f,Ï+24);Õ.ᵥ=BitConverter.ToInt32(f,Ï+28);Õ.ᵤ=BitConverter.ToInt32(f,Ï+32);Ï+=36;Ý.ᄩ(Õ);break;case ǧ.ǖ:º();var Ô
=new ฉ(l);Ô.Ⴙ=Ò(f,Ï+8);Ô.Ŀ=l.શ.ᛩ[BitConverter.ToInt32(f,Ï+12)];Ô.ŏ=BitConverter.ToInt32(f,Ï+16);Ô.ง=BitConverter.ToInt32(
f,Ï+20);Ô.ฆ=BitConverter.ToInt32(f,Ï+24);Ô.ฅ=BitConverter.ToInt32(f,Ï+28);Ô.ค=BitConverter.ToInt32(f,Ï+32);Ï+=36;Ý.ᄩ(Ô);
break;case ǧ.Ǖ:º();var Ó=new ᯗ(l);Ó.Ⴙ=Ò(f,Ï+8);Ó.Ŀ=l.શ.ᛩ[BitConverter.ToInt32(f,Ï+12)];Ó.ง=BitConverter.ToInt32(f,Ï+16);Ó.ฆ=
BitConverter.ToInt32(f,Ï+20);Ó.ಣ=BitConverter.ToInt32(f,Ï+24);Ï+=28;Ý.ᄩ(Ó);break;default:throw new Exception(
"Unknown special in savegame!");}}}private static Ⴙ Ò(byte[]f,int É){switch(BitConverter.ToInt32(f,É)){case 0:return Ⴙ.ڿ;default:return Ⴙ.ᄧ;}}private
static int Ñ(ђ Ð,byte[]f,int É){Ð.ŷ();Ð.Ų=(Ų)BitConverter.ToInt32(f,É+4);Ð.Ɯ=new Ꮖ(BitConverter.ToInt32(f,É+16));Ð.Ɩ=new Ꮖ(
BitConverter.ToInt32(f,É+20));Ð.Ɨ=new Ꮖ(BitConverter.ToInt32(f,É+24));Ð.Ƙ=new Ꮖ(BitConverter.ToInt32(f,É+28));Ð.ƙ=BitConverter.
ToInt32(f,É+32);Ð.ƚ=BitConverter.ToInt32(f,É+36);Ð.ƛ=BitConverter.ToInt32(f,É+40);for(var Ä=0;Ä<(int)Ů.ŏ;Ä++){Ð.Ɲ[Ä]=
BitConverter.ToInt32(f,É+44+4*Ä);}for(var Ä=0;Ä<(int)Ů.ŏ;Ä++){Ð.Ƒ[Ä]=BitConverter.ToInt32(f,É+68+4*Ä)!=0;}Ð.Ɛ=BitConverter.ToInt32(f
,É+92)!=0;for(var Ä=0;Ä<ђ.ۇ;Ä++){Ð.Ž[Ä]=BitConverter.ToInt32(f,É+96+4*Ä);}Ð.Ə=(ਖ਼)BitConverter.ToInt32(f,É+112);Ð.Ǝ=(ਖ਼)
BitConverter.ToInt32(f,É+116);for(var Ä=0;Ä<(int)ਖ਼.ŏ;Ä++){Ð.ƍ[Ä]=BitConverter.ToInt32(f,É+120+4*Ä)!=0;}for(var Ä=0;Ä<(int)ᓩ.ŏ;Ä++){Ð
.ƌ[Ä]=BitConverter.ToInt32(f,É+156+4*Ä);}for(var Ä=0;Ä<(int)ᓩ.ŏ;Ä++){Ð.Ƌ[Ä]=BitConverter.ToInt32(f,É+172+4*Ä);}Ð.Ɗ=
BitConverter.ToInt32(f,É+188)!=0;Ð.Ɖ=BitConverter.ToInt32(f,É+192)!=0;Ð.ƈ=(ᖶ)BitConverter.ToInt32(f,É+196);Ð.Ƈ=BitConverter.ToInt32(
f,É+200);Ð.Ź=BitConverter.ToInt32(f,É+204);Ð.ź=BitConverter.ToInt32(f,É+208);Ð.Ż=BitConverter.ToInt32(f,É+212);Ð.ƞ=
BitConverter.ToInt32(f,É+220);Ð.ư=BitConverter.ToInt32(f,É+224);Ð.ƪ=BitConverter.ToInt32(f,É+232);Ð.ƫ=BitConverter.ToInt32(f,É+236);
Ð.Ƭ=BitConverter.ToInt32(f,É+240);for(var Ä=0;Ä<(int)ſ.ŏ;Ä++){Ð.ƭ[Ä].Ŷ=ኑ.ጔ[BitConverter.ToInt32(f,É+244+16*Ä)];if(Ð.ƭ[Ä].
Ŷ.ġ==(int)ᚏ.ᚠ){Ð.ƭ[Ä].Ŷ=null;}Ð.ƭ[Ä].ŵ=BitConverter.ToInt32(f,É+244+16*Ä+4);Ð.ƭ[Ä].Ŵ=new Ꮖ(BitConverter.ToInt32(f,É+244+
16*Ä+8));Ð.ƭ[Ä].ų=new Ꮖ(BitConverter.ToInt32(f,É+244+16*Ä+12));}Ð.Ʈ=BitConverter.ToInt32(f,É+276)!=0;return É+280;}private
static int Ĳ(Ŀ ô,byte[]f,int É){ô.Ģ=Ꮖ.Ꭸ(BitConverter.ToInt16(f,É));ô.ģ=Ꮖ.Ꭸ(BitConverter.ToInt16(f,É+2));ô.Ĥ=BitConverter.
ToInt16(f,É+4);ô.ĥ=BitConverter.ToInt16(f,É+6);ô.Ħ=BitConverter.ToInt16(f,É+8);ô.Ě=(Ϋ)BitConverter.ToInt16(f,É+10);ô.ę=
BitConverter.ToInt16(f,É+12);ô.Ē=null;ô.ė=null;return É+14;}private static int Ĵ(ɢ ò,byte[]f,int É){ò.ᘾ=(ᶘ)BitConverter.ToInt16(f,É)
;ò.Ě=(ᶍ)BitConverter.ToInt16(f,É+2);ò.ę=BitConverter.ToInt16(f,É+4);É+=6;if(ò.ᶖ!=null){var ñ=ò.ᶖ;ñ.ʜ=Ꮖ.Ꭸ(BitConverter.
ToInt16(f,É));ñ.ʛ=Ꮖ.Ꭸ(BitConverter.ToInt16(f,É+2));ñ.ɻ=BitConverter.ToInt16(f,É+4);ñ.ʄ=BitConverter.ToInt16(f,É+6);ñ.ʅ=
BitConverter.ToInt16(f,É+8);É+=10;}if(ò.ᶗ!=null){var ñ=ò.ᶗ;ñ.ʜ=Ꮖ.Ꭸ(BitConverter.ToInt16(f,É));ñ.ʛ=Ꮖ.Ꭸ(BitConverter.ToInt16(f,É+2));ñ
.ɻ=BitConverter.ToInt16(f,É+4);ñ.ʄ=BitConverter.ToInt16(f,É+6);ñ.ʅ=BitConverter.ToInt16(f,É+8);É+=10;}return É;}}}sealed
class ĵ:ᜮ{private string[]Ĭ;private int[]ī;private int[]Ī;private ท[]Ĩ;private int ı;private ท İ;private ණ į;private int Į;
public ĵ(ባ ĭ,string Ĭ,int ī,int Ī,int ĩ,params ท[]Ĩ):base(ĭ){this.Ĭ=new[]{Ĭ};this.ī=new[]{ī};this.Ī=new[]{Ī};this.Ĩ=Ĩ;ı=ĩ;İ=Ĩ[
ı];Į=-1;}public override void ರ(){if(ኘ.ጟ.Ŷ!=Ꮣ.સ||ኘ.ጟ.સ.Ŷ!=ᵈ.ᔜ){ኘ.Ꮯ();return;}for(var Ä=0;Ä<Ĩ.Length;Ä++){Ĩ[Ä].ฑ(ኘ.ŉ[Ä]);}
}private void ĳ(){ı--;if(ı<0){ı=Ĩ.Length-1;}İ=Ĩ[ı];}private void Ķ(){ı++;if(ı>=Ĩ.Length){ı=0;}İ=Ĩ[ı];}public override
bool ଅ(ᕨ Ň){if(Ň.ܫ!=ፎ.ፏ){return true;}if(į!=null){var ŀ=į.ଅ(Ň);if(į.Ŷ==ඳ.ද){į=null;}else if(į.Ŷ==ඳ.ධ){į=null;}if(ŀ){return
true;}}if(Ň.ᕤ==ኈ.ĳ){ĳ();ኘ.ૐ(ɴ.Ȩ);}if(Ň.ᕤ==ኈ.Ķ){Ķ();ኘ.ૐ(ɴ.Ȩ);}if(Ň.ᕤ==ኈ.ቔ){į=İ.ฒ(()=>Ł(ı));ኘ.ૐ(ɴ.ɲ);}if(Ň.ᕤ==ኈ.ኡ){ኘ.ಯ();ኘ.ૐ(ɴ
.ȭ);}return true;}public void Ł(int ł){ኘ.ŉ[ł]=new string(Ĩ[ł].ǭ.ToArray());if(ኘ.ጟ.Ő(ł,ኘ.ŉ[ł])){ኘ.ಯ();Į=ł;}else{ኘ.Ꮯ();}ኘ.ૐ
(ɴ.ɲ);}public IReadOnlyList<string>Ń=>Ĭ;public IReadOnlyList<int>ń=>ī;public IReadOnlyList<int>Ņ=>Ī;public IReadOnlyList<
ᜯ>ņ=>Ĩ;public ᜯ ň=>İ;public int Ŏ=>Į;}sealed class ŉ{private static int Ŋ=6;private static int ŋ=24;private string[]Ō;
private void ō(){Ō=new string[Ŋ];}public string this[int Ć]{get{if(Ō==null){ō();}return Ō[Ć];}set{Ō[Ć]=value;}}public int ŏ=>Ō.
Length;}sealed class Ŀ{private static int ľ=26;private int Ć;private Ꮖ ć;private Ꮖ ą;private int Ĉ;private int ĉ;private int Ă
;private Ϋ ā;private int Ā;private int Ľ;private Ɠ ļ;private int[]Ļ;private Ɠ ĺ;private int Ĺ;private Ɠ ĸ;private Ⴞ ķ;
private ɢ[]ß;private Ꮖ ħ;private Ꮖ ø;public Ŀ(int Ć,Ꮖ ć,Ꮖ ą,int Ĉ,int ĉ,int Ă,Ϋ ā,int Ā){this.Ć=Ć;this.ć=ć;this.ą=ą;this.Ĉ=Ĉ;
this.ĉ=ĉ;this.Ă=Ă;this.ā=ā;this.Ā=Ā;ħ=ć;ø=ą;}public static Ŀ ċ(byte[]f,int ù,int Ć,ᮟ ü){var ć=BitConverter.ToInt16(f,ù);var
ą=BitConverter.ToInt16(f,ù+2);var Ą=ን.ቦ(f,ù+4,8);var ă=ን.ቦ(f,ù+12,8);var Ă=BitConverter.ToInt16(f,ù+20);var ā=
BitConverter.ToInt16(f,ù+22);var Ā=BitConverter.ToInt16(f,ù+24);return new Ŀ(Ć,Ꮖ.Ꭸ(ć),Ꮖ.Ꭸ(ą),ü.ᮾ(Ą),ü.ᮾ(ă),Ă,(Ϋ)ā,Ā);}public static
Ŀ[]ÿ(ಆ þ,int ý,ᮟ ü){var û=þ.ಡ(ý);if(û%ľ!=0){throw new Exception();}var f=þ.ಠ(ý);var ú=û/ľ;var Þ=new Ŀ[ú];;for(var Ä=0;Ä<ú
;Ä++){var ù=ľ*Ä;Þ[Ä]=ċ(f,ù,Ä,ü);}return Þ;}public void Ċ(){ħ=ć;ø=ą;}public Ꮖ Č(Ꮖ č){return ħ+č*(ć-ħ);}public Ꮖ ě(Ꮖ č){
return ø+č*(ą-ø);}public void Ĝ(){ħ=ć;ø=ą;}public Ğ ĝ(){return new Ğ(this);}public struct Ğ:IEnumerator<Ɠ>{private Ŀ ô;private
Ɠ ğ;private Ɠ Ġ;public Ğ(Ŀ ô){this.ô=ô;ğ=ô.ĸ;Ġ=null;}public bool MoveNext(){if(ğ!=null){Ġ=ğ;ğ=ğ.ᙃ;return true;}else{Ġ=
null;return false;}}public void Reset(){ğ=ô.ĸ;Ġ=null;}public void Dispose(){}public Ɠ Current=>Ġ;object IEnumerator.Current{
get{throw new Exception("NotImplemented");}}}public int ġ=>Ć;public Ꮖ Ģ{get{return ć;}set{ć=value;}}public Ꮖ ģ{get{return ą
;}set{ą=value;}}public int Ĥ{get{return Ĉ;}set{Ĉ=value;}}public int ĥ{get{return ĉ;}set{ĉ=value;}}public int Ħ{get{return
Ă;}set{Ă=value;}}public Ϋ Ě{get{return ā;}set{ā=value;}}public int ę{get{return Ā;}set{Ā=value;}}public int Ę{get{return
Ľ;}set{Ľ=value;}}public Ɠ ė{get{return ļ;}set{ļ=value;}}public int[]Ė{get{return Ļ;}set{Ļ=value;}}public Ɠ ĕ{get{return ĺ
;}set{ĺ=value;}}public int Ĕ{get{return Ĺ;}set{Ĺ=value;}}public Ɠ ē{get{return ĸ;}set{ĸ=value;}}public Ⴞ Ē{get{return ķ;}
set{ķ=value;}}public ɢ[]đ{get{return ß;}set{ß=value;}}}sealed class Đ{private ଦ l;public Đ(ଦ l){this.l=l;ȑ();}private bool
ď;private bool Ď;private Func<Ɠ,bool>ë;private void ȑ(){ë=Ι;}private bool ɛ(Ɠ ğ){var Η=(ğ.ኴ==ğ.ᙈ);var Θ=l.લ;Θ.ბ(ğ,ğ.ޚ,ğ.ޙ
);ğ.ᙈ=Θ.Ⴭ;ğ.ᙉ=Θ.Ⴧ;if(Η){ğ.ኴ=ğ.ᙈ;}else{if(ğ.ኴ+ğ.ǡ>ğ.ᙉ){ğ.ኴ=ğ.ᙉ-ğ.ǡ;}}if(ğ.ᙉ-ğ.ᙈ<ğ.ǡ){return false;}return true;}private
bool Ι(Ɠ ğ){if(ɛ(ğ)){return true;}if(ğ.ƙ<=0){ğ.ᘸ(ᚏ.գ);ğ.ᘾ&=~ళ.ᘏ;ğ.ǡ=Ꮖ.Ꮓ;ğ.ᙊ=Ꮖ.Ꮓ;return true;}if((ğ.ᘾ&ళ.ᘌ)!=0){l.ળ.ᆖ(ğ);
return true;}if((ğ.ᘾ&ళ.ᘐ)==0){return true;}Ď=true;if(ď&&(l.ଖ&3)==0){l.ર.ᅹ(ğ,null,null,10);var Κ=l.ળ.ᆕ(ğ.ޚ,ğ.ޙ,ğ.ኴ+ğ.ǡ/2,ё.ϩ);
var ǵ=l.ષ;Κ.ᙂ=new Ꮖ((ǵ.ѐ()-ǵ.ѐ())<<12);Κ.ᙁ=new Ꮖ((ǵ.ѐ()-ǵ.ѐ())<<12);}return true;}private bool Λ(Ŀ ô,bool Μ){Ď=false;ď=Μ;
var Ν=l.શ.ᑋ;var Ļ=ô.Ė;for(var ǃ=Ꮖ.ቀ(Ļ);ǃ<=Ꮖ.ሿ(Ļ);ǃ++){for(var ǂ=Ꮖ.Ꭿ(Ļ);ǂ<=Ꮖ.Ꮀ(Ļ);ǂ++){Ν.ᒟ(ǃ,ǂ,ë);}}return Ď;}public ί Ξ(Ŀ ô
,Ꮖ Ζ,Ꮖ Ε,bool Δ,int Γ,int Β){switch(Γ){case 0:switch(Β){case-1:if(ô.Ģ-Ζ<Ε){var Α=ô.Ģ;ô.Ģ=Ε;if(Λ(ô,Δ)){ô.Ģ=Α;Λ(ô,Δ);}
return ί.ά;}else{var Α=ô.Ģ;ô.Ģ-=Ζ;if(Λ(ô,Δ)){ô.Ģ=Α;Λ(ô,Δ);return ί.έ;}}break;case 1:if(ô.Ģ+Ζ>Ε){var Α=ô.Ģ;ô.Ģ=Ε;if(Λ(ô,Δ)){ô.Ģ
=Α;Λ(ô,Δ);}return ί.ά;}else{var Α=ô.Ģ;ô.Ģ+=Ζ;if(Λ(ô,Δ)){if(Δ){return ί.έ;}ô.Ģ=Α;Λ(ô,Δ);return ί.έ;}}break;}break;case 1:
switch(Β){case-1:if(ô.ģ-Ζ<Ε){var Α=ô.ģ;ô.ģ=Ε;if(Λ(ô,Δ)){ô.ģ=Α;Λ(ô,Δ);}return ί.ά;}else{var Α=ô.ģ;ô.ģ-=Ζ;if(Λ(ô,Δ)){if(Δ){
return ί.έ;}ô.ģ=Α;Λ(ô,Δ);return ί.έ;}}break;case 1:if(ô.ģ+Ζ>Ε){var Α=ô.ģ;ô.ģ=Ε;if(Λ(ô,Δ)){ô.ģ=Α;Λ(ô,Δ);}return ί.ά;}else{ô.ģ+=
Ζ;Λ(ô,Δ);}break;}break;}return ί.ή;}private Ŀ ΐ(ɢ ò,Ŀ ô){if((ò.ᘾ&ᶘ.ᶔ)==0){return null;}if(ò.ɣ==ô){return ò.ɤ;}return ò.ɣ;
}private Ꮖ Ώ(Ŀ ô){var Ø=ô.Ģ;for(var Ä=0;Ä<ô.đ.Length;Ä++){var ˠ=ô.đ[Ä];var ˑ=ΐ(ˠ,ô);if(ˑ==null){continue;}if(ˑ.Ģ<Ø){Ø=ˑ.Ģ
;}}return Ø;}private Ꮖ Υ(Ŀ ô){var Ø=Ꮖ.Ꭸ(-500);for(var Ä=0;Ä<ô.đ.Length;Ä++){var ˠ=ô.đ[Ä];var ˑ=ΐ(ˠ,ô);if(ˑ==null){
continue;}if(ˑ.Ģ>Ø){Ø=ˑ.Ģ;}}return Ø;}private Ꮖ Χ(Ŀ ô){var ˢ=Ꮖ.Ꮑ;for(var Ä=0;Ä<ô.đ.Length;Ä++){var ˠ=ô.đ[Ä];var ˑ=ΐ(ˠ,ô);if(ˑ==
null){continue;}if(ˑ.ģ<ˢ){ˢ=ˑ.ģ;}}return ˢ;}private Ꮖ Φ(Ŀ ô){var ˢ=Ꮖ.Ꮓ;for(var Ä=0;Ä<ô.đ.Length;Ä++){var ˠ=ô.đ[Ä];var ˑ=ΐ(ˠ,
ô);if(ˑ==null){continue;}if(ˑ.ģ>ˢ){ˢ=ˑ.ģ;}}return ˢ;}private int Τ(ɢ ò,int Σ){var Þ=l.શ.ᛩ;for(var Ä=Σ+1;Ä<Þ.Length;Ä++){
if(Þ[Ä].ę==ò.ę){return Ä;}}return-1;}private static Ꮖ Ρ=Ꮖ.Ꭸ(2);private static int Π=150;public void Ο(ɢ ò,Ɠ ğ){var Ð=ğ.ђ;
switch((int)ò.Ě){case 26:case 32:if(Ð==null){return;}if(!Ð.Ƒ[(int)ᒨ.ᒧ]&&!Ð.Ƒ[(int)ᒨ.ᒤ]){Ð.ۍ(ኑ.ጓ.ዑ);l.ૐ(Ð.Ɠ,ɴ.Ȟ,ʡ.ʟ);return;}
break;case 27:case 34:if(Ð==null){return;}if(!Ð.Ƒ[(int)ᒨ.ᒦ]&&!Ð.Ƒ[(int)ᒨ.ᒣ]){Ð.ۍ(ኑ.ጓ.ዏ);l.ૐ(Ð.Ɠ,ɴ.Ȟ,ʡ.ʟ);return;}break;case
28:case 33:if(Ð==null){return;}if(!Ð.Ƒ[(int)ᒨ.ᒥ]&&!Ð.Ƒ[(int)ᒨ.ᒢ]){Ð.ۍ(ኑ.ጓ.ዐ);l.ૐ(Ð.Ɠ,ɴ.Ȟ,ʡ.ʟ);return;}break;}var ô=ò.ᶗ.Ŀ;
if(ô.Ē!=null){var Ù=(ಪ)ô.Ē;switch((int)ò.Ě){case 1:case 26:case 27:case 28:case 117:if(Ù.ಣ==-1){Ù.ಣ=1;}else{if(ğ.ђ==null){
return;}Ù.ಣ=-1;}return;}}switch((int)ò.Ě){case 117:case 118:l.ૐ(ô.ĕ,ɴ.ʯ,ʡ.ʝ);break;case 1:case 31:l.ૐ(ô.ĕ,ɴ.ȩ,ʡ.ʝ);break;
default:l.ૐ(ô.ĕ,ɴ.ȩ,ʡ.ʝ);break;}var Ύ=new ಪ(l);l.વ.ᄩ(Ύ);ô.Ē=Ύ;Ύ.Ŀ=ô;Ύ.ಣ=1;Ύ.ݏ=Ρ;Ύ.ಢ=Π;switch((int)ò.Ě){case 1:case 26:case 27:
case 28:Ύ.ܫ=ಫ.Ϊ;break;case 31:case 32:case 33:case 34:Ύ.ܫ=ಫ.ರ;ò.Ě=0;break;case 117:Ύ.ܫ=ಫ.ಲ;Ύ.ݏ=Ρ*4;break;case 118:Ύ.ܫ=ಫ.ಳ;ò.
Ě=0;Ύ.ݏ=Ρ*4;break;}Ύ.ತ=Χ(ô);Ύ.ತ-=Ꮖ.Ꭸ(4);}public bool Ό(ɢ ò,ಫ ˌ){var Þ=l.શ.ᛩ;var ˤ=-1;var ŀ=false;while((ˤ=Τ(ò,ˤ))>=0){var
ô=Þ[ˤ];if(ô.Ē!=null){continue;}ŀ=true;var Ù=new ಪ(l);l.વ.ᄩ(Ù);ô.Ē=Ù;Ù.Ŀ=ô;Ù.ܫ=ˌ;Ù.ಢ=Π;Ù.ݏ=Ρ;switch(ˌ){case ಫ.ವ:Ù.ತ=Χ(ô);Ù
.ತ-=Ꮖ.Ꭸ(4);Ù.ಣ=-1;Ù.ݏ=Ρ*4;l.ૐ(Ù.Ŀ.ĕ,ɴ.ʰ,ʡ.ʝ);break;case ಫ.ಯ:Ù.ತ=Χ(ô);Ù.ತ-=Ꮖ.Ꭸ(4);Ù.ಣ=-1;l.ૐ(Ù.Ŀ.ĕ,ɴ.ȫ,ʡ.ʝ);break;case ಫ.ಮ
:Ù.ತ=ô.ģ;Ù.ಣ=-1;l.ૐ(Ù.Ŀ.ĕ,ɴ.ȫ,ʡ.ʝ);break;case ಫ.ಲ:case ಫ.ಳ:Ù.ಣ=1;Ù.ತ=Χ(ô);Ù.ತ-=Ꮖ.Ꭸ(4);Ù.ݏ=Ρ*4;if(Ù.ತ!=ô.ģ){l.ૐ(Ù.Ŀ.ĕ,ɴ.ʯ,
ʡ.ʝ);}break;case ಫ.Ϊ:case ಫ.ರ:Ù.ಣ=1;Ù.ತ=Χ(ô);Ù.ತ-=Ꮖ.Ꭸ(4);if(Ù.ತ!=ô.ģ){l.ૐ(Ù.Ŀ.ĕ,ɴ.ȩ,ʡ.ʝ);}break;default:break;}}return ŀ;
}public bool ˬ(ɢ ò,ಫ ˌ,Ɠ ğ){var Ð=ğ.ђ;if(Ð==null){return false;}switch((int)ò.Ě){case 99:case 133:if(Ð==null){return
false;}if(!Ð.Ƒ[(int)ᒨ.ᒧ]&&!Ð.Ƒ[(int)ᒨ.ᒤ]){Ð.ۍ(ኑ.ጓ.ዔ);l.ૐ(Ð.Ɠ,ɴ.Ȟ,ʡ.ʟ);return false;}break;case 134:case 135:if(Ð==null){
return false;}if(!Ð.Ƒ[(int)ᒨ.ᒥ]&&!Ð.Ƒ[(int)ᒨ.ᒢ]){Ð.ۍ(ኑ.ጓ.ዓ);l.ૐ(Ð.Ɠ,ɴ.Ȟ,ʡ.ʟ);return false;}break;case 136:case 137:if(Ð==null)
{return false;}if(!Ð.Ƒ[(int)ᒨ.ᒦ]&&!Ð.Ƒ[(int)ᒨ.ᒣ]){Ð.ۍ(ኑ.ጓ.ዒ);l.ૐ(Ð.Ɠ,ɴ.Ȟ,ʡ.ʟ);return false;}break;}return Ό(ò,ˌ);}private
static int ˮ=64;private Ꮖ[]Ͱ=new Ꮖ[ˮ];private Ꮖ ͱ(Ŀ ô,Ꮖ ˣ){var ˢ=ˣ;var ˡ=0;for(var Ä=0;Ä<ô.đ.Length;Ä++){var ˠ=ô.đ[Ä];var ˑ=ΐ(
ˠ,ô);if(ˑ==null){continue;}if(ˑ.Ģ>ˢ){Ͱ[ˡ++]=ˑ.Ģ;}if(ˡ>=Ͱ.Length){throw new Exception("Too many adjoining sectors!");}}if(
ˡ==0){return ˣ;}var ː=Ͱ[0];for(var Ä=1;Ä<ˡ;Ä++){if(Ͱ[Ä]<ː){ː=Ͱ[Ä];}}return ː;}private static int ˏ=3;private static Ꮖ ˎ=Ꮖ
.Ꮒ;public bool ˍ(ɢ ò,ۀ ˌ,int ˋ){switch(ˌ){case ۀ.ہ:ͺ(ò.ę);break;default:break;}var Þ=l.શ.ᛩ;var ˊ=-1;var ŀ=false;while((ˊ=
Τ(ò,ˊ))>=0){var ô=Þ[ˊ];if(ô.Ē!=null){continue;}ŀ=true;var Ö=new ݔ(l);l.વ.ᄩ(Ö);Ö.ܫ=ˌ;Ö.Ŀ=ô;Ö.Ŀ.Ē=Ö;Ö.ܬ=false;Ö.ę=ò.ę;
switch(ˌ){case ۀ.ۄ:Ö.ݏ=ˎ/2;ô.Ĥ=ò.ᶖ.Ŀ.Ĥ;Ö.ݍ=ͱ(ô,ô.Ģ);Ö.ܯ=0;Ö.ܮ=ܪ.ĳ;ô.Ě=0;l.ૐ(ô.ĕ,ɴ.Ȳ,ʡ.ʝ);break;case ۀ.ۃ:Ö.ݏ=ˎ/2;ô.Ĥ=ò.ᶖ.Ŀ.Ĥ;Ö.
ݍ=ô.Ģ+ˋ*Ꮖ.Ꮒ;Ö.ܯ=0;Ö.ܮ=ܪ.ĳ;l.ૐ(ô.ĕ,ɴ.Ȳ,ʡ.ʝ);break;case ۀ.ۂ:Ö.ݏ=ˎ*4;Ö.ݎ=Ώ(ô);if(Ö.ݎ>ô.Ģ){Ö.ݎ=ô.Ģ;}Ö.ݍ=ô.Ģ;Ö.ܯ=35*ˏ;Ö.ܮ=ܪ.Ķ;
l.ૐ(ô.ĕ,ɴ.ȧ,ʡ.ʝ);break;case ۀ.ۆ:Ö.ݏ=ˎ*8;Ö.ݎ=Ώ(ô);if(Ö.ݎ>ô.Ģ){Ö.ݎ=ô.Ģ;}Ö.ݍ=ô.Ģ;Ö.ܯ=35*ˏ;Ö.ܮ=ܪ.Ķ;l.ૐ(ô.ĕ,ɴ.ȧ,ʡ.ʝ);break;
case ۀ.ہ:Ö.ݏ=ˎ;Ö.ݎ=Ώ(ô);if(Ö.ݎ>ô.Ģ){Ö.ݎ=ô.Ģ;}Ö.ݍ=Υ(ô);if(Ö.ݍ<ô.Ģ){Ö.ݍ=ô.Ģ;}Ö.ܯ=35*ˏ;Ö.ܮ=(ܪ)(l.ષ.ѐ()&1);l.ૐ(ô.ĕ,ɴ.ȧ,ʡ.ʝ);
break;}Ή(Ö);}return ŀ;}private static int Ͷ=60;private ݔ[]ͷ=new ݔ[Ͷ];public void ͺ(int Ā){for(var Ä=0;Ä<ͷ.Length;Ä++){if(ͷ[Ä]
!=null&&ͷ[Ä].ę==Ā&&ͷ[Ä].ܮ==ܪ.ڿ){ͷ[Ä].ܮ=ͷ[Ä].ܭ;ͷ[Ä].Ⴙ=Ⴙ.ᄧ;}}}public void ͻ(ɢ ò){for(var ͼ=0;ͼ<ͷ.Length;ͼ++){if(ͷ[ͼ]!=null&&
ͷ[ͼ].ܮ!=ܪ.ڿ&&ͷ[ͼ].ę==ò.ę){ͷ[ͼ].ܭ=ͷ[ͼ].ܮ;ͷ[ͼ].ܮ=ܪ.ڿ;ͷ[ͼ].Ⴙ=Ⴙ.ڿ;}}}public void Ή(ݔ ͽ){for(var Ä=0;Ä<ͷ.Length;Ä++){if(ͷ[Ä]==
null){ͷ[Ä]=ͽ;return;}}throw new Exception("Too many active platforms!");}public void Ά(ݔ ͽ){for(var Ä=0;Ä<ͷ.Length;Ä++){if(ͽ
==ͷ[Ä]){ͷ[Ä].Ŀ.Ē=null;l.વ.ᄪ(ͷ[Ä]);ͷ[Ä]=null;return;}}throw new Exception("The platform was not found!");}private static Ꮖ
Έ=Ꮖ.Ꮒ;public bool Ί(ɢ ò,ᴀ ˌ){var Þ=l.શ.ᛩ;var ˊ=-1;var ŀ=false;while((ˊ=Τ(ò,ˊ))>=0){var ô=Þ[ˊ];if(ô.Ē!=null){continue;}ŀ=
true;var Ø=new ᴘ(l);l.વ.ᄩ(Ø);ô.Ē=Ø;Ø.ܫ=ˌ;Ø.ܬ=false;switch(ˌ){case ᴀ.ᳶ:Ø.ಣ=-1;Ø.Ŀ=ô;Ø.ݏ=Έ;Ø.ᴄ=Υ(ô);break;case ᴀ.ᳵ:Ø.ಣ=-1;Ø.Ŀ=
ô;Ø.ݏ=Έ;Ø.ᴄ=Ώ(ô);break;case ᴀ.ᳱ:Ø.ಣ=-1;Ø.Ŀ=ô;Ø.ݏ=Έ*4;Ø.ᴄ=Υ(ô);if(Ø.ᴄ!=ô.Ģ){Ø.ᴄ+=Ꮖ.Ꭸ(8);}break;case ᴀ.ᳩ:case ᴀ.ᳰ:if(ˌ==ᴀ.ᳩ
){Ø.ܬ=true;}Ø.ಣ=1;Ø.Ŀ=ô;Ø.ݏ=Έ;Ø.ᴄ=Χ(ô);if(Ø.ᴄ>ô.ģ){Ø.ᴄ=ô.ģ;}Ø.ᴄ-=Ꮖ.Ꭸ(8)*(ˌ==ᴀ.ᳩ?1:0);break;case ᴀ.ᱽ:Ø.ಣ=1;Ø.Ŀ=ô;Ø.ݏ=Έ*4;Ø
.ᴄ=ͱ(ô,ô.Ģ);break;case ᴀ.ᳯ:Ø.ಣ=1;Ø.Ŀ=ô;Ø.ݏ=Έ;Ø.ᴄ=ͱ(ô,ô.Ģ);break;case ᴀ.ᳫ:Ø.ಣ=1;Ø.Ŀ=ô;Ø.ݏ=Έ;Ø.ᴄ=Ø.Ŀ.Ģ+Ꮖ.Ꭸ(24);break;case ᴀ
.ᱻ:Ø.ಣ=1;Ø.Ŀ=ô;Ø.ݏ=Έ;Ø.ᴄ=Ø.Ŀ.Ģ+Ꮖ.Ꭸ(512);break;case ᴀ.ᳪ:Ø.ಣ=1;Ø.Ŀ=ô;Ø.ݏ=Έ;Ø.ᴄ=Ø.Ŀ.Ģ+Ꮖ.Ꭸ(24);ô.Ĥ=ò.ɣ.Ĥ;ô.Ě=ò.ɣ.Ě;break;case
ᴀ.ᳮ:var ː=int.MaxValue;Ø.ಣ=1;Ø.Ŀ=ô;Ø.ݏ=Έ;var ʽ=l.શ.ᜁ;for(var Ä=0;Ä<ô.đ.Length;Ä++){if((ô.đ[Ä].ᘾ&ᶘ.ᶔ)!=0){var ʹ=ô.đ[Ä].ᶖ;
if(ʹ.ʄ>=0){if(ʽ[ʹ.ʄ].ǡ<ː){ː=ʽ[ʹ.ʄ].ǡ;}}var ͳ=ô.đ[Ä].ᶗ;if(ͳ.ʄ>=0){if(ʽ[ͳ.ʄ].ǡ<ː){ː=ʽ[ͳ.ʄ].ǡ;}}}}Ø.ᴄ=Ø.Ŀ.Ģ+Ꮖ.Ꭸ(ː);break;case
ᴀ.ᳬ:Ø.ಣ=-1;Ø.Ŀ=ô;Ø.ݏ=Έ;Ø.ᴄ=Ώ(ô);Ø.ථ=ô.Ĥ;for(var Ä=0;Ä<ô.đ.Length;Ä++){if((ô.đ[Ä].ᘾ&ᶘ.ᶔ)!=0){if(ô.đ[Ä].ᶖ.Ŀ.ġ==ˊ){ô=ô.đ[Ä].
ᶗ.Ŀ;if(ô.Ģ==Ø.ᴄ){Ø.ථ=ô.Ĥ;Ø.ᴃ=ô.Ě;break;}}else{ô=ô.đ[Ä].ᶖ.Ŀ;if(ô.Ģ==Ø.ᴄ){Ø.ථ=ô.Ĥ;Ø.ᴃ=ô.Ě;break;}}}}break;}}return ŀ;}
public bool Ͳ(ɢ ò,ခ ˌ){var Þ=l.શ.ᛩ;var ˊ=-1;var ŀ=false;while((ˊ=Τ(ò,ˊ))>=0){var ô=Þ[ˊ];if(ô.Ē!=null){continue;}ŀ=true;var Ø=
new ᴘ(l);l.વ.ᄩ(Ø);ô.Ē=Ø;Ø.ಣ=1;Ø.Ŀ=ô;Ꮖ Ζ;Ꮖ ϊ;switch(ˌ){case ခ.ဂ:Ζ=Έ/4;ϊ=Ꮖ.Ꭸ(8);break;case ခ.ဃ:Ζ=Έ*4;ϊ=Ꮖ.Ꭸ(16);break;default:
throw new Exception("Unknown stair type!");}Ø.ݏ=Ζ;var ˢ=ô.Ģ+ϊ;Ø.ᴄ=ˢ;var ϋ=ô.Ĥ;bool ό;do{ό=false;for(var Ä=0;Ä<ô.đ.Length;Ä++)
{if(((ô.đ[Ä]).ᘾ&ᶘ.ᶔ)==0){continue;}var Ψ=(ô.đ[Ä]).ɣ;var ύ=Ψ.ġ;if(ˊ!=ύ){continue;}Ψ=(ô.đ[Ä]).ɤ;ύ=Ψ.ġ;if(Ψ.Ĥ!=ϋ){continue;}
ˢ+=ϊ;if(Ψ.Ē!=null){continue;}ô=Ψ;ˊ=ύ;Ø=new ᴘ(l);l.વ.ᄩ(Ø);ô.Ē=Ø;Ø.ಣ=1;Ø.Ŀ=ô;Ø.ݏ=Ζ;Ø.ᴄ=ˢ;ό=true;break;}}while(ό);}return ŀ;
}public bool ώ(ɢ ò,ᒓ ˌ){switch(ˌ){case ᒓ.ᒌ:case ᒓ.ᒋ:case ᒓ.ᒍ:ς(ò);break;default:break;}var Þ=l.શ.ᛩ;var ˊ=-1;var ŀ=false;
while((ˊ=Τ(ò,ˊ))>=0){var ô=Þ[ˊ];if(ô.Ē!=null){continue;}ŀ=true;var Ú=new ᒡ(l);l.વ.ᄩ(Ú);ô.Ē=Ú;Ú.Ŀ=ô;Ú.ܬ=false;switch(ˌ){case ᒓ
.ᒌ:Ú.ܬ=true;Ú.ತ=ô.ģ;Ú.ᒑ=ô.Ģ+Ꮖ.Ꭸ(8);Ú.ಣ=-1;Ú.ݏ=ω*2;break;case ᒓ.ᒋ:case ᒓ.ᒍ:case ᒓ.ᒎ:case ᒓ.ᒔ:if(ˌ==ᒓ.ᒋ||ˌ==ᒓ.ᒍ){Ú.ܬ=true;Ú
.ತ=ô.ģ;}Ú.ᒑ=ô.Ģ;if(ˌ!=ᒓ.ᒔ){Ú.ᒑ+=Ꮖ.Ꭸ(8);}Ú.ಣ=-1;Ú.ݏ=ω;break;case ᒓ.ᒏ:Ú.ತ=Φ(ô);Ú.ಣ=1;Ú.ݏ=ω;break;}Ú.ę=ô.ę;Ú.ܫ=ˌ;υ(Ú);}
return ŀ;}public static Ꮖ ω=Ꮖ.Ꮒ;public static int ψ=150;private static int χ=30;private ᒡ[]φ=new ᒡ[χ];public void υ(ᒡ Ú){for(
var Ä=0;Ä<φ.Length;Ä++){if(φ[Ä]==null){φ[Ä]=Ú;return;}}}public void τ(ᒡ Ú){for(var Ä=0;Ä<φ.Length;Ä++){if(φ[Ä]==Ú){φ[Ä].Ŀ.Ē
=null;l.વ.ᄪ(φ[Ä]);φ[Ä]=null;break;}}}public bool σ(ᒡ Ú){if(Ú==null){return false;}for(var Ä=0;Ä<φ.Length;Ä++){if(φ[Ä]==Ú)
{return true;}}return false;}public void ς(ɢ ò){for(var Ä=0;Ä<φ.Length;Ä++){if(φ[Ä]!=null&&φ[Ä].ę==ò.ę&&φ[Ä].ಣ==0){φ[Ä].ಣ
=φ[Ä].ᒒ;φ[Ä].Ⴙ=Ⴙ.ᄧ;}}}public bool ϕ(ɢ ò){var ŀ=false;for(var Ä=0;Ä<φ.Length;Ä++){if(φ[Ä]!=null&&φ[Ä].ę==ò.ę&&φ[Ä].ಣ!=0){φ
[Ä].ᒒ=φ[Ä].ಣ;φ[Ä].Ⴙ=Ⴙ.ڿ;φ[Ä].ಣ=0;ŀ=true;}}return ŀ;}public bool ϔ(ɢ ò,int ñ,Ɠ ğ){if((ğ.ᘾ&ళ.પ)!=0){return false;}if(ñ==1){
return false;}var Þ=l.શ.ᛩ;var Ā=ò.ę;for(var Ä=0;Ä<Þ.Length;Ä++){if(Þ[Ä].ę==Ā){foreach(var â in l.વ){var Ε=â as Ɠ;if(Ε==null){
continue;}if(Ε.ܫ!=ё.Ϧ){continue;}var ô=Ε.ฃ.Ŀ;if(ô.ġ!=Ä){continue;}var ϖ=ğ.ޚ;var ϗ=ğ.ޙ;var Ϙ=ğ.ኴ;if(!l.લ.Ⴣ(ğ,Ε.ޚ,Ε.ޙ)){return
false;}if(l.હ.ಗ!=ಗ.ᵌ){ğ.ኴ=ğ.ᙈ;}if(ğ.ђ!=null){ğ.ђ.Ɯ=ğ.ኴ+ğ.ђ.Ɩ;}var æ=l.ળ;var ϓ=æ.ᆕ(ϖ,ϗ,Ϙ,ё.Ϩ);l.ૐ(ϓ,ɴ.ȝ,ʡ.ʝ);var Ş=Ε.ɡ;var ϒ=æ
.ᆕ(Ε.ޚ+20*ஜ.ௐ(Ş),Ε.ޙ+20*ஜ.ஸ(Ş),ğ.ኴ,ё.Ϩ);l.ૐ(ϒ,ɴ.ȝ,ʡ.ʝ);if(ğ.ђ!=null){ğ.ᘺ=18;}ğ.ɡ=Ε.ɡ;ğ.ᙂ=ğ.ᙁ=ğ.ᙀ=Ꮖ.Ꮓ;ğ.Ĝ();if(ğ.ђ!=null){
ğ.ђ.Ĝ();}return true;}}}return false;}public void ϑ(ɢ ò){var Þ=l.શ.ᛩ;for(var Ä=0;Ä<Þ.Length;Ä++){var ô=Þ[Ä];if(ô.ę==ò.ę){
var ː=ô.Ħ;for(var ͼ=0;ͼ<ô.đ.Length;ͼ++){var Ψ=ΐ(ô.đ[ͼ],ô);if(Ψ==null){continue;}if(Ψ.Ħ<ː){ː=Ψ.Ħ;}}ô.Ħ=ː;}}}public void ϐ(ɢ
ò,int Ϗ){var Þ=l.શ.ᛩ;for(var Ä=0;Ä<Þ.Length;Ä++){var ô=Þ[Ä];if(ô.ę==ò.ę){if(Ϗ==0){for(var ͼ=0;ͼ<ô.đ.Length;ͼ++){var Ψ=ΐ(ô
.đ[ͼ],ô);if(Ψ==null){continue;}if(Ψ.Ħ>Ϗ){Ϗ=Ψ.Ħ;}}}ô.Ħ=Ϗ;}}}public void δ(ɢ ò){var Þ=l.શ.ᛩ;var ˊ=-1;while((ˊ=Τ(ò,ˊ))>=0){
var ô=Þ[ˊ];if(ô.Ē!=null){continue;}l.ଋ.ᶠ(ô,ฉ.ฌ,false);}}public bool ε(ɢ ò){var Þ=l.શ.ᛩ;var ˊ=-1;var ŀ=false;while((ˊ=Τ(ò,ˊ)
)>=0){var Ȅ=Þ[ˊ];if(Ȅ.Ē!=null){continue;}ŀ=true;var ȅ=ΐ(Ȅ.đ[0],Ȅ);if(ȅ==null){break;}for(var Ä=0;Ä<ȅ.đ.Length;Ä++){var η=
ȅ.đ[Ä].ɤ;if(η==Ȅ){continue;}if(η==null){return ŀ;}var Ý=l.વ;var γ=new ᴘ(l);Ý.ᄩ(γ);ȅ.Ē=γ;γ.ܫ=ᴀ.ᱼ;γ.ܬ=false;γ.ಣ=1;γ.Ŀ=ȅ;γ.ݏ
=Έ/2;γ.ථ=η.Ĥ;γ.ᴃ=0;γ.ᴄ=η.Ģ;var β=new ᴘ(l);Ý.ᄩ(β);Ȅ.Ē=β;β.ܫ=ᴀ.ᳶ;β.ܬ=false;β.ಣ=-1;β.Ŀ=Ȅ;β.ݏ=Έ/2;β.ᴄ=η.Ģ;break;}}return ŀ;}
public void α(Ŀ ô){var Ù=new ಪ(l);l.વ.ᄩ(Ù);ô.Ē=Ù;ô.Ě=0;Ù.Ŀ=ô;Ù.ಣ=0;Ù.ܫ=ಫ.Ϊ;Ù.ݏ=Ρ;Ù.ನ=30*35;}public void ΰ(Ŀ ô){var Ù=new ಪ(l);
l.વ.ᄩ(Ù);ô.Ē=Ù;ô.Ě=0;Ù.Ŀ=ô;Ù.ಣ=2;Ù.ܫ=ಫ.ಱ;Ù.ݏ=Ρ;Ù.ತ=Χ(ô);Ù.ತ-=Ꮖ.Ꭸ(4);Ù.ಢ=Π;Ù.ನ=5*60*35;}}public enum ί{ή,έ,ά}public enum Ϋ
{Ϊ=0}sealed class Ω{private static int ľ=12;private ட ζ;private ட θ;private Ꮖ ù;private ɡ Ş;private ɦ ο;private ɢ κ;
private Ŀ π;private Ŀ ρ;public Ω(ட ζ,ட θ,Ꮖ ù,ɡ Ş,ɦ ο,ɢ κ,Ŀ π,Ŀ ρ){this.ζ=ζ;this.θ=θ;this.ù=ù;this.Ş=Ş;this.ο=ο;this.κ=κ;this.π=
π;this.ρ=ρ;}public static Ω ċ(byte[]f,int ù,ட[]ι,ɢ[]ß){var ξ=BitConverter.ToInt16(f,ù);var ν=BitConverter.ToInt16(f,ù+2);
var Ş=BitConverter.ToInt16(f,ù+4);var μ=BitConverter.ToInt16(f,ù+6);var ñ=BitConverter.ToInt16(f,ù+8);var λ=BitConverter.
ToInt16(f,ù+10);var κ=ß[μ];var ʹ=ñ==0?κ.ᶖ:κ.ᶗ;var ͳ=ñ==0?κ.ᶗ:κ.ᶖ;return new Ω(ι[ξ],ι[ν],Ꮖ.Ꭸ(λ),new ɡ((uint)Ş<<16),ʹ,κ,ʹ.Ŀ,(κ.ᘾ&
ᶘ.ᶔ)!=0?ͳ?.Ŀ:null);}public static Ω[]ÿ(ಆ þ,int ý,ட[]ι,ɢ[]ß){var û=þ.ಡ(ý);if(û%Ω.ľ!=0){throw new Exception();}var f=þ.ಠ(ý)
;var ú=û/Ω.ľ;var ɜ=new Ω[ú];;for(var Ä=0;Ä<ú;Ä++){var ù=Ω.ľ*Ä;ɜ[Ä]=Ω.ċ(f,ù,ι,ß);}return ɜ;}public ட ɝ=>ζ;public ட ɞ=>θ;
public Ꮖ ɟ=>ù;public ɡ ɡ=>Ş;public ɦ ɦ=>ο;public ɢ ɢ=>κ;public Ŀ ɣ=>π;public Ŀ ɤ=>ρ;}sealed class ɥ:ᜮ{private string[]Ĭ;
private int[]ī;private int[]Ī;private ᜯ[]Ĩ;private int ı;private ᜯ İ;private ණ į;public ɥ(ባ ĭ,string Ĭ,int ī,int Ī,int ĩ,params
ᜯ[]Ĩ):base(ĭ){this.Ĭ=new[]{Ĭ};this.ī=new[]{ī};this.Ī=new[]{Ī};this.Ĩ=Ĩ;ı=ĩ;İ=Ĩ[ı];}public ɥ(ባ ĭ,string ɚ,int ə,int ɘ,
string ɠ,int ɧ,int ɸ,int ĩ,params ᜯ[]Ĩ):base(ĭ){this.Ĭ=new[]{ɚ,ɠ};this.ī=new[]{ə,ɧ};this.Ī=new[]{ɘ,ɸ};this.Ĩ=Ĩ;ı=ĩ;İ=Ĩ[ı];}
public override void ರ(){foreach(var ɵ in Ĩ){var ɶ=ɵ as ஔ;if(ɶ!=null){ɶ.ʕ();}var ɷ=ɵ as ʖ;if(ɷ!=null){ɷ.ʕ();}}}private void ĳ(
){ı--;if(ı<0){ı=Ĩ.Length-1;}İ=Ĩ[ı];}private void Ķ(){ı++;if(ı>=Ĩ.Length){ı=0;}İ=Ĩ[ı];}public override bool ଅ(ᕨ Ň){if(Ň.ܫ
!=ፎ.ፏ){return true;}if(į!=null){var ŀ=į.ଅ(Ň);if(į.Ŷ==ඳ.ද){į=null;}else if(į.Ŷ==ඳ.ධ){į=null;}if(ŀ){return true;}}if(Ň.ᕤ==ኈ.
ĳ){ĳ();ኘ.ૐ(ɴ.Ȩ);}if(Ň.ᕤ==ኈ.Ķ){Ķ();ኘ.ૐ(ɴ.Ȩ);}if(Ň.ᕤ==ኈ.ቀ){var ɶ=İ as ஔ;if(ɶ!=null){ɶ.Ķ();ኘ.ૐ(ɴ.ɲ);}var ɷ=İ as ʖ;if(ɷ!=null
){ɷ.Ķ();ኘ.ૐ(ɴ.Ȳ);}}if(Ň.ᕤ==ኈ.ሿ){var ɶ=İ as ஔ;if(ɶ!=null){ɶ.ĳ();ኘ.ૐ(ɴ.ɲ);}var ɷ=İ as ʖ;if(ɷ!=null){ɷ.ĳ();ኘ.ૐ(ɴ.Ȳ);}}if(Ň.ᕤ
==ኈ.ቔ){var ɶ=İ as ஔ;if(ɶ!=null){ɶ.ĳ();ኘ.ૐ(ɴ.ɲ);}var ɹ=İ as ʉ;if(ɹ!=null){if(ɹ.ʚ){if(ɹ.ʊ!=null){ɹ.ʊ();}if(ɹ.ѐ!=null){ኘ.Ꮰ(ɹ.
ѐ);}else{ኘ.ಯ();}}ኘ.ૐ(ɴ.ɲ);return true;}if(İ.ѐ!=null){ኘ.Ꮰ(İ.ѐ);ኘ.ૐ(ɴ.ɲ);}}if(Ň.ᕤ==ኈ.ኡ){ኘ.ಯ();ኘ.ૐ(ɴ.ȭ);}return true;}public
IReadOnlyList<string>Ń=>Ĭ;public IReadOnlyList<int>ń=>ī;public IReadOnlyList<int>Ņ=>Ī;public IReadOnlyList<ᜯ>ņ=>Ĩ;public ᜯ ň=>İ;}
public enum ɴ{ɳ,ɲ,ɱ,ɰ,ɯ,ɮ,ɭ,ɬ,ɫ,ɪ,ɩ,ɨ,ɗ,ɖ,Ȓ,Ȥ,ȥ,Ȧ,ȧ,Ȩ,ȩ,ȫ,Ȳ,Ȭ,ȭ,Ȯ,ȯ,Ȱ,ȱ,ȳ,Ȣ,ȡ,Ƞ,ȟ,Ȟ,ȝ,Ȝ,ț,Ț,ș,Ș,ȗ,Ȗ,ȕ,Ȕ,ȓ,Ȫ,ȴ,Ɍ,Ʌ,Ɇ,ɇ,Ɉ,ɉ,Ɋ,ɋ,
ɍ,ɔ,Ɏ,ɏ,ɐ,ɑ,ɒ,ɓ,ɕ,Ʉ,Ƀ,ɂ,Ɂ,ɀ,ȿ,Ⱦ,Ƚ,ȼ,Ȼ,Ⱥ,ȹ,ȸ,ȷ,ȶ,ȵ,ȣ,ɺ,ʃ,ʫ,ʬ,ʭ,ʮ,ʯ,ʰ,ʱ,ʲ,ʹ,ʳ,ʴ,ʵ,ʶ,ʷ,ʸ,ʺ,ʪ,ʩ,ʨ,ʧ,ʦ,ʥ,ʤ,ʣ,ʢ}public enum ʡ{ʠ
,Ƅ,ʟ,ʞ,ʝ}sealed class ɦ{private static int ľ=30;private Ꮖ ʻ;private Ꮖ ˆ;private int ˇ;private int ˈ;private int ˉ;private
Ŀ ô;public ɦ(Ꮖ ʻ,Ꮖ ˆ,int ˇ,int ˈ,int ˉ,Ŀ ô){this.ʻ=ʻ;this.ˆ=ˆ;this.ˇ=ˇ;this.ˈ=ˈ;this.ˉ=ˉ;this.ô=ô;}public static ɦ ċ(byte
[]f,int ù,Ṇ ʽ,Ŀ[]Þ){var ʻ=BitConverter.ToInt16(f,ù);var ˆ=BitConverter.ToInt16(f,ù+2);var ˁ=ን.ቦ(f,ù+4,8);var ˀ=ን.ቦ(f,ù+12
,8);var ʿ=ን.ቦ(f,ù+20,8);var ʾ=BitConverter.ToInt16(f,ù+28);return new ɦ(Ꮖ.Ꭸ(ʻ),Ꮖ.Ꭸ(ˆ),ʽ.ᮾ(ˁ),ʽ.ᮾ(ˀ),ʽ.ᮾ(ʿ),ʾ!=-1?Þ[ʾ]:
null);}public static ɦ[]ÿ(ಆ þ,int ý,Ṇ ʽ,Ŀ[]Þ){var û=þ.ಡ(ý);if(û%ľ!=0){throw new Exception();}var f=þ.ಠ(ý);var ú=û/ľ;var ʼ=
new ɦ[ú];;for(var Ä=0;Ä<ú;Ä++){var ù=ľ*Ä;ʼ[Ä]=ċ(f,ù,ʽ,Þ);}return ʼ;}public Ꮖ ʜ{get{return ʻ;}set{ʻ=value;}}public Ꮖ ʛ{get{
return ˆ;}set{ˆ=value;}}public int ɻ{get{return ˇ;}set{ˇ=value;}}public int ʄ{get{return ˈ;}set{ˈ=value;}}public int ʅ{get{
return ˉ;}set{ˉ=value;}}public Ŀ Ŀ=>ô;}static class ʆ{public static ᗮ ʇ(){var ǽ=new ᗮ(ᗄ.ᗇ());ǽ.ᗟ=640;ǽ.ᗞ=480;return ǽ;}}class
ʉ:ᜯ{private string Ĭ;private int ʀ;private int ɿ;private Action Ǳ;private Func<bool>ɽ;public ʉ(string Ĭ,int ʂ,int ʁ,int ʀ
,int ɿ,Action Ǳ,ᜮ ɾ):base(ʂ,ʁ,ɾ){this.Ĭ=Ĭ;this.ʀ=ʀ;this.ɿ=ɿ;this.Ǳ=Ǳ;this.ɽ=null;}public ʉ(string Ĭ,int ʂ,int ʁ,int ʀ,int
ɿ,Action Ǳ,ᜮ ɾ,Func<bool>ɽ):base(ʂ,ʁ,ɾ){this.Ĭ=Ĭ;this.ʀ=ʀ;this.ɿ=ɿ;this.Ǳ=Ǳ;this.ɽ=ɽ;}public string Ń=>Ĭ;public int ɼ=>ʀ;
public int ʈ=>ɿ;public Action ʊ=>Ǳ;public bool ʚ{get{if(ɽ==null){return true;}else{return ɽ();}}}}class ʖ:ᜯ{private string Ĭ;
private int ʀ;private int ɿ;private int ʗ;private int ʘ;private Func<int>ʙ;private Action<int>Ǳ;public ʖ(string Ĭ,int ʂ,int ʁ,
int ʀ,int ɿ,int ʗ,Func<int>ʙ,Action<int>Ǳ):base(ʂ,ʁ,null){this.Ĭ=Ĭ;this.ʀ=ʀ;this.ɿ=ɿ;this.ʗ=ʗ;ʘ=0;this.Ǳ=Ǳ;this.ʙ=ʙ;}public
void ʕ(){if(ʙ!=null){ʘ=ʙ();}}public void ĳ(){if(ʘ<ʒ-1){ʘ++;}if(Ǳ!=null){Ǳ(ʘ);}}public void Ķ(){if(ʘ>0){ʘ--;}if(Ǳ!=null){Ǳ(ʘ)
;}}public string Ń=>Ĭ;public int ɼ=>ʀ;public int ʈ=>ɿ;public int ʔ=>ʀ;public int ʓ=>ɿ+16;public int ʒ=>ʗ;public int ʑ=>ʘ;
}public enum ʐ{ʏ,ʎ,ʍ,ʌ}class ƒ{private static int ʋ=32;private static int ࠁ=35;private ଦ l;private bool ཀ;private int ཁ;
private ᒩ[]ག;private int[]གྷ;private int[]ང;private ɢ[]ཅ;public ƒ(ଦ l){this.l=l;ཀ=false;ག=new ᒩ[ʋ];for(var Ä=0;Ä<ག.Length;Ä++){ག
[Ä]=new ᒩ();}གྷ=new int[l.શ.ᜁ.Count];for(var Ä=0;Ä<གྷ.Length;Ä++){གྷ[Ä]=Ä;}ང=new int[l.શ.ᛧ.Count];for(var Ä=0;Ä<ང.Length;Ä++
){ང[Ä]=Ä;}}public void ཆ(int ཁ){ཀ=true;this.ཁ=ཁ;ཆ();}public void ཆ(){var ཇ=l.ଋ;var Ü=l.Đ;foreach(var ô in l.શ.ᛩ){if(ô.Ě==
0){continue;}switch((int)ô.Ě){case 1:ཇ.ᵔ(ô);break;case 2:ཇ.ᶠ(ô,ฉ.ซ,false);break;case 3:ཇ.ᶠ(ô,ฉ.ฌ,false);break;case 4:ཇ.ᶠ(
ô,ฉ.ซ,false);ô.Ě=(Ϋ)4;break;case 8:ཇ.ᶢ(ô);break;case 9:l.ଜ++;break;case 10:Ü.α(ô);break;case 12:ཇ.ᶠ(ô,ฉ.ฌ,true);break;
case 13:ཇ.ᶠ(ô,ฉ.ซ,true);break;case 14:Ü.ΰ(ô);break;case 17:ཇ.ᵣ(ô);break;}}var ༀ=new List<ɢ>();foreach(var ò in l.શ.đ){switch
((int)ò.Ě){case 48:ༀ.Add(ò);break;}}ཅ=ༀ.ToArray();}public void ໟ(ɢ ò,bool ໞ){if(!ໞ){ò.Ě=0;}var ʹ=ò.ᶖ;var ˇ=ʹ.ɻ;var ˉ=ʹ.ʅ;
var ˈ=ʹ.ʄ;var Ŗ=ɴ.Ȭ;if((int)ò.Ě==11){Ŗ=ɴ.ȭ;}var ໝ=l.શ.ᜁ.ṇ;for(var Ä=0;Ä<ໝ.Length;Ä++){if(ໝ[Ä]==ˇ){l.ૐ(ò.ĕ,Ŗ,ʡ.ʝ);ʹ.ɻ=ໝ[Ä^1]
;if(ໞ){ໜ(ò,ᒬ.Ꮀ,ໝ[Ä],ࠁ);}return;}else{if(ໝ[Ä]==ˉ){l.ૐ(ò.ĕ,Ŗ,ʡ.ʝ);ʹ.ʅ=ໝ[Ä^1];if(ໞ){ໜ(ò,ᒬ.ᒮ,ໝ[Ä],ࠁ);}return;}else{if(ໝ[Ä]==ˈ
){l.ૐ(ò.ĕ,Ŗ,ʡ.ʝ);ʹ.ʄ=ໝ[Ä^1];if(ໞ){ໜ(ò,ᒬ.Ꭿ,ໝ[Ä],ࠁ);}return;}}}}}private void ໜ(ɢ ò,ᒬ ೡ,int ϋ,int Ŧ){for(var Ä=0;Ä<ʋ;Ä++){
if(ག[Ä].ᒫ!=0&&ག[Ä].ᒭ==ò){return;}}for(var Ä=0;Ä<ʋ;Ä++){if(ག[Ä].ᒫ==0){ག[Ä].ᒭ=ò;ག[Ä].ᒪ=ೡ;ག[Ä].ථ=ϋ;ག[Ä].ᒫ=Ŧ;ག[Ä].ĕ=ò.ĕ;return
;}}throw new Exception("No button slots left!");}public void ڞ(){if(ཀ){ཁ--;if(ཁ==0){l.ଞ();}}var ཙ=l.શ.ᒳ.ᅓ;for(var ཚ=0;ཚ<ཙ
.Length;ཚ++){var ཛ=ཙ[ཚ];for(var Ä=ཛ.ᅮ;Ä<ཛ.ᅮ+ཛ.ᅯ;Ä++){var ཛྷ=ཛ.ᅮ+((l.ଖ/ཛ.ݏ+Ä)%ཛ.ᅯ);if(ཛ.ᅬ){གྷ[Ä]=ཛྷ;}else{ང[Ä]=ཛྷ;}}}foreach(
var ò in ཅ){ò.ᶖ.ʜ+=Ꮖ.Ꮒ;}for(var Ä=0;Ä<ʋ;Ä++){if(ག[Ä].ᒫ>0){ག[Ä].ᒫ--;if(ག[Ä].ᒫ==0){switch(ག[Ä].ᒪ){case ᒬ.Ꮀ:ག[Ä].ᒭ.ᶖ.ɻ=ག[Ä].ථ;
break;case ᒬ.ᒮ:ག[Ä].ᒭ.ᶖ.ʅ=ག[Ä].ථ;break;case ᒬ.Ꭿ:ག[Ä].ᒭ.ᶖ.ʄ=ག[Ä].ථ;break;}l.ૐ(ག[Ä].ĕ,ɴ.Ȭ,ʡ.ʝ,50);ག[Ä].ŷ();}}}}public int[]ཝ=>གྷ
;public int[]ཞ=>ང;}class ཟ{public void འ(String མ){}public byte[]ཡ(String མ){return new byte[1234];}}public enum э{བྷ,བ,ཕ,
པ,ན,དྷ,ད,ཐ,ཏ,ཎ,ཌྷ,ཌ,ཋ,ཊ,ཉ,ໆ,ໄ,น,ห,ฬ,อ,ฮ,ฯ,ะ,ำ,ๆ,เ,แ,โ,ใ,ไ,ๅ,ກ,ษ,ศ,ว,ฦ,ล,ฤ,ร,ย,ม,ภ,ฟ,พ,ฝ,ผ,ป,บ,า,ຂ,ຮ,ຢ,ຣ,ລ,ວ,ສ,ຫ,ອ,ຯ,ໂ,ະ,າ,ຳ
,ຽ,ເ,ແ,ໃ,ມ,ຟ,ພ,ຝ,ຜ,ປ,ບ,ນ,ທ,ຖ,ຕ,ດ,ຍ,ຊ,ຈ,ງ,ส,ར,ཨ,သ,ဟ,ဠ,အ,ဢ,ဣ,ဤ,ဦ,ၑ,ဧ,ဨ,ဩ,ဪ,ဿ,ၐ,ၒ,ဝ,လ,ရ,ယ,မ,ဘ,ဗ,ဖ,ပ,န,ဓ,ဒ,ထ,တ,ဏ,ဎ,ဥ,ၓ,ၵ,ၝ,ၡ,
ၥ,ၦ,ၮ,ၯ,ၰ,ၶ,ၽ,ၷ,ၸ,ၹ,ၺ,ၻ,ၼ,ၾ,ŏ}sealed class ၜ{private ၚ[]ཧ;public ၜ(ၚ[]ཧ){this.ཧ=ཧ;}public ၚ[]ၛ=>ཧ;}sealed class ၚ{private
bool ၕ;private ڦ[]ഉ;private bool[]ࢣ;public ၚ(bool ၕ,ڦ[]ഉ,bool[]ࢣ){this.ၕ=ၕ;this.ഉ=ഉ;this.ࢣ=ࢣ;}public bool ၔ=>ၕ;public ڦ[]ജ=>
ഉ;public bool[]ཀྵ=>ࢣ;}sealed class ဍ:Ḃ{private ၜ[]ဌ;public ဍ(ಆ þ){try{ᕣ.ᔵ.འ("Load sprites: ");var ў=new Dictionary<string,
List<ལ>>();for(var Ä=0;Ä<(int)э.ŏ;Ä++){if(!ў.ContainsKey(ኑ.ጙ[Ä]))ў.Add(ኑ.ጙ[Ä],new List<ལ>());}var چ=new Dictionary<int,ڦ>();
foreach(var ý in ས(þ)){var Ĭ=þ.ಘ[ý].Ń.Substring(0,4);if(!ў.ContainsKey(Ĭ)){continue;}var ǯ=ў[Ĭ];{var Ч=þ.ಘ[ý].Ń[4]-'A';var ཪ=þ.
ಘ[ý].Ń[5]-'0';while(ǯ.Count<Ч+1){ǯ.Add(new ལ());}if(ཪ==0){for(var Ä=0;Ä<8;Ä++){if(ǯ[Ч].ജ[Ä]==null){ǯ[Ч].ജ[Ä]=ཤ(ý,þ,چ);ǯ[Ч
].ཀྵ[Ä]=false;}}}else{if(ǯ[Ч].ജ[ཪ-1]==null){ǯ[Ч].ജ[ཪ-1]=ཤ(ý,þ,چ);ǯ[Ч].ཀྵ[ཪ-1]=false;}}}if(þ.ಘ[ý].Ń.Length==8){var Ч=þ.ಘ[ý].
Ń[6]-'A';var ཪ=þ.ಘ[ý].Ń[7]-'0';while(ǯ.Count<Ч+1){ǯ.Add(new ལ());}if(ཪ==0){for(var Ä=0;Ä<8;Ä++){if(ǯ[Ч].ജ[Ä]==null){ǯ[Ч].
ജ[Ä]=ཤ(ý,þ,چ);ǯ[Ч].ཀྵ[Ä]=true;}}}else{if(ǯ[Ч].ജ[ཪ-1]==null){ǯ[Ч].ജ[ཪ-1]=ཤ(ý,þ,چ);ǯ[Ч].ཀྵ[ཪ-1]=true;}}}}ဌ=new ၜ[(int)э.ŏ];
for(var Ä=0;Ä<ဌ.Length;Ä++){var ǯ=ў[ኑ.ጙ[Ä]];var ཧ=new ၚ[ǯ.Count];for(var ͼ=0;ͼ<ཧ.Length;ͼ++){ǯ[ͼ].င();var Ч=new ၚ(ǯ[ͼ].က(),
ǯ[ͼ].ജ,ǯ[ͼ].ཀྵ);ཧ[ͼ]=Ч;}ဌ[Ä]=new ၜ(ཧ);}ᕣ.ᔵ.འ("OK ("+چ.Count+" sprites)");}catch(Exception e){ᕣ.ᔵ.འ("Failed");throw e;}}
private static IEnumerable<int>ས(ಆ þ){var ཥ=false;for(var ý=þ.ಘ.Count-1;ý>=0;ý--){var Ĭ=þ.ಘ[ý].Ń;if(Ĭ.StartsWith("S")){if(Ĭ.
EndsWith("_END")){ཥ=true;continue;}else if(Ĭ.EndsWith("_START")){ཥ=false;continue;}}if(ཥ){if(þ.ಘ[ý].ᶛ>0){yield return ý;}}}}
private static ڦ ཤ(int ý,ಆ þ,Dictionary<int,ڦ>چ){if(!چ.ContainsKey(ý)){var Ĭ=þ.ಘ[ý].Ń;چ.Add(ý,ڦ.ċ(Ĭ,þ.ಠ(ý)));}return چ[ý];}
public ၜ this[э Т]{get{return ဌ[(int)Т];}}private class ལ{public ڦ[]ജ;public bool[]ཀྵ;public ལ(){ജ=new ڦ[8];ཀྵ=new bool[8];}
public void င(){for(var Ä=0;Ä<ജ.Length;Ä++){if(ജ[Ä]==null){throw new Exception("Missing sprite!");}}}public bool က(){for(var Ä
=1;Ä<ജ.Length;Ä++){if(ജ[Ä]!=ജ[0]){return true;}}return false;}}}public enum ခ{ဂ,ဃ}sealed class ଌ{private ଦ l;private int
ည;private bool[]စ;private int จ;private int ဆ;private int ဇ;private int ဈ;private int ဉ;private int ဋ;private Ᏺ ǵ;public
ଌ(ଦ l){this.l=l;ည=-1;စ=new bool[ኑ.ኔ.Length];Array.Copy(l.ଙ.ƍ,စ,ኑ.ኔ.Length);จ=0;ဆ=0;ဇ=0;ဈ=0;ဉ=-1;ဋ=0;ǵ=new Ᏺ();}public
void ʕ(){ည=-1;Array.Copy(l.ଙ.ƍ,စ,ኑ.ኔ.Length);จ=0;ဆ=0;ဇ=0;ဈ=0;ဉ=-1;ဋ=0;}public void ڞ(){ဇ=ǵ.ѐ();ྌ();}private void ྌ(){var Ð=l
.ଙ;if(ဈ<10){if(Ð.ƙ==0){ဈ=9;ဆ=ཫ.ൡ;จ=1;}}if(ဈ<9){if(Ð.ư!=0){var ྋ=false;for(var Ä=0;Ä<ኑ.ኔ.Length;Ä++){if(စ[Ä]!=Ð.ƍ[Ä]){ྋ=
true;စ[Ä]=Ð.ƍ[Ä];}}if(ྋ){ဈ=8;จ=ཫ.ൺ;ဆ=ྈ()+ཫ.ൎ;}}}if(ဈ<8){if(Ð.ƞ!=0&&Ð.Ʃ!=null&&Ð.Ʃ!=Ð.Ɠ){ဈ=7;if(Ð.ƙ-ည>ཫ.ഴ){จ=ཫ.ർ;ဆ=ྈ()+ཫ.ഺ;}
else{var ྊ=ᵎ.ᴥ(Ð.Ɠ.ޚ,Ð.Ɠ.ޙ,Ð.Ʃ.ޚ,Ð.Ʃ.ޙ);ɡ ྉ;bool ර;if(ྊ>Ð.Ɠ.ɡ){ྉ=ྊ-Ð.Ɠ.ɡ;ර=ྉ>ɡ.ᓣ;}else{ྉ=Ð.Ɠ.ɡ-ྊ;ර=ྉ<=ɡ.ᓣ;}จ=ཫ.ർ;ဆ=ྈ();if(ྉ<
ɡ.ᓭ){ဆ+=ཫ.ൾ;}else if(ර){ဆ+=ཫ.ഹ;}else{ဆ+=ཫ.ഹ+1;}}}}if(ဈ<7){if(Ð.ƞ!=0){if(Ð.ƙ-ည>ཫ.ഴ){ဈ=7;จ=ཫ.ർ;ဆ=ྈ()+ཫ.ഺ;}else{ဈ=6;จ=ཫ.ർ;ဆ=
ྈ()+ཫ.ൾ;}}}if(ဈ<6){if(Ð.Ɗ){if(ဉ==-1){ဉ=ཫ.ൿ;}else if(--ဉ==0){ဈ=5;ဆ=ྈ()+ཫ.ൾ;จ=1;ဉ=1;}}else{ဉ=-1;}}if(ဈ<5){if((Ð.ƈ&ᖶ.ᖱ)!=0||
Ð.Ɲ[(int)Ů.ŭ]!=0){ဈ=4;ဆ=ཫ.ൠ;จ=1;}}if(จ==0){ဆ=ྈ()+(ဇ%3);จ=ཫ.ൻ;ဈ=0;}จ--;}private int ྈ(){var Ð=l.હ.ᰁ[l.હ.ଙ];var ڹ=Ð.ƙ>100?
100:Ð.ƙ;if(ڹ!=ည){ဋ=ཫ.ശ*(((100-ڹ)*ཫ.ຄ)/101);ည=ڹ;}return ဋ;}public int ཬ=>ဆ;public static class ཫ{public static int ຄ=5;
public static int ธ=3;public static int ഐ=2;public static int വ=3;public static int ശ=ธ+ഐ+വ;public static int ഷ=2;public
static int സ=ശ*ຄ+ഷ;public static int ഹ=ธ;public static int ഺ=ഹ+ഐ;public static int ൎ=ഺ+1;public static int ൾ=ൎ+1;public static
int ൠ=ຄ*ശ;public static int ൡ=ൠ+1;public static int ൺ=(2*ᱺ.ᱹ);public static int ൻ=(ᱺ.ᱹ/2);public static int ർ=(1*ᱺ.ᱹ);
public static int ൽ=(1*ᱺ.ᱹ);public static int ൿ=(2*ᱺ.ᱹ);public static int ഴ=20;}}sealed class ള{public static int ǡ=32;private
static int ല=3;private static int റ=44;private static int ര=171;private static int യ=90;private static int മ=171;private
static int ഭ=111;private static int ബ=172;private static int ഫ=104;private static int പ=168;private static int ഩ=12;private
static int ന=10;private static int ധ=2;private static int ദ=138;private static int ഽ=171;private static int අ=221;private
static int ඡ=171;private static int ක=8;private static int ඛ=239;private static int ග=171;private static int ඝ=ක;private
static int ඞ=239;private static int ඟ=181;private static int ච=ක;private static int ජ=239;private static int ඩ=191;private
static int ඣ=3;private static int ඤ=288;private static int ඥ=173;private static int ඦ=ඣ;private static int ට=288;private
static int ඨ=179;private static int ඪ=ඣ;private static int ඖ=288;private static int ඕ=191;private static int ඔ=ඣ;private
static int ඓ=288;private static int ඒ=185;private static int එ=3;private static int ඐ=314;private static int ඏ=173;private
static int ඎ=එ;private static int ඍ=314;private static int ඌ=179;private static int උ=එ;private static int ඊ=314;private
static int ඉ=191;private static int ඈ=එ;private static int ඇ=314;private static int ආ=185;private static int ഥ=143;private
static int ത=168;private static int ೠ=143;private static int ഈ=169;private Ꮝ ȋ;private ജ ഉ;private int ǉ;private ഠ ഊ;private ഢ
ڹ;private ഢ ഋ;private ഠ[]ۅ;private ഠ[]ۊ;private ണ[]ഌ;private ഠ ž;private ണ[]എ;public ള(ಆ þ,Ꮝ ȋ){this.ȋ=ȋ;ഉ=new ജ(þ);ǉ=ȋ.Ǒ
/320;ഊ=new ഠ();ഊ.ജ=ഉ.ച;ഊ.Ǒ=ല;ഊ.ޚ=റ;ഊ.ޙ=ര;ڹ=new ഢ();ڹ.ഠ.ജ=ഉ.ച;ڹ.ഠ.Ǒ=3;ڹ.ഠ.ޚ=യ;ڹ.ഠ.ޙ=മ;ڹ.ڦ=ഉ.ഗ;ഋ=new ഢ();ഋ.ഠ.ജ=ഉ.ച;ഋ.ഠ.Ǒ=3;
ഋ.ഠ.ޚ=අ;ഋ.ഠ.ޙ=ඡ;ഋ.ڦ=ഉ.ഗ;ۅ=new ഠ[(int)ᓩ.ŏ];ۅ[0]=new ഠ();ۅ[0].ജ=ഉ.ങ;ۅ[0].Ǒ=ඣ;ۅ[0].ޚ=ඤ;ۅ[0].ޙ=ඥ;ۅ[1]=new ഠ();ۅ[1].ജ=ഉ.ങ;ۅ[1]
.Ǒ=ඦ;ۅ[1].ޚ=ට;ۅ[1].ޙ=ඨ;ۅ[2]=new ഠ();ۅ[2].ജ=ഉ.ങ;ۅ[2].Ǒ=ඪ;ۅ[2].ޚ=ඖ;ۅ[2].ޙ=ඕ;ۅ[3]=new ഠ();ۅ[3].ജ=ഉ.ങ;ۅ[3].Ǒ=ඔ;ۅ[3].ޚ=ඓ;ۅ[3].
ޙ=ඒ;ۊ=new ഠ[(int)ᓩ.ŏ];ۊ[0]=new ഠ();ۊ[0].ജ=ഉ.ങ;ۊ[0].Ǒ=එ;ۊ[0].ޚ=ඐ;ۊ[0].ޙ=ඏ;ۊ[1]=new ഠ();ۊ[1].ജ=ഉ.ങ;ۊ[1].Ǒ=ඎ;ۊ[1].ޚ=ඍ;ۊ[1].ޙ
=ඌ;ۊ[2]=new ഠ();ۊ[2].ജ=ഉ.ങ;ۊ[2].Ǒ=උ;ۊ[2].ޚ=ඊ;ۊ[2].ޙ=ඉ;ۊ[3]=new ഠ();ۊ[3].ജ=ഉ.ങ;ۊ[3].Ǒ=ඈ;ۊ[3].ޚ=ඇ;ۊ[3].ޙ=ආ;ഌ=new ണ[6];for(
var Ä=0;Ä<ഌ.Length;Ä++){ഌ[Ä]=new ണ();ഌ[Ä].ޚ=ഭ+(Ä%3)*ഩ;ഌ[Ä].ޙ=ബ+(Ä/3)*ന;ഌ[Ä].ജ=ഉ.ഔ[Ä];}ž=new ഠ();ž.ജ=ഉ.ച;ž.Ǒ=ധ;ž.ޚ=ദ;ž.ޙ=ഽ;എ
=new ണ[3];എ[0]=new ണ();എ[0].ޚ=ඛ;എ[0].ޙ=ග;എ[0].ജ=ഉ.ഖ;എ[1]=new ണ();എ[1].ޚ=ඞ;എ[1].ޙ=ඟ;എ[1].ജ=ഉ.ഖ;എ[2]=new ണ();എ[2].ޚ=ජ;എ[2].
ޙ=ඩ;എ[2].ജ=ഉ.ഖ;}public void Ǌ(ђ Ð,bool ഇ){if(ഇ){ȋ.Ꮘ(ഉ.ഛ,0,ǉ*(200-ǡ),ǉ);}if(ኑ.ኔ[(int)Ð.Ə].ƌ!=ᓩ.ᓫ){var ܣ=Ð.ƌ[(int)ኑ.ኔ[(int)
Ð.Ə].ƌ];അ(ഊ,ܣ);}ഡ(ڹ,Ð.ƙ);ഡ(ഋ,Ð.ƚ);for(var Ä=0;Ä<(int)ᓩ.ŏ;Ä++){അ(ۅ[Ä],Ð.ƌ[Ä]);അ(ۊ[Ä],Ð.Ƌ[Ä]);}if(Ð.Ɠ.ଦ.હ.ᴵ==0){if(ഇ){ȋ.Ꮘ(ഉ
.ക,ǉ*ഫ,ǉ*പ,ǉ);}for(var Ä=0;Ä<ഌ.Length;Ä++){ഞ(ഌ[Ä],Ð.ƍ[Ä+1]?1:0);}}else{var ആ=0;for(var Ä=0;Ä<Ð.Ž.Length;Ä++){ആ+=Ð.Ž[Ä];}അ
(ž,ആ);}if(ഇ){if(Ð.Ɠ.ଦ.હ.ᴶ){ȋ.Ꮘ(ഉ.ഓ[Ð.ġ],ǉ*ೠ,ǉ*ഈ,ǉ);}ȋ.Ꮘ(ഉ.ഒ[Ð.Ɠ.ଦ.ଌ.ཬ],ǉ*ഥ,ǉ*ത,ǉ);}for(var Ä=0;Ä<3;Ä++){if(Ð.Ƒ[Ä+3]){ഞ(എ[
Ä],Ä+3);}else if(Ð.Ƒ[Ä]){ഞ(എ[Ä],Ä);}}}private void അ(ഠ ೲ,int ܣ){var ೱ=ೲ.Ǒ;var ೡ=ೲ.ജ[0].Ǒ;var ˡ=ೲ.ജ[0].ǡ;var ǃ=ೲ.ޚ;var ഏ=ܣ
<0;if(ഏ){if(ೱ==2&&ܣ<-9){ܣ=-9;}else if(ೱ==3&&ܣ<-99){ܣ=-99;}ܣ=-ܣ;}ǃ=ೲ.ޚ-ೱ*ೡ;if(ܣ==1994){return;}ǃ=ೲ.ޚ;if(ܣ==0){ȋ.Ꮘ(ೲ.ജ[0],ǉ
*(ǃ-ೡ),ǉ*ೲ.ޙ,ǉ);}while(ܣ!=0&&ೱ--!=0){ǃ-=ೡ;ȋ.Ꮘ(ೲ.ജ[ܣ%10],ǉ*ǃ,ǉ*ೲ.ޙ,ǉ);ܣ/=10;}if(ഏ){ȋ.Ꮘ(ഉ.ഘ,ǉ*(ǃ-8),ǉ*ೲ.ޙ,ǉ);}}private void
ഡ(ഢ ഝ,int u){ȋ.Ꮘ(ഝ.ڦ,ǉ*ഝ.ഠ.ޚ,ǉ*ഝ.ഠ.ޙ,ǉ);അ(ഝ.ഠ,u);}private void ഞ(ണ ട,int u){ȋ.Ꮘ(ട.ജ[u],ǉ*ട.ޚ,ǉ*ട.ޙ,ǉ);}private class ഠ{
public int ޚ;public int ޙ;public int Ǒ;public ڦ[]ജ;}private class ഢ{public ഠ ഠ=new ഠ();public ڦ ڦ;}private class ണ{public int
ޚ;public int ޙ;public ڦ[]ജ;}private class ജ{public ڦ ഛ;public ڦ[]ച;public ڦ[]ങ;public ڦ ഘ;public ڦ ഗ;public ڦ[]ഖ;public ڦ
ക;public ڦ[][]ഔ;public ڦ[]ഓ;public ڦ[]ഒ;public ജ(ಆ þ){ഛ=ڦ.ÿ(þ,"STBAR");ച=new ڦ[10];ങ=new ڦ[10];for(var Ä=0;Ä<10;Ä++){ച[Ä]
=ڦ.ÿ(þ,"STTNUM"+Ä);ങ[Ä]=ڦ.ÿ(þ,"STYSNUM"+Ä);}ഘ=ڦ.ÿ(þ,"STTMINUS");ഗ=ڦ.ÿ(þ,"STTPRCNT");ഖ=new ڦ[(int)ᒨ.ŏ];for(var Ä=0;Ä<ഖ.
Length;Ä++){ഖ[Ä]=ڦ.ÿ(þ,"STKEYS"+Ä);}ക=ڦ.ÿ(þ,"STARMS");ഔ=new ڦ[6][];for(var Ä=0;Ä<6;Ä++){var ܣ=Ä+2;ഔ[Ä]=new ڦ[2];ഔ[Ä][0]=ڦ.ÿ(þ,
"STGNUM"+ܣ);ഔ[Ä][1]=ങ[ܣ];}ഓ=new ڦ[ђ.ۇ];for(var Ä=0;Ä<ഓ.Length;Ä++){ഓ[Ä]=ڦ.ÿ(þ,"STFB"+Ä);}ഒ=new ڦ[ଌ.ཫ.സ];var จ=0;for(var Ä=0;Ä<ଌ.
ཫ.ຄ;Ä++){for(var ͼ=0;ͼ<ଌ.ཫ.ธ;ͼ++){ഒ[จ++]=ڦ.ÿ(þ,"STFST"+Ä+ͼ);}ഒ[จ++]=ڦ.ÿ(þ,"STFTR"+Ä+"0");ഒ[จ++]=ڦ.ÿ(þ,"STFTL"+Ä+"0");ഒ[จ
++]=ڦ.ÿ(þ,"STFOUCH"+Ä);ഒ[จ++]=ڦ.ÿ(þ,"STFEVL"+Ä);ഒ[จ++]=ڦ.ÿ(þ,"STFKILL"+Ä);}ഒ[จ++]=ڦ.ÿ(þ,"STFGOD0");ഒ[จ++]=ڦ.ÿ(þ,"STFDEAD0"
);}}}sealed class ฉ:Ⴞ{public static int ช=5;public static int ซ=15;public static int ฌ=35;private ଦ l;private Ŀ ô;private
int ú;private int ญ;private int ฎ;private int ฏ;private int ฐ;public ฉ(ଦ l){this.l=l;}public override void Ⴛ(){if(--ú>0){
return;}if(ô.Ħ==ญ){ô.Ħ=ฎ;ú=ฐ;}else{ô.Ħ=ญ;ú=ฏ;}}public Ŀ Ŀ{get{return ô;}set{ô=value;}}public int ŏ{get{return ú;}set{ú=value;}
}public int ง{get{return ญ;}set{ญ=value;}}public int ฆ{get{return ฎ;}set{ฎ=value;}}public int ฅ{get{return ฏ;}set{ฏ=value
;}}public int ค{get{return ฐ;}set{ฐ=value;}}}sealed class ฃ{private static int ľ=4;private Ŀ ô;private int ข;private int
ก;public ฃ(Ŀ ô,int ข,int ก){this.ô=ô;this.ข=ข;this.ก=ก;}public static ฃ ċ(byte[]f,int ù,Ω[]ɜ){var ข=BitConverter.ToInt16(
f,ù);var ด=BitConverter.ToInt16(f,ù+2);return new ฃ(ɜ[ด].ɦ.Ŀ,ข,ด);}public static ฃ[]ÿ(ಆ þ,int ý,Ω[]ɜ){var û=þ.ಡ(ý);if(û%ฃ
.ľ!=0){throw new Exception();}var f=þ.ಠ(ý);var ú=û/ฃ.ľ;var ߍ=new ฃ[ú];for(var Ä=0;Ä<ú;Ä++){var ù=ฃ.ľ*Ä;ߍ[Ä]=ฃ.ċ(f,ù,ɜ);}
return ߍ;}public Ŀ Ŀ=>ô;public int ต=>ข;public int ถ=>ก;}class ท:ᜯ{private int ʀ;private int ɿ;private IReadOnlyList<char>Ǉ;
private ණ ณ;public ท(int ʂ,int ʁ,int ʀ,int ɿ):base(ʂ,ʁ,null){this.ʀ=ʀ;this.ɿ=ɿ;}public ණ ฒ(Action ඵ){ณ=new ණ(Ǉ!=null?Ǉ:new char
[0],ڇ=>{},ڇ=>{Ǉ=ڇ;ณ=null;ඵ();},()=>{ณ=null;});return ณ;}public void ฑ(string Ǉ){if(Ǉ!=null){this.Ǉ=Ǉ.ToCharArray();}}
public IReadOnlyList<char>ǭ{get{if(ณ==null){return Ǉ;}else{return ณ.ǭ;}}}public int ɼ=>ʀ;public int ʈ=>ɿ;public bool ෆ=>ณ!=
null;}sealed class ණ{private List<char>Ǉ;private Action<IReadOnlyList<char>>ප;private Action<IReadOnlyList<char>>ඵ;private
Action බ;private ඳ Í;public ණ(IReadOnlyList<char>භ,Action<IReadOnlyList<char>>ප,Action<IReadOnlyList<char>>ඵ,Action බ){this.Ǉ=
භ.ToList();this.ප=ප;this.ඵ=ඵ;this.බ=බ;Í=ඳ.න;}public bool ଅ(ᕨ Ň){var ම=ቨ.ቧ(Ň.ᕤ);if(ම!=0){Ǉ.Add(ම);ප(Ǉ);return true;}if(Ň.ᕤ
==ኈ.ቕ&&Ň.ܫ==ፎ.ፏ){if(Ǉ.Count>0){Ǉ.RemoveAt(Ǉ.Count-1);}ප(Ǉ);return true;}if(Ň.ᕤ==ኈ.ቔ&&Ň.ܫ==ፎ.ፏ){ඵ(Ǉ);Í=ඳ.ධ;return true;}if(
Ň.ᕤ==ኈ.ኡ&&Ň.ܫ==ፎ.ፏ){බ();Í=ඳ.ද;return true;}return true;}public IReadOnlyList<char>ǭ=>Ǉ;public ඳ Ŷ=>Í;}public enum ඳ{න,ධ,ද
}sealed class ථ{private string Ĭ;private bool ඬ;private int ځ;private int ˢ;private ᅂ[]ഉ;private ڦ ත;public ථ(string Ĭ,
bool ඬ,int ځ,int ˢ,ᅂ[]ഉ){this.Ĭ=Ĭ;this.ඬ=ඬ;this.ځ=ځ;this.ˢ=ˢ;this.ഉ=ഉ;ත=ෂ(Ĭ,ځ,ˢ,ഉ);}public static ථ ċ(byte[]f,int ù,ڦ[]ස){
var Ĭ=ን.ቦ(f,ù,8);var ඬ=BitConverter.ToInt32(f,ù+8);var ځ=BitConverter.ToInt16(f,ù+12);var ˢ=BitConverter.ToInt16(f,ù+14);
var ශ=BitConverter.ToInt16(f,ù+20);var ഉ=new ᅂ[ශ];for(var Ä=0;Ä<ශ;Ä++){var හ=ù+22+ᅂ.ᅁ*Ä;ഉ[Ä]=ᅂ.ċ(f,හ,ස);}return new ථ(Ĭ,ඬ!=
0,ځ,ˢ,ഉ);}public static string ළ(byte[]f,int ù){return ን.ቦ(f,ù,8);}public static int ڎ(byte[]f,int ù){return BitConverter
.ToInt16(f,ù+14);}private static ڦ ෂ(string Ĭ,int ځ,int ˢ,ᅂ[]ഉ){var ශ=new int[ځ];var څ=new ᖒ[ځ][];var ව=0;foreach(var ڌ
in ഉ){var ල=ڌ.ᅌ;var ර=ල+ڌ.Ǒ;var Σ=Math.Max(ල,0);var ය=Math.Min(ර,ځ);for(var ǃ=Σ;ǃ<ය;ǃ++){ශ[ǃ]++;if(ශ[ǃ]==2){ව++;}څ[ǃ]=ڌ.ټ[
ǃ-ڌ.ᅌ];}}var ඹ=Math.Max(128-ˢ,0);var f=new byte[ˢ*ව+ඹ];var Ä=0;for(var ǃ=0;ǃ<ځ;ǃ++){if(ශ[ǃ]==0){څ[ǃ]=Array.Empty<ᖒ>();}if
(ශ[ǃ]>=2){var ࠐ=new ᖒ(0,f,ˢ*Ä,ˢ);foreach(var ڌ in ഉ){var ᅚ=ǃ-ڌ.ᅌ;if(ᅚ<0||ᅚ>=ڌ.Ǒ){continue;}var ᅛ=ڌ.ټ[ᅚ];ᅜ(ᅛ,ࠐ.Ꮁ,ࠐ.ɟ,ڌ.ᅍ,ˢ
);}څ[ǃ]=new[]{ࠐ};Ä++;}}return new ڦ(Ĭ,ځ,ˢ,0,0,څ);}private static void ᅜ(ᖒ[]ࠒ,byte[]ƻ,int ᅝ,int ᅟ,int ᅤ){foreach(var ࠐ in
ࠒ){var ᅠ=ࠐ.ɟ;var ᅡ=ᅝ+ᅟ+ࠐ.ᖔ;var û=ࠐ.ᖏ;var ᅢ=-(ᅟ+ࠐ.ᖔ);if(ᅢ>0){ᅠ+=ᅢ;ᅡ+=ᅢ;û-=ᅢ;}var ᅣ=ᅟ+ࠐ.ᖔ+ࠐ.ᖏ-ᅤ;if(ᅣ>0){û-=ᅣ;}if(û>0){Array
.Copy(ࠐ.Ꮁ,ᅠ,ƻ,ᅡ,û);}}}public override string ToString(){return Ĭ;}public string Ń=>Ĭ;public bool ᅙ=>ඬ;public int Ǒ=>ځ;
public int ǡ=>ˢ;public IReadOnlyList<ᅂ>ജ=>ഉ;public ڦ ᅘ=>ත;}sealed class ᅗ{private ᅞ[]ཙ;public ᅗ(Ṇ ʽ,ᮟ ü){try{ᕣ.ᔵ.འ(
"Load texture animation info: ");var ǯ=new List<ᅞ>();foreach(var ᅖ in ኑ.ᅗ){int ᅕ;int ᅔ;if(ᅖ.ᅬ){if(ʽ.ᮾ(ᅖ.ᒷ)==-1){continue;}ᅕ=ʽ.ᮾ(ᅖ.ᒸ);ᅔ=ʽ.ᮾ(ᅖ.ᒷ);}else{
if(ü.ᮾ(ᅖ.ᒷ)==-1){continue;}ᅕ=ü.ᮾ(ᅖ.ᒸ);ᅔ=ü.ᮾ(ᅖ.ᒷ);}var ཛ=new ᅞ(ᅖ.ᅬ,ᅕ,ᅔ,ᅕ-ᅔ+1,ᅖ.ݏ);if(ཛ.ᅯ<2){throw new Exception(
"Bad animation cycle from "+ᅖ.ᒷ+" to "+ᅖ.ᒸ+"!");}ǯ.Add(ཛ);}ཙ=ǯ.ToArray();ᕣ.ᔵ.འ("OK");}catch(Exception e){ᕣ.ᔵ.འ("Failed");throw e;}}public ᅞ[]ᅓ=>ཙ;}
sealed class ᅞ{private bool ᅥ;private int ᅕ;private int ᅔ;private int ᅫ;private int Ζ;public ᅞ(bool ᅥ,int ᅕ,int ᅔ,int ᅫ,int Ζ)
{this.ᅥ=ᅥ;this.ᅕ=ᅕ;this.ᅔ=ᅔ;this.ᅫ=ᅫ;this.Ζ=Ζ;}public bool ᅬ=>ᅥ;public int ᅭ=>ᅕ;public int ᅮ=>ᅔ;public int ᅯ=>ᅫ;public
int ݏ=>Ζ;}sealed class ᅰ:Ṇ{private List<ථ>ʽ;private Dictionary<string,ථ>ᅪ;private Dictionary<string,int>ᅩ;private int[]ໝ;
public ᅰ(ಆ þ):this(þ,false){}public ᅰ(ಆ þ,bool ᅨ){ᅧ(þ);ᅒ();}private void ᅧ(ಆ þ){ʽ=new List<ථ>();ᅪ=new Dictionary<string,ථ>();ᅩ
=new Dictionary<string,int>();var ഉ=ᅆ(þ);for(var ᅦ=1;ᅦ<=2;ᅦ++){var ಜ=þ.ಞ("TEXTURE"+ᅦ);if(ಜ==-1){break;}var f=þ.ಠ(ಜ);var ú
=BitConverter.ToInt32(f,0);for(var Ä=0;Ä<ú;Ä++){var ù=BitConverter.ToInt32(f,4+4*Ä);var ϋ=ථ.ċ(f,ù,ഉ);if(!ᅩ.ContainsKey(ϋ.
Ń)){ᅩ.Add(ϋ.Ń,ʽ.Count);}ʽ.Add(ϋ);if(!ᅪ.ContainsKey(ϋ.Ń)){ᅪ.Add(ϋ.Ń,ϋ);}}}}private void ᅒ(){var ǯ=new List<int>();foreach(
var ᅃ in ኑ.ኒ){var ᅄ=ᮾ(ᅃ.Item1);var ᅅ=ᮾ(ᅃ.Item2);if(ᅄ!=-1&&ᅅ!=-1){ǯ.Add(ᅄ);ǯ.Add(ᅅ);}}ໝ=ǯ.ToArray();}public int ᮾ(string Ĭ){
if(Ĭ[0]=='-'){return 0;}int Ć;if(ᅩ.TryGetValue(Ĭ,out Ć)){return Ć;}else{return-1;}}private static ڦ[]ᅆ(ಆ þ){var ᅈ=ᅇ(þ);var
ഉ=new ڦ[ᅈ.Length];for(var Ä=0;Ä<ഉ.Length;Ä++){var Ĭ=ᅈ[Ä];if(þ.ಞ(Ĭ)==-1){continue;}var f=þ.ಠ(Ĭ);ഉ[Ä]=ڦ.ċ(Ĭ,f);}return ഉ;}
private static string[]ᅇ(ಆ þ){var f=þ.ಠ("PNAMES");var ú=BitConverter.ToInt32(f,0);var ಅ=new string[ú];for(var Ä=0;Ä<ಅ.Length;Ä
++){ಅ[Ä]=ን.ቦ(f,4+8*Ä,8);}return ಅ;}public IEnumerator<ථ>GetEnumerator(){return ʽ.GetEnumerator();}IEnumerator IEnumerable.
GetEnumerator(){return ʽ.GetEnumerator();}public int Count=>ʽ.Count;public ථ this[int ܣ]=>ʽ[ܣ];public ථ this[string Ĭ]=>ᅪ[Ĭ];public
int[]ṇ=>ໝ;}sealed class ᅂ{public const int ᅁ=10;private int ᅀ;private int ᄿ;private ڦ ڌ;public ᅂ(int ᅀ,int ᄿ,ڦ ڌ){this.ᅀ=ᅀ;
this.ᄿ=ᄿ;this.ڌ=ڌ;}public static ᅂ ċ(byte[]f,int ù,ڦ[]ഉ){var ᅀ=BitConverter.ToInt16(f,ù);var ᄿ=BitConverter.ToInt16(f,ù+2);
var ᅋ=BitConverter.ToInt16(f,ù+4);return new ᅂ(ᅀ,ᄿ,ഉ[ᅋ]);}public string Ń=>ڌ.Ń;public int ᅌ=>ᅀ;public int ᅍ=>ᄿ;public int Ǒ
=>ڌ.Ǒ;public int ǡ=>ڌ.ǡ;public ᖒ[][]ټ=>ڌ.ټ;}sealed class ળ{private ଦ l;public ળ(ଦ l){this.l=l;ᅐ();ᆨ();ᆡ();}private ᛔ[]ᅎ;
private List<ᛔ>ᅏ;private void ᅐ(){ᅎ=new ᛔ[ђ.ۇ];ᅏ=new List<ᛔ>();}public void ᅑ(ᛔ ର){if(ର.ܫ==11){if(ᅏ.Count<10){ᅏ.Add(ର);}return;
}if(ର.ܫ<=4){var ä=ର.ܫ-1;if(ä<0){return;}ᅎ[ä]=ର;if(l.હ.ᴵ==0){ᅉ(ର);}return;}if(ର.ܫ==11||ର.ܫ<=4){return;}if(!l.હ.ᴶ&&((int)ର.
ᘾ&16)!=0){return;}int ᅊ;if(l.હ.ᴷ==ᵅ.Р){ᅊ=1;}else if(l.હ.ᴷ==ᵅ.ᵇ){ᅊ=4;}else{ᅊ=1<<((int)l.હ.ᴷ-1);}if(((int)ର.ᘾ&ᅊ)==0){return
;}int Ä;for(Ä=0;Ä<ኑ.ዃ.Length;Ä++){if(ର.ܫ==ኑ.ዃ[Ä].ᚦ){break;}}if(Ä==ኑ.ዃ.Length){throw new Exception("Unknown type!");}if(l.
હ.ᴵ!=0&&(ኑ.ዃ[Ä].ᘾ&ళ.ᘄ)!=0){return;}if(l.હ.ᴲ&&(Ä==(int)ё.м||(ኑ.ዃ[Ä].ᘾ&ళ.ᘇ)!=0)){return;}Ꮖ ǃ=ର.ޚ;Ꮖ ǂ=ର.ޙ;Ꮖ ݳ;if((ኑ.ዃ[Ä].ᘾ&ళ
.ᘕ)!=0){ݳ=Ɠ.ᜧ;}else{ݳ=Ɠ.ᜦ;}var ã=ᆕ(ǃ,ǂ,ݳ,(ё)Ä);ã.ᘬ=ର;if(ã.ŵ>0){ã.ŵ=1+(l.ષ.ѐ()%ã.ŵ);}if((ã.ᘾ&ళ.ᘇ)!=0){l.ଓ++;}if((ã.ᘾ&ళ.ᘆ)
!=0){l.କ++;}ã.ɡ=ର.ɡ;if((ର.ᘾ&ᅻ.ᅾ)!=0){ã.ᘾ|=ళ.ᅾ;}}public void ᅉ(ᛔ ର){var Ë=l.હ.ᰁ;var ä=ର.ܫ-1;if(!Ë[ä].Ÿ){return;}var Ð=Ë[ä];
if(Ð.Ų==Ų.ů){Ë[ä].ů();}var ǃ=ର.ޚ;var ǂ=ର.ޙ;var ݳ=Ɠ.ᜦ;var ã=ᆕ(ǃ,ǂ,ݳ,ё.ђ);if(ର.ܫ-1==l.હ.ଙ){l.ଌ.ʕ();l.હ.ᴽ.ᶪ(ã);}if(ä>=1){ã.ᘾ
|=(ళ)((ର.ܫ-1)<<(int)ళ.ᘂ);}ã.ɡ=ର.ɡ;ã.ђ=Ð;ã.ƙ=Ð.ƙ;Ð.Ɠ=ã;Ð.Ų=Ų.ű;Ð.Ƈ=0;Ð.Ɔ=null;Ð.Ɣ=0;Ð.ƞ=0;Ð.ư=0;Ð.ƪ=0;Ð.ƫ=0;Ð.Ɩ=ђ.ۈ;l.Ư.Š(Ð
);if(l.હ.ᴵ!=0){for(var Ä=0;Ä<(int)ᒨ.ŏ;Ä++){Ð.Ƒ[Ä]=true;}}}public IReadOnlyList<ᛔ>ᅱ=>ᅎ;public IReadOnlyList<ᛔ>ᆔ=>ᅏ;public
Ɠ ᆕ(Ꮖ ǃ,Ꮖ ǂ,Ꮖ ݳ,ё ˌ){var ã=new Ɠ(l);var ݤ=ኑ.ዃ[(int)ˌ];ã.ܫ=ˌ;ã.ᘿ=ݤ;ã.ޚ=ǃ;ã.ޙ=ǂ;ã.ᙊ=ݤ.ᙊ;ã.ǡ=ݤ.ǡ;ã.ᘾ=ݤ.ᘾ;ã.ƙ=ݤ.ᚢ;if(l.હ.ᴷ!=ᵅ
.ᵇ){ã.ᘺ=ݤ.ᘺ;}ã.ᘭ=l.ષ.ѐ()%ђ.ۇ;var ᆙ=ኑ.ጔ[(int)ݤ.ᚡ];ã.Ŷ=ᆙ;ã.ŵ=ᆙ.ŵ;ã.э=ᆙ.э;ã.ю=ᆙ.ю;l.લ.ლ(ã);ã.ᙈ=ã.ฃ.Ŀ.Ģ;ã.ᙉ=ã.ฃ.Ŀ.ģ;if(ݳ==Ɠ.ᜦ
){ã.ኴ=ã.ᙈ;}else if(ݳ==Ɠ.ᜧ){ã.ኴ=ã.ᙉ-ã.ᘿ.ǡ;}else{ã.ኴ=ݳ;}l.વ.ᄩ(ã);return ã;}public void ᆖ(Ɠ ã){var Θ=l.લ;if((ã.ᘾ&ళ.Ě)!=0&&(ã
.ᘾ&ళ.ᘌ)==0&&(ã.ܫ!=ё.Џ)&&(ã.ܫ!=ё.Б)){ᆞ[ᆟ]=ã.ᘬ;ᆤ[ᆟ]=l.ଖ;ᆟ=(ᆟ+1)&(ᆜ-1);if(ᆟ==ᆠ){ᆠ=(ᆠ+1)&(ᆜ-1);}}Θ.ნ(ã);l.ૠ(ã);l.વ.ᄪ(ã);}
private int ᆗ(ё ˌ){if(l.હ.ᴴ||l.હ.ᴷ==ᵅ.ᵇ){switch(ˌ){case ё.о:case ё.Ϸ:case ё.ϵ:return 20*Ꮖ.Ꮔ;default:return ኑ.ዃ[(int)ˌ].ݏ;}}else
{return ኑ.ዃ[(int)ˌ].ݏ;}}private void ᆘ(Ɠ ݞ){ݞ.ŵ-=l.ષ.ѐ()&3;if(ݞ.ŵ<1){ݞ.ŵ=1;}ݞ.ޚ+=(ݞ.ᙂ>>1);ݞ.ޙ+=(ݞ.ᙁ>>1);ݞ.ኴ+=(ݞ.ᙀ>>1);if(
!l.લ.შ(ݞ,ݞ.ޚ,ݞ.ޙ)){l.ર.ᅴ(ݞ);}}public Ɠ ᆓ(Ɠ ࠒ,Ɠ Ε,ё ˌ){var ݞ=ᆕ(ࠒ.ޚ,ࠒ.ޙ,ࠒ.ኴ+Ꮖ.Ꭸ(32),ˌ);if(ݞ.ᘿ.ᚤ!=0){l.ૐ(ݞ,ݞ.ᘿ.ᚤ,ʡ.ʝ);}ݞ.ᘻ=ࠒ
;var Ş=ᵎ.ᴥ(ࠒ.ޚ,ࠒ.ޙ,Ε.ޚ,Ε.ޙ);if((Ε.ᘾ&ళ.ᘋ)!=0){var ǵ=l.ષ;Ş+=new ɡ((ǵ.ѐ()-ǵ.ѐ())<<20);}var Ζ=ᆗ(ݞ.ܫ);ݞ.ɡ=Ş;ݞ.ᙂ=new Ꮖ(Ζ)*ஜ.ௐ(Ş
);ݞ.ᙁ=new Ꮖ(Ζ)*ஜ.ஸ(Ş);var њ=ᵎ.ᴭ(Ε.ޚ-ࠒ.ޚ,Ε.ޙ-ࠒ.ޙ);var ܣ=(Ε.ኴ-ࠒ.ኴ).Ꮁ;var ܢ=(њ/Ζ).Ꮁ;if(ܢ<1){ܢ=1;}ݞ.ᙀ=new Ꮖ(ܣ/ܢ);ᆘ(ݞ);return
ݞ;}public void ᆒ(Ɠ ࠒ,ё ˌ){var ਲ਼=l.ભ;var Ş=ࠒ.ɡ;var ѭ=ਲ਼.ᮼ(ࠒ,Ş,Ꮖ.Ꭸ(16*64));if(ਲ਼.ᮯ==null){Ş+=new ɡ(1<<26);ѭ=ਲ਼.ᮼ(ࠒ,Ş,Ꮖ.Ꭸ(16*64
));if(ਲ਼.ᮯ==null){Ş-=new ɡ(2<<26);ѭ=ਲ਼.ᮼ(ࠒ,Ş,Ꮖ.Ꭸ(16*64));}if(ਲ਼.ᮯ==null){Ş=ࠒ.ɡ;ѭ=Ꮖ.Ꮓ;}}var ǃ=ࠒ.ޚ;var ǂ=ࠒ.ޙ;var ݳ=ࠒ.ኴ+Ꮖ.Ꭸ(32)
;var ݞ=ᆕ(ǃ,ǂ,ݳ,ˌ);if(ݞ.ᘿ.ᚤ!=0){l.ૐ(ݞ,ݞ.ᘿ.ᚤ,ʡ.ʝ);}ݞ.ᘻ=ࠒ;ݞ.ɡ=Ş;ݞ.ᙂ=new Ꮖ(ݞ.ᘿ.ݏ)*ஜ.ௐ(Ş);ݞ.ᙁ=new Ꮖ(ݞ.ᘿ.ݏ)*ஜ.ஸ(Ş);ݞ.ᙀ=new Ꮖ(ݞ.
ᘿ.ݏ)*ѭ;ᆘ(ݞ);}private static int ᆥ=32;private int ᆦ;private Ɠ[]ᆧ;private void ᆨ(){ᆦ=0;ᆧ=new Ɠ[ᆥ];}public bool ᆩ(int ᆫ,ᛔ ᆣ)
{var Ë=l.હ.ᰁ;if(Ë[ᆫ].Ɠ==null){for(var Ä=0;Ä<ᆫ;Ä++){if(Ë[Ä].Ɠ.ޚ==ᆣ.ޚ&&Ë[Ä].Ɠ.ޙ==ᆣ.ޙ){return false;}}return true;}var ǃ=ᆣ.ޚ
;var ǂ=ᆣ.ޙ;if(!l.લ.ბ(Ë[ᆫ].Ɠ,ǃ,ǂ)){return false;}if(ᆦ>=ᆥ){ᆖ(ᆧ[ᆦ%ᆥ]);}ᆧ[ᆦ%ᆥ]=Ë[ᆫ].Ɠ;ᆦ++;var त=ᵎ.ᴩ(ǃ,ǂ,l.શ);var Ş=(ɡ.ᓭ.Ꮁ>>ஜ.
ல)*((int)Math.Round(ᆣ.ɡ.ᓜ())/45);Ꮖ ᆬ;Ꮖ ᆪ;switch(Ş){case 4096:ᆬ=ஜ.ழ(2048);ᆪ=ஜ.ழ(0);break;case 5120:ᆬ=ஜ.ழ(3072);ᆪ=ஜ.ழ(1024)
;break;case 6144:ᆬ=ஜ.ஸ(0);ᆪ=ஜ.ழ(2048);break;case 7168:ᆬ=ஜ.ஸ(1024);ᆪ=ஜ.ழ(3072);break;case 0:case 1024:case 2048:case 3072:
ᆬ=ஜ.ௐ((int)Ş);ᆪ=ஜ.ஸ((int)Ş);break;default:throw new Exception("Unexpected angle: "+Ş);}var ઠ=ᆕ(ǃ+20*ᆬ,ǂ+20*ᆪ,त.Ŀ.Ģ,ё.Ϩ);
if(!l.ଛ){l.ૐ(ઠ,ɴ.ȝ,ʡ.ʝ);}return true;}public void ᆝ(int ä){var ᆛ=ᅏ.Count;if(ᆛ<4){throw new Exception("Only "+ᆛ+
" deathmatch spots, 4 required");}var ǵ=l.ષ;for(var ͼ=0;ͼ<20;ͼ++){var Ä=ǵ.ѐ()%ᆛ;if(ᆩ(ä,ᅏ[Ä])){ᅏ[Ä].ܫ=ä+1;ᅉ(ᅏ[Ä]);return;}}ᅉ(ᅎ[ä]);}private static int ᆜ
=128;private ᛔ[]ᆞ;private int[]ᆤ;private int ᆟ;private int ᆠ;private void ᆡ(){ᆞ=new ᛔ[ᆜ];ᆤ=new int[ᆜ];ᆟ=0;ᆠ=0;}public
void ᆢ(){if(l.હ.ᴵ!=2){return;}if(ᆟ==ᆠ){return;}if(l.ଖ-ᆤ[ᆠ]<30*35){return;}var ᆣ=ᆞ[ᆠ];var ǃ=ᆣ.ޚ;var ǂ=ᆣ.ޙ;var Ⴤ=ᵎ.ᴩ(ǃ,ǂ,l.શ);
var ઠ=ᆕ(ǃ,ǂ,Ⴤ.Ŀ.Ģ,ё.ϧ);l.ૐ(ઠ,ɴ.ʱ,ʡ.ʝ);int Ä;for(Ä=0;Ä<ኑ.ዃ.Length;Ä++){if(ᆣ.ܫ==ኑ.ዃ[Ä].ᚦ){break;}}Ꮖ ݳ;if((ኑ.ዃ[Ä].ᘾ&ళ.ᘕ)!=0){ݳ
=Ɠ.ᜧ;}else{ݳ=Ɠ.ᜦ;}ઠ=ᆕ(ǃ,ǂ,ݳ,(ё)Ä);ઠ.ᘬ=ᆣ;ઠ.ɡ=ᆣ.ɡ;ᆠ=(ᆠ+1)&(ᆜ-1);}}[Flags]public enum ᅻ{ᅼ=1,Ϊ=2,ᅽ=4,ᅾ=8}sealed class ર{
private ଦ l;public ર(ଦ l){this.l=l;ᆑ();}public void ᆀ(Ɠ ࠒ,Ɠ Ψ){Ψ.ᘾ&=~(ళ.ᘐ|ళ.ᘍ|ళ.ᘅ);if(Ψ.ܫ!=ё.м){Ψ.ᘾ&=~ళ.ᘖ;}Ψ.ᘾ|=ళ.ᘉ|ళ.ᘗ;Ψ.ǡ=new
Ꮖ(Ψ.ǡ.Ꮁ>>2);if(ࠒ!=null&&ࠒ.ђ!=null){if((Ψ.ᘾ&ళ.ᘇ)!=0){ࠒ.ђ.Ź++;}if(Ψ.ђ!=null){ࠒ.ђ.Ž[Ψ.ђ.ġ]++;}}else if(!l.હ.ᴶ&&(Ψ.ᘾ&ళ.ᘇ)!=0)
{l.હ.ᰁ[0].Ź++;}if(Ψ.ђ!=null){if(ࠒ==null){Ψ.ђ.Ž[Ψ.ђ.ġ]++;}Ψ.ᘾ&=~ళ.ᘏ;Ψ.ђ.Ų=Ų.Ű;l.Ư.Ř(Ψ.ђ);var ᆁ=l.ଏ;if(Ψ.ђ.ġ==l.હ.ଙ&&ᆁ.ᔢ){ᆁ
.ಯ();}}if(Ψ.ƙ<-Ψ.ᘿ.ᚢ&&Ψ.ᘿ.ᚕ!=0){Ψ.ᘸ(Ψ.ᘿ.ᚕ);}else{Ψ.ᘸ(Ψ.ᘿ.ᚖ);}Ψ.ŵ-=l.ષ.ѐ()&3;if(Ψ.ŵ<1){Ψ.ŵ=1;}ё ɵ;switch(Ψ.ܫ){case ё.ϭ:
case ё.є:ɵ=ё.З;break;case ё.ь:ɵ=ё.Љ;break;case ё.ф:ɵ=ё.Ѝ;break;default:return;}var ઠ=l.ળ.ᆕ(Ψ.ޚ,Ψ.ޙ,Ɠ.ᜦ,ɵ);ઠ.ᘾ|=ళ.ᘌ;}private
static int ᅺ=100;public void ᅹ(Ɠ Ψ,Ɠ ᅸ,Ɠ ࠒ,int Ь){if((Ψ.ᘾ&ళ.ᘐ)==0){return;}if(Ψ.ƙ<=0){return;}if((Ψ.ᘾ&ళ.ᘅ)!=0){Ψ.ᙂ=Ψ.ᙁ=Ψ.ᙀ=Ꮖ.Ꮓ
;}var Ð=Ψ.ђ;if(Ð!=null&&l.હ.ᴷ==ᵅ.Р){Ь>>=1;}var ᅷ=ࠒ==null||ࠒ.ђ==null||ࠒ.ђ.Ə!=ਖ਼.ନ;if(ᅸ!=null&&(Ψ.ᘾ&ళ.ᖮ)==0&&ᅷ){var ࢢ=ᵎ.ᴥ(ᅸ.
ޚ,ᅸ.ޙ,Ψ.ޚ,Ψ.ޙ);var ᅶ=new Ꮖ(Ь*(Ꮖ.Ꮔ>>3)*100/Ψ.ᘿ.ᚓ);if(Ь<40&&Ь>Ψ.ƙ&&Ψ.ኴ-ᅸ.ኴ>Ꮖ.Ꭸ(64)&&(l.ષ.ѐ()&1)!=0){ࢢ+=ɡ.ᓣ;ᅶ*=4;}Ψ.ᙂ+=ᅶ*ஜ.ௐ
(ࢢ);Ψ.ᙁ+=ᅶ*ஜ.ஸ(ࢢ);}if(Ð!=null){if(Ψ.ฃ.Ŀ.Ě==(Ϋ)11&&Ь>=Ψ.ƙ){Ь=Ψ.ƙ-1;}if(Ь<1000&&((Ð.ƈ&ᖶ.ᖱ)!=0||Ð.Ɲ[(int)Ů.ŭ]>0)){return;}
int ᅵ;if(Ð.ƛ!=0){if(Ð.ƛ==1){ᅵ=Ь/3;}else{ᅵ=Ь/2;}if(Ð.ƚ<=ᅵ){ᅵ=Ð.ƚ;Ð.ƛ=0;}Ð.ƚ-=ᅵ;Ь-=ᅵ;}Ð.ƙ-=Ь;if(Ð.ƙ<0){Ð.ƙ=0;}Ð.Ʃ=ࠒ;Ð.ƞ+=Ь;if
(Ð.ƞ>100){Ð.ƞ=100;}}Ψ.ƙ-=Ь;if(Ψ.ƙ<=0){ᆀ(ࠒ,Ψ);return;}if((l.ષ.ѐ()<Ψ.ᘿ.ᚚ)&&(Ψ.ᘾ&ళ.ᘅ)==0){Ψ.ᘾ|=ళ.ᘙ;Ψ.ᘸ(Ψ.ᘿ.ᚧ);}Ψ.ᘺ=0;if((Ψ.ᘹ
==0||Ψ.ܫ==ё.ы)&&ࠒ!=null&&ࠒ!=Ψ&&ࠒ.ܫ!=ё.ы){Ψ.ᘻ=ࠒ;Ψ.ᘹ=ᅺ;if(Ψ.Ŷ==ኑ.ጔ[(int)Ψ.ᘿ.ᚡ]&&Ψ.ᘿ.ᚣ!=ᚏ.ᚠ){Ψ.ᘸ(Ψ.ᘿ.ᚣ);}}}public void ᅴ(Ɠ ğ)
{ğ.ᙂ=ğ.ᙁ=ğ.ᙀ=Ꮖ.Ꮓ;ğ.ᘸ(ኑ.ዃ[(int)ğ.ܫ].ᚖ);ğ.ŵ-=l.ષ.ѐ()&3;if(ğ.ŵ<1){ğ.ŵ=1;}ğ.ᘾ&=~ళ.પ;if(ğ.ᘿ.ᚔ!=0){l.ૐ(ğ,ğ.ᘿ.ᚔ,ʡ.ʝ);}}private Ɠ
ᅳ;private Ɠ ᅲ;private int ᅿ;private Func<Ɠ,bool>ᆂ;private void ᆑ(){ᆂ=ᆏ;}private bool ᆏ(Ɠ ğ){if((ğ.ᘾ&ళ.ᘐ)==0){return true;
}if(ğ.ܫ==ё.Ϛ||ğ.ܫ==ё.С){return true;}var ߝ=Ꮖ.Ꭼ(ğ.ޚ-ᅲ.ޚ);var Ǆ=Ꮖ.Ꭼ(ğ.ޙ-ᅲ.ޙ);var њ=ߝ>Ǆ?ߝ:Ǆ;њ=new Ꮖ((њ-ğ.ᙊ).Ꮁ>>Ꮖ.Ꮕ);if(њ<Ꮖ.Ꮓ
){њ=Ꮖ.Ꮓ;}if(њ.Ꮁ>=ᅿ){return true;}if(l.બ.ಏ(ğ,ᅲ)){ᅹ(ğ,ᅲ,ᅳ,ᅿ-њ.Ꮁ);}return true;}public void ᆐ(Ɠ ࠃ,Ɠ ࠒ,int Ь){var Ν=l.શ.ᑋ;var
њ=Ꮖ.Ꭸ(Ь+ᱺ.ᴂ.Ꮁ);var ܝ=Ν.ᑖ(ࠃ.ޙ-њ);var ܛ=Ν.ᑖ(ࠃ.ޙ+њ);var ܞ=Ν.ᑗ(ࠃ.ޚ-њ);var ܜ=Ν.ᑗ(ࠃ.ޚ+њ);ᅲ=ࠃ;ᅳ=ࠒ;ᅿ=Ь;for(var ܧ=ܝ;ܧ<=ܛ;ܧ++){for(
var ܓ=ܞ;ܓ<=ܜ;ܓ++){Ν.ᒟ(ܓ,ܧ,ᆂ);}}}}sealed class લ{private ଦ l;public લ(ଦ l){this.l=l;კ();Ⴒ();Ⴠ();}public static Ꮖ ᆎ=Ꮖ.Ꭸ(4);
private static int ᆍ=64;private static Ꮖ ᆌ=Ꮖ.Ꭸ(30);private static Ꮖ ᆋ=Ꮖ.Ꮒ;private Ɠ ᆊ;private ళ ᆉ;private Ꮖ ᆈ;private Ꮖ ᆇ;
private Ꮖ[]ᆆ;private Ꮖ ᆅ;private Ꮖ ᆄ;private Ꮖ ᆃ;private bool ᄾ;private ɢ ვ;public int Ⴘ;public ɢ[]ზ;private Func<ɢ,bool>თ;
private Func<Ɠ,bool>ი;private void კ(){ᆆ=new Ꮖ[4];ზ=new ɢ[ᆍ];თ=ე;ი=დ;}public void ლ(Ɠ ğ){var ࠎ=l.શ;var त=ᵎ.ᴩ(ğ.ޚ,ğ.ޙ,ࠎ);ğ.ฃ=त;
if((ğ.ᘾ&ళ.ᘑ)==0){var ô=त.Ŀ;ğ.ᙄ=null;ğ.ᙃ=ô.ē;if(ô.ē!=null){ô.ē.ᙄ=ğ;}ô.ē=ğ;}if((ğ.ᘾ&ళ.ᘒ)==0){var ı=ࠎ.ᑋ.ᒉ(ğ.ޚ,ğ.ޙ);if(ı!=-1){
var მ=ࠎ.ᑋ.ᒛ[ı];ğ.ᙇ=null;ğ.ᙆ=მ;if(მ!=null){მ.ᙇ=ğ;}ࠎ.ᑋ.ᒛ[ı]=ğ;}else{ğ.ᙆ=null;ğ.ᙇ=null;}}}public void ნ(Ɠ ğ){var ࠎ=l.શ;if((ğ.ᘾ
&ళ.ᘑ)==0){if(ğ.ᙃ!=null){ğ.ᙃ.ᙄ=ğ.ᙄ;}if(ğ.ᙄ!=null){ğ.ᙄ.ᙃ=ğ.ᙃ;}else{ğ.ฃ.Ŀ.ē=ğ.ᙃ;}}if((ğ.ᘾ&ళ.ᘒ)==0){if(ğ.ᙆ!=null){ğ.ᙆ.ᙇ=ğ.ᙇ;}
if(ğ.ᙇ!=null){ğ.ᙇ.ᙆ=ğ.ᙆ;}else{var ı=ࠎ.ᑋ.ᒉ(ğ.ޚ,ğ.ޙ);if(ı!=-1){ࠎ.ᑋ.ᒛ[ı]=ğ.ᙆ;}}}}private bool ე(ɢ ò){var ઑ=l.ય;if(Ꮖ.ሿ(ᆆ)<=Ꮖ.ቀ
(ò.ޖ)||Ꮖ.ቀ(ᆆ)>=Ꮖ.ሿ(ò.ޖ)||Ꮖ.Ꮀ(ᆆ)<=Ꮖ.Ꭿ(ò.ޖ)||Ꮖ.Ꭿ(ᆆ)>=Ꮖ.Ꮀ(ò.ޖ)){return true;}if(ᵎ.ᴮ(ᆆ,ò)!=-1){return true;}if(ò.ɤ==null){
return false;}if((ᆊ.ᘾ&ళ.પ)==0){if((ò.ᘾ&ᶘ.ᶙ)!=0){return false;}if(ᆊ.ђ==null&&(ò.ᘾ&ᶘ.ᶚ)!=0){return false;}}ઑ.ᛣ(ò);if(ઑ.ᛢ<ᆄ){ᆄ=ઑ.
ᛢ;ვ=ò;}if(ઑ.ᛡ>ᆅ){ᆅ=ઑ.ᛡ;}if(ઑ.ᛕ<ᆃ){ᆃ=ઑ.ᛕ;}if(ò.Ě!=0){ზ[Ⴘ]=ò;Ⴘ++;}return true;}private bool დ(Ɠ ğ){if((ğ.ᘾ&(ళ.ᘏ|ళ.Ě|ళ.ᘐ))==
0){return true;}var Ⴢ=ğ.ᙊ+ᆊ.ᙊ;if(Ꮖ.Ꭼ(ğ.ޚ-ᆈ)>=Ⴢ||Ꮖ.Ꭼ(ğ.ޙ-ᆇ)>=Ⴢ){return true;}if(ğ==ᆊ){return true;}if((ᆊ.ᘾ&ళ.ᘅ)!=0){var Ь=
((l.ષ.ѐ()%8)+1)*ᆊ.ᘿ.ᚒ;l.ર.ᅹ(ğ,ᆊ,ᆊ,Ь);ᆊ.ᘾ&=~ళ.ᘅ;ᆊ.ᙂ=ᆊ.ᙁ=ᆊ.ᙀ=Ꮖ.Ꮓ;ᆊ.ᘸ(ᆊ.ᘿ.ᚡ);return false;}if((ᆊ.ᘾ&ళ.પ)!=0){if(ᆊ.ኴ>ğ.ኴ+ğ.ǡ){
return true;}if(ᆊ.ኴ+ᆊ.ǡ<ğ.ኴ){return true;}if(ᆊ.ᘻ!=null&&(ᆊ.ᘻ.ܫ==ğ.ܫ||(ᆊ.ᘻ.ܫ==ё.н&&ğ.ܫ==ё.п)||(ᆊ.ᘻ.ܫ==ё.п&&ğ.ܫ==ё.н))){if(ğ==ᆊ.
ᘻ){return true;}if(ğ.ܫ!=ё.ђ&&!ኑ.ᕺ.ᕫ){return false;}}if((ğ.ᘾ&ళ.ᘐ)==0){return(ğ.ᘾ&ళ.ᘏ)==0;}var Ь=((l.ષ.ѐ()%8)+1)*ᆊ.ᘿ.ᚒ;l.ર.
ᅹ(ğ,ᆊ,ᆊ.ᘻ,Ь);return false;}if((ğ.ᘾ&ళ.Ě)!=0){var გ=(ğ.ᘾ&ళ.ᘏ)!=0;if((ᆉ&ళ.ᘘ)!=0){l.ଔ.Ṅ(ğ,ᆊ);}return!გ;}return(ğ.ᘾ&ళ.ᘏ)==0;}
public bool ბ(Ɠ ğ,Ꮖ ǃ,Ꮖ ǂ){var ࠎ=l.શ;var Ν=ࠎ.ᑋ;ᆊ=ğ;ᆉ=ğ.ᘾ;ᆈ=ǃ;ᆇ=ǂ;ᆆ[ᒚ.Ꮀ]=ǂ+ᆊ.ᙊ;ᆆ[ᒚ.Ꭿ]=ǂ-ᆊ.ᙊ;ᆆ[ᒚ.ሿ]=ǃ+ᆊ.ᙊ;ᆆ[ᒚ.ቀ]=ǃ-ᆊ.ᙊ;var ა=ᵎ.ᴩ
(ǃ,ǂ,ࠎ);ვ=null;ᆅ=ᆃ=ა.Ŀ.Ģ;ᆄ=ა.Ŀ.ģ;var Ĺ=l.ૡ();Ⴘ=0;if((ᆉ&ళ.ᖮ)!=0){return true;}{var ܞ=Ν.ᑗ(ᆆ[ᒚ.ቀ]-ᱺ.ᴂ);var ܜ=Ν.ᑗ(ᆆ[ᒚ.ሿ]+ᱺ.ᴂ)
;var ܝ=Ν.ᑖ(ᆆ[ᒚ.Ꭿ]-ᱺ.ᴂ);var ܛ=Ν.ᑖ(ᆆ[ᒚ.Ꮀ]+ᱺ.ᴂ);for(var ܓ=ܞ;ܓ<=ܜ;ܓ++){for(var ܧ=ܝ;ܧ<=ܛ;ܧ++){if(!ࠎ.ᑋ.ᒟ(ܓ,ܧ,ი)){return false;}
}}}{var ܞ=Ν.ᑗ(ᆆ[ᒚ.ቀ]);var ܜ=Ν.ᑗ(ᆆ[ᒚ.ሿ]);var ܝ=Ν.ᑖ(ᆆ[ᒚ.Ꭿ]);var ܛ=Ν.ᑖ(ᆆ[ᒚ.Ꮀ]);for(var ܓ=ܞ;ܓ<=ܜ;ܓ++){for(var ܧ=ܝ;ܧ<=ܛ;ܧ++){
if(!ࠎ.ᑋ.ᒝ(ܓ,ܧ,თ,Ĺ)){return false;}}}}return true;}public bool შ(Ɠ ğ,Ꮖ ǃ,Ꮖ ǂ){ᄾ=false;if(!ბ(ğ,ǃ,ǂ)){return false;}if((ğ.ᘾ&ళ
.ᖮ)==0){if(ᆄ-ᆅ<ğ.ǡ){return false;}ᄾ=true;if((ğ.ᘾ&ళ.ϔ)==0&&ᆄ-ğ.ኴ<ğ.ǡ){return false;}if((ğ.ᘾ&ళ.ϔ)==0&&ᆅ-ğ.ኴ>Ꮖ.Ꭸ(24)){return
false;}if((ğ.ᘾ&(ళ.ᘗ|ళ.ᘍ))==0&&ᆅ-ᆃ>Ꮖ.Ꭸ(24)){return false;}}ნ(ğ);var ჩ=ğ.ޚ;var ც=ğ.ޙ;ğ.ᙈ=ᆅ;ğ.ᙉ=ᆄ;ğ.ޚ=ǃ;ğ.ޙ=ǂ;ლ(ğ);if((ğ.ᘾ&(ళ.ϔ|
ళ.ᖮ))==0){while(Ⴘ-->0){var ò=ზ[Ⴘ];var ყ=ᵎ.ᴫ(ğ.ޚ,ğ.ޙ,ò);var ღ=ᵎ.ᴫ(ჩ,ც,ò);if(ყ!=ღ){if(ò.Ě!=0){l.મ.ᛊ(ò,ღ,ğ);}}}}return true;
}private static Ꮖ ქ=new Ꮖ(0x1000);private static Ꮖ ფ=new Ꮖ(0xe800);public void უ(Ɠ ğ){if(ğ.ᙂ==Ꮖ.Ꮓ&&ğ.ᙁ==Ꮖ.Ꮓ){if((ğ.ᘾ&ళ.ᘅ)
!=0){ğ.ᘾ&=~ళ.ᘅ;ğ.ᙂ=ğ.ᙁ=ğ.ᙀ=Ꮖ.Ꮓ;ğ.ᘸ(ğ.ᘿ.ᚡ);}return;}var Ð=ğ.ђ;if(ğ.ᙂ>ᆌ){ğ.ᙂ=ᆌ;}else if(ğ.ᙂ<-ᆌ){ğ.ᙂ=-ᆌ;}if(ğ.ᙁ>ᆌ){ğ.ᙁ=ᆌ;}
else if(ğ.ᙁ<-ᆌ){ğ.ᙁ=-ᆌ;}var ტ=ğ.ᙂ;var ს=ğ.ᙁ;do{Ꮖ რ;Ꮖ ჟ;if(ტ>ᆌ/2||ს>ᆌ/2){რ=ğ.ޚ+ტ/2;ჟ=ğ.ޙ+ს/2;ტ>>=1;ს>>=1;}else{რ=ğ.ޚ+ტ;ჟ=ğ.ޙ+
ს;ტ=ს=Ꮖ.Ꮓ;}if(!შ(ğ,რ,ჟ)){if(ğ.ђ!=null){Ⴄ(ğ);}else if((ğ.ᘾ&ళ.પ)!=0){if(ვ!=null&&ვ.ɤ!=null&&ვ.ɤ.ĥ==l.શ.ᜅ){l.ળ.ᆖ(ğ);return;}
l.ર.ᅴ(ğ);}else{ğ.ᙂ=ğ.ᙁ=Ꮖ.Ꮓ;}}}while(ტ!=Ꮖ.Ꮓ||ს!=Ꮖ.Ꮓ);if(Ð!=null&&(Ð.ƈ&ᖶ.ᖵ)!=0){ğ.ᙂ=ğ.ᙁ=Ꮖ.Ꮓ;return;}if((ğ.ᘾ&(ళ.પ|ళ.ᘅ))!=0){
return;}if(ğ.ኴ>ğ.ᙈ){return;}if((ğ.ᘾ&ళ.ᘉ)!=0){if(ğ.ᙂ>Ꮖ.Ꮒ/4||ğ.ᙂ<-Ꮖ.Ꮒ/4||ğ.ᙁ>Ꮖ.Ꮒ/4||ğ.ᙁ<-Ꮖ.Ꮒ/4){if(ğ.ᙈ!=ğ.ฃ.Ŀ.Ģ){return;}}}if(ğ.
ᙂ>-ქ&&ğ.ᙂ<ქ&&ğ.ᙁ>-ქ&&ğ.ᙁ<ქ&&(Ð==null||(Ð.ƕ.Ʊ==0&&Ð.ƕ.Ƶ==0))){if(Ð!=null&&(Ð.Ɠ.Ŷ.ġ-(int)ᚏ.ᨋ)<4){Ð.Ɠ.ᘸ(ᚏ.ᨊ);}ğ.ᙂ=Ꮖ.Ꮓ;ğ.ᙁ=Ꮖ.
Ꮓ;}else{ğ.ᙂ=ğ.ᙂ*ფ;ğ.ᙁ=ğ.ᙁ*ფ;}}public void პ(Ɠ ğ){if(ğ.ђ!=null&&ğ.ኴ<ğ.ᙈ){ğ.ђ.Ɩ-=ğ.ᙈ-ğ.ኴ;ğ.ђ.Ɨ=(ђ.ۈ-ğ.ђ.Ɩ)>>3;}ğ.ኴ+=ğ.ᙀ;if(
(ğ.ᘾ&ళ.ᘍ)!=0&&ğ.ᘻ!=null){if((ğ.ᘾ&ళ.ᘅ)==0&&(ğ.ᘾ&ళ.ᘈ)==0){var њ=ᵎ.ᴭ(ğ.ޚ-ğ.ᘻ.ޚ,ğ.ޙ-ğ.ᘻ.ޙ);var ş=(ğ.ᘻ.ኴ+(ğ.ǡ>>1))-ğ.ኴ;if(ş<Ꮖ.
Ꮓ&&њ<-(ş*3)){ğ.ኴ-=ᆎ;}else if(ş>Ꮖ.Ꮓ&&њ<(ş*3)){ğ.ኴ+=ᆎ;}}}if(ğ.ኴ<=ğ.ᙈ){var ო=l.હ.ಗ>=ಗ.ᵋ;if(ო&&(ğ.ᘾ&ళ.ᘅ)!=0){ğ.ᙀ=-ğ.ᙀ;}if(ğ.ᙀ
<Ꮖ.Ꮓ){if(ğ.ђ!=null&&ğ.ᙀ<-ᆋ*8){ğ.ђ.Ɨ=(ğ.ᙀ>>3);l.ૐ(ğ,ɴ.Ȟ,ʡ.ʟ);}ğ.ᙀ=Ꮖ.Ꮓ;}ğ.ኴ=ğ.ᙈ;if(!ო&&(ğ.ᘾ&ళ.ᘅ)!=0){ğ.ᙀ=-ğ.ᙀ;}if((ğ.ᘾ&ళ.પ)
!=0&&(ğ.ᘾ&ళ.ᖮ)==0){l.ર.ᅴ(ğ);return;}}else if((ğ.ᘾ&ళ.ᘖ)==0){if(ğ.ᙀ==Ꮖ.Ꮓ){ğ.ᙀ=-ᆋ*2;}else{ğ.ᙀ-=ᆋ;}}if(ğ.ኴ+ğ.ǡ>ğ.ᙉ){if(ğ.ᙀ>Ꮖ.Ꮓ
){ğ.ᙀ=Ꮖ.Ꮓ;}{ğ.ኴ=ğ.ᙉ-ğ.ǡ;}if((ğ.ᘾ&ళ.ᘅ)!=0){ğ.ᙀ=-ğ.ᙀ;}if((ğ.ᘾ&ళ.પ)!=0&&(ğ.ᘾ&ళ.ᖮ)==0){l.ર.ᅴ(ğ);return;}}}public Ꮖ Ⴭ=>ᆅ;
public Ꮖ Ⴧ=>ᆄ;public Ꮖ ၿ=>ᆃ;public bool Ⴉ=>ᄾ;private Ꮖ Ⴊ;private Ꮖ Ⴋ;private ɢ Ⴌ;private ɢ Ⴍ;private Ɠ Ⴎ;private Ꮖ Ⴐ;private Ꮖ
Ⴕ;private Func<ᯍ,bool>Ⴑ;private void Ⴒ(){Ⴑ=Ⴅ;}private void Ⴓ(ɢ ò){if(ò.ʐ==ʐ.ʏ){Ⴕ=Ꮖ.Ꮓ;return;}if(ò.ʐ==ʐ.ʎ){Ⴐ=Ꮖ.Ꮓ;return;}
var ñ=ᵎ.ᴫ(Ⴎ.ޚ,Ⴎ.ޙ,ò);var Ⴔ=ᵎ.ᴥ(Ꮖ.Ꮓ,Ꮖ.Ꮓ,ò.ޘ,ò.ޗ);if(ñ==1){Ⴔ+=ɡ.ᓣ;}var Ⴖ=ᵎ.ᴥ(Ꮖ.Ꮓ,Ꮖ.Ꮓ,Ⴐ,Ⴕ);var Ⴈ=Ⴖ-Ⴔ;if(Ⴈ>ɡ.ᓣ){Ⴈ+=ɡ.ᓣ;}var Ⴇ=ᵎ
.ᴭ(Ⴐ,Ⴕ);var Ⴆ=Ⴇ*ஜ.ௐ(Ⴈ);Ⴐ=Ⴆ*ஜ.ௐ(Ⴔ);Ⴕ=Ⴆ*ஜ.ஸ(Ⴔ);}private bool Ⴅ(ᯍ ܩ){var ઑ=l.ય;if(ܩ.ᒭ==null){throw new Exception(
"ThingMovement.SlideTraverse: Not a line?");}var ò=ܩ.ᒭ;if((ò.ᘾ&ᶘ.ᶔ)==0){if(ᵎ.ᴫ(Ⴎ.ޚ,Ⴎ.ޙ,ò)!=0){return true;}goto isBlocking;}ઑ.ᛣ(ò);if(ઑ.ᛖ<Ⴎ.ǡ){goto isBlocking;}if
(ઑ.ᛢ-Ⴎ.ኴ<Ⴎ.ǡ){goto isBlocking;}if(ઑ.ᛡ-Ⴎ.ኴ>Ꮖ.Ꭸ(24)){goto isBlocking;}return true;isBlocking:if(ܩ.ᯆ<Ⴊ){Ⴋ=Ⴊ;Ⴍ=Ⴌ;Ⴊ=ܩ.ᯆ;Ⴌ=ò;}
return false;}private void Ⴄ(Ɠ ğ){var Ⴃ=l.ڏ;Ⴎ=ğ;var Ⴂ=0;retry:if(++Ⴂ==3){Ⴗ(ğ);return;}Ꮖ Ⴁ;Ꮖ Ⴀ;Ꮖ ႎ;Ꮖ ႁ;if(ğ.ᙂ>Ꮖ.Ꮓ){Ⴁ=ğ.ޚ+ğ.ᙊ;ႎ=
ğ.ޚ-ğ.ᙊ;}else{Ⴁ=ğ.ޚ-ğ.ᙊ;ႎ=ğ.ޚ+ğ.ᙊ;}if(ğ.ᙁ>Ꮖ.Ꮓ){Ⴀ=ğ.ޙ+ğ.ᙊ;ႁ=ğ.ޙ-ğ.ᙊ;}else{Ⴀ=ğ.ޙ-ğ.ᙊ;ႁ=ğ.ޙ+ğ.ᙊ;}Ⴊ=Ꮖ.Ꮎ;Ⴃ.ܨ(Ⴁ,Ⴀ,Ⴁ+ğ.ᙂ,Ⴀ+ğ.ᙁ,ݐ
.ݑ,Ⴑ);Ⴃ.ܨ(ႎ,Ⴀ,ႎ+ğ.ᙂ,Ⴀ+ğ.ᙁ,ݐ.ݑ,Ⴑ);Ⴃ.ܨ(Ⴁ,ႁ,Ⴁ+ğ.ᙂ,ႁ+ğ.ᙁ,ݐ.ݑ,Ⴑ);if(Ⴊ==Ꮖ.Ꮎ){Ⴗ(ğ);return;}Ⴊ=new Ꮖ(Ⴊ.Ꮁ-0x800);if(Ⴊ>Ꮖ.Ꮓ){var ႀ=ğ.
ᙂ*Ⴊ;var Ⴏ=ğ.ᙁ*Ⴊ;if(!შ(ğ,ğ.ޚ+ႀ,ğ.ޙ+Ⴏ)){Ⴗ(ğ);return;}}Ⴊ=new Ꮖ(Ꮖ.Ꮔ-(Ⴊ.Ꮁ+0x800));if(Ⴊ>Ꮖ.Ꮒ){Ⴊ=Ꮖ.Ꮒ;}if(Ⴊ<=Ꮖ.Ꮓ){return;}Ⴐ=ğ.ᙂ*Ⴊ;
Ⴕ=ğ.ᙁ*Ⴊ;Ⴓ(Ⴌ);ğ.ᙂ=Ⴐ;ğ.ᙁ=Ⴕ;if(!შ(ğ,ğ.ޚ+Ⴐ,ğ.ޙ+Ⴕ)){goto retry;}}private void Ⴗ(Ɠ ğ){if(!შ(ğ,ğ.ޚ,ğ.ޙ+ğ.ᙁ)){შ(ğ,ğ.ޚ+ğ.ᙂ,ğ.ޙ);}}
private Func<Ɠ,bool>Ⴟ;private void Ⴠ(){Ⴟ=Ⴡ;}private bool Ⴡ(Ɠ ğ){if((ğ.ᘾ&ళ.ᘐ)==0){return true;}var Ⴢ=ğ.ᙊ+ᆊ.ᙊ;var ߝ=Ꮖ.Ꭼ(ğ.ޚ-ᆈ);
var Ǆ=Ꮖ.Ꭼ(ğ.ޙ-ᆇ);if(ߝ>=Ⴢ||Ǆ>=Ⴢ){return true;}if(ğ==ᆊ){return true;}if(ᆊ.ђ==null&&l.હ.શ!=30){return false;}l.ર.ᅹ(ğ,ᆊ,ᆊ,10000
);return true;}public bool Ⴣ(Ɠ ğ,Ꮖ ǃ,Ꮖ ǂ){ᆊ=ğ;ᆉ=ğ.ᘾ;ᆈ=ǃ;ᆇ=ǂ;ᆆ[ᒚ.Ꮀ]=ǂ+ᆊ.ᙊ;ᆆ[ᒚ.Ꭿ]=ǂ-ᆊ.ᙊ;ᆆ[ᒚ.ሿ]=ǃ+ᆊ.ᙊ;ᆆ[ᒚ.ቀ]=ǃ-ᆊ.ᙊ;var Ⴤ=ᵎ.ᴩ
(ǃ,ǂ,l.શ);ვ=null;ᆅ=ᆃ=Ⴤ.Ŀ.Ģ;ᆄ=Ⴤ.Ŀ.ģ;var Ⴥ=l.ૡ();Ⴘ=0;var Ν=l.શ.ᑋ;var ܞ=Ν.ᑗ(ᆆ[ᒚ.ቀ]-ᱺ.ᴂ);var ܜ=Ν.ᑗ(ᆆ[ᒚ.ሿ]+ᱺ.ᴂ);var ܝ=Ν.ᑖ(ᆆ[ᒚ.
Ꭿ]-ᱺ.ᴂ);var ܛ=Ν.ᑖ(ᆆ[ᒚ.Ꮀ]+ᱺ.ᴂ);for(var ܓ=ܞ;ܓ<=ܜ;ܓ++){for(var ܧ=ܝ;ܧ<=ܛ;ܧ++){if(!Ν.ᒟ(ܓ,ܧ,Ⴟ)){return false;}}}ნ(ğ);ğ.ᙈ=ᆅ;ğ.ᙉ=
ᆄ;ğ.ޚ=ǃ;ğ.ޙ=ǂ;ლ(ğ);return true;}}class Ⴞ{private Ⴞ Ⴝ;private Ⴞ ɾ;private Ⴙ Ⴜ;public Ⴞ(){}public virtual void Ⴛ(){}public
virtual void Ċ(){}public Ⴞ Ⴚ{get{return Ⴝ;}set{Ⴝ=value;}}public Ⴞ ѐ{get{return ɾ;}set{ɾ=value;}}public Ⴙ Ⴙ{get{return Ⴜ;}set{Ⴜ=
value;}}}sealed class વ{private ଦ l;public વ(ଦ l){this.l=l;ᄨ();}private Ⴞ ჸ;private void ᄨ(){ჸ=new Ⴞ();ჸ.Ⴚ=ჸ.ѐ=ჸ;}public void
ᄩ(Ⴞ â){ჸ.Ⴚ.ѐ=â;â.ѐ=ჸ;â.Ⴚ=ჸ.Ⴚ;ჸ.Ⴚ=â;}public void ᄪ(Ⴞ â){â.Ⴙ=Ⴙ.ᄦ;}public void Ⴛ(){var Ġ=ჸ.ѐ;while(Ġ!=ჸ){if(Ġ.Ⴙ==Ⴙ.ᄦ){Ġ.ѐ.Ⴚ=
Ġ.Ⴚ;Ġ.Ⴚ.ѐ=Ġ.ѐ;}else{if(Ġ.Ⴙ==Ⴙ.ᄧ){Ġ.Ⴛ();}}Ġ=Ġ.ѐ;}}public void Ċ(){var Ġ=ჸ.ѐ;while(Ġ!=ჸ){Ġ.Ċ();Ġ=Ġ.ѐ;}}public void ʕ(){ჸ.Ⴚ=
ჸ.ѐ=ჸ;}public ᄫ ĝ(){return new ᄫ(this);}public struct ᄫ:IEnumerator<Ⴞ>{private વ Ý;private Ⴞ Ġ;public ᄫ(વ Ý){this.Ý=Ý;Ġ=Ý
.ჸ;}public bool MoveNext(){while(true){Ġ=Ġ.ѐ;if(Ġ==Ý.ჸ){return false;}else if(Ġ.Ⴙ!=Ⴙ.ᄦ){return true;}}}public void Reset(
){Ġ=Ý.ჸ;}public void Dispose(){}public Ⴞ Current=>Ġ;object IEnumerator.Current{get{throw new Exception("NotImplemented");
}}}}public enum Ⴙ{ᄧ,ڿ,ᄦ}sealed class ᄥ{public static int ᄤ=9;private Ƭ ۼ;private Ṇ ʽ;private ᮟ ü;private Ḃ ᄣ;private Ꮝ ȋ;
private int ᄢ;private int ᄡ;private byte[]ᄠ;private int ᄟ;private int ᄞ;private Ꮖ č;public ᄥ(ᴍ Ǽ,Ꮝ ȋ,int ᄞ){ۼ=Ǽ.Ƭ;ʽ=Ǽ.ᜁ;ü=Ǽ.ᛧ;ᄣ
=Ǽ.ᴌ;this.ȋ=ȋ;ᄢ=ȋ.Ǒ;ᄡ=ȋ.ǡ;ᄠ=ȋ.Ꮁ;ᄟ=ᄢ/320;this.ᄞ=ᄞ;ᄬ();წ();ᄑ();ᄊ();ॻ();ज़();ॴ();ঌ();ঔ();উ(Ǽ.ಆ);ᄹ(ᄞ);}private void ᄹ(int ᄺ){
var ǉ=ᄢ/320;if(ᄺ<7){var ځ=ǉ*(96+32*ᄺ);var ˢ=ǉ*(48+16*ᄺ);var ǃ=(ᄢ-ځ)/2;var ǂ=(ᄡ-ള.ǡ*ǉ-ˢ)/2;ᄲ(ǃ,ǂ,ځ,ˢ);}else if(ᄺ==7){var ځ=ᄢ
;var ˢ=ᄡ-ള.ǡ*ǉ;ᄲ(0,0,ځ,ˢ);}else{var ځ=ᄢ;var ˢ=ᄡ;ᄲ(0,0,ځ,ˢ);}ᄝ();ძ();ᄒ();ᄉ();ॡ();ॽ();}private int ᄻ;private int ᄼ;private
int ᄽ;private int ᄸ;private int ᄷ;private int ᄶ;private Ꮖ ᄵ;private Ꮖ ᄴ;private Ꮖ ᄳ;private void ᄲ(int ǃ,int ǂ,int ځ,int ˢ)
{ᄻ=ǃ;ᄼ=ǂ;ᄽ=ځ;ᄸ=ˢ;ᄷ=ᄽ/2;ᄶ=ᄸ/2;ᄵ=Ꮖ.Ꭸ(ᄷ);ᄴ=Ꮖ.Ꭸ(ᄶ);ᄳ=ᄵ;}private const int ᄱ=2048;private int[]ᄰ;private ɡ[]ᄯ;private ɡ ᄮ;
private ɡ ᄭ;private void ᄬ(){ᄰ=new int[ஜ.ஷ/2];ᄯ=new ɡ[ᄢ];}private void ᄝ(){var ᄜ=ᄵ/ஜ.ழ(ஜ.ஷ/4+ᄱ/2);for(var Ä=0;Ä<ஜ.ஷ/2;Ä++){int
ჹ;if(ஜ.ழ(Ä)>Ꮖ.Ꭸ(2)){ჹ=-1;}else if(ஜ.ழ(Ä)<Ꮖ.Ꭸ(-2)){ჹ=ᄽ+1;}else{ჹ=(ᄵ-ஜ.ழ(Ä)*ᄜ).Ꮃ();if(ჹ<-1){ჹ=-1;}else if(ჹ>ᄽ+1){ჹ=ᄽ+1;}}ᄰ[
Ä]=ჹ;}for(var ǃ=0;ǃ<ᄽ;ǃ++){var Ä=0;while(ᄰ[Ä]>ǃ){Ä++;}ᄯ[ǃ]=new ɡ((uint)(Ä<<ஜ.ல))-ɡ.ᓮ;}for(var Ä=0;Ä<ஜ.ஷ/2;Ä++){if(ᄰ[Ä]==-
1){ᄰ[Ä]=0;}else if(ᄰ[Ä]==ᄽ+1){ᄰ[Ä]=ᄽ;}}ᄮ=ᄯ[0];ᄭ=new ɡ(2*ᄮ.Ꮁ);}private Ꮖ[]ჺ;private Ꮖ[]ჼ;private Ꮖ ჾ;private Ꮖ ᄅ;private Ŀ
ჿ;private int ᄀ;private int ᄁ;private int ᄂ;private Ꮖ[]ᄃ;private Ꮖ[]ᄄ;private Ꮖ[]ᄆ;private Ꮖ[]ჷ;private byte[][]ჶ;private
Ŀ ჵ;private int ჴ;private int ჳ;private int ჲ;private Ꮖ[]ჱ;private Ꮖ[]ჰ;private Ꮖ[]ჯ;private Ꮖ[]ხ;private byte[][]ჭ;
private void წ(){ჺ=new Ꮖ[ᄡ];ჼ=new Ꮖ[ᄢ];ᄃ=new Ꮖ[ᄡ];ᄄ=new Ꮖ[ᄡ];ᄆ=new Ꮖ[ᄡ];ჷ=new Ꮖ[ᄡ];ჶ=new byte[ᄡ][];ჱ=new Ꮖ[ᄡ];ჰ=new Ꮖ[ᄡ];ჯ=new
Ꮖ[ᄡ];ხ=new Ꮖ[ᄡ];ჭ=new byte[ᄡ][];}private void ძ(){for(int Ä=0;Ä<ᄸ;Ä++){var Ǆ=Ꮖ.Ꭸ(Ä-ᄸ/2)+Ꮖ.Ꮒ/2;Ǆ=Ꮖ.Ꭼ(Ǆ);ჺ[Ä]=Ꮖ.Ꭸ(ᄽ/2)/Ǆ;}
for(var Ä=0;Ä<ᄽ;Ä++){var ჽ=Ꮖ.Ꭼ(ஜ.ௐ(ᄯ[Ä]));ჼ[Ä]=Ꮖ.Ꮒ/ჽ;}}private void ᄇ(){var Ş=ब-ɡ.ᓮ;ჾ=ஜ.ௐ(Ş)/ᄵ;ᄅ=-(ஜ.ஸ(Ş)/ᄵ);ჿ=null;ᄀ=int.
MaxValue;ჵ=null;ჴ=int.MaxValue;}private const int ᄎ=22;private Ꮖ ᄏ;private Ꮖ ᄐ;private void ᄑ(){ᄏ=Ꮖ.Ꭸ(100);}private void ᄒ(){var
ܣ=(long)Ꮖ.Ꮔ*ᄢ*200;var ܢ=ᄽ*ᄡ;ᄐ=new Ꮖ((int)(ܣ/ܢ));}private const int ᄓ=16;private const int ᄚ=4;private const int ᄔ=12;
private const int ᄕ=20;private const int ᄖ=32;private int ᄗ;private const int ᄘ=128;private byte[][][]ᄙ;private byte[][][]ᄛ;
private byte[][][]ᄍ;private byte[][][]ᄌ;private byte[][][]ᄋ;private int ۺ;private int ۻ;private void ᄊ(){ᄗ=48*(ᄢ/320);ᄙ=new
byte[ᄓ][][];ᄛ=new byte[ᄓ][][];ᄍ=new byte[ᄓ][][];for(var Ä=0;Ä<ᄓ;Ä++){ᄙ[Ä]=new byte[ᄗ][];ᄛ[Ä]=new byte[ᄘ][];ᄍ[Ä]=new byte[
Math.Max(ᄗ,ᄘ)][];}var ᄈ=2;for(var Ä=0;Ä<ᄓ;Ä++){var Σ=((ᄓ-1-Ä)*2)*ᄖ/ᄓ;for(var ͼ=0;ͼ<ᄘ;ͼ++){var ǉ=Ꮖ.Ꭸ(320/2)/new Ꮖ((ͼ+1)<<ᄕ);ǉ
=new Ꮖ(ǉ.Ꮁ>>ᄔ);var ॱ=Σ-ǉ.Ꮁ/ᄈ;if(ॱ<0){ॱ=0;}if(ॱ>=ᄖ){ॱ=ᄖ-1;}ᄛ[Ä][ͼ]=ۼ[ॱ];}}}private void ᄉ(){var ᄈ=2;for(var Ä=0;Ä<ᄓ;Ä++){
var Σ=((ᄓ-1-Ä)*2)*ᄖ/ᄓ;for(var ͼ=0;ͼ<ᄗ;ͼ++){var ॱ=Σ-ͼ*320/ᄽ/ᄈ;if(ॱ<0){ॱ=0;}if(ॱ>=ᄖ){ॱ=ᄖ-1;}ᄙ[Ä][ͼ]=ۼ[ॱ];}}}private void ऋ(){
if(ۻ==0){ᄌ=ᄙ;ᄋ=ᄛ;ᄍ[0][0]=null;}else if(ᄍ[0][0]!=ۼ[ۻ]){for(var Ä=0;Ä<ᄓ;Ä++){for(var ͼ=0;ͼ<ᄍ[Ä].Length;ͼ++){ᄍ[Ä][ͼ]=ۼ[ۻ];}}ᄌ
=ᄍ;ᄋ=ᄍ;}}private short[]ग;private short[]ङ;private int ॲ;private int ॳ;private int ॵ;private చ[]ॼ;private int ॶ;private
short[]ॷ;private int ॹ;private గ[]ॺ;private void ॻ(){ग=new short[ᄢ];ङ=new short[ᄢ];ॼ=new చ[256];for(var Ä=0;Ä<ॼ.Length;Ä++){ॼ
[Ä]=new చ();}ॷ=new short[128*ᄢ];ॺ=new గ[512];for(var Ä=0;Ä<ॺ.Length;Ä++){ॺ[Ä]=new గ();}}private void ॡ(){for(var Ä=0;Ä<ᄽ;
Ä++){ॷ[Ä]=-1;}ॲ=0;for(var Ä=ᄽ;Ä<2*ᄽ;Ä++){ॷ[Ä]=(short)ᄸ;}ॳ=ᄽ;}private void ॠ(){for(var ǃ=0;ǃ<ᄽ;ǃ++){ग[ǃ]=-1;}for(var ǃ=0;ǃ
<ᄽ;ǃ++){ङ[ǃ]=(short)ᄸ;}ॼ[0].ఛ=-0x7fffffff;ॼ[0].జ=-1;ॼ[1].ఛ=ᄽ;ॼ[1].జ=0x7fffffff;ॵ=2;ॶ=2*ᄽ;ॹ=0;}private static Ꮖ य़=Ꮖ.Ꭸ(4);
private int फ़;private న[]ढ़;private ద ड़;private void ज़(){ढ़=new న[256];for(var Ä=0;Ä<ढ़.Length;Ä++){ढ़[Ä]=new న();}ड़=new ద();}
private void ग़(){फ़=0;}private న ख़;private Ꮖ क़;private Ꮖ ॐ;private void ॴ(){ख़=new న();}private void ॽ(){क़=new Ꮖ(Ꮖ.Ꮔ*ᄽ/320);ॐ=new
Ꮖ(Ꮖ.Ꮔ*320/ᄽ);}private static sbyte[]ক=new sbyte[]{1,-1,1,-1,1,1,-1,1,1,-1,1,1,1,-1,1,1,1,-1,-1,-1,-1,1,-1,-1,1,1,1,1,-1,1
,-1,1,1,-1,-1,1,1,-1,-1,-1,-1,1,1,1,1,-1,1,1,-1,1};private int ঋ;private void ঌ(){ঋ=0;}private byte[]এ;private byte[]ঐ;
private byte[]ও;private void ঔ(){এ=new byte[256];ঐ=new byte[256];ও=new byte[256];for(var Ä=0;Ä<256;Ä++){এ[Ä]=(byte)Ä;ঐ[Ä]=(byte
)Ä;ও[Ä]=(byte)Ä;}for(var Ä=112;Ä<128;Ä++){এ[Ä]-=16;ঐ[Ä]-=48;ও[Ä]-=80;}}private ڦ জ;private ڦ খ;private ڦ গ;private ڦ ঘ;
private ڦ ঙ;private ڦ চ;private ڦ ছ;private ڦ ঝ;private ᎂ ঊ;private void উ(ಆ þ){জ=ڦ.ÿ(þ,"BRDR_TL");খ=ڦ.ÿ(þ,"BRDR_TR");গ=ڦ.ÿ(þ,
"BRDR_BL");ঘ=ڦ.ÿ(þ,"BRDR_BR");ঙ=ڦ.ÿ(þ,"BRDR_T");চ=ڦ.ÿ(þ,"BRDR_B");ছ=ڦ.ÿ(þ,"BRDR_L");ঝ=ڦ.ÿ(þ,"BRDR_R");if(þ.ಖ==ಖ.ᴉ){ঊ=ü["GRNROCK"]
;}else{ঊ=ü["FLOOR7_2"];}}private void ঈ(){var ই=ᄡ-ᄟ*ള.ǡ;অ(0,0,ᄻ,ই);অ(ᄢ-ᄻ,0,ᄻ,ই);অ(ᄻ,0,ᄢ-2*ᄻ,ᄼ);অ(ᄻ,ই-ᄼ,ᄢ-2*ᄻ,ᄼ);var আ=8*ᄟ
;for(var ǃ=ᄻ;ǃ<ᄢ-ᄻ;ǃ+=আ){ȋ.Ꮘ(ঙ,ǃ,ᄼ-আ,ᄟ);ȋ.Ꮘ(চ,ǃ,ই-ᄼ,ᄟ);}for(var ǂ=ᄼ;ǂ<ই-ᄼ;ǂ+=আ){ȋ.Ꮘ(ছ,ᄻ-আ,ǂ,ᄟ);ȋ.Ꮘ(ঝ,ᄢ-ᄻ,ǂ,ᄟ);}ȋ.Ꮘ(জ,ᄻ-আ,
ᄼ-আ,ᄟ);ȋ.Ꮘ(খ,ᄢ-ᄻ,ᄼ-আ,ᄟ);ȋ.Ꮘ(গ,ᄻ-আ,ই-ᄼ,ᄟ);ȋ.Ꮘ(ঘ,ᄢ-ᄻ,ই-ᄼ,ᄟ);}private void অ(int ǃ,int ǂ,int ځ,int ˢ){var f=ঊ.Ꮁ;var ॿ=ǃ/ᄟ;
var ॾ=ǂ/ᄟ;var ࠤ=Ꮖ.Ꮒ/ᄟ;var ࠅ=ࠤ-Ꮖ.Ꮏ;for(var Ä=0;Ä<ځ;Ä++){var ट=((ॿ+ࠅ.Ꮄ())&63)<<6;var ध=ᄡ*(ǃ+Ä)+ǂ;var ࠄ=ࠤ-Ꮖ.Ꮏ;for(var ͼ=0;ͼ<ˢ;
ͼ++){ᄠ[ध+ͼ]=f[ट|((ॾ+ࠄ.Ꮄ())&63)];ࠄ+=ࠤ;}ࠅ+=ࠤ;}}private ଦ l;private Ꮖ न;private Ꮖ ऩ;private Ꮖ ڼ;private ɡ ब;private Ꮖ प;
private Ꮖ फ;private int Ĺ;public void Ǌ(ђ Ð,Ꮖ č){this.č=č;l=Ð.Ɠ.ଦ;न=Ð.Ɠ.ᘮ(č);ऩ=Ð.Ɠ.ᘶ(č);ڼ=Ð.ۋ(č);ब=Ð.ϙ(č);प=ஜ.ஸ(ब);फ=ஜ.ௐ(ब);Ĺ=l
.ૡ();ۺ=Ð.ƪ;ۻ=Ð.ƫ;ᄇ();ऋ();ॠ();ग़();भ(l.શ.ᜆ.Length-1);ࡗ();ࡒ();ఘ(Ð);if(ᄞ<7){ঈ();}}private void भ(int ޛ){if(ߵ.ޝ(ޛ)){if(ޛ==-1){
थ(0);}else{थ(ߵ.ޜ(ޛ));}return;}var द=l.શ.ᜆ[ޛ];var ñ=ᵎ.ᴰ(न,ऩ,द);भ(द.ޕ[ñ]);if(ढ(द.ޖ[ñ^1])){भ(द.ޕ[ñ^1]);}}private void थ(int
त){var Ψ=l.શ.ᜂ[त];औ(Ψ.Ŀ,Ĺ);for(var Ä=0;Ä<Ψ.ต;Ä++){ऽ(l.શ.ᜀ[Ψ.ถ+Ä]);}}private static int[][]ण={new[]{3,0,2,1},new[]{3,0,2,0
},new[]{3,1,2,0},new[]{0},new[]{2,0,2,1},new[]{0,0,0,0},new[]{3,1,3,0},new[]{0},new[]{2,0,3,1},new[]{2,1,3,1},new[]{2,1,3
,0}};private bool ढ(Ꮖ[]ड){int ܓ;int ܧ;if(न<=ड[ᒚ.ቀ]){ܓ=0;}else if(न<ड[ᒚ.ሿ]){ܓ=1;}else{ܓ=2;}if(ऩ>=ड[ᒚ.Ꮀ]){ܧ=0;}else if(ऩ>ड[
ᒚ.Ꭿ]){ܧ=1;}else{ܧ=2;}var ठ=(ܧ<<2)+ܓ;if(ठ==5){return true;}var ǎ=ड[ण[ठ][0]];var ǆ=ड[ण[ठ][1]];var ǐ=ड[ण[ठ][2]];var ǅ=ड[ण[ठ]
[3]];var ވ=ᵎ.ᴥ(न,ऩ,ǎ,ǆ)-ब;var ފ=ᵎ.ᴥ(न,ऩ,ǐ,ǅ)-ब;var व=ވ-ފ;if(व>=ɡ.ᓣ){return true;}var श=ވ+ᄮ;if(श>ᄭ){श-=ᄭ;if(श>=व){return
false;}ވ=ᄮ;}var ष=ᄮ-ފ;if(ष>ᄭ){ष-=ᄭ;if(ष>=व){return false;}ފ=-ᄮ;}var स=ᄰ[(ވ+ɡ.ᓮ).Ꮁ>>ஜ.ல];var ह=ᄰ[(ފ+ɡ.ᓮ).Ꮁ>>ஜ.ல];if(स==ह){
return false;}ह--;var Σ=0;while(ॼ[Σ].జ<ह){Σ++;}if(स>=ॼ[Σ].ఛ&&ह<=ॼ[Σ].జ){return false;}return true;}private void ऽ(Ω ࡉ){var ވ=ᵎ
.ᴥ(न,ऩ,ࡉ.ɝ.ޚ,ࡉ.ɝ.ޙ);var ފ=ᵎ.ᴥ(न,ऩ,ࡉ.ɞ.ޚ,ࡉ.ɞ.ޙ);var व=ވ-ފ;if(व>=ɡ.ᓣ){return;}var म=ވ;ވ-=ब;ފ-=ब;var श=ވ+ᄮ;if(श>ᄭ){श-=ᄭ;if(श
>=व){return;}ވ=ᄮ;}var ष=ᄮ-ފ;if(ष>ᄭ){ष-=ᄭ;if(ष>=व){return;}ފ=-ᄮ;}var ǎ=ᄰ[(ވ+ɡ.ᓮ).Ꮁ>>ஜ.ல];var ǐ=ᄰ[(ފ+ɡ.ᓮ).Ꮁ>>ஜ.ல];if(ǎ==ǐ){
return;}var π=ࡉ.ɣ;var ρ=ࡉ.ɤ;var ऴ=π.Č(č);var ळ=π.ě(č);if(ρ==null){र(ࡉ,म,ǎ,ǐ-1);return;}var ल=ρ.Č(č);var ऱ=ρ.ě(č);if(ऱ<=ऴ||ल>=ळ
){र(ࡉ,म,ǎ,ǐ-1);return;}if(ऱ!=ळ||ल!=ऴ){य(ࡉ,म,ǎ,ǐ-1);return;}if(ρ.ĥ==π.ĥ&&ρ.Ĥ==π.Ĥ&&ρ.Ħ==π.Ħ&&ࡉ.ɦ.ʅ==0){return;}य(ࡉ,म,ǎ,ǐ-1
);}private void र(Ω ࡉ,ɡ म,int ǎ,int ǐ){int ɾ;int Σ;Σ=0;while(ॼ[Σ].జ<ǎ-1){Σ++;}if(ǎ<ॼ[Σ].ఛ){if(ǐ<ॼ[Σ].ఛ-1){ਡ(ࡉ,म,ǎ,ǐ);ɾ=ॵ;
ॵ++;while(ɾ!=Σ){ॼ[ɾ].ఝ(ॼ[ɾ-1]);ɾ--;}ॼ[ɾ].ఛ=ǎ;ॼ[ɾ].జ=ǐ;return;}ਡ(ࡉ,म,ǎ,ॼ[Σ].ఛ-1);ॼ[Σ].ఛ=ǎ;}if(ǐ<=ॼ[Σ].జ){return;}ɾ=Σ;while
(ǐ>=ॼ[ɾ+1].ఛ-1){ਡ(ࡉ,म,ॼ[ɾ].జ+1,ॼ[ɾ+1].ఛ-1);ɾ++;if(ǐ<=ॼ[ɾ].జ){ॼ[Σ].జ=ॼ[ɾ].జ;goto crunch;}}ਡ(ࡉ,म,ॼ[ɾ].జ+1,ǐ);ॼ[Σ].జ=ǐ;
crunch:if(ɾ==Σ){return;}while(ɾ++!=ॵ){ॼ[++Σ].ఝ(ॼ[ɾ]);}ॵ=Σ+1;}private void य(Ω ࡉ,ɡ म,int ǎ,int ǐ){int Σ;Σ=0;while(ॼ[Σ].జ<ǎ-1){Σ
++;}if(ǎ<ॼ[Σ].ఛ){if(ǐ<ॼ[Σ].ఛ-1){ਣ(ࡉ,म,ǎ,ǐ,false);return;}ਣ(ࡉ,म,ǎ,ॼ[Σ].ఛ-1,false);}if(ǐ<=ॼ[Σ].జ){return;}while(ǐ>=ॼ[Σ+1].ఛ-
1){ਣ(ࡉ,म,ॼ[Σ].జ+1,ॼ[Σ+1].ఛ-1,false);Σ++;if(ǐ<=ॼ[Σ].జ){return;}}ਣ(ࡉ,म,ॼ[Σ].జ+1,ǐ,false);}private Ꮖ ਝ(ɡ ਞ,ɡ ब,ɡ ਟ,Ꮖ ল){var
ܣ=ᄳ*ஜ.ஸ(ɡ.ᓮ+(ਞ-ਟ));var ܢ=ল*ஜ.ஸ(ɡ.ᓮ+(ਞ-ब));Ꮖ ǉ;if(ܢ.Ꮁ>ܣ.Ꮁ>>16){ǉ=ܣ/ܢ;if(ǉ>Ꮖ.Ꭸ(64)){ǉ=Ꮖ.Ꭸ(64);}else if(ǉ.Ꮁ<256){ǉ=new Ꮖ(256
);}}else{ǉ=Ꮖ.Ꭸ(64);}return ǉ;}private const int ਢ=12;private const int ਠ=1<<ਢ;private void ਡ(Ω ࡉ,ɡ म,int ǎ,int ǐ){if(ࡉ.ɤ
!=null){ਣ(ࡉ,म,ǎ,ǐ,true);return;}if(ॹ==ॺ.Length){return;}var ò=ࡉ.ɢ;var ñ=ࡉ.ɦ;var π=ࡉ.ɣ;var ऴ=π.Č(č);var ळ=π.ě(č);ò.ᘾ|=ᶘ.ᶎ;
var শ=ळ-ڼ;var ষ=ऴ-ڼ;var ਜ=ñ.ʅ!=0;var ম=শ>Ꮖ.Ꮓ||π.ĥ==ü.ᜅ;var ব=ষ<Ꮖ.Ꮓ;var ࡆ=ʽ[l.ƒ.ཝ[ñ.ʅ]];var ਛ=ࡆ.Ǒ-1;Ꮖ ਚ;if((ò.ᘾ&ᶘ.ᶒ)!=0){var
থ=ऴ+Ꮖ.Ꭸ(ࡆ.ǡ);ਚ=থ-ڼ;}else{ਚ=শ;}ਚ+=ñ.ʛ;var ড=ࡉ.ɡ+ɡ.ᓮ;var ঠ=ɡ.Ꭼ(ড-म);if(ঠ>ɡ.ᓮ){ঠ=ɡ.ᓮ;}var ট=ɡ.ᓮ-ঠ;var ঞ=ᵎ.ᴿ(न,ऩ,ࡉ.ɝ.ޚ,ࡉ.ɝ.ޙ)
;var ল=ঞ*ஜ.ஸ(ট);var ৎ=ਝ(ब+ᄯ[ǎ],ब,ড,ল);Ꮖ ਊ=ৎ;Ꮖ ਅ;Ꮖ ਆ;if(ǐ>ǎ){ਅ=ਝ(ब+ᄯ[ǐ],ब,ড,ল);ਆ=(ਅ-ৎ)/(ǐ-ǎ);}else{ਅ=ਊ;ਆ=Ꮖ.Ꮓ;}var ਉ=ড-म;if
(ਉ>ɡ.ᓣ){ਉ=-ਉ;}if(ਉ>ɡ.ᓮ){ਉ=ɡ.ᓮ;}var ਇ=ঞ*ஜ.ஸ(ਉ);if(ড-म<ɡ.ᓣ){ਇ=-ਇ;}ਇ+=ࡉ.ɟ+ñ.ʜ;var ਈ=ɡ.ᓮ+ब-ড;var ࡈ=(π.Ħ>>ᄚ)+ۺ;if(ࡉ.ɝ.ޙ==ࡉ.ɞ.ޙ
){ࡈ--;}else if(ࡉ.ɝ.ޚ==ࡉ.ɞ.ޚ){ࡈ++;}var ࡇ=ᄌ[ᕣ.ᕊ(ࡈ,0,ᄓ-1)];শ>>=4;ষ>>=4;var ਏ=(ᄴ>>4)-শ*ৎ;var ਘ=-(ਆ*শ);var ਐ=(ᄴ>>4)-ষ*ৎ;var ਓ=
-(ਆ*ষ);var ਙ=(π.Ħ>>ᄚ)+ۺ;var ࠋ=ᄋ[ᕣ.ᕊ(ਙ,0,ᄓ-1)];var ৱ=ॺ[ॹ];ॹ++;ৱ.Ω=ࡉ;ৱ.ఖ=ǎ;ৱ.క=ǐ;ৱ.ఔ=ਊ;ৱ.ఓ=ਅ;ৱ.ఒ=ਆ;ৱ.ఐ=ఐ.ధ;ৱ.ఎ=Ꮖ.Ꮑ;ৱ.ఏ=Ꮖ.Ꮐ;
ৱ.ఊ=-1;ৱ.ఌ=ॳ;ৱ.ఋ=ॲ;ৱ.ఉ=ऴ;ৱ.ఈ=ळ;var ĉ=ü[l.ƒ.ཞ[π.ĥ]];var Ĉ=ü[l.ƒ.ཞ[π.Ĥ]];for(var ǃ=ǎ;ǃ<=ǐ;ǃ++){var ৡ=(ਏ.Ꮁ+ਠ-1)>>ਢ;var ৠ=ਐ.Ꮁ
>>ਢ;if(ম){var ࠍ=ग[ǃ]+1;var ࡊ=Math.Min(ৡ-1,ङ[ǃ]-1);ࡕ(π,ĉ,ࠋ,ǃ,ࠍ,ࡊ,ळ);}if(ਜ){var ࡍ=Math.Max(ৡ,ग[ǃ]+1);var ࡎ=Math.Min(ৠ,ङ[ǃ]-1
);var Ş=ਈ+ᄯ[ǃ];Ş=new ɡ(Ş.Ꮁ&0x7FFFFFFF);var ऎ=(ਇ-ஜ.ழ(Ş)*ল).Ꮄ();var ࠒ=ࡆ.ᅘ.ټ[ऎ&ਛ];if(ࠒ.Length>0){var য়=ৎ.Ꮁ>>ᄔ;if(য়>=ᄗ){য়=ᄗ-1
;}var ࠤ=new Ꮖ((int)(0xffffffffu/(uint)ৎ.Ꮁ));ࠨ(ࠒ[0],ࡇ[য়],ǃ,ࡍ,ࡎ,ࠤ,ਚ);}}if(ব){var ࡐ=Math.Max(ৠ+1,ग[ǃ]+1);var ࡑ=ङ[ǃ]-1;ࠈ(π,Ĉ,
ࠋ,ǃ,ࡐ,ࡑ,ऴ);}ৎ+=ਆ;ਏ+=ਘ;ਐ+=ਓ;}}private void ਣ(Ω ࡉ,ɡ म,int ǎ,int ǐ,bool য){if(ॹ==ॺ.Length){return;}var র=ǐ-ǎ+1;if(ॶ+3*র>=ॷ.
Length){return;}var ò=ࡉ.ɢ;var ñ=ࡉ.ɦ;var π=ࡉ.ɣ;var ρ=ࡉ.ɤ;var ऴ=π.Č(č);var ळ=π.ě(č);var ल=ρ.Č(č);var ऱ=ρ.ě(č);ò.ᘾ|=ᶘ.ᶎ;var শ=ळ-ڼ
;var ষ=ऴ-ڼ;var স=ऱ-ڼ;var হ=ल-ڼ;if(π.ĥ==ü.ᜅ&&ρ.ĥ==ü.ᜅ){শ=স;}bool ঽ;bool ম;if(য||শ!=স||π.ĥ!=ρ.ĥ||π.Ħ!=ρ.Ħ){ঽ=ñ.ɻ!=0&&স<শ;ম=
শ>=Ꮖ.Ꮓ||π.ĥ==ü.ᜅ;}else{ঽ=false;ম=false;}bool ভ;bool ব;if(য||ষ!=হ||π.Ĥ!=ρ.Ĥ||π.Ħ!=ρ.Ħ){ভ=ñ.ʄ!=0&&হ>ষ;ব=ষ<=Ꮖ.Ꮓ;}else{ভ=
false;ব=false;}var ফ=ñ.ʅ!=0;if(!ঽ&&!ম&&!ভ&&!ব&&!ফ){return;}var প=ঽ||ভ||ফ;var ন=default(ථ);var ধ=default(int);var দ=default(Ꮖ)
;if(ঽ){ন=ʽ[l.ƒ.ཝ[ñ.ɻ]];ধ=ন.Ǒ-1;if((ò.ᘾ&ᶘ.ᶓ)!=0){দ=শ;}else{var থ=ऱ+Ꮖ.Ꭸ(ন.ǡ);দ=থ-ڼ;}দ+=ñ.ʛ;}var ত=default(ථ);var ণ=default(
int);var ঢ=default(Ꮖ);if(ভ){ত=ʽ[l.ƒ.ཝ[ñ.ʄ]];ণ=ত.Ǒ-1;if((ò.ᘾ&ᶘ.ᶒ)!=0){ঢ=শ;}else{ঢ=হ;}ঢ+=ñ.ʛ;}var ড=ࡉ.ɡ+ɡ.ᓮ;var ঠ=ɡ.Ꭼ(ড-म);if
(ঠ>ɡ.ᓮ){ঠ=ɡ.ᓮ;}var ট=ɡ.ᓮ-ঠ;var ঞ=ᵎ.ᴿ(न,ऩ,ࡉ.ɝ.ޚ,ࡉ.ɝ.ޙ);var ল=ঞ*ஜ.ஸ(ট);var ৎ=ਝ(ब+ᄯ[ǎ],ब,ড,ল);Ꮖ ਊ=ৎ;Ꮖ ਅ;Ꮖ ਆ;if(ǐ>ǎ){ਅ=ਝ(ब+ᄯ[
ǐ],ब,ড,ল);ਆ=(ਅ-ৎ)/(ǐ-ǎ);}else{ਅ=ਊ;ਆ=Ꮖ.Ꮓ;}var ਇ=default(Ꮖ);var ਈ=default(ɡ);var ࡇ=default(byte[][]);if(প){var ਉ=ড-म;if(ਉ>ɡ
.ᓣ){ਉ=-ਉ;}if(ਉ>ɡ.ᓮ){ਉ=ɡ.ᓮ;}ਇ=ঞ*ஜ.ஸ(ਉ);if(ড-म<ɡ.ᓣ){ਇ=-ਇ;}ਇ+=ࡉ.ɟ+ñ.ʜ;ਈ=ɡ.ᓮ+ब-ড;var ࡈ=(π.Ħ>>ᄚ)+ۺ;if(ࡉ.ɝ.ޙ==ࡉ.ɞ.ޙ){ࡈ--;}else
if(ࡉ.ɝ.ޚ==ࡉ.ɞ.ޚ){ࡈ++;}ࡇ=ᄌ[ᕣ.ᕊ(ࡈ,0,ᄓ-1)];}শ>>=4;ষ>>=4;স>>=4;হ>>=4;var ਏ=(ᄴ>>4)-শ*ৎ;var ਘ=-(ਆ*শ);var ਐ=(ᄴ>>4)-ষ*ৎ;var ਓ=-(ਆ*
ষ);var ਔ=default(Ꮖ);var ਕ=default(Ꮖ);if(ঽ){if(স>ষ){ਔ=(ᄴ>>4)-স*ৎ;ਕ=-(ਆ*স);}else{ਔ=(ᄴ>>4)-ষ*ৎ;ਕ=-(ਆ*ষ);}}var ਖ=default(Ꮖ);
var ਗ=default(Ꮖ);if(ভ){if(হ<শ){ਖ=(ᄴ>>4)-হ*ৎ;ਗ=-(ਆ*হ);}else{ਖ=(ᄴ>>4)-শ*ৎ;ਗ=-(ਆ*শ);}}var ਙ=(π.Ħ>>ᄚ)+ۺ;var ࠋ=ᄋ[ᕣ.ᕊ(ਙ,0,ᄓ-1)];
var ৱ=ॺ[ॹ];ॹ++;ৱ.Ω=ࡉ;ৱ.ఖ=ǎ;ৱ.క=ǐ;ৱ.ఔ=ਊ;ৱ.ఓ=ਅ;ৱ.ఒ=ਆ;ৱ.ఌ=-1;ৱ.ఋ=-1;ৱ.ఐ=0;if(ऴ>ल){ৱ.ఐ=ఐ.ઞ;ৱ.ఎ=ऴ;}else if(ल>ڼ){ৱ.ఐ=ఐ.ઞ;ৱ.ఎ=Ꮖ.Ꮑ;
}if(ळ<ऱ){ৱ.ఐ|=ఐ.బ;ৱ.ఏ=ळ;}else if(ऱ<ڼ){ৱ.ఐ|=ఐ.బ;ৱ.ఏ=Ꮖ.Ꮐ;}if(ऱ<=ऴ){ৱ.ఋ=ॲ;ৱ.ఎ=Ꮖ.Ꮑ;ৱ.ఐ|=ఐ.ઞ;}if(ल>=ळ){ৱ.ఌ=ॳ;ৱ.ఏ=Ꮖ.Ꮐ;ৱ.ఐ|=ఐ.బ;
}var ৰ=default(int);if(ফ){ৰ=ॶ-ǎ;ৱ.ఊ=ৰ;ॶ+=র;}else{ৱ.ఊ=-1;}ৱ.ఉ=ऴ;ৱ.ఈ=ळ;ৱ.ఇ=ल;ৱ.ఙ=ऱ;var ĉ=ü[l.ƒ.ཞ[π.ĥ]];var Ĉ=ü[l.ƒ.ཞ[π.Ĥ]];
for(var ǃ=ǎ;ǃ<=ǐ;ǃ++){var ৡ=(ਏ.Ꮁ+ਠ-1)>>ਢ;var ৠ=ਐ.Ꮁ>>ਢ;var ऎ=default(int);var য়=default(int);var ࠤ=default(Ꮖ);if(প){var Ş=ਈ+
ᄯ[ǃ];Ş=new ɡ(Ş.Ꮁ&0x7FFFFFFF);ऎ=(ਇ-ஜ.ழ(Ş)*ল).Ꮄ();য়=ৎ.Ꮁ>>ᄔ;if(য়>=ᄗ){য়=ᄗ-1;}ࠤ=new Ꮖ((int)(0xffffffffu/(uint)ৎ.Ꮁ));}if(ঽ){var
ঢ়=(ਏ.Ꮁ+ਠ-1)>>ਢ;var ড়=ਔ.Ꮁ>>ਢ;if(ম){var ࠍ=ग[ǃ]+1;var ࡊ=Math.Min(ৡ-1,ङ[ǃ]-1);ࡕ(π,ĉ,ࠋ,ǃ,ࠍ,ࡊ,ळ);}var ࡍ=Math.Max(ঢ়,ग[ǃ]+1);var
ࡎ=Math.Min(ড়,ङ[ǃ]-1);var ࠒ=ন.ᅘ.ټ[ऎ&ধ];if(ࠒ.Length>0){ࠨ(ࠒ[0],ࡇ[য়],ǃ,ࡍ,ࡎ,ࠤ,দ);}if(ग[ǃ]<ࡎ){ग[ǃ]=(short)ࡎ;}ਔ+=ਕ;}else if(ম){
var ࠍ=ग[ǃ]+1;var ࡊ=Math.Min(ৡ-1,ङ[ǃ]-1);ࡕ(π,ĉ,ࠋ,ǃ,ࠍ,ࡊ,ळ);if(ग[ǃ]<ࡊ){ग[ǃ]=(short)ࡊ;}}if(ভ){var ࡋ=(ਖ.Ꮁ+ਠ-1)>>ਢ;var ࡌ=ਐ.Ꮁ>>ਢ;
var ࡍ=Math.Max(ࡋ,ग[ǃ]+1);var ࡎ=Math.Min(ࡌ,ङ[ǃ]-1);var ࠒ=ত.ᅘ.ټ[ऎ&ণ];if(ࠒ.Length>0){ࠨ(ࠒ[0],ࡇ[য়],ǃ,ࡍ,ࡎ,ࠤ,ঢ);}if(ব){var ࡐ=Math.
Max(ৠ+1,ग[ǃ]+1);var ࡑ=ङ[ǃ]-1;ࠈ(π,Ĉ,ࠋ,ǃ,ࡐ,ࡑ,ऴ);}if(ङ[ǃ]>ࡍ){ङ[ǃ]=(short)ࡍ;}ਖ+=ਗ;}else if(ব){var ࡐ=Math.Max(ৠ+1,ग[ǃ]+1);var ࡑ=
ङ[ǃ]-1;ࠈ(π,Ĉ,ࠋ,ǃ,ࡐ,ࡑ,ऴ);if(ङ[ǃ]>ৠ+1){ङ[ǃ]=(short)ࡐ;}}if(ফ){ॷ[ৰ+ǃ]=(short)ऎ;}ৎ+=ਆ;ਏ+=ਘ;ਐ+=ਓ;}if(((ৱ.ఐ&ఐ.బ)!=0||ফ)&&ৱ.ఌ==-1
){Array.Copy(ग,ǎ,ॷ,ॶ,র);ৱ.ఌ=ॶ-ǎ;ॶ+=র;}if(((ৱ.ఐ&ఐ.ઞ)!=0||ফ)&&ৱ.ఋ==-1){Array.Copy(ङ,ǎ,ॷ,ॶ,র);ৱ.ఋ=ॶ-ǎ;ॶ+=র;}if(ফ&&(ৱ.ఐ&ఐ.బ)
==0){ৱ.ఐ|=ఐ.బ;ৱ.ఏ=Ꮖ.Ꮐ;}if(ফ&&(ৱ.ఐ&ఐ.ઞ)==0){ৱ.ఐ|=ఐ.ઞ;ৱ.ఎ=Ꮖ.Ꮑ;}}private void ࡒ(){for(var Ä=ॹ-1;Ä>=0;Ä--){var ࡓ=ॺ[Ä];if(ࡓ.ఊ!=
-1){ࡔ(ࡓ,ࡓ.ఖ,ࡓ.క);}}}private void ࡔ(గ ࡓ,int ǎ,int ǐ){var ࡉ=ࡓ.Ω;var ࡈ=(ࡉ.ɣ.Ħ>>ᄚ)+ۺ;if(ࡉ.ɝ.ޙ==ࡉ.ɞ.ޙ){ࡈ--;}else if(ࡉ.ɝ.ޚ==ࡉ.ɞ
.ޚ){ࡈ++;}var ࡇ=ᄌ[ᕣ.ᕊ(ࡈ,0,ᄓ-1)];var ࡆ=ʽ[l.ƒ.ཝ[ࡉ.ɦ.ʅ]];var ࡅ=ࡆ.Ǒ-1;Ꮖ ࡄ;if((ࡉ.ɢ.ᘾ&ᶘ.ᶒ)!=0){ࡄ=ࡓ.ఉ>ࡓ.ఇ?ࡓ.ఉ:ࡓ.ఇ;ࡄ=ࡄ+Ꮖ.Ꭸ(ࡆ.ǡ)-ڼ;
}else{ࡄ=ࡓ.ఈ<ࡓ.ఙ?ࡓ.ఈ:ࡓ.ఙ;ࡄ=ࡄ-ڼ;}ࡄ+=ࡉ.ɦ.ʛ;var ࡃ=ࡓ.ఒ;var ǉ=ࡓ.ఔ+(ǎ-ࡓ.ఖ)*ࡃ;for(var ǃ=ǎ;ǃ<=ǐ;ǃ++){var ı=Math.Min(ǉ.Ꮁ>>ᄔ,ᄗ-1);
var ࡂ=ॷ[ࡓ.ఊ+ǃ];if(ࡂ!=short.MaxValue){var ࡁ=ᄴ-ࡄ*ǉ;var ࠤ=new Ꮖ((int)(0xffffffffu/(uint)ǉ.Ꮁ));var ࡀ=ॷ[ࡓ.ఌ+ǃ];var ࡏ=ॷ[ࡓ.ఋ+ǃ];क(
ࡆ.ᅘ.ټ[ࡂ&ࡅ],ࡇ[ı],ǃ,ࡁ,ǉ,ࠤ,ࡄ,ࡀ,ࡏ);ॷ[ࡓ.ఊ+ǃ]=short.MaxValue;}ǉ+=ࡃ;}}private void ࡕ(Ŀ ô,ᎂ ࠉ,byte[][]ࠋ,int ǃ,int ǆ,int ǅ,Ꮖ ą){if
(ࠉ==ü.ᯇ){ख(ǃ,ǆ,ǅ);return;}if(ǅ-ǆ<0){return;}var ˢ=Ꮖ.Ꭼ(ą-ڼ);var ࠊ=ࠉ.Ꮁ;if(ô==ჿ&&ᄀ==ǃ-1){var ࠌ=Math.Max(ǆ,ᄁ);var ࠇ=Math.Min(
ǅ,ᄂ);var ࠆ=ᄡ*(ᄻ+ǃ)+ᄼ+ǆ;for(var ǂ=ǆ;ǂ<ࠌ;ǂ++){var ࠂ=ˢ*ჺ[ǂ];ᄆ[ǂ]=ࠂ*ჾ;ჷ[ǂ]=ࠂ*ᄅ;var û=ࠂ*ჼ[ǃ];var Ş=ब+ᄯ[ǃ];var ࠅ=न+ஜ.ௐ(Ş)*û;var
ࠄ=-ऩ-ஜ.ஸ(Ş)*û;ᄃ[ǂ]=ࠅ;ᄄ[ǂ]=ࠄ;var ۼ=ࠋ[Math.Min((uint)(ࠂ.Ꮁ>>ᄕ),ᄘ-1)];ჶ[ǂ]=ۼ;var ࠃ=((ࠄ.Ꮁ>>(16-6))&(63*64))+((ࠅ.Ꮁ>>16)&63);ᄠ[ࠆ
]=ۼ[ࠊ[ࠃ]];ࠆ++;}for(var ǂ=ࠌ;ǂ<=ࠇ;ǂ++){var ࠅ=ᄃ[ǂ]+ᄆ[ǂ];var ࠄ=ᄄ[ǂ]+ჷ[ǂ];var ࠃ=((ࠄ.Ꮁ>>(16-6))&(63*64))+((ࠅ.Ꮁ>>16)&63);ᄠ[ࠆ]=ჶ[
ǂ][ࠊ[ࠃ]];ࠆ++;ᄃ[ǂ]=ࠅ;ᄄ[ǂ]=ࠄ;}for(var ǂ=ࠇ+1;ǂ<=ǅ;ǂ++){var ࠂ=ˢ*ჺ[ǂ];ᄆ[ǂ]=ࠂ*ჾ;ჷ[ǂ]=ࠂ*ᄅ;var û=ࠂ*ჼ[ǃ];var Ş=ब+ᄯ[ǃ];var ࠅ=न+ஜ.ௐ(
Ş)*û;var ࠄ=-ऩ-ஜ.ஸ(Ş)*û;ᄃ[ǂ]=ࠅ;ᄄ[ǂ]=ࠄ;var ۼ=ࠋ[Math.Min((uint)(ࠂ.Ꮁ>>ᄕ),ᄘ-1)];ჶ[ǂ]=ۼ;var ࠃ=((ࠄ.Ꮁ>>(16-6))&(63*64))+((ࠅ.Ꮁ>>16
)&63);ᄠ[ࠆ]=ۼ[ࠊ[ࠃ]];ࠆ++;}}else{var ࠆ=ᄡ*(ᄻ+ǃ)+ᄼ+ǆ;for(var ǂ=ǆ;ǂ<=ǅ;ǂ++){var ࠂ=ˢ*ჺ[ǂ];ᄆ[ǂ]=ࠂ*ჾ;ჷ[ǂ]=ࠂ*ᄅ;var û=ࠂ*ჼ[ǃ];var Ş=ब
+ᄯ[ǃ];var ࠅ=न+ஜ.ௐ(Ş)*û;var ࠄ=-ऩ-ஜ.ஸ(Ş)*û;ᄃ[ǂ]=ࠅ;ᄄ[ǂ]=ࠄ;var ۼ=ࠋ[Math.Min((uint)(ࠂ.Ꮁ>>ᄕ),ᄘ-1)];ჶ[ǂ]=ۼ;var ࠃ=((ࠄ.Ꮁ>>(16-6))&
(63*64))+((ࠅ.Ꮁ>>16)&63);ᄠ[ࠆ]=ۼ[ࠊ[ࠃ]];ࠆ++;}}ჿ=ô;ᄀ=ǃ;ᄁ=ǆ;ᄂ=ǅ;}private void ࠈ(Ŀ ô,ᎂ ࠉ,byte[][]ࠋ,int ǃ,int ǆ,int ǅ,Ꮖ ć){if(ࠉ
==ü.ᯇ){ख(ǃ,ǆ,ǅ);return;}if(ǅ-ǆ<0){return;}var ˢ=Ꮖ.Ꭼ(ć-ڼ);var ࠊ=ࠉ.Ꮁ;if(ô==ჵ&&ჴ==ǃ-1){var ࠌ=Math.Max(ǆ,ჳ);var ࠇ=Math.Min(ǅ,ჲ
);var ࠆ=ᄡ*(ᄻ+ǃ)+ᄼ+ǆ;for(var ǂ=ǆ;ǂ<ࠌ;ǂ++){var ࠂ=ˢ*ჺ[ǂ];ჯ[ǂ]=ࠂ*ჾ;ხ[ǂ]=ࠂ*ᄅ;var û=ࠂ*ჼ[ǃ];var Ş=ब+ᄯ[ǃ];var ࠅ=न+ஜ.ௐ(Ş)*û;var ࠄ=
-ऩ-ஜ.ஸ(Ş)*û;ჱ[ǂ]=ࠅ;ჰ[ǂ]=ࠄ;var ۼ=ࠋ[Math.Min((uint)(ࠂ.Ꮁ>>ᄕ),ᄘ-1)];ჭ[ǂ]=ۼ;var ࠃ=((ࠄ.Ꮁ>>(16-6))&(63*64))+((ࠅ.Ꮁ>>16)&63);ᄠ[ࠆ]=
ۼ[ࠊ[ࠃ]];ࠆ++;}for(var ǂ=ࠌ;ǂ<=ࠇ;ǂ++){var ࠅ=ჱ[ǂ]+ჯ[ǂ];var ࠄ=ჰ[ǂ]+ხ[ǂ];var ࠃ=((ࠄ.Ꮁ>>(16-6))&(63*64))+((ࠅ.Ꮁ>>16)&63);ᄠ[ࠆ]=ჭ[ǂ]
[ࠊ[ࠃ]];ࠆ++;ჱ[ǂ]=ࠅ;ჰ[ǂ]=ࠄ;}for(var ǂ=ࠇ+1;ǂ<=ǅ;ǂ++){var ࠂ=ˢ*ჺ[ǂ];ჯ[ǂ]=ࠂ*ჾ;ხ[ǂ]=ࠂ*ᄅ;var û=ࠂ*ჼ[ǃ];var Ş=ब+ᄯ[ǃ];var ࠅ=न+ஜ.ௐ(Ş)
*û;var ࠄ=-ऩ-ஜ.ஸ(Ş)*û;ჱ[ǂ]=ࠅ;ჰ[ǂ]=ࠄ;var ۼ=ࠋ[Math.Min((uint)(ࠂ.Ꮁ>>ᄕ),ᄘ-1)];ჭ[ǂ]=ۼ;var ࠃ=((ࠄ.Ꮁ>>(16-6))&(63*64))+((ࠅ.Ꮁ>>16)&
63);ᄠ[ࠆ]=ۼ[ࠊ[ࠃ]];ࠆ++;}}else{var ࠆ=ᄡ*(ᄻ+ǃ)+ᄼ+ǆ;for(var ǂ=ǆ;ǂ<=ǅ;ǂ++){var ࠂ=ˢ*ჺ[ǂ];ჯ[ǂ]=ࠂ*ჾ;ხ[ǂ]=ࠂ*ᄅ;var û=ࠂ*ჼ[ǃ];var Ş=ब+ᄯ[
ǃ];var ࠅ=न+ஜ.ௐ(Ş)*û;var ࠄ=-ऩ-ஜ.ஸ(Ş)*û;ჱ[ǂ]=ࠅ;ჰ[ǂ]=ࠄ;var ۼ=ࠋ[Math.Min((uint)(ࠂ.Ꮁ>>ᄕ),ᄘ-1)];ჭ[ǂ]=ۼ;var ࠃ=((ࠄ.Ꮁ>>(16-6))&(63
*64))+((ࠅ.Ꮁ>>16)&63);ᄠ[ࠆ]=ۼ[ࠊ[ࠃ]];ࠆ++;}}ჵ=ô;ჴ=ǃ;ჳ=ǆ;ჲ=ǅ;}private void ࠨ(ᖒ ࠐ,byte[]ࠎ,int ǃ,int ǆ,int ǅ,Ꮖ ࠤ,Ꮖ ࠚ){if(ǅ-ǆ<0){
return;}var ࠕ=ᄡ*(ᄻ+ǃ)+ᄼ+ǆ;var ࠔ=ࠕ+(ǅ-ǆ);var ࠓ=ࠤ;var ڂ=ࠚ+(ǆ-ᄶ)*ࠓ;var ࠒ=ࠐ.Ꮁ;var ù=ࠐ.ɟ;for(var ࠆ=ࠕ;ࠆ<=ࠔ;ࠆ++){ᄠ[ࠆ]=ࠎ[ࠒ[ù+((ڂ.Ꮁ>>Ꮖ.
Ꮕ)&127)]];ڂ+=ࠓ;}}private void ࠑ(ᖒ ࠐ,byte[]ࠏ,byte[]ࠎ,int ǃ,int ǆ,int ǅ,Ꮖ ࠤ,Ꮖ ࠚ){if(ǅ-ǆ<0){return;}var ࠕ=ᄡ*(ᄻ+ǃ)+ᄼ+ǆ;var ࠔ=
ࠕ+(ǅ-ǆ);var ࠓ=ࠤ;var ڂ=ࠚ+(ǆ-ᄶ)*ࠓ;var ࠒ=ࠐ.Ꮁ;var ù=ࠐ.ɟ;for(var ࠆ=ࠕ;ࠆ<=ࠔ;ࠆ++){ᄠ[ࠆ]=ࠎ[ࠏ[ࠒ[ù+((ڂ.Ꮁ>>Ꮖ.Ꮕ)&127)]]];ڂ+=ࠓ;}}private
void घ(ᖒ ࠐ,int ǃ,int ǆ,int ǅ){if(ǅ-ǆ<0){return;}if(ǆ==0){ǆ=1;}if(ǅ==ᄸ-1){ǅ=ᄸ-2;}var ࠕ=ᄡ*(ᄻ+ǃ)+ᄼ+ǆ;var ࠔ=ࠕ+(ǅ-ǆ);var ࠎ=ۼ[6];
for(var ࠆ=ࠕ;ࠆ<=ࠔ;ࠆ++){ᄠ[ࠆ]=ࠎ[ᄠ[ࠆ+ক[ঋ]]];if(++ঋ==ক.Length){ঋ=0;}}}private void ख(int ǃ,int ǆ,int ǅ){var Ş=(ब+ᄯ[ǃ]).Ꮁ>>ᄎ;var
ࡅ=l.શ.ᜄ.Ǒ-1;var ࠒ=l.શ.ᜄ.ᅘ.ټ[Ş&ࡅ];ࠨ(ࠒ[0],ۼ[0],ǃ,ǆ,ǅ,ᄐ,ᄏ);}private void क(ᖒ[]څ,byte[]ࠎ,int ǃ,Ꮖ ࡁ,Ꮖ ǉ,Ꮖ ࠤ,Ꮖ ࠚ,int ग,int ङ){
foreach(var ࠐ in څ){var छ=ࡁ+ǉ*ࠐ.ᖔ;var च=छ+ǉ*ࠐ.ᖏ;var ǆ=(छ.Ꮁ+Ꮖ.Ꮔ-1)>>Ꮖ.Ꮕ;var ǅ=(च.Ꮁ-1)>>Ꮖ.Ꮕ;ǆ=Math.Max(ǆ,ग+1);ǅ=Math.Min(ǅ,ङ-1);
if(ǆ<=ǅ){var झ=new Ꮖ(ࠚ.Ꮁ-(ࠐ.ᖔ<<Ꮖ.Ꮕ));ࠨ(ࠐ,ࠎ,ǃ,ǆ,ǅ,ࠤ,झ);}}}private void ञ(ᖒ[]څ,byte[]ࠏ,byte[]ࠎ,int ǃ,Ꮖ ࡁ,Ꮖ ǉ,Ꮖ ࠤ,Ꮖ ࠚ,int ग,
int ङ){foreach(var ࠐ in څ){var छ=ࡁ+ǉ*ࠐ.ᖔ;var च=छ+ǉ*ࠐ.ᖏ;var ǆ=(छ.Ꮁ+Ꮖ.Ꮔ-1)>>Ꮖ.Ꮕ;var ǅ=(च.Ꮁ-1)>>Ꮖ.Ꮕ;ǆ=Math.Max(ǆ,ग+1);ǅ=Math.
Min(ǅ,ङ-1);if(ǆ<=ǅ){var झ=new Ꮖ(ࠚ.Ꮁ-(ࠐ.ᖔ<<Ꮖ.Ꮕ));ࠑ(ࠐ,ࠏ,ࠎ,ǃ,ǆ,ǅ,ࠤ,झ);}}}private void ज(ᖒ[]څ,int ǃ,Ꮖ ࡁ,Ꮖ ǉ,int ग,int ङ){
foreach(var ࠐ in څ){var छ=ࡁ+ǉ*ࠐ.ᖔ;var च=छ+ǉ*ࠐ.ᖏ;var ǆ=(छ.Ꮁ+Ꮖ.Ꮔ-1)>>Ꮖ.Ꮕ;var ǅ=(च.Ꮁ-1)>>Ꮖ.Ꮕ;ǆ=Math.Max(ǆ,ग+1);ǅ=Math.Min(ǅ,ङ-1);
if(ǆ<=ǅ){घ(ࠐ,ǃ,ǆ,ǅ);}}}private void औ(Ŀ ô,int Ĺ){if(ô.Ĕ==Ĺ){return;}ô.Ĕ=Ĺ;var ࢩ=(ô.Ħ>>ᄚ)+ۺ;var ࢪ=ᄌ[ᕣ.ᕊ(ࢩ,0,ᄓ-1)];foreach(
var ğ in ô){ProjectSprite(ğ,ࢪ);}}private void ࢫ(Ɠ ğ,byte[][]ࢪ){if(फ़==ढ़.Length){return;}var उ=ğ.ᘮ(č);var ࢬ=ğ.ᘶ(č);var ऄ=ğ.ᙅ(
č);var अ=उ-न;var आ=ࢬ-ऩ;var इ=(अ*फ);var ई=-(आ*प);var ऊ=इ-ई;if(ऊ<य़){return;}var ࢨ=ᄳ/ऊ;इ=-अ*प;ई=आ*फ;var ࢧ=-(ई+इ);if(Ꮖ.Ꭼ(ࢧ)>(
ऊ<<2)){return;}var ࢦ=ᄣ[ğ.э];var ࢥ=ğ.ю&0x7F;var ࢤ=ࢦ.ၛ[ࢥ];ڦ ý;bool ࢣ;if(ࢤ.ၔ){var ࢢ=ᵎ.ᴥ(न,ऩ,उ,ࢬ);var ࢠ=(ࢢ.Ꮁ-ğ.ɡ.Ꮁ+(uint)(ɡ.ᓭ
.Ꮁ/2)*9)>>29;ý=ࢤ.ജ[ࢠ];ࢣ=ࢤ.ཀྵ[ࢠ];}else{ý=ࢤ.ജ[0];ࢣ=ࢤ.ཀྵ[0];}ࢧ-=Ꮖ.Ꭸ(ý.پ);var ǎ=(ᄵ+(ࢧ*ࢨ)).Ꮁ>>Ꮖ.Ꮕ;if(ǎ>ᄽ){return;}ࢧ+=Ꮖ.Ꭸ(ý.Ǒ);
var ǐ=((ᄵ+(ࢧ*ࢨ)).Ꮁ>>Ꮖ.Ꮕ)-1;if(ǐ<0){return;}var ࡘ=ढ़[फ़];फ़++;ࡘ.ళ=ğ.ᘾ;ࡘ.య=ࢨ;ࡘ.ప=उ;ࡘ.ఫ=ࢬ;ࡘ.భ=ऄ;ࡘ.ల=ऄ+Ꮖ.Ꭸ(ý.ٽ);ࡘ.ఱ=ࡘ.ల-ڼ;ࡘ.ఖ=ǎ<0?
0:ǎ;ࡘ.క=ǐ>=ᄽ?ᄽ-1:ǐ;var ࠤ=Ꮖ.Ꮒ/ࢨ;if(ࢣ){ࡘ.మ=new Ꮖ(Ꮖ.Ꭸ(ý.Ǒ).Ꮁ-1);ࡘ.ర=-ࠤ;}else{ࡘ.మ=Ꮖ.Ꮓ;ࡘ.ర=ࠤ;}if(ࡘ.ఖ>ǎ){ࡘ.మ+=ࡘ.ర*(ࡘ.ఖ-ǎ);}ࡘ.ڦ=
ý;if(ۻ==0){if((ğ.ю&0x8000)==0){ࡘ.Ƭ=ࢪ[Math.Min(ࢨ.Ꮁ>>ᄔ,ᄗ-1)];}else{ࡘ.Ƭ=ۼ.ᖑ;}}else{ࡘ.Ƭ=ۼ[ۻ];}}private void ࡗ(){Array.Sort(ढ़,
0,फ़,ड़);for(var Ä=फ़-1;Ä>=0;Ä--){ࡖ(ढ़[Ä]);}}private void ࡖ(న Т){for(var ǃ=Т.ఖ;ǃ<=Т.క;ǃ++){ङ[ǃ]=-2;ग[ǃ]=-2;}for(var Ä=ॹ-1;Ä>=
0;Ä--){var ए=ॺ[Ä];if(ए.ఖ>Т.క||ए.క<Т.ఖ||(ए.ఐ==0&&ए.ఊ==-1)){continue;}var ऐ=ए.ఖ<Т.ఖ?Т.ఖ:ए.ఖ;var ऑ=ए.క>Т.క?Т.క:ए.క;Ꮖ ऒ;Ꮖ ǉ;
if(ए.ఔ>ए.ఓ){ऒ=ए.ఓ;ǉ=ए.ఔ;}else{ऒ=ए.ఔ;ǉ=ए.ఓ;}if(ǉ<Т.య||(ऒ<Т.య&&ᵎ.ᴤ(Т.ప,Т.ఫ,ए.Ω)==0)){if(ए.ఊ!=-1){ࡔ(ए,ऐ,ऑ);}continue;}var ओ=ए
.ఐ;if(Т.భ>=ए.ఎ){ओ&=~ఐ.ઞ;}if(Т.ల<=ए.ఏ){ओ&=~ఐ.బ;}if(ओ==ఐ.ઞ){for(var ǃ=ऐ;ǃ<=ऑ;ǃ++){if(ङ[ǃ]==-2){ङ[ǃ]=ॷ[ए.ఋ+ǃ];}}}else if(ओ==
ఐ.బ){for(var ǃ=ऐ;ǃ<=ऑ;ǃ++){if(ग[ǃ]==-2){ग[ǃ]=ॷ[ए.ఌ+ǃ];}}}else if(ओ==ఐ.ధ){for(var ǃ=ऐ;ǃ<=ऑ;ǃ++){if(ङ[ǃ]==-2){ङ[ǃ]=ॷ[ए.ఋ+ǃ]
;}if(ग[ǃ]==-2){ग[ǃ]=ॷ[ए.ఌ+ǃ];}}}}for(var ǃ=Т.ఖ;ǃ<=Т.క;ǃ++){if(ङ[ǃ]==-2){ङ[ǃ]=(short)ᄸ;}if(ग[ǃ]==-2){ग[ǃ]=-1;}}if((Т.ళ&ళ.ᘋ
)!=0){var ڂ=Т.మ;for(var ǃ=Т.ఖ;ǃ<=Т.క;ǃ++){var ऎ=ڂ.Ꮄ();ज(Т.ڦ.ټ[ऎ],ǃ,ᄴ-(Т.ఱ*Т.య),Т.య,ग[ǃ],ङ[ǃ]);ڂ+=Т.ర;}}else if(((int)(Т.ళ
&ళ.ᘃ)>>(int)ళ.ᘂ)!=0){byte[]ࠏ;switch(((int)(Т.ళ&ళ.ᘃ)>>(int)ళ.ᘂ)){case 1:ࠏ=এ;break;case 2:ࠏ=ঐ;break;default:ࠏ=ও;break;}var
ڂ=Т.మ;for(var ǃ=Т.ఖ;ǃ<=Т.క;ǃ++){var ऎ=ڂ.Ꮄ();ञ(Т.ڦ.ټ[ऎ],ࠏ,Т.Ƭ,ǃ,ᄴ-(Т.ఱ*Т.య),Т.య,Ꮖ.Ꭼ(Т.ర),Т.ఱ,ग[ǃ],ङ[ǃ]);ڂ+=Т.ర;}}else{var
ڂ=Т.మ;for(var ǃ=Т.ఖ;ǃ<=Т.క;ǃ++){var ऎ=ڂ.Ꮄ();क(Т.ڦ.ټ[ऎ],Т.Ƭ,ǃ,ᄴ-(Т.ఱ*Т.య),Т.య,Ꮖ.Ꭼ(Т.ర),Т.ఱ,ग[ǃ],ङ[ǃ]);ڂ+=Т.ర;}}}private
void ऍ(Ɓ Ś,byte[][]ࢪ,bool ऌ){var ࢦ=ᄣ[Ś.Ŷ.э];var ࢤ=ࢦ.ၛ[Ś.Ŷ.ю&0x7fff];var ý=ࢤ.ജ[0];var ࢣ=ࢤ.ཀྵ[0];var ࢧ=Ś.Ŵ-Ꮖ.Ꭸ(160);ࢧ-=Ꮖ.Ꭸ(ý.پ)
;var ǎ=(ᄵ+ࢧ*क़).Ꮁ>>Ꮖ.Ꮕ;if(ǎ>ᄽ){return;}ࢧ+=Ꮖ.Ꭸ(ý.Ǒ);var ǐ=((ᄵ+ࢧ*क़).Ꮁ>>Ꮖ.Ꮕ)-1;if(ǐ<0){return;}var ࡘ=ख़;ࡘ.ళ=0;ࡘ.ఱ=Ꮖ.Ꭸ(100)+Ꮖ.Ꮒ
/4-(Ś.ų-Ꮖ.Ꭸ(ý.ٽ));ࡘ.ఖ=ǎ<0?0:ǎ;ࡘ.క=ǐ>=ᄽ?ᄽ-1:ǐ;ࡘ.య=क़;if(ࢣ){ࡘ.ర=-ॐ;ࡘ.మ=Ꮖ.Ꭸ(ý.Ǒ)-new Ꮖ(1);}else{ࡘ.ర=ॐ;ࡘ.మ=Ꮖ.Ꮓ;}if(ࡘ.ఖ>ǎ){ࡘ.మ
+=ࡘ.ర*(ࡘ.ఖ-ǎ);}ࡘ.ڦ=ý;if(ۻ==0){if((Ś.Ŷ.ю&0x8000)==0){ࡘ.Ƭ=ࢪ[ᄗ-1];}else{ࡘ.Ƭ=ۼ.ᖑ;}}else{ࡘ.Ƭ=ۼ[ۻ];}if(ऌ){var ڂ=ࡘ.మ;for(var ǃ=ࡘ.
ఖ;ǃ<=ࡘ.క;ǃ++){var ગ=ڂ.Ꮁ>>Ꮖ.Ꮕ;ज(ࡘ.ڦ.ټ[ગ],ǃ,ᄴ-(ࡘ.ఱ*ࡘ.య),ࡘ.య,-1,ᄸ);ڂ+=ࡘ.ర;}}else{var ڂ=ࡘ.మ;for(var ǃ=ࡘ.ఖ;ǃ<=ࡘ.క;ǃ++){var ગ=ڂ
.Ꮁ>>Ꮖ.Ꮕ;क(ࡘ.ڦ.ټ[ગ],ࡘ.Ƭ,ǃ,ᄴ-(ࡘ.ఱ*ࡘ.య),ࡘ.య,Ꮖ.Ꭼ(ࡘ.ర),ࡘ.ఱ,-1,ᄸ);ڂ+=ࡘ.ర;}}}private void ఘ(ђ Ð){var ࢩ=(Ð.Ɠ.ฃ.Ŀ.Ħ>>ᄚ)+ۺ;byte[][]
ࢪ;if(ࢩ<0){ࢪ=ᄌ[0];}else if(ࢩ>=ᄓ){ࢪ=ᄌ[ᄓ-1];}else{ࢪ=ᄌ[ࢩ];}bool ऌ;if(Ð.Ɲ[(int)Ů.ū]>4*32||(Ð.Ɲ[(int)Ů.ū]&8)!=0){ऌ=true;}else{ऌ
=false;}for(var Ä=0;Ä<(int)ſ.ŏ;Ä++){var Ś=Ð.ƭ[Ä];if(Ś.Ŷ!=null){ऍ(Ś,ࢪ,ऌ);}}}public int ǝ{get{return ᄞ;}set{ᄞ=value;ᄹ(ᄞ);}}
private class చ{public int ఛ;public int జ;public void ఝ(చ র){ఛ=র.ఛ;జ=র.జ;}}private class గ{public Ω Ω;public int ఖ;public int క
;public Ꮖ ఔ;public Ꮖ ఓ;public Ꮖ ఒ;public ఐ ఐ;public Ꮖ ఏ;public Ꮖ ఎ;public int ఌ;public int ఋ;public int ఊ;public Ꮖ ఉ;
public Ꮖ ఈ;public Ꮖ ఇ;public Ꮖ ఙ;}[Flags]private enum ఐ{బ=1,ઞ=2,ధ=3}private class న{public int ఖ;public int క;public Ꮖ ప;
public Ꮖ ఫ;public Ꮖ భ;public Ꮖ ల;public Ꮖ మ;public Ꮖ య;public Ꮖ ర;public Ꮖ ఱ;public ڦ ڦ;public byte[]Ƭ;public ళ ళ;}private
class ద:IComparer<న>{public int Compare(న ǃ,న ǂ){return ǂ.య.Ꮁ-ǃ.య.Ꮁ;}}}sealed class థ{private sbyte త;private sbyte ణ;private
short ఢ;private byte డ;public void ŷ(){త=0;ణ=0;ఢ=0;డ=0;}public void ఝ(థ Ƥ){త=Ƥ.త;ణ=Ƥ.ణ;ఢ=Ƥ.ఢ;డ=Ƥ.డ;}public sbyte Ʊ{get{return
త;}set{త=value;}}public sbyte Ƶ{get{return ణ;}set{ణ=value;}}public short Ʋ{get{return ఢ;}set{ఢ=value;}}public byte ఠ{get{
return డ;}set{డ=value;}}}static class ట{public static byte ఞ=1;public static byte ఆ=2;public static byte Ě=128;public static
byte ஈ=3;public static byte ஏ=4;public static byte ஐ=8+16+32;public static byte ஒ=3;public static byte ஓ=1;}class ஔ:ᜯ{
private string Ĭ;private int ʀ;private int ɿ;private string[]ங;private int உ;private int ச;private Func<int>ʙ;private Action<
int>Ǳ;public ஔ(string Ĭ,int ʂ,int ʁ,int ʀ,int ɿ,string எ,string ஊ,int உ,Func<int>ʙ,Action<int>Ǳ):base(ʂ,ʁ,null){this.Ĭ=Ĭ;
this.ʀ=ʀ;this.ɿ=ɿ;this.ங=new[]{எ,ஊ};this.உ=உ;ச=0;this.Ǳ=Ǳ;this.ʙ=ʙ;}public void ʕ(){if(ʙ!=null){ச=ʙ();}}public void ĳ(){ச++;
if(ச==ங.Length){ச=0;}if(Ǳ!=null){Ǳ(ச);}}public void Ķ(){ச--;if(ச==-1){ச=ங.Length-1;}if(Ǳ!=null){Ǳ(ச);}}public string Ń=>Ĭ;
public int ɼ=>ʀ;public int ʈ=>ɿ;public string Ŷ=>ங[ச];public int க=>உ;}static partial class ஜ{public const int ஷ=8192;public
const int ற=ஷ-1;public const int ல=19;private const int ள=ஷ/4;public static Ꮖ ழ(ɡ வ){return new Ꮖ(ய[வ.Ꮁ>>ல]);}public static Ꮖ
ழ(int ஶ){return new Ꮖ(ய[ஶ]);}public static Ꮖ ஸ(ɡ Ş){return new Ꮖ(ம[Ş.Ꮁ>>ல]);}public static Ꮖ ஸ(int ஹ){return new Ꮖ(ம[ஹ]);
}public static Ꮖ ௐ(ɡ Ş){return new Ꮖ(ம[(Ş.Ꮁ>>ல)+ள]);}public static Ꮖ ௐ(int ஹ){return new Ꮖ(ம[ஹ+ள]);}public static ɡ అ(
uint ர){return new ɡ(ப[ர]);}}static partial class ஜ{private static int[]ய={-170910304,-56965752,-34178904,-24413316,-
18988036,-15535599,-13145455,-11392683,-10052327,-8994149,-8137527,-7429880,-6835455,-6329090,-5892567,-5512368,-5178251,-
4882318,-4618375,-4381502,-4167737,-3973855,-3797206,-3635590,-3487165,-3350381,-3223918,-3106651,-2997613,-2895966,-2800983,-
2712030,-2628549,-2550052,-2476104,-2406322,-2340362,-2277919,-2218719,-2162516,-2109087,-2058233,-2009771,-1963536,-1919378,-
1877161,-1836758,-1798063,-1760956,-1725348,-1691149,-1658278,-1626658,-1596220,-1566898,-1538632,-1511367,-1485049,-1459630,-
1435065,-1411312,-1388330,-1366084,-1344537,-1323658,-1303416,-1283783,-1264730,-1246234,-1228269,-1210813,-1193846,-1177345,-
1161294,-1145673,-1130465,-1115654,-1101225,-1087164,-1073455,-1060087,-1047046,-1034322,-1021901,-1009774,-997931,-986361,-
975054,-964003,-953199,-942633,-932298,-922186,-912289,-902602,-893117,-883829,-874730,-865817,-857081,-848520,-840127,-831898
,-823827,-815910,-808143,-800521,-793041,-785699,-778490,-771411,-764460,-757631,-750922,-744331,-737853,-731486,-725227,
-719074,-713023,-707072,-701219,-695462,-689797,-684223,-678737,-673338,-668024,-662792,-657640,-652568,-647572,-642651,-
637803,-633028,-628323,-623686,-619117,-614613,-610174,-605798,-601483,-597229,-593033,-588896,-584815,-580789,-576818,-572901
,-569035,-565221,-561456,-557741,-554074,-550455,-546881,-543354,-539870,-536431,-533034,-529680,-526366,-523094,-519861,
-516667,-513512,-510394,-507313,-504269,-501261,-498287,-495348,-492443,-489571,-486732,-483925,-481150,-478406,-475692,-
473009,-470355,-467730,-465133,-462565,-460024,-457511,-455024,-452564,-450129,-447720,-445337,-442978,-440643,-438332,-436045
,-433781,-431540,-429321,-427125,-424951,-422798,-420666,-418555,-416465,-414395,-412344,-410314,-408303,-406311,-404338,
-402384,-400448,-398530,-396630,-394747,-392882,-391034,-389202,-387387,-385589,-383807,-382040,-380290,-378555,-376835,-
375130,-373440,-371765,-370105,-368459,-366826,-365208,-363604,-362013,-360436,-358872,-357321,-355783,-354257,-352744,-351244
,-349756,-348280,-346816,-345364,-343924,-342495,-341078,-339671,-338276,-336892,-335519,-334157,-332805,-331464,-330133,
-328812,-327502,-326201,-324910,-323629,-322358,-321097,-319844,-318601,-317368,-316143,-314928,-313721,-312524,-311335,-
310154,-308983,-307819,-306664,-305517,-304379,-303248,-302126,-301011,-299904,-298805,-297714,-296630,-295554,-294485,-293423
,-292369,-291322,-290282,-289249,-288223,-287204,-286192,-285186,-284188,-283195,-282210,-281231,-280258,-279292,-278332,
-277378,-276430,-275489,-274553,-273624,-272700,-271782,-270871,-269965,-269064,-268169,-267280,-266397,-265519,-264646,-
263779,-262917,-262060,-261209,-260363,-259522,-258686,-257855,-257029,-256208,-255392,-254581,-253774,-252973,-252176,-251384
,-250596,-249813,-249035,-248261,-247492,-246727,-245966,-245210,-244458,-243711,-242967,-242228,-241493,-240763,-240036,
-239314,-238595,-237881,-237170,-236463,-235761,-235062,-234367,-233676,-232988,-232304,-231624,-230948,-230275,-229606,-
228941,-228279,-227621,-226966,-226314,-225666,-225022,-224381,-223743,-223108,-222477,-221849,-221225,-220603,-219985,-219370
,-218758,-218149,-217544,-216941,-216341,-215745,-215151,-214561,-213973,-213389,-212807,-212228,-211652,-211079,-210509,
-209941,-209376,-208815,-208255,-207699,-207145,-206594,-206045,-205500,-204956,-204416,-203878,-203342,-202809,-202279,-
201751,-201226,-200703,-200182,-199664,-199149,-198636,-198125,-197616,-197110,-196606,-196105,-195606,-195109,-194614,-194122
,-193631,-193143,-192658,-192174,-191693,-191213,-190736,-190261,-189789,-189318,-188849,-188382,-187918,-187455,-186995,
-186536,-186080,-185625,-185173,-184722,-184274,-183827,-183382,-182939,-182498,-182059,-181622,-181186,-180753,-180321,-
179891,-179463,-179037,-178612,-178190,-177769,-177349,-176932,-176516,-176102,-175690,-175279,-174870,-174463,-174057,-173653
,-173251,-172850,-172451,-172053,-171657,-171263,-170870,-170479,-170089,-169701,-169315,-168930,-168546,-168164,-167784,
-167405,-167027,-166651,-166277,-165904,-165532,-165162,-164793,-164426,-164060,-163695,-163332,-162970,-162610,-162251,-
161893,-161537,-161182,-160828,-160476,-160125,-159775,-159427,-159079,-158734,-158389,-158046,-157704,-157363,-157024,-156686
,-156349,-156013,-155678,-155345,-155013,-154682,-154352,-154024,-153697,-153370,-153045,-152722,-152399,-152077,-151757,
-151438,-151120,-150803,-150487,-150172,-149859,-149546,-149235,-148924,-148615,-148307,-148000,-147693,-147388,-147084,-
146782,-146480,-146179,-145879,-145580,-145282,-144986,-144690,-144395,-144101,-143808,-143517,-143226,-142936,-142647,-142359
,-142072,-141786,-141501,-141217,-140934,-140651,-140370,-140090,-139810,-139532,-139254,-138977,-138701,-138426,-138152,
-137879,-137607,-137335,-137065,-136795,-136526,-136258,-135991,-135725,-135459,-135195,-134931,-134668,-134406,-134145,-
133884,-133625,-133366,-133108,-132851,-132594,-132339,-132084,-131830,-131576,-131324,-131072,-130821,-130571,-130322,-130073
,-129825,-129578,-129332,-129086,-128841,-128597,-128353,-128111,-127869,-127627,-127387,-127147,-126908,-126669,-126432,
-126195,-125959,-125723,-125488,-125254,-125020,-124787,-124555,-124324,-124093,-123863,-123633,-123404,-123176,-122949,-
122722,-122496,-122270,-122045,-121821,-121597,-121374,-121152,-120930,-120709,-120489,-120269,-120050,-119831,-119613,-119396
,-119179,-118963,-118747,-118532,-118318,-118104,-117891,-117678,-117466,-117254,-117044,-116833,-116623,-116414,-116206,
-115998,-115790,-115583,-115377,-115171,-114966,-114761,-114557,-114354,-114151,-113948,-113746,-113545,-113344,-113143,-
112944,-112744,-112546,-112347,-112150,-111952,-111756,-111560,-111364,-111169,-110974,-110780,-110586,-110393,-110200,-110008
,-109817,-109626,-109435,-109245,-109055,-108866,-108677,-108489,-108301,-108114,-107927,-107741,-107555,-107369,-107184,
-107000,-106816,-106632,-106449,-106266,-106084,-105902,-105721,-105540,-105360,-105180,-105000,-104821,-104643,-104465,-
104287,-104109,-103933,-103756,-103580,-103404,-103229,-103054,-102880,-102706,-102533,-102360,-102187,-102015,-101843,-101671
,-101500,-101330,-101159,-100990,-100820,-100651,-100482,-100314,-100146,-99979,-99812,-99645,-99479,-99313,-99148,-98982
,-98818,-98653,-98489,-98326,-98163,-98000,-97837,-97675,-97513,-97352,-97191,-97030,-96870,-96710,-96551,-96391,-96233,-
96074,-95916,-95758,-95601,-95444,-95287,-95131,-94975,-94819,-94664,-94509,-94354,-94200,-94046,-93892,-93739,-93586,-93434,
-93281,-93129,-92978,-92826,-92675,-92525,-92375,-92225,-92075,-91926,-91777,-91628,-91480,-91332,-91184,-91036,-90889,-
90742,-90596,-90450,-90304,-90158,-90013,-89868,-89724,-89579,-89435,-89292,-89148,-89005,-88862,-88720,-88577,-88435,-88294,
-88152,-88011,-87871,-87730,-87590,-87450,-87310,-87171,-87032,-86893,-86755,-86616,-86479,-86341,-86204,-86066,-85930,-
85793,-85657,-85521,-85385,-85250,-85114,-84980,-84845,-84710,-84576,-84443,-84309,-84176,-84043,-83910,-83777,-83645,-83513,
-83381,-83250,-83118,-82987,-82857,-82726,-82596,-82466,-82336,-82207,-82078,-81949,-81820,-81691,-81563,-81435,-81307,-
81180,-81053,-80925,-80799,-80672,-80546,-80420,-80294,-80168,-80043,-79918,-79793,-79668,-79544,-79420,-79296,-79172,-79048,
-78925,-78802,-78679,-78557,-78434,-78312,-78190,-78068,-77947,-77826,-77705,-77584,-77463,-77343,-77223,-77103,-76983,-
76864,-76744,-76625,-76506,-76388,-76269,-76151,-76033,-75915,-75797,-75680,-75563,-75446,-75329,-75213,-75096,-74980,-74864,
-74748,-74633,-74517,-74402,-74287,-74172,-74058,-73944,-73829,-73715,-73602,-73488,-73375,-73262,-73149,-73036,-72923,-
72811,-72699,-72587,-72475,-72363,-72252,-72140,-72029,-71918,-71808,-71697,-71587,-71477,-71367,-71257,-71147,-71038,-70929,
-70820,-70711,-70602,-70494,-70385,-70277,-70169,-70061,-69954,-69846,-69739,-69632,-69525,-69418,-69312,-69205,-69099,-
68993,-68887,-68781,-68676,-68570,-68465,-68360,-68255,-68151,-68046,-67942,-67837,-67733,-67629,-67526,-67422,-67319,-67216,
-67113,-67010,-66907,-66804,-66702,-66600,-66498,-66396,-66294,-66192,-66091,-65989,-65888,-65787,-65686,-65586,-65485,-
65385,-65285,-65185,-65085,-64985,-64885,-64786,-64687,-64587,-64488,-64389,-64291,-64192,-64094,-63996,-63897,-63799,-63702,
-63604,-63506,-63409,-63312,-63215,-63118,-63021,-62924,-62828,-62731,-62635,-62539,-62443,-62347,-62251,-62156,-62060,-
61965,-61870,-61775,-61680,-61585,-61491,-61396,-61302,-61208,-61114,-61020,-60926,-60833,-60739,-60646,-60552,-60459,-60366,
-60273,-60181,-60088,-59996,-59903,-59811,-59719,-59627,-59535,-59444,-59352,-59261,-59169,-59078,-58987,-58896,-58805,-
58715,-58624,-58534,-58443,-58353,-58263,-58173,-58083,-57994,-57904,-57815,-57725,-57636,-57547,-57458,-57369,-57281,-57192,
-57104,-57015,-56927,-56839,-56751,-56663,-56575,-56487,-56400,-56312,-56225,-56138,-56051,-55964,-55877,-55790,-55704,-
55617,-55531,-55444,-55358,-55272,-55186,-55100,-55015,-54929,-54843,-54758,-54673,-54587,-54502,-54417,-54333,-54248,-54163,
-54079,-53994,-53910,-53826,-53741,-53657,-53574,-53490,-53406,-53322,-53239,-53156,-53072,-52989,-52906,-52823,-52740,-
52657,-52575,-52492,-52410,-52327,-52245,-52163,-52081,-51999,-51917,-51835,-51754,-51672,-51591,-51509,-51428,-51347,-51266,
-51185,-51104,-51023,-50942,-50862,-50781,-50701,-50621,-50540,-50460,-50380,-50300,-50221,-50141,-50061,-49982,-49902,-
49823,-49744,-49664,-49585,-49506,-49427,-49349,-49270,-49191,-49113,-49034,-48956,-48878,-48799,-48721,-48643,-48565,-48488,
-48410,-48332,-48255,-48177,-48100,-48022,-47945,-47868,-47791,-47714,-47637,-47560,-47484,-47407,-47331,-47254,-47178,-
47102,-47025,-46949,-46873,-46797,-46721,-46646,-46570,-46494,-46419,-46343,-46268,-46193,-46118,-46042,-45967,-45892,-45818,
-45743,-45668,-45593,-45519,-45444,-45370,-45296,-45221,-45147,-45073,-44999,-44925,-44851,-44778,-44704,-44630,-44557,-
44483,-44410,-44337,-44263,-44190,-44117,-44044,-43971,-43898,-43826,-43753,-43680,-43608,-43535,-43463,-43390,-43318,-43246,
-43174,-43102,-43030,-42958,-42886,-42814,-42743,-42671,-42600,-42528,-42457,-42385,-42314,-42243,-42172,-42101,-42030,-
41959,-41888,-41817,-41747,-41676,-41605,-41535,-41465,-41394,-41324,-41254,-41184,-41113,-41043,-40973,-40904,-40834,-40764,
-40694,-40625,-40555,-40486,-40416,-40347,-40278,-40208,-40139,-40070,-40001,-39932,-39863,-39794,-39726,-39657,-39588,-
39520,-39451,-39383,-39314,-39246,-39178,-39110,-39042,-38973,-38905,-38837,-38770,-38702,-38634,-38566,-38499,-38431,-38364,
-38296,-38229,-38161,-38094,-38027,-37960,-37893,-37826,-37759,-37692,-37625,-37558,-37491,-37425,-37358,-37291,-37225,-
37158,-37092,-37026,-36959,-36893,-36827,-36761,-36695,-36629,-36563,-36497,-36431,-36365,-36300,-36234,-36168,-36103,-36037,
-35972,-35907,-35841,-35776,-35711,-35646,-35580,-35515,-35450,-35385,-35321,-35256,-35191,-35126,-35062,-34997,-34932,-
34868,-34803,-34739,-34675,-34610,-34546,-34482,-34418,-34354,-34289,-34225,-34162,-34098,-34034,-33970,-33906,-33843,-33779,
-33715,-33652,-33588,-33525,-33461,-33398,-33335,-33272,-33208,-33145,-33082,-33019,-32956,-32893,-32830,-32767,-32705,-
32642,-32579,-32516,-32454,-32391,-32329,-32266,-32204,-32141,-32079,-32017,-31955,-31892,-31830,-31768,-31706,-31644,-31582,
-31520,-31458,-31396,-31335,-31273,-31211,-31150,-31088,-31026,-30965,-30904,-30842,-30781,-30719,-30658,-30597,-30536,-
30474,-30413,-30352,-30291,-30230,-30169,-30108,-30048,-29987,-29926,-29865,-29805,-29744,-29683,-29623,-29562,-29502,-29441,
-29381,-29321,-29260,-29200,-29140,-29080,-29020,-28959,-28899,-28839,-28779,-28719,-28660,-28600,-28540,-28480,-28420,-
28361,-28301,-28241,-28182,-28122,-28063,-28003,-27944,-27884,-27825,-27766,-27707,-27647,-27588,-27529,-27470,-27411,-27352,
-27293,-27234,-27175,-27116,-27057,-26998,-26940,-26881,-26822,-26763,-26705,-26646,-26588,-26529,-26471,-26412,-26354,-
26295,-26237,-26179,-26120,-26062,-26004,-25946,-25888,-25830,-25772,-25714,-25656,-25598,-25540,-25482,-25424,-25366,-25308,
-25251,-25193,-25135,-25078,-25020,-24962,-24905,-24847,-24790,-24732,-24675,-24618,-24560,-24503,-24446,-24389,-24331,-
24274,-24217,-24160,-24103,-24046,-23989,-23932,-23875,-23818,-23761,-23704,-23647,-23591,-23534,-23477,-23420,-23364,-23307,
-23250,-23194,-23137,-23081,-23024,-22968,-22911,-22855,-22799,-22742,-22686,-22630,-22573,-22517,-22461,-22405,-22349,-
22293,-22237,-22181,-22125,-22069,-22013,-21957,-21901,-21845,-21789,-21733,-21678,-21622,-21566,-21510,-21455,-21399,-21343,
-21288,-21232,-21177,-21121,-21066,-21010,-20955,-20900,-20844,-20789,-20734,-20678,-20623,-20568,-20513,-20457,-20402,-
20347,-20292,-20237,-20182,-20127,-20072,-20017,-19962,-19907,-19852,-19797,-19742,-19688,-19633,-19578,-19523,-19469,-19414,
-19359,-19305,-19250,-19195,-19141,-19086,-19032,-18977,-18923,-18868,-18814,-18760,-18705,-18651,-18597,-18542,-18488,-
18434,-18380,-18325,-18271,-18217,-18163,-18109,-18055,-18001,-17946,-17892,-17838,-17784,-17731,-17677,-17623,-17569,-17515,
-17461,-17407,-17353,-17300,-17246,-17192,-17138,-17085,-17031,-16977,-16924,-16870,-16817,-16763,-16710,-16656,-16603,-
16549,-16496,-16442,-16389,-16335,-16282,-16229,-16175,-16122,-16069,-16015,-15962,-15909,-15856,-15802,-15749,-15696,-15643,
-15590,-15537,-15484,-15431,-15378,-15325,-15272,-15219,-15166,-15113,-15060,-15007,-14954,-14901,-14848,-14795,-14743,-
14690,-14637,-14584,-14531,-14479,-14426,-14373,-14321,-14268,-14215,-14163,-14110,-14057,-14005,-13952,-13900,-13847,-13795,
-13742,-13690,-13637,-13585,-13533,-13480,-13428,-13375,-13323,-13271,-13218,-13166,-13114,-13062,-13009,-12957,-12905,-
12853,-12800,-12748,-12696,-12644,-12592,-12540,-12488,-12436,-12383,-12331,-12279,-12227,-12175,-12123,-12071,-12019,-11967,
-11916,-11864,-11812,-11760,-11708,-11656,-11604,-11552,-11501,-11449,-11397,-11345,-11293,-11242,-11190,-11138,-11086,-
11035,-10983,-10931,-10880,-10828,-10777,-10725,-10673,-10622,-10570,-10519,-10467,-10415,-10364,-10312,-10261,-10209,-10158,
-10106,-10055,-10004,-9952,-9901,-9849,-9798,-9747,-9695,-9644,-9592,-9541,-9490,-9438,-9387,-9336,-9285,-9233,-9182,-
9131,-9080,-9028,-8977,-8926,-8875,-8824,-8772,-8721,-8670,-8619,-8568,-8517,-8466,-8414,-8363,-8312,-8261,-8210,-8159,-8108
,-8057,-8006,-7955,-7904,-7853,-7802,-7751,-7700,-7649,-7598,-7547,-7496,-7445,-7395,-7344,-7293,-7242,-7191,-7140,-7089,
-7038,-6988,-6937,-6886,-6835,-6784,-6733,-6683,-6632,-6581,-6530,-6480,-6429,-6378,-6327,-6277,-6226,-6175,-6124,-6074,-
6023,-5972,-5922,-5871,-5820,-5770,-5719,-5668,-5618,-5567,-5517,-5466,-5415,-5365,-5314,-5264,-5213,-5162,-5112,-5061,-5011
,-4960,-4910,-4859,-4808,-4758,-4707,-4657,-4606,-4556,-4505,-4455,-4404,-4354,-4303,-4253,-4202,-4152,-4101,-4051,-4001,
-3950,-3900,-3849,-3799,-3748,-3698,-3648,-3597,-3547,-3496,-3446,-3395,-3345,-3295,-3244,-3194,-3144,-3093,-3043,-2992,-
2942,-2892,-2841,-2791,-2741,-2690,-2640,-2590,-2539,-2489,-2439,-2388,-2338,-2288,-2237,-2187,-2137,-2086,-2036,-1986,-1935
,-1885,-1835,-1784,-1734,-1684,-1633,-1583,-1533,-1483,-1432,-1382,-1332,-1281,-1231,-1181,-1131,-1080,-1030,-980,-929,-
879,-829,-779,-728,-678,-628,-578,-527,-477,-427,-376,-326,-276,-226,-175,-125,-75,-25,25,75,125,175,226,276,326,376,427,
477,527,578,628,678,728,779,829,879,929,980,1030,1080,1131,1181,1231,1281,1332,1382,1432,1483,1533,1583,1633,1684,1734,1784
,1835,1885,1935,1986,2036,2086,2137,2187,2237,2288,2338,2388,2439,2489,2539,2590,2640,2690,2741,2791,2841,2892,2942,2992,
3043,3093,3144,3194,3244,3295,3345,3395,3446,3496,3547,3597,3648,3698,3748,3799,3849,3900,3950,4001,4051,4101,4152,4202,4253
,4303,4354,4404,4455,4505,4556,4606,4657,4707,4758,4808,4859,4910,4960,5011,5061,5112,5162,5213,5264,5314,5365,5415,5466,
5517,5567,5618,5668,5719,5770,5820,5871,5922,5972,6023,6074,6124,6175,6226,6277,6327,6378,6429,6480,6530,6581,6632,6683,6733
,6784,6835,6886,6937,6988,7038,7089,7140,7191,7242,7293,7344,7395,7445,7496,7547,7598,7649,7700,7751,7802,7853,7904,7955,
8006,8057,8108,8159,8210,8261,8312,8363,8414,8466,8517,8568,8619,8670,8721,8772,8824,8875,8926,8977,9028,9080,9131,9182,9233
,9285,9336,9387,9438,9490,9541,9592,9644,9695,9747,9798,9849,9901,9952,10004,10055,10106,10158,10209,10261,10312,10364,
10415,10467,10519,10570,10622,10673,10725,10777,10828,10880,10931,10983,11035,11086,11138,11190,11242,11293,11345,11397,11449
,11501,11552,11604,11656,11708,11760,11812,11864,11916,11967,12019,12071,12123,12175,12227,12279,12331,12383,12436,12488,
12540,12592,12644,12696,12748,12800,12853,12905,12957,13009,13062,13114,13166,13218,13271,13323,13375,13428,13480,13533,13585
,13637,13690,13742,13795,13847,13900,13952,14005,14057,14110,14163,14215,14268,14321,14373,14426,14479,14531,14584,14637,
14690,14743,14795,14848,14901,14954,15007,15060,15113,15166,15219,15272,15325,15378,15431,15484,15537,15590,15643,15696,15749
,15802,15856,15909,15962,16015,16069,16122,16175,16229,16282,16335,16389,16442,16496,16549,16603,16656,16710,16763,16817,
16870,16924,16977,17031,17085,17138,17192,17246,17300,17353,17407,17461,17515,17569,17623,17677,17731,17784,17838,17892,17946
,18001,18055,18109,18163,18217,18271,18325,18380,18434,18488,18542,18597,18651,18705,18760,18814,18868,18923,18977,19032,
19086,19141,19195,19250,19305,19359,19414,19469,19523,19578,19633,19688,19742,19797,19852,19907,19962,20017,20072,20127,20182
,20237,20292,20347,20402,20457,20513,20568,20623,20678,20734,20789,20844,20900,20955,21010,21066,21121,21177,21232,21288,
21343,21399,21455,21510,21566,21622,21678,21733,21789,21845,21901,21957,22013,22069,22125,22181,22237,22293,22349,22405,22461
,22517,22573,22630,22686,22742,22799,22855,22911,22968,23024,23081,23137,23194,23250,23307,23364,23420,23477,23534,23591,
23647,23704,23761,23818,23875,23932,23989,24046,24103,24160,24217,24274,24331,24389,24446,24503,24560,24618,24675,24732,24790
,24847,24905,24962,25020,25078,25135,25193,25251,25308,25366,25424,25482,25540,25598,25656,25714,25772,25830,25888,25946,
26004,26062,26120,26179,26237,26295,26354,26412,26471,26529,26588,26646,26705,26763,26822,26881,26940,26998,27057,27116,27175
,27234,27293,27352,27411,27470,27529,27588,27647,27707,27766,27825,27884,27944,28003,28063,28122,28182,28241,28301,28361,
28420,28480,28540,28600,28660,28719,28779,28839,28899,28959,29020,29080,29140,29200,29260,29321,29381,29441,29502,29562,29623
,29683,29744,29805,29865,29926,29987,30048,30108,30169,30230,30291,30352,30413,30474,30536,30597,30658,30719,30781,30842,
30904,30965,31026,31088,31150,31211,31273,31335,31396,31458,31520,31582,31644,31706,31768,31830,31892,31955,32017,32079,32141
,32204,32266,32329,32391,32454,32516,32579,32642,32705,32767,32830,32893,32956,33019,33082,33145,33208,33272,33335,33398,
33461,33525,33588,33652,33715,33779,33843,33906,33970,34034,34098,34162,34225,34289,34354,34418,34482,34546,34610,34675,34739
,34803,34868,34932,34997,35062,35126,35191,35256,35321,35385,35450,35515,35580,35646,35711,35776,35841,35907,35972,36037,
36103,36168,36234,36300,36365,36431,36497,36563,36629,36695,36761,36827,36893,36959,37026,37092,37158,37225,37291,37358,37425
,37491,37558,37625,37692,37759,37826,37893,37960,38027,38094,38161,38229,38296,38364,38431,38499,38566,38634,38702,38770,
38837,38905,38973,39042,39110,39178,39246,39314,39383,39451,39520,39588,39657,39726,39794,39863,39932,40001,40070,40139,40208
,40278,40347,40416,40486,40555,40625,40694,40764,40834,40904,40973,41043,41113,41184,41254,41324,41394,41465,41535,41605,
41676,41747,41817,41888,41959,42030,42101,42172,42243,42314,42385,42457,42528,42600,42671,42743,42814,42886,42958,43030,43102
,43174,43246,43318,43390,43463,43535,43608,43680,43753,43826,43898,43971,44044,44117,44190,44263,44337,44410,44483,44557,
44630,44704,44778,44851,44925,44999,45073,45147,45221,45296,45370,45444,45519,45593,45668,45743,45818,45892,45967,46042,46118
,46193,46268,46343,46419,46494,46570,46646,46721,46797,46873,46949,47025,47102,47178,47254,47331,47407,47484,47560,47637,
47714,47791,47868,47945,48022,48100,48177,48255,48332,48410,48488,48565,48643,48721,48799,48878,48956,49034,49113,49191,49270
,49349,49427,49506,49585,49664,49744,49823,49902,49982,50061,50141,50221,50300,50380,50460,50540,50621,50701,50781,50862,
50942,51023,51104,51185,51266,51347,51428,51509,51591,51672,51754,51835,51917,51999,52081,52163,52245,52327,52410,52492,52575
,52657,52740,52823,52906,52989,53072,53156,53239,53322,53406,53490,53574,53657,53741,53826,53910,53994,54079,54163,54248,
54333,54417,54502,54587,54673,54758,54843,54929,55015,55100,55186,55272,55358,55444,55531,55617,55704,55790,55877,55964,56051
,56138,56225,56312,56400,56487,56575,56663,56751,56839,56927,57015,57104,57192,57281,57369,57458,57547,57636,57725,57815,
57904,57994,58083,58173,58263,58353,58443,58534,58624,58715,58805,58896,58987,59078,59169,59261,59352,59444,59535,59627,59719
,59811,59903,59996,60088,60181,60273,60366,60459,60552,60646,60739,60833,60926,61020,61114,61208,61302,61396,61491,61585,
61680,61775,61870,61965,62060,62156,62251,62347,62443,62539,62635,62731,62828,62924,63021,63118,63215,63312,63409,63506,63604
,63702,63799,63897,63996,64094,64192,64291,64389,64488,64587,64687,64786,64885,64985,65085,65185,65285,65385,65485,65586,
65686,65787,65888,65989,66091,66192,66294,66396,66498,66600,66702,66804,66907,67010,67113,67216,67319,67422,67526,67629,67733
,67837,67942,68046,68151,68255,68360,68465,68570,68676,68781,68887,68993,69099,69205,69312,69418,69525,69632,69739,69846,
69954,70061,70169,70277,70385,70494,70602,70711,70820,70929,71038,71147,71257,71367,71477,71587,71697,71808,71918,72029,72140
,72252,72363,72475,72587,72699,72811,72923,73036,73149,73262,73375,73488,73602,73715,73829,73944,74058,74172,74287,74402,
74517,74633,74748,74864,74980,75096,75213,75329,75446,75563,75680,75797,75915,76033,76151,76269,76388,76506,76625,76744,76864
,76983,77103,77223,77343,77463,77584,77705,77826,77947,78068,78190,78312,78434,78557,78679,78802,78925,79048,79172,79296,
79420,79544,79668,79793,79918,80043,80168,80294,80420,80546,80672,80799,80925,81053,81180,81307,81435,81563,81691,81820,81949
,82078,82207,82336,82466,82596,82726,82857,82987,83118,83250,83381,83513,83645,83777,83910,84043,84176,84309,84443,84576,
84710,84845,84980,85114,85250,85385,85521,85657,85793,85930,86066,86204,86341,86479,86616,86755,86893,87032,87171,87310,87450
,87590,87730,87871,88011,88152,88294,88435,88577,88720,88862,89005,89148,89292,89435,89579,89724,89868,90013,90158,90304,
90450,90596,90742,90889,91036,91184,91332,91480,91628,91777,91926,92075,92225,92375,92525,92675,92826,92978,93129,93281,93434
,93586,93739,93892,94046,94200,94354,94509,94664,94819,94975,95131,95287,95444,95601,95758,95916,96074,96233,96391,96551,
96710,96870,97030,97191,97352,97513,97675,97837,98000,98163,98326,98489,98653,98818,98982,99148,99313,99479,99645,99812,99979
,100146,100314,100482,100651,100820,100990,101159,101330,101500,101671,101843,102015,102187,102360,102533,102706,102880,
103054,103229,103404,103580,103756,103933,104109,104287,104465,104643,104821,105000,105180,105360,105540,105721,105902,106084,
106266,106449,106632,106816,107000,107184,107369,107555,107741,107927,108114,108301,108489,108677,108866,109055,109245,109435,
109626,109817,110008,110200,110393,110586,110780,110974,111169,111364,111560,111756,111952,112150,112347,112546,112744,112944,
113143,113344,113545,113746,113948,114151,114354,114557,114761,114966,115171,115377,115583,115790,115998,116206,116414,116623,
116833,117044,117254,117466,117678,117891,118104,118318,118532,118747,118963,119179,119396,119613,119831,120050,120269,120489,
120709,120930,121152,121374,121597,121821,122045,122270,122496,122722,122949,123176,123404,123633,123863,124093,124324,124555,
124787,125020,125254,125488,125723,125959,126195,126432,126669,126908,127147,127387,127627,127869,128111,128353,128597,128841,
129086,129332,129578,129825,130073,130322,130571,130821,131072,131324,131576,131830,132084,132339,132594,132851,133108,133366,
133625,133884,134145,134406,134668,134931,135195,135459,135725,135991,136258,136526,136795,137065,137335,137607,137879,138152,
138426,138701,138977,139254,139532,139810,140090,140370,140651,140934,141217,141501,141786,142072,142359,142647,142936,143226,
143517,143808,144101,144395,144690,144986,145282,145580,145879,146179,146480,146782,147084,147388,147693,148000,148307,148615,
148924,149235,149546,149859,150172,150487,150803,151120,151438,151757,152077,152399,152722,153045,153370,153697,154024,154352,
154682,155013,155345,155678,156013,156349,156686,157024,157363,157704,158046,158389,158734,159079,159427,159775,160125,160476,
160828,161182,161537,161893,162251,162610,162970,163332,163695,164060,164426,164793,165162,165532,165904,166277,166651,167027,
167405,167784,168164,168546,168930,169315,169701,170089,170479,170870,171263,171657,172053,172451,172850,173251,173653,174057,
174463,174870,175279,175690,176102,176516,176932,177349,177769,178190,178612,179037,179463,179891,180321,180753,181186,181622,
182059,182498,182939,183382,183827,184274,184722,185173,185625,186080,186536,186995,187455,187918,188382,188849,189318,189789,
190261,190736,191213,191693,192174,192658,193143,193631,194122,194614,195109,195606,196105,196606,197110,197616,198125,198636,
199149,199664,200182,200703,201226,201751,202279,202809,203342,203878,204416,204956,205500,206045,206594,207145,207699,208255,
208815,209376,209941,210509,211079,211652,212228,212807,213389,213973,214561,215151,215745,216341,216941,217544,218149,218758,
219370,219985,220603,221225,221849,222477,223108,223743,224381,225022,225666,226314,226966,227621,228279,228941,229606,230275,
230948,231624,232304,232988,233676,234367,235062,235761,236463,237170,237881,238595,239314,240036,240763,241493,242228,242967,
243711,244458,245210,245966,246727,247492,248261,249035,249813,250596,251384,252176,252973,253774,254581,255392,256208,257029,
257855,258686,259522,260363,261209,262060,262917,263779,264646,265519,266397,267280,268169,269064,269965,270871,271782,272700,
273624,274553,275489,276430,277378,278332,279292,280258,281231,282210,283195,284188,285186,286192,287204,288223,289249,290282,
291322,292369,293423,294485,295554,296630,297714,298805,299904,301011,302126,303248,304379,305517,306664,307819,308983,310154,
311335,312524,313721,314928,316143,317368,318601,319844,321097,322358,323629,324910,326201,327502,328812,330133,331464,332805,
334157,335519,336892,338276,339671,341078,342495,343924,345364,346816,348280,349756,351244,352744,354257,355783,357321,358872,
360436,362013,363604,365208,366826,368459,370105,371765,373440,375130,376835,378555,380290,382040,383807,385589,387387,389202,
391034,392882,394747,396630,398530,400448,402384,404338,406311,408303,410314,412344,414395,416465,418555,420666,422798,424951,
427125,429321,431540,433781,436045,438332,440643,442978,445337,447720,450129,452564,455024,457511,460024,462565,465133,467730,
470355,473009,475692,478406,481150,483925,486732,489571,492443,495348,498287,501261,504269,507313,510394,513512,516667,519861,
523094,526366,529680,533034,536431,539870,543354,546881,550455,554074,557741,561456,565221,569035,572901,576818,580789,584815,
588896,593033,597229,601483,605798,610174,614613,619117,623686,628323,633028,637803,642651,647572,652568,657640,662792,668024,
673338,678737,684223,689797,695462,701219,707072,713023,719074,725227,731486,737853,744331,750922,757631,764460,771411,778490,
785699,793041,800521,808143,815910,823827,831898,840127,848520,857081,865817,874730,883829,893117,902602,912289,922186,932298,
942633,953199,964003,975054,986361,997931,1009774,1021901,1034322,1047046,1060087,1073455,1087164,1101225,1115654,1130465,
1145673,1161294,1177345,1193846,1210813,1228269,1246234,1264730,1283783,1303416,1323658,1344537,1366084,1388330,1411312,1435065
,1459630,1485049,1511367,1538632,1566898,1596220,1626658,1658278,1691149,1725348,1760956,1798063,1836758,1877161,1919378,
1963536,2009771,2058233,2109087,2162516,2218719,2277919,2340362,2406322,2476104,2550052,2628549,2712030,2800983,2895966,2997613
,3106651,3223918,3350381,3487165,3635590,3797206,3973855,4167737,4381502,4618375,4882318,5178251,5512368,5892567,6329090,
6835455,7429880,8137527,8994149,10052327,11392683,13145455,15535599,18988036,24413316,34178904,56965752,170910304};private
static int[]ம={25,75,125,175,226,276,326,376,427,477,527,578,628,678,728,779,829,879,929,980,1030,1080,1130,1181,1231,1281,
1331,1382,1432,1482,1532,1583,1633,1683,1733,1784,1834,1884,1934,1985,2035,2085,2135,2186,2236,2286,2336,2387,2437,2487,2537
,2587,2638,2688,2738,2788,2839,2889,2939,2989,3039,3090,3140,3190,3240,3291,3341,3391,3441,3491,3541,3592,3642,3692,3742,
3792,3843,3893,3943,3993,4043,4093,4144,4194,4244,4294,4344,4394,4445,4495,4545,4595,4645,4695,4745,4796,4846,4896,4946,4996
,5046,5096,5146,5197,5247,5297,5347,5397,5447,5497,5547,5597,5647,5697,5748,5798,5848,5898,5948,5998,6048,6098,6148,6198,
6248,6298,6348,6398,6448,6498,6548,6598,6648,6698,6748,6798,6848,6898,6948,6998,7048,7098,7148,7198,7248,7298,7348,7398,7448
,7498,7548,7598,7648,7697,7747,7797,7847,7897,7947,7997,8047,8097,8147,8196,8246,8296,8346,8396,8446,8496,8545,8595,8645,
8695,8745,8794,8844,8894,8944,8994,9043,9093,9143,9193,9243,9292,9342,9392,9442,9491,9541,9591,9640,9690,9740,9790,9839,9889
,9939,9988,10038,10088,10137,10187,10237,10286,10336,10386,10435,10485,10534,10584,10634,10683,10733,10782,10832,10882,
10931,10981,11030,11080,11129,11179,11228,11278,11327,11377,11426,11476,11525,11575,11624,11674,11723,11773,11822,11872,11921
,11970,12020,12069,12119,12168,12218,12267,12316,12366,12415,12464,12514,12563,12612,12662,12711,12760,12810,12859,12908,
12957,13007,13056,13105,13154,13204,13253,13302,13351,13401,13450,13499,13548,13597,13647,13696,13745,13794,13843,13892,13941
,13990,14040,14089,14138,14187,14236,14285,14334,14383,14432,14481,14530,14579,14628,14677,14726,14775,14824,14873,14922,
14971,15020,15069,15118,15167,15215,15264,15313,15362,15411,15460,15509,15557,15606,15655,15704,15753,15802,15850,15899,15948
,15997,16045,16094,16143,16191,16240,16289,16338,16386,16435,16484,16532,16581,16629,16678,16727,16775,16824,16872,16921,
16970,17018,17067,17115,17164,17212,17261,17309,17358,17406,17455,17503,17551,17600,17648,17697,17745,17793,17842,17890,17939
,17987,18035,18084,18132,18180,18228,18277,18325,18373,18421,18470,18518,18566,18614,18663,18711,18759,18807,18855,18903,
18951,19000,19048,19096,19144,19192,19240,19288,19336,19384,19432,19480,19528,19576,19624,19672,19720,19768,19816,19864,19912
,19959,20007,20055,20103,20151,20199,20246,20294,20342,20390,20438,20485,20533,20581,20629,20676,20724,20772,20819,20867,
20915,20962,21010,21057,21105,21153,21200,21248,21295,21343,21390,21438,21485,21533,21580,21628,21675,21723,21770,21817,21865
,21912,21960,22007,22054,22102,22149,22196,22243,22291,22338,22385,22433,22480,22527,22574,22621,22668,22716,22763,22810,
22857,22904,22951,22998,23045,23092,23139,23186,23233,23280,23327,23374,23421,23468,23515,23562,23609,23656,23703,23750,23796
,23843,23890,23937,23984,24030,24077,24124,24171,24217,24264,24311,24357,24404,24451,24497,24544,24591,24637,24684,24730,
24777,24823,24870,24916,24963,25009,25056,25102,25149,25195,25241,25288,25334,25381,25427,25473,25520,25566,25612,25658,25705
,25751,25797,25843,25889,25936,25982,26028,26074,26120,26166,26212,26258,26304,26350,26396,26442,26488,26534,26580,26626,
26672,26718,26764,26810,26856,26902,26947,26993,27039,27085,27131,27176,27222,27268,27313,27359,27405,27450,27496,27542,27587
,27633,27678,27724,27770,27815,27861,27906,27952,27997,28042,28088,28133,28179,28224,28269,28315,28360,28405,28451,28496,
28541,28586,28632,28677,28722,28767,28812,28858,28903,28948,28993,29038,29083,29128,29173,29218,29263,29308,29353,29398,29443
,29488,29533,29577,29622,29667,29712,29757,29801,29846,29891,29936,29980,30025,30070,30114,30159,30204,30248,30293,30337,
30382,30426,30471,30515,30560,30604,30649,30693,30738,30782,30826,30871,30915,30959,31004,31048,31092,31136,31181,31225,31269
,31313,31357,31402,31446,31490,31534,31578,31622,31666,31710,31754,31798,31842,31886,31930,31974,32017,32061,32105,32149,
32193,32236,32280,32324,32368,32411,32455,32499,32542,32586,32630,32673,32717,32760,32804,32847,32891,32934,32978,33021,33065
,33108,33151,33195,33238,33281,33325,33368,33411,33454,33498,33541,33584,33627,33670,33713,33756,33799,33843,33886,33929,
33972,34015,34057,34100,34143,34186,34229,34272,34315,34358,34400,34443,34486,34529,34571,34614,34657,34699,34742,34785,34827
,34870,34912,34955,34997,35040,35082,35125,35167,35210,35252,35294,35337,35379,35421,35464,35506,35548,35590,35633,35675,
35717,35759,35801,35843,35885,35927,35969,36011,36053,36095,36137,36179,36221,36263,36305,36347,36388,36430,36472,36514,36555
,36597,36639,36681,36722,36764,36805,36847,36889,36930,36972,37013,37055,37096,37137,37179,37220,37262,37303,37344,37386,
37427,37468,37509,37551,37592,37633,37674,37715,37756,37797,37838,37879,37920,37961,38002,38043,38084,38125,38166,38207,38248
,38288,38329,38370,38411,38451,38492,38533,38573,38614,38655,38695,38736,38776,38817,38857,38898,38938,38979,39019,39059,
39100,39140,39180,39221,39261,39301,39341,39382,39422,39462,39502,39542,39582,39622,39662,39702,39742,39782,39822,39862,39902
,39942,39982,40021,40061,40101,40141,40180,40220,40260,40300,40339,40379,40418,40458,40497,40537,40576,40616,40655,40695,
40734,40773,40813,40852,40891,40931,40970,41009,41048,41087,41127,41166,41205,41244,41283,41322,41361,41400,41439,41478,41517
,41556,41595,41633,41672,41711,41750,41788,41827,41866,41904,41943,41982,42020,42059,42097,42136,42174,42213,42251,42290,
42328,42366,42405,42443,42481,42520,42558,42596,42634,42672,42711,42749,42787,42825,42863,42901,42939,42977,43015,43053,43091
,43128,43166,43204,43242,43280,43317,43355,43393,43430,43468,43506,43543,43581,43618,43656,43693,43731,43768,43806,43843,
43880,43918,43955,43992,44029,44067,44104,44141,44178,44215,44252,44289,44326,44363,44400,44437,44474,44511,44548,44585,44622
,44659,44695,44732,44769,44806,44842,44879,44915,44952,44989,45025,45062,45098,45135,45171,45207,45244,45280,45316,45353,
45389,45425,45462,45498,45534,45570,45606,45642,45678,45714,45750,45786,45822,45858,45894,45930,45966,46002,46037,46073,46109
,46145,46180,46216,46252,46287,46323,46358,46394,46429,46465,46500,46536,46571,46606,46642,46677,46712,46747,46783,46818,
46853,46888,46923,46958,46993,47028,47063,47098,47133,47168,47203,47238,47273,47308,47342,47377,47412,47446,47481,47516,47550
,47585,47619,47654,47688,47723,47757,47792,47826,47860,47895,47929,47963,47998,48032,48066,48100,48134,48168,48202,48237,
48271,48305,48338,48372,48406,48440,48474,48508,48542,48575,48609,48643,48676,48710,48744,48777,48811,48844,48878,48911,48945
,48978,49012,49045,49078,49112,49145,49178,49211,49244,49278,49311,49344,49377,49410,49443,49476,49509,49542,49575,49608,
49640,49673,49706,49739,49771,49804,49837,49869,49902,49935,49967,50000,50032,50065,50097,50129,50162,50194,50226,50259,50291
,50323,50355,50387,50420,50452,50484,50516,50548,50580,50612,50644,50675,50707,50739,50771,50803,50834,50866,50898,50929,
50961,50993,51024,51056,51087,51119,51150,51182,51213,51244,51276,51307,51338,51369,51401,51432,51463,51494,51525,51556,51587
,51618,51649,51680,51711,51742,51773,51803,51834,51865,51896,51926,51957,51988,52018,52049,52079,52110,52140,52171,52201,
52231,52262,52292,52322,52353,52383,52413,52443,52473,52503,52534,52564,52594,52624,52653,52683,52713,52743,52773,52803,52832
,52862,52892,52922,52951,52981,53010,53040,53069,53099,53128,53158,53187,53216,53246,53275,53304,53334,53363,53392,53421,
53450,53479,53508,53537,53566,53595,53624,53653,53682,53711,53739,53768,53797,53826,53854,53883,53911,53940,53969,53997,54026
,54054,54082,54111,54139,54167,54196,54224,54252,54280,54308,54337,54365,54393,54421,54449,54477,54505,54533,54560,54588,
54616,54644,54672,54699,54727,54755,54782,54810,54837,54865,54892,54920,54947,54974,55002,55029,55056,55084,55111,55138,55165
,55192,55219,55246,55274,55300,55327,55354,55381,55408,55435,55462,55489,55515,55542,55569,55595,55622,55648,55675,55701,
55728,55754,55781,55807,55833,55860,55886,55912,55938,55965,55991,56017,56043,56069,56095,56121,56147,56173,56199,56225,56250
,56276,56302,56328,56353,56379,56404,56430,56456,56481,56507,56532,56557,56583,56608,56633,56659,56684,56709,56734,56760,
56785,56810,56835,56860,56885,56910,56935,56959,56984,57009,57034,57059,57083,57108,57133,57157,57182,57206,57231,57255,57280
,57304,57329,57353,57377,57402,57426,57450,57474,57498,57522,57546,57570,57594,57618,57642,57666,57690,57714,57738,57762,
57785,57809,57833,57856,57880,57903,57927,57950,57974,57997,58021,58044,58067,58091,58114,58137,58160,58183,58207,58230,58253
,58276,58299,58322,58345,58367,58390,58413,58436,58459,58481,58504,58527,58549,58572,58594,58617,58639,58662,58684,58706,
58729,58751,58773,58795,58818,58840,58862,58884,58906,58928,58950,58972,58994,59016,59038,59059,59081,59103,59125,59146,59168
,59190,59211,59233,59254,59276,59297,59318,59340,59361,59382,59404,59425,59446,59467,59488,59509,59530,59551,59572,59593,
59614,59635,59656,59677,59697,59718,59739,59759,59780,59801,59821,59842,59862,59883,59903,59923,59944,59964,59984,60004,60025
,60045,60065,60085,60105,60125,60145,60165,60185,60205,60225,60244,60264,60284,60304,60323,60343,60363,60382,60402,60421,
60441,60460,60479,60499,60518,60537,60556,60576,60595,60614,60633,60652,60671,60690,60709,60728,60747,60766,60785,60803,60822
,60841,60859,60878,60897,60915,60934,60952,60971,60989,61007,61026,61044,61062,61081,61099,61117,61135,61153,61171,61189,
61207,61225,61243,61261,61279,61297,61314,61332,61350,61367,61385,61403,61420,61438,61455,61473,61490,61507,61525,61542,61559
,61577,61594,61611,61628,61645,61662,61679,61696,61713,61730,61747,61764,61780,61797,61814,61831,61847,61864,61880,61897,
61913,61930,61946,61963,61979,61995,62012,62028,62044,62060,62076,62092,62108,62125,62141,62156,62172,62188,62204,62220,62236
,62251,62267,62283,62298,62314,62329,62345,62360,62376,62391,62407,62422,62437,62453,62468,62483,62498,62513,62528,62543,
62558,62573,62588,62603,62618,62633,62648,62662,62677,62692,62706,62721,62735,62750,62764,62779,62793,62808,62822,62836,62850
,62865,62879,62893,62907,62921,62935,62949,62963,62977,62991,63005,63019,63032,63046,63060,63074,63087,63101,63114,63128,
63141,63155,63168,63182,63195,63208,63221,63235,63248,63261,63274,63287,63300,63313,63326,63339,63352,63365,63378,63390,63403
,63416,63429,63441,63454,63466,63479,63491,63504,63516,63528,63541,63553,63565,63578,63590,63602,63614,63626,63638,63650,
63662,63674,63686,63698,63709,63721,63733,63745,63756,63768,63779,63791,63803,63814,63825,63837,63848,63859,63871,63882,63893
,63904,63915,63927,63938,63949,63960,63971,63981,63992,64003,64014,64025,64035,64046,64057,64067,64078,64088,64099,64109,
64120,64130,64140,64151,64161,64171,64181,64192,64202,64212,64222,64232,64242,64252,64261,64271,64281,64291,64301,64310,64320
,64330,64339,64349,64358,64368,64377,64387,64396,64405,64414,64424,64433,64442,64451,64460,64469,64478,64487,64496,64505,
64514,64523,64532,64540,64549,64558,64566,64575,64584,64592,64601,64609,64617,64626,64634,64642,64651,64659,64667,64675,64683
,64691,64699,64707,64715,64723,64731,64739,64747,64754,64762,64770,64777,64785,64793,64800,64808,64815,64822,64830,64837,
64844,64852,64859,64866,64873,64880,64887,64895,64902,64908,64915,64922,64929,64936,64943,64949,64956,64963,64969,64976,64982
,64989,64995,65002,65008,65015,65021,65027,65033,65040,65046,65052,65058,65064,65070,65076,65082,65088,65094,65099,65105,
65111,65117,65122,65128,65133,65139,65144,65150,65155,65161,65166,65171,65177,65182,65187,65192,65197,65202,65207,65212,65217
,65222,65227,65232,65237,65242,65246,65251,65256,65260,65265,65270,65274,65279,65283,65287,65292,65296,65300,65305,65309,
65313,65317,65321,65325,65329,65333,65337,65341,65345,65349,65352,65356,65360,65363,65367,65371,65374,65378,65381,65385,65388
,65391,65395,65398,65401,65404,65408,65411,65414,65417,65420,65423,65426,65429,65431,65434,65437,65440,65442,65445,65448,
65450,65453,65455,65458,65460,65463,65465,65467,65470,65472,65474,65476,65478,65480,65482,65484,65486,65488,65490,65492,65494
,65496,65497,65499,65501,65502,65504,65505,65507,65508,65510,65511,65513,65514,65515,65516,65518,65519,65520,65521,65522,
65523,65524,65525,65526,65527,65527,65528,65529,65530,65530,65531,65531,65532,65532,65533,65533,65534,65534,65534,65535,65535
,65535,65535,65535,65535,65535,65535,65535,65535,65535,65535,65535,65535,65534,65534,65534,65533,65533,65532,65532,65531,
65531,65530,65530,65529,65528,65527,65527,65526,65525,65524,65523,65522,65521,65520,65519,65518,65516,65515,65514,65513,65511
,65510,65508,65507,65505,65504,65502,65501,65499,65497,65496,65494,65492,65490,65488,65486,65484,65482,65480,65478,65476,
65474,65472,65470,65467,65465,65463,65460,65458,65455,65453,65450,65448,65445,65442,65440,65437,65434,65431,65429,65426,65423
,65420,65417,65414,65411,65408,65404,65401,65398,65395,65391,65388,65385,65381,65378,65374,65371,65367,65363,65360,65356,
65352,65349,65345,65341,65337,65333,65329,65325,65321,65317,65313,65309,65305,65300,65296,65292,65287,65283,65279,65274,65270
,65265,65260,65256,65251,65246,65242,65237,65232,65227,65222,65217,65212,65207,65202,65197,65192,65187,65182,65177,65171,
65166,65161,65155,65150,65144,65139,65133,65128,65122,65117,65111,65105,65099,65094,65088,65082,65076,65070,65064,65058,65052
,65046,65040,65033,65027,65021,65015,65008,65002,64995,64989,64982,64976,64969,64963,64956,64949,64943,64936,64929,64922,
64915,64908,64902,64895,64887,64880,64873,64866,64859,64852,64844,64837,64830,64822,64815,64808,64800,64793,64785,64777,64770
,64762,64754,64747,64739,64731,64723,64715,64707,64699,64691,64683,64675,64667,64659,64651,64642,64634,64626,64617,64609,
64600,64592,64584,64575,64566,64558,64549,64540,64532,64523,64514,64505,64496,64487,64478,64469,64460,64451,64442,64433,64424
,64414,64405,64396,64387,64377,64368,64358,64349,64339,64330,64320,64310,64301,64291,64281,64271,64261,64252,64242,64232,
64222,64212,64202,64192,64181,64171,64161,64151,64140,64130,64120,64109,64099,64088,64078,64067,64057,64046,64035,64025,64014
,64003,63992,63981,63971,63960,63949,63938,63927,63915,63904,63893,63882,63871,63859,63848,63837,63825,63814,63803,63791,
63779,63768,63756,63745,63733,63721,63709,63698,63686,63674,63662,63650,63638,63626,63614,63602,63590,63578,63565,63553,63541
,63528,63516,63504,63491,63479,63466,63454,63441,63429,63416,63403,63390,63378,63365,63352,63339,63326,63313,63300,63287,
63274,63261,63248,63235,63221,63208,63195,63182,63168,63155,63141,63128,63114,63101,63087,63074,63060,63046,63032,63019,63005
,62991,62977,62963,62949,62935,62921,62907,62893,62879,62865,62850,62836,62822,62808,62793,62779,62764,62750,62735,62721,
62706,62692,62677,62662,62648,62633,62618,62603,62588,62573,62558,62543,62528,62513,62498,62483,62468,62453,62437,62422,62407
,62391,62376,62360,62345,62329,62314,62298,62283,62267,62251,62236,62220,62204,62188,62172,62156,62141,62125,62108,62092,
62076,62060,62044,62028,62012,61995,61979,61963,61946,61930,61913,61897,61880,61864,61847,61831,61814,61797,61780,61764,61747
,61730,61713,61696,61679,61662,61645,61628,61611,61594,61577,61559,61542,61525,61507,61490,61473,61455,61438,61420,61403,
61385,61367,61350,61332,61314,61297,61279,61261,61243,61225,61207,61189,61171,61153,61135,61117,61099,61081,61062,61044,61026
,61007,60989,60971,60952,60934,60915,60897,60878,60859,60841,60822,60803,60785,60766,60747,60728,60709,60690,60671,60652,
60633,60614,60595,60576,60556,60537,60518,60499,60479,60460,60441,60421,60402,60382,60363,60343,60323,60304,60284,60264,60244
,60225,60205,60185,60165,60145,60125,60105,60085,60065,60045,60025,60004,59984,59964,59944,59923,59903,59883,59862,59842,
59821,59801,59780,59759,59739,59718,59697,59677,59656,59635,59614,59593,59572,59551,59530,59509,59488,59467,59446,59425,59404
,59382,59361,59340,59318,59297,59276,59254,59233,59211,59190,59168,59146,59125,59103,59081,59059,59038,59016,58994,58972,
58950,58928,58906,58884,58862,58840,58818,58795,58773,58751,58729,58706,58684,58662,58639,58617,58594,58572,58549,58527,58504
,58481,58459,58436,58413,58390,58367,58345,58322,58299,58276,58253,58230,58207,58183,58160,58137,58114,58091,58067,58044,
58021,57997,57974,57950,57927,57903,57880,57856,57833,57809,57785,57762,57738,57714,57690,57666,57642,57618,57594,57570,57546
,57522,57498,57474,57450,57426,57402,57377,57353,57329,57304,57280,57255,57231,57206,57182,57157,57133,57108,57083,57059,
57034,57009,56984,56959,56935,56910,56885,56860,56835,56810,56785,56760,56734,56709,56684,56659,56633,56608,56583,56557,56532
,56507,56481,56456,56430,56404,56379,56353,56328,56302,56276,56250,56225,56199,56173,56147,56121,56095,56069,56043,56017,
55991,55965,55938,55912,55886,55860,55833,55807,55781,55754,55728,55701,55675,55648,55622,55595,55569,55542,55515,55489,55462
,55435,55408,55381,55354,55327,55300,55274,55246,55219,55192,55165,55138,55111,55084,55056,55029,55002,54974,54947,54920,
54892,54865,54837,54810,54782,54755,54727,54699,54672,54644,54616,54588,54560,54533,54505,54477,54449,54421,54393,54365,54337
,54308,54280,54252,54224,54196,54167,54139,54111,54082,54054,54026,53997,53969,53940,53911,53883,53854,53826,53797,53768,
53739,53711,53682,53653,53624,53595,53566,53537,53508,53479,53450,53421,53392,53363,53334,53304,53275,53246,53216,53187,53158
,53128,53099,53069,53040,53010,52981,52951,52922,52892,52862,52832,52803,52773,52743,52713,52683,52653,52624,52594,52564,
52534,52503,52473,52443,52413,52383,52353,52322,52292,52262,52231,52201,52171,52140,52110,52079,52049,52018,51988,51957,51926
,51896,51865,51834,51803,51773,51742,51711,51680,51649,51618,51587,51556,51525,51494,51463,51432,51401,51369,51338,51307,
51276,51244,51213,51182,51150,51119,51087,51056,51024,50993,50961,50929,50898,50866,50834,50803,50771,50739,50707,50675,50644
,50612,50580,50548,50516,50484,50452,50420,50387,50355,50323,50291,50259,50226,50194,50162,50129,50097,50065,50032,50000,
49967,49935,49902,49869,49837,49804,49771,49739,49706,49673,49640,49608,49575,49542,49509,49476,49443,49410,49377,49344,49311
,49278,49244,49211,49178,49145,49112,49078,49045,49012,48978,48945,48911,48878,48844,48811,48777,48744,48710,48676,48643,
48609,48575,48542,48508,48474,48440,48406,48372,48338,48304,48271,48237,48202,48168,48134,48100,48066,48032,47998,47963,47929
,47895,47860,47826,47792,47757,47723,47688,47654,47619,47585,47550,47516,47481,47446,47412,47377,47342,47308,47273,47238,
47203,47168,47133,47098,47063,47028,46993,46958,46923,46888,46853,46818,46783,46747,46712,46677,46642,46606,46571,46536,46500
,46465,46429,46394,46358,46323,46287,46252,46216,46180,46145,46109,46073,46037,46002,45966,45930,45894,45858,45822,45786,
45750,45714,45678,45642,45606,45570,45534,45498,45462,45425,45389,45353,45316,45280,45244,45207,45171,45135,45098,45062,45025
,44989,44952,44915,44879,44842,44806,44769,44732,44695,44659,44622,44585,44548,44511,44474,44437,44400,44363,44326,44289,
44252,44215,44178,44141,44104,44067,44029,43992,43955,43918,43880,43843,43806,43768,43731,43693,43656,43618,43581,43543,43506
,43468,43430,43393,43355,43317,43280,43242,43204,43166,43128,43091,43053,43015,42977,42939,42901,42863,42825,42787,42749,
42711,42672,42634,42596,42558,42520,42481,42443,42405,42366,42328,42290,42251,42213,42174,42136,42097,42059,42020,41982,41943
,41904,41866,41827,41788,41750,41711,41672,41633,41595,41556,41517,41478,41439,41400,41361,41322,41283,41244,41205,41166,
41127,41088,41048,41009,40970,40931,40891,40852,40813,40773,40734,40695,40655,40616,40576,40537,40497,40458,40418,40379,40339
,40300,40260,40220,40180,40141,40101,40061,40021,39982,39942,39902,39862,39822,39782,39742,39702,39662,39622,39582,39542,
39502,39462,39422,39382,39341,39301,39261,39221,39180,39140,39100,39059,39019,38979,38938,38898,38857,38817,38776,38736,38695
,38655,38614,38573,38533,38492,38451,38411,38370,38329,38288,38248,38207,38166,38125,38084,38043,38002,37961,37920,37879,
37838,37797,37756,37715,37674,37633,37592,37551,37509,37468,37427,37386,37344,37303,37262,37220,37179,37137,37096,37055,37013
,36972,36930,36889,36847,36805,36764,36722,36681,36639,36597,36556,36514,36472,36430,36388,36347,36305,36263,36221,36179,
36137,36095,36053,36011,35969,35927,35885,35843,35801,35759,35717,35675,35633,35590,35548,35506,35464,35421,35379,35337,35294
,35252,35210,35167,35125,35082,35040,34997,34955,34912,34870,34827,34785,34742,34699,34657,34614,34571,34529,34486,34443,
34400,34358,34315,34272,34229,34186,34143,34100,34057,34015,33972,33929,33886,33843,33799,33756,33713,33670,33627,33584,33541
,33498,33454,33411,33368,33325,33281,33238,33195,33151,33108,33065,33021,32978,32934,32891,32847,32804,32760,32717,32673,
32630,32586,32542,32499,32455,32411,32368,32324,32280,32236,32193,32149,32105,32061,32017,31974,31930,31886,31842,31798,31754
,31710,31666,31622,31578,31534,31490,31446,31402,31357,31313,31269,31225,31181,31136,31092,31048,31004,30959,30915,30871,
30826,30782,30738,30693,30649,30604,30560,30515,30471,30426,30382,30337,30293,30248,30204,30159,30114,30070,30025,29980,29936
,29891,29846,29801,29757,29712,29667,29622,29577,29533,29488,29443,29398,29353,29308,29263,29218,29173,29128,29083,29038,
28993,28948,28903,28858,28812,28767,28722,28677,28632,28586,28541,28496,28451,28405,28360,28315,28269,28224,28179,28133,28088
,28042,27997,27952,27906,27861,27815,27770,27724,27678,27633,27587,27542,27496,27450,27405,27359,27313,27268,27222,27176,
27131,27085,27039,26993,26947,26902,26856,26810,26764,26718,26672,26626,26580,26534,26488,26442,26396,26350,26304,26258,26212
,26166,26120,26074,26028,25982,25936,25889,25843,25797,25751,25705,25658,25612,25566,25520,25473,25427,25381,25334,25288,
25241,25195,25149,25102,25056,25009,24963,24916,24870,24823,24777,24730,24684,24637,24591,24544,24497,24451,24404,24357,24311
,24264,24217,24171,24124,24077,24030,23984,23937,23890,23843,23796,23750,23703,23656,23609,23562,23515,23468,23421,23374,
23327,23280,23233,23186,23139,23092,23045,22998,22951,22904,22857,22810,22763,22716,22668,22621,22574,22527,22480,22433,22385
,22338,22291,22243,22196,22149,22102,22054,22007,21960,21912,21865,21817,21770,21723,21675,21628,21580,21533,21485,21438,
21390,21343,21295,21248,21200,21153,21105,21057,21010,20962,20915,20867,20819,20772,20724,20676,20629,20581,20533,20485,20438
,20390,20342,20294,20246,20199,20151,20103,20055,20007,19959,19912,19864,19816,19768,19720,19672,19624,19576,19528,19480,
19432,19384,19336,19288,19240,19192,19144,19096,19048,19000,18951,18903,18855,18807,18759,18711,18663,18614,18566,18518,18470
,18421,18373,18325,18277,18228,18180,18132,18084,18035,17987,17939,17890,17842,17793,17745,17697,17648,17600,17551,17503,
17455,17406,17358,17309,17261,17212,17164,17115,17067,17018,16970,16921,16872,16824,16775,16727,16678,16629,16581,16532,16484
,16435,16386,16338,16289,16240,16191,16143,16094,16045,15997,15948,15899,15850,15802,15753,15704,15655,15606,15557,15509,
15460,15411,15362,15313,15264,15215,15167,15118,15069,15020,14971,14922,14873,14824,14775,14726,14677,14628,14579,14530,14481
,14432,14383,14334,14285,14236,14187,14138,14089,14040,13990,13941,13892,13843,13794,13745,13696,13646,13597,13548,13499,
13450,13401,13351,13302,13253,13204,13154,13105,13056,13007,12957,12908,12859,12810,12760,12711,12662,12612,12563,12514,12464
,12415,12366,12316,12267,12218,12168,12119,12069,12020,11970,11921,11872,11822,11773,11723,11674,11624,11575,11525,11476,
11426,11377,11327,11278,11228,11179,11129,11080,11030,10981,10931,10882,10832,10782,10733,10683,10634,10584,10534,10485,10435
,10386,10336,10286,10237,10187,10137,10088,10038,9988,9939,9889,9839,9790,9740,9690,9640,9591,9541,9491,9442,9392,9342,
9292,9243,9193,9143,9093,9043,8994,8944,8894,8844,8794,8745,8695,8645,8595,8545,8496,8446,8396,8346,8296,8246,8196,8147,8097
,8047,7997,7947,7897,7847,7797,7747,7697,7648,7598,7548,7498,7448,7398,7348,7298,7248,7198,7148,7098,7048,6998,6948,6898,
6848,6798,6748,6698,6648,6598,6548,6498,6448,6398,6348,6298,6248,6198,6148,6098,6048,5998,5948,5898,5848,5798,5748,5697,5647
,5597,5547,5497,5447,5397,5347,5297,5247,5197,5146,5096,5046,4996,4946,4896,4846,4796,4745,4695,4645,4595,4545,4495,4445,
4394,4344,4294,4244,4194,4144,4093,4043,3993,3943,3893,3843,3792,3742,3692,3642,3592,3541,3491,3441,3391,3341,3291,3240,3190
,3140,3090,3039,2989,2939,2889,2839,2788,2738,2688,2638,2587,2537,2487,2437,2387,2336,2286,2236,2186,2135,2085,2035,1985,
1934,1884,1834,1784,1733,1683,1633,1583,1532,1482,1432,1382,1331,1281,1231,1181,1130,1080,1030,980,929,879,829,779,728,678,
628,578,527,477,427,376,326,276,226,175,125,75,25,-25,-75,-125,-175,-226,-276,-326,-376,-427,-477,-527,-578,-628,-678,-728,
-779,-829,-879,-929,-980,-1030,-1080,-1130,-1181,-1231,-1281,-1331,-1382,-1432,-1482,-1532,-1583,-1633,-1683,-1733,-1784,
-1834,-1884,-1934,-1985,-2035,-2085,-2135,-2186,-2236,-2286,-2336,-2387,-2437,-2487,-2537,-2588,-2638,-2688,-2738,-2788,-
2839,-2889,-2939,-2989,-3039,-3090,-3140,-3190,-3240,-3291,-3341,-3391,-3441,-3491,-3541,-3592,-3642,-3692,-3742,-3792,-3843
,-3893,-3943,-3993,-4043,-4093,-4144,-4194,-4244,-4294,-4344,-4394,-4445,-4495,-4545,-4595,-4645,-4695,-4745,-4796,-4846,
-4896,-4946,-4996,-5046,-5096,-5146,-5197,-5247,-5297,-5347,-5397,-5447,-5497,-5547,-5597,-5647,-5697,-5748,-5798,-5848,-
5898,-5948,-5998,-6048,-6098,-6148,-6198,-6248,-6298,-6348,-6398,-6448,-6498,-6548,-6598,-6648,-6698,-6748,-6798,-6848,-6898
,-6948,-6998,-7048,-7098,-7148,-7198,-7248,-7298,-7348,-7398,-7448,-7498,-7548,-7598,-7648,-7697,-7747,-7797,-7847,-7897,
-7947,-7997,-8047,-8097,-8147,-8196,-8246,-8296,-8346,-8396,-8446,-8496,-8545,-8595,-8645,-8695,-8745,-8794,-8844,-8894,-
8944,-8994,-9043,-9093,-9143,-9193,-9243,-9292,-9342,-9392,-9442,-9491,-9541,-9591,-9640,-9690,-9740,-9790,-9839,-9889,-9939
,-9988,-10038,-10088,-10137,-10187,-10237,-10286,-10336,-10386,-10435,-10485,-10534,-10584,-10634,-10683,-10733,-10782,-
10832,-10882,-10931,-10981,-11030,-11080,-11129,-11179,-11228,-11278,-11327,-11377,-11426,-11476,-11525,-11575,-11624,-11674,
-11723,-11773,-11822,-11872,-11921,-11970,-12020,-12069,-12119,-12168,-12218,-12267,-12316,-12366,-12415,-12464,-12514,-
12563,-12612,-12662,-12711,-12760,-12810,-12859,-12908,-12957,-13007,-13056,-13105,-13154,-13204,-13253,-13302,-13351,-13401,
-13450,-13499,-13548,-13597,-13647,-13696,-13745,-13794,-13843,-13892,-13941,-13990,-14040,-14089,-14138,-14187,-14236,-
14285,-14334,-14383,-14432,-14481,-14530,-14579,-14628,-14677,-14726,-14775,-14824,-14873,-14922,-14971,-15020,-15069,-15118,
-15167,-15215,-15264,-15313,-15362,-15411,-15460,-15509,-15557,-15606,-15655,-15704,-15753,-15802,-15850,-15899,-15948,-
15997,-16045,-16094,-16143,-16191,-16240,-16289,-16338,-16386,-16435,-16484,-16532,-16581,-16629,-16678,-16727,-16775,-16824,
-16872,-16921,-16970,-17018,-17067,-17115,-17164,-17212,-17261,-17309,-17358,-17406,-17455,-17503,-17551,-17600,-17648,-
17697,-17745,-17793,-17842,-17890,-17939,-17987,-18035,-18084,-18132,-18180,-18228,-18277,-18325,-18373,-18421,-18470,-18518,
-18566,-18614,-18663,-18711,-18759,-18807,-18855,-18903,-18951,-19000,-19048,-19096,-19144,-19192,-19240,-19288,-19336,-
19384,-19432,-19480,-19528,-19576,-19624,-19672,-19720,-19768,-19816,-19864,-19912,-19959,-20007,-20055,-20103,-20151,-20199,
-20246,-20294,-20342,-20390,-20438,-20485,-20533,-20581,-20629,-20676,-20724,-20772,-20819,-20867,-20915,-20962,-21010,-
21057,-21105,-21153,-21200,-21248,-21295,-21343,-21390,-21438,-21485,-21533,-21580,-21628,-21675,-21723,-21770,-21817,-21865,
-21912,-21960,-22007,-22054,-22102,-22149,-22196,-22243,-22291,-22338,-22385,-22433,-22480,-22527,-22574,-22621,-22668,-
22716,-22763,-22810,-22857,-22904,-22951,-22998,-23045,-23092,-23139,-23186,-23233,-23280,-23327,-23374,-23421,-23468,-23515,
-23562,-23609,-23656,-23703,-23750,-23796,-23843,-23890,-23937,-23984,-24030,-24077,-24124,-24171,-24217,-24264,-24311,-
24357,-24404,-24451,-24497,-24544,-24591,-24637,-24684,-24730,-24777,-24823,-24870,-24916,-24963,-25009,-25056,-25102,-25149,
-25195,-25241,-25288,-25334,-25381,-25427,-25473,-25520,-25566,-25612,-25658,-25705,-25751,-25797,-25843,-25889,-25936,-
25982,-26028,-26074,-26120,-26166,-26212,-26258,-26304,-26350,-26396,-26442,-26488,-26534,-26580,-26626,-26672,-26718,-26764,
-26810,-26856,-26902,-26947,-26993,-27039,-27085,-27131,-27176,-27222,-27268,-27313,-27359,-27405,-27450,-27496,-27542,-
27587,-27633,-27678,-27724,-27770,-27815,-27861,-27906,-27952,-27997,-28042,-28088,-28133,-28179,-28224,-28269,-28315,-28360,
-28405,-28451,-28496,-28541,-28586,-28632,-28677,-28722,-28767,-28812,-28858,-28903,-28948,-28993,-29038,-29083,-29128,-
29173,-29218,-29263,-29308,-29353,-29398,-29443,-29488,-29533,-29577,-29622,-29667,-29712,-29757,-29801,-29846,-29891,-29936,
-29980,-30025,-30070,-30114,-30159,-30204,-30248,-30293,-30337,-30382,-30426,-30471,-30515,-30560,-30604,-30649,-30693,-
30738,-30782,-30826,-30871,-30915,-30959,-31004,-31048,-31092,-31136,-31181,-31225,-31269,-31313,-31357,-31402,-31446,-31490,
-31534,-31578,-31622,-31666,-31710,-31754,-31798,-31842,-31886,-31930,-31974,-32017,-32061,-32105,-32149,-32193,-32236,-
32280,-32324,-32368,-32411,-32455,-32499,-32542,-32586,-32630,-32673,-32717,-32760,-32804,-32847,-32891,-32934,-32978,-33021,
-33065,-33108,-33151,-33195,-33238,-33281,-33325,-33368,-33411,-33454,-33498,-33541,-33584,-33627,-33670,-33713,-33756,-
33799,-33843,-33886,-33929,-33972,-34015,-34057,-34100,-34143,-34186,-34229,-34272,-34315,-34358,-34400,-34443,-34486,-34529,
-34571,-34614,-34657,-34699,-34742,-34785,-34827,-34870,-34912,-34955,-34997,-35040,-35082,-35125,-35167,-35210,-35252,-
35294,-35337,-35379,-35421,-35464,-35506,-35548,-35590,-35633,-35675,-35717,-35759,-35801,-35843,-35885,-35927,-35969,-36011,
-36053,-36095,-36137,-36179,-36221,-36263,-36305,-36347,-36388,-36430,-36472,-36514,-36555,-36597,-36639,-36681,-36722,-
36764,-36805,-36847,-36889,-36930,-36972,-37013,-37055,-37096,-37137,-37179,-37220,-37262,-37303,-37344,-37386,-37427,-37468,
-37509,-37551,-37592,-37633,-37674,-37715,-37756,-37797,-37838,-37879,-37920,-37961,-38002,-38043,-38084,-38125,-38166,-
38207,-38248,-38288,-38329,-38370,-38411,-38451,-38492,-38533,-38573,-38614,-38655,-38695,-38736,-38776,-38817,-38857,-38898,
-38938,-38979,-39019,-39059,-39100,-39140,-39180,-39221,-39261,-39301,-39341,-39382,-39422,-39462,-39502,-39542,-39582,-
39622,-39662,-39702,-39742,-39782,-39822,-39862,-39902,-39942,-39982,-40021,-40061,-40101,-40141,-40180,-40220,-40260,-40299,
-40339,-40379,-40418,-40458,-40497,-40537,-40576,-40616,-40655,-40695,-40734,-40773,-40813,-40852,-40891,-40931,-40970,-
41009,-41048,-41087,-41127,-41166,-41205,-41244,-41283,-41322,-41361,-41400,-41439,-41478,-41517,-41556,-41595,-41633,-41672,
-41711,-41750,-41788,-41827,-41866,-41904,-41943,-41982,-42020,-42059,-42097,-42136,-42174,-42213,-42251,-42290,-42328,-
42366,-42405,-42443,-42481,-42520,-42558,-42596,-42634,-42672,-42711,-42749,-42787,-42825,-42863,-42901,-42939,-42977,-43015,
-43053,-43091,-43128,-43166,-43204,-43242,-43280,-43317,-43355,-43393,-43430,-43468,-43506,-43543,-43581,-43618,-43656,-
43693,-43731,-43768,-43806,-43843,-43880,-43918,-43955,-43992,-44029,-44067,-44104,-44141,-44178,-44215,-44252,-44289,-44326,
-44363,-44400,-44437,-44474,-44511,-44548,-44585,-44622,-44659,-44695,-44732,-44769,-44806,-44842,-44879,-44915,-44952,-
44989,-45025,-45062,-45098,-45135,-45171,-45207,-45244,-45280,-45316,-45353,-45389,-45425,-45462,-45498,-45534,-45570,-45606,
-45642,-45678,-45714,-45750,-45786,-45822,-45858,-45894,-45930,-45966,-46002,-46037,-46073,-46109,-46145,-46180,-46216,-
46252,-46287,-46323,-46358,-46394,-46429,-46465,-46500,-46536,-46571,-46606,-46642,-46677,-46712,-46747,-46783,-46818,-46853,
-46888,-46923,-46958,-46993,-47028,-47063,-47098,-47133,-47168,-47203,-47238,-47273,-47308,-47342,-47377,-47412,-47446,-
47481,-47516,-47550,-47585,-47619,-47654,-47688,-47723,-47757,-47792,-47826,-47860,-47895,-47929,-47963,-47998,-48032,-48066,
-48100,-48134,-48168,-48202,-48236,-48271,-48304,-48338,-48372,-48406,-48440,-48474,-48508,-48542,-48575,-48609,-48643,-
48676,-48710,-48744,-48777,-48811,-48844,-48878,-48911,-48945,-48978,-49012,-49045,-49078,-49112,-49145,-49178,-49211,-49244,
-49278,-49311,-49344,-49377,-49410,-49443,-49476,-49509,-49542,-49575,-49608,-49640,-49673,-49706,-49739,-49771,-49804,-
49837,-49869,-49902,-49935,-49967,-50000,-50032,-50065,-50097,-50129,-50162,-50194,-50226,-50259,-50291,-50323,-50355,-50387,
-50420,-50452,-50484,-50516,-50548,-50580,-50612,-50644,-50675,-50707,-50739,-50771,-50803,-50834,-50866,-50898,-50929,-
50961,-50993,-51024,-51056,-51087,-51119,-51150,-51182,-51213,-51244,-51276,-51307,-51338,-51369,-51401,-51432,-51463,-51494,
-51525,-51556,-51587,-51618,-51649,-51680,-51711,-51742,-51773,-51803,-51834,-51865,-51896,-51926,-51957,-51988,-52018,-
52049,-52079,-52110,-52140,-52171,-52201,-52231,-52262,-52292,-52322,-52353,-52383,-52413,-52443,-52473,-52503,-52534,-52564,
-52594,-52624,-52653,-52683,-52713,-52743,-52773,-52803,-52832,-52862,-52892,-52922,-52951,-52981,-53010,-53040,-53069,-
53099,-53128,-53158,-53187,-53216,-53246,-53275,-53304,-53334,-53363,-53392,-53421,-53450,-53479,-53508,-53537,-53566,-53595,
-53624,-53653,-53682,-53711,-53739,-53768,-53797,-53826,-53854,-53883,-53911,-53940,-53969,-53997,-54026,-54054,-54082,-
54111,-54139,-54167,-54196,-54224,-54252,-54280,-54308,-54337,-54365,-54393,-54421,-54449,-54477,-54505,-54533,-54560,-54588,
-54616,-54644,-54672,-54699,-54727,-54755,-54782,-54810,-54837,-54865,-54892,-54920,-54947,-54974,-55002,-55029,-55056,-
55084,-55111,-55138,-55165,-55192,-55219,-55246,-55274,-55300,-55327,-55354,-55381,-55408,-55435,-55462,-55489,-55515,-55542,
-55569,-55595,-55622,-55648,-55675,-55701,-55728,-55754,-55781,-55807,-55833,-55860,-55886,-55912,-55938,-55965,-55991,-
56017,-56043,-56069,-56095,-56121,-56147,-56173,-56199,-56225,-56250,-56276,-56302,-56328,-56353,-56379,-56404,-56430,-56456,
-56481,-56507,-56532,-56557,-56583,-56608,-56633,-56659,-56684,-56709,-56734,-56760,-56785,-56810,-56835,-56860,-56885,-
56910,-56935,-56959,-56984,-57009,-57034,-57059,-57083,-57108,-57133,-57157,-57182,-57206,-57231,-57255,-57280,-57304,-57329,
-57353,-57377,-57402,-57426,-57450,-57474,-57498,-57522,-57546,-57570,-57594,-57618,-57642,-57666,-57690,-57714,-57738,-
57762,-57785,-57809,-57833,-57856,-57880,-57903,-57927,-57950,-57974,-57997,-58021,-58044,-58067,-58091,-58114,-58137,-58160,
-58183,-58207,-58230,-58253,-58276,-58299,-58322,-58345,-58367,-58390,-58413,-58436,-58459,-58481,-58504,-58527,-58549,-
58572,-58594,-58617,-58639,-58662,-58684,-58706,-58729,-58751,-58773,-58795,-58818,-58840,-58862,-58884,-58906,-58928,-58950,
-58972,-58994,-59016,-59038,-59059,-59081,-59103,-59125,-59146,-59168,-59190,-59211,-59233,-59254,-59276,-59297,-59318,-
59340,-59361,-59382,-59404,-59425,-59446,-59467,-59488,-59509,-59530,-59551,-59572,-59593,-59614,-59635,-59656,-59677,-59697,
-59718,-59739,-59759,-59780,-59801,-59821,-59842,-59862,-59883,-59903,-59923,-59944,-59964,-59984,-60004,-60025,-60045,-
60065,-60085,-60105,-60125,-60145,-60165,-60185,-60205,-60225,-60244,-60264,-60284,-60304,-60323,-60343,-60363,-60382,-60402,
-60421,-60441,-60460,-60479,-60499,-60518,-60537,-60556,-60576,-60595,-60614,-60633,-60652,-60671,-60690,-60709,-60728,-
60747,-60766,-60785,-60803,-60822,-60841,-60859,-60878,-60897,-60915,-60934,-60952,-60971,-60989,-61007,-61026,-61044,-61062,
-61081,-61099,-61117,-61135,-61153,-61171,-61189,-61207,-61225,-61243,-61261,-61279,-61297,-61314,-61332,-61350,-61367,-
61385,-61403,-61420,-61438,-61455,-61473,-61490,-61507,-61525,-61542,-61559,-61577,-61594,-61611,-61628,-61645,-61662,-61679,
-61696,-61713,-61730,-61747,-61764,-61780,-61797,-61814,-61831,-61847,-61864,-61880,-61897,-61913,-61930,-61946,-61963,-
61979,-61995,-62012,-62028,-62044,-62060,-62076,-62092,-62108,-62125,-62141,-62156,-62172,-62188,-62204,-62220,-62236,-62251,
-62267,-62283,-62298,-62314,-62329,-62345,-62360,-62376,-62391,-62407,-62422,-62437,-62453,-62468,-62483,-62498,-62513,-
62528,-62543,-62558,-62573,-62588,-62603,-62618,-62633,-62648,-62662,-62677,-62692,-62706,-62721,-62735,-62750,-62764,-62779,
-62793,-62808,-62822,-62836,-62850,-62865,-62879,-62893,-62907,-62921,-62935,-62949,-62963,-62977,-62991,-63005,-63019,-
63032,-63046,-63060,-63074,-63087,-63101,-63114,-63128,-63141,-63155,-63168,-63182,-63195,-63208,-63221,-63235,-63248,-63261,
-63274,-63287,-63300,-63313,-63326,-63339,-63352,-63365,-63378,-63390,-63403,-63416,-63429,-63441,-63454,-63466,-63479,-
63491,-63504,-63516,-63528,-63541,-63553,-63565,-63578,-63590,-63602,-63614,-63626,-63638,-63650,-63662,-63674,-63686,-63698,
-63709,-63721,-63733,-63745,-63756,-63768,-63779,-63791,-63803,-63814,-63825,-63837,-63848,-63859,-63871,-63882,-63893,-
63904,-63915,-63927,-63938,-63949,-63960,-63971,-63981,-63992,-64003,-64014,-64025,-64035,-64046,-64057,-64067,-64078,-64088,
-64099,-64109,-64120,-64130,-64140,-64151,-64161,-64171,-64181,-64192,-64202,-64212,-64222,-64232,-64242,-64252,-64261,-
64271,-64281,-64291,-64301,-64310,-64320,-64330,-64339,-64349,-64358,-64368,-64377,-64387,-64396,-64405,-64414,-64424,-64433,
-64442,-64451,-64460,-64469,-64478,-64487,-64496,-64505,-64514,-64523,-64532,-64540,-64549,-64558,-64566,-64575,-64584,-
64592,-64601,-64609,-64617,-64626,-64634,-64642,-64651,-64659,-64667,-64675,-64683,-64691,-64699,-64707,-64715,-64723,-64731,
-64739,-64747,-64754,-64762,-64770,-64777,-64785,-64793,-64800,-64808,-64815,-64822,-64830,-64837,-64844,-64852,-64859,-
64866,-64873,-64880,-64887,-64895,-64902,-64908,-64915,-64922,-64929,-64936,-64943,-64949,-64956,-64963,-64969,-64976,-64982,
-64989,-64995,-65002,-65008,-65015,-65021,-65027,-65033,-65040,-65046,-65052,-65058,-65064,-65070,-65076,-65082,-65088,-
65094,-65099,-65105,-65111,-65117,-65122,-65128,-65133,-65139,-65144,-65150,-65155,-65161,-65166,-65171,-65177,-65182,-65187,
-65192,-65197,-65202,-65207,-65212,-65217,-65222,-65227,-65232,-65237,-65242,-65246,-65251,-65256,-65260,-65265,-65270,-
65274,-65279,-65283,-65287,-65292,-65296,-65300,-65305,-65309,-65313,-65317,-65321,-65325,-65329,-65333,-65337,-65341,-65345,
-65349,-65352,-65356,-65360,-65363,-65367,-65371,-65374,-65378,-65381,-65385,-65388,-65391,-65395,-65398,-65401,-65404,-
65408,-65411,-65414,-65417,-65420,-65423,-65426,-65429,-65431,-65434,-65437,-65440,-65442,-65445,-65448,-65450,-65453,-65455,
-65458,-65460,-65463,-65465,-65467,-65470,-65472,-65474,-65476,-65478,-65480,-65482,-65484,-65486,-65488,-65490,-65492,-
65494,-65496,-65497,-65499,-65501,-65502,-65504,-65505,-65507,-65508,-65510,-65511,-65513,-65514,-65515,-65516,-65518,-65519,
-65520,-65521,-65522,-65523,-65524,-65525,-65526,-65527,-65527,-65528,-65529,-65530,-65530,-65531,-65531,-65532,-65532,-
65533,-65533,-65534,-65534,-65534,-65535,-65535,-65535,-65535,-65535,-65535,-65535,-65535,-65535,-65535,-65535,-65535,-65535,
-65535,-65534,-65534,-65534,-65533,-65533,-65532,-65532,-65531,-65531,-65530,-65530,-65529,-65528,-65527,-65527,-65526,-
65525,-65524,-65523,-65522,-65521,-65520,-65519,-65518,-65516,-65515,-65514,-65513,-65511,-65510,-65508,-65507,-65505,-65504,
-65502,-65501,-65499,-65497,-65496,-65494,-65492,-65490,-65488,-65486,-65484,-65482,-65480,-65478,-65476,-65474,-65472,-
65470,-65467,-65465,-65463,-65460,-65458,-65455,-65453,-65450,-65448,-65445,-65442,-65440,-65437,-65434,-65431,-65429,-65426,
-65423,-65420,-65417,-65414,-65411,-65408,-65404,-65401,-65398,-65395,-65391,-65388,-65385,-65381,-65378,-65374,-65371,-
65367,-65363,-65360,-65356,-65352,-65349,-65345,-65341,-65337,-65333,-65329,-65325,-65321,-65317,-65313,-65309,-65305,-65300,
-65296,-65292,-65287,-65283,-65279,-65274,-65270,-65265,-65260,-65256,-65251,-65246,-65242,-65237,-65232,-65227,-65222,-
65217,-65212,-65207,-65202,-65197,-65192,-65187,-65182,-65177,-65171,-65166,-65161,-65155,-65150,-65144,-65139,-65133,-65128,
-65122,-65117,-65111,-65105,-65099,-65094,-65088,-65082,-65076,-65070,-65064,-65058,-65052,-65046,-65040,-65033,-65027,-
65021,-65015,-65008,-65002,-64995,-64989,-64982,-64976,-64969,-64963,-64956,-64949,-64943,-64936,-64929,-64922,-64915,-64908,
-64902,-64895,-64887,-64880,-64873,-64866,-64859,-64852,-64844,-64837,-64830,-64822,-64815,-64808,-64800,-64793,-64785,-
64777,-64770,-64762,-64754,-64747,-64739,-64731,-64723,-64715,-64707,-64699,-64691,-64683,-64675,-64667,-64659,-64651,-64642,
-64634,-64626,-64617,-64609,-64601,-64592,-64584,-64575,-64566,-64558,-64549,-64540,-64532,-64523,-64514,-64505,-64496,-
64487,-64478,-64469,-64460,-64451,-64442,-64433,-64424,-64414,-64405,-64396,-64387,-64377,-64368,-64358,-64349,-64339,-64330,
-64320,-64310,-64301,-64291,-64281,-64271,-64261,-64252,-64242,-64232,-64222,-64212,-64202,-64192,-64181,-64171,-64161,-
64151,-64140,-64130,-64120,-64109,-64099,-64088,-64078,-64067,-64057,-64046,-64035,-64025,-64014,-64003,-63992,-63981,-63971,
-63960,-63949,-63938,-63927,-63915,-63904,-63893,-63882,-63871,-63859,-63848,-63837,-63825,-63814,-63803,-63791,-63779,-
63768,-63756,-63745,-63733,-63721,-63709,-63698,-63686,-63674,-63662,-63650,-63638,-63626,-63614,-63602,-63590,-63578,-63565,
-63553,-63541,-63528,-63516,-63504,-63491,-63479,-63466,-63454,-63441,-63429,-63416,-63403,-63390,-63378,-63365,-63352,-
63339,-63326,-63313,-63300,-63287,-63274,-63261,-63248,-63235,-63221,-63208,-63195,-63182,-63168,-63155,-63141,-63128,-63114,
-63101,-63087,-63074,-63060,-63046,-63032,-63019,-63005,-62991,-62977,-62963,-62949,-62935,-62921,-62907,-62893,-62879,-
62865,-62850,-62836,-62822,-62808,-62793,-62779,-62764,-62750,-62735,-62721,-62706,-62692,-62677,-62662,-62648,-62633,-62618,
-62603,-62588,-62573,-62558,-62543,-62528,-62513,-62498,-62483,-62468,-62453,-62437,-62422,-62407,-62391,-62376,-62360,-
62345,-62329,-62314,-62298,-62283,-62267,-62251,-62236,-62220,-62204,-62188,-62172,-62156,-62141,-62125,-62108,-62092,-62076,
-62060,-62044,-62028,-62012,-61995,-61979,-61963,-61946,-61930,-61913,-61897,-61880,-61864,-61847,-61831,-61814,-61797,-
61780,-61764,-61747,-61730,-61713,-61696,-61679,-61662,-61645,-61628,-61611,-61594,-61577,-61559,-61542,-61525,-61507,-61490,
-61473,-61455,-61438,-61420,-61403,-61385,-61367,-61350,-61332,-61314,-61297,-61279,-61261,-61243,-61225,-61207,-61189,-
61171,-61153,-61135,-61117,-61099,-61081,-61062,-61044,-61026,-61007,-60989,-60971,-60952,-60934,-60915,-60897,-60878,-60859,
-60841,-60822,-60803,-60785,-60766,-60747,-60728,-60709,-60690,-60671,-60652,-60633,-60614,-60595,-60576,-60556,-60537,-
60518,-60499,-60479,-60460,-60441,-60421,-60402,-60382,-60363,-60343,-60323,-60304,-60284,-60264,-60244,-60225,-60205,-60185,
-60165,-60145,-60125,-60105,-60085,-60065,-60045,-60025,-60004,-59984,-59964,-59944,-59923,-59903,-59883,-59862,-59842,-
59821,-59801,-59780,-59759,-59739,-59718,-59697,-59677,-59656,-59635,-59614,-59593,-59572,-59551,-59530,-59509,-59488,-59467,
-59446,-59425,-59404,-59382,-59361,-59340,-59318,-59297,-59276,-59254,-59233,-59211,-59189,-59168,-59146,-59125,-59103,-
59081,-59059,-59038,-59016,-58994,-58972,-58950,-58928,-58906,-58884,-58862,-58840,-58818,-58795,-58773,-58751,-58729,-58706,
-58684,-58662,-58639,-58617,-58594,-58572,-58549,-58527,-58504,-58481,-58459,-58436,-58413,-58390,-58367,-58345,-58322,-
58299,-58276,-58253,-58230,-58207,-58183,-58160,-58137,-58114,-58091,-58067,-58044,-58021,-57997,-57974,-57950,-57927,-57903,
-57880,-57856,-57833,-57809,-57785,-57762,-57738,-57714,-57690,-57666,-57642,-57618,-57594,-57570,-57546,-57522,-57498,-
57474,-57450,-57426,-57402,-57377,-57353,-57329,-57304,-57280,-57255,-57231,-57206,-57182,-57157,-57133,-57108,-57083,-57059,
-57034,-57009,-56984,-56959,-56935,-56910,-56885,-56860,-56835,-56810,-56785,-56760,-56734,-56709,-56684,-56659,-56633,-
56608,-56583,-56557,-56532,-56507,-56481,-56456,-56430,-56404,-56379,-56353,-56328,-56302,-56276,-56250,-56225,-56199,-56173,
-56147,-56121,-56095,-56069,-56043,-56017,-55991,-55965,-55938,-55912,-55886,-55860,-55833,-55807,-55781,-55754,-55728,-
55701,-55675,-55648,-55622,-55595,-55569,-55542,-55515,-55489,-55462,-55435,-55408,-55381,-55354,-55327,-55300,-55274,-55246,
-55219,-55192,-55165,-55138,-55111,-55084,-55056,-55029,-55002,-54974,-54947,-54920,-54892,-54865,-54837,-54810,-54782,-
54755,-54727,-54699,-54672,-54644,-54616,-54588,-54560,-54533,-54505,-54477,-54449,-54421,-54393,-54365,-54337,-54308,-54280,
-54252,-54224,-54196,-54167,-54139,-54111,-54082,-54054,-54026,-53997,-53969,-53940,-53911,-53883,-53854,-53826,-53797,-
53768,-53739,-53711,-53682,-53653,-53624,-53595,-53566,-53537,-53508,-53479,-53450,-53421,-53392,-53363,-53334,-53304,-53275,
-53246,-53216,-53187,-53158,-53128,-53099,-53069,-53040,-53010,-52981,-52951,-52922,-52892,-52862,-52832,-52803,-52773,-
52743,-52713,-52683,-52653,-52624,-52594,-52564,-52534,-52503,-52473,-52443,-52413,-52383,-52353,-52322,-52292,-52262,-52231,
-52201,-52171,-52140,-52110,-52079,-52049,-52018,-51988,-51957,-51926,-51896,-51865,-51834,-51803,-51773,-51742,-51711,-
51680,-51649,-51618,-51587,-51556,-51525,-51494,-51463,-51432,-51401,-51369,-51338,-51307,-51276,-51244,-51213,-51182,-51150,
-51119,-51087,-51056,-51024,-50993,-50961,-50929,-50898,-50866,-50834,-50803,-50771,-50739,-50707,-50675,-50644,-50612,-
50580,-50548,-50516,-50484,-50452,-50420,-50387,-50355,-50323,-50291,-50259,-50226,-50194,-50162,-50129,-50097,-50065,-50032,
-50000,-49967,-49935,-49902,-49869,-49837,-49804,-49771,-49739,-49706,-49673,-49640,-49608,-49575,-49542,-49509,-49476,-
49443,-49410,-49377,-49344,-49311,-49278,-49244,-49211,-49178,-49145,-49112,-49078,-49045,-49012,-48978,-48945,-48911,-48878,
-48844,-48811,-48777,-48744,-48710,-48676,-48643,-48609,-48575,-48542,-48508,-48474,-48440,-48406,-48372,-48338,-48305,-
48271,-48237,-48202,-48168,-48134,-48100,-48066,-48032,-47998,-47963,-47929,-47895,-47860,-47826,-47792,-47757,-47723,-47688,
-47654,-47619,-47585,-47550,-47516,-47481,-47446,-47412,-47377,-47342,-47307,-47273,-47238,-47203,-47168,-47133,-47098,-
47063,-47028,-46993,-46958,-46923,-46888,-46853,-46818,-46783,-46747,-46712,-46677,-46642,-46606,-46571,-46536,-46500,-46465,
-46429,-46394,-46358,-46323,-46287,-46251,-46216,-46180,-46145,-46109,-46073,-46037,-46002,-45966,-45930,-45894,-45858,-
45822,-45786,-45750,-45714,-45678,-45642,-45606,-45570,-45534,-45498,-45462,-45425,-45389,-45353,-45316,-45280,-45244,-45207,
-45171,-45135,-45098,-45062,-45025,-44989,-44952,-44915,-44879,-44842,-44806,-44769,-44732,-44695,-44659,-44622,-44585,-
44548,-44511,-44474,-44437,-44400,-44363,-44326,-44289,-44252,-44215,-44178,-44141,-44104,-44067,-44029,-43992,-43955,-43918,
-43880,-43843,-43806,-43768,-43731,-43693,-43656,-43618,-43581,-43543,-43506,-43468,-43430,-43393,-43355,-43317,-43280,-
43242,-43204,-43166,-43128,-43091,-43053,-43015,-42977,-42939,-42901,-42863,-42825,-42787,-42749,-42711,-42672,-42634,-42596,
-42558,-42520,-42481,-42443,-42405,-42366,-42328,-42290,-42251,-42213,-42174,-42136,-42097,-42059,-42020,-41982,-41943,-
41904,-41866,-41827,-41788,-41750,-41711,-41672,-41633,-41595,-41556,-41517,-41478,-41439,-41400,-41361,-41322,-41283,-41244,
-41205,-41166,-41127,-41087,-41048,-41009,-40970,-40931,-40891,-40852,-40813,-40773,-40734,-40695,-40655,-40616,-40576,-
40537,-40497,-40458,-40418,-40379,-40339,-40299,-40260,-40220,-40180,-40141,-40101,-40061,-40021,-39982,-39942,-39902,-39862,
-39822,-39782,-39742,-39702,-39662,-39622,-39582,-39542,-39502,-39462,-39422,-39382,-39341,-39301,-39261,-39221,-39180,-
39140,-39100,-39059,-39019,-38979,-38938,-38898,-38857,-38817,-38776,-38736,-38695,-38655,-38614,-38573,-38533,-38492,-38451,
-38411,-38370,-38329,-38288,-38248,-38207,-38166,-38125,-38084,-38043,-38002,-37961,-37920,-37879,-37838,-37797,-37756,-
37715,-37674,-37633,-37592,-37550,-37509,-37468,-37427,-37386,-37344,-37303,-37262,-37220,-37179,-37137,-37096,-37055,-37013,
-36972,-36930,-36889,-36847,-36805,-36764,-36722,-36681,-36639,-36597,-36556,-36514,-36472,-36430,-36388,-36347,-36305,-
36263,-36221,-36179,-36137,-36095,-36053,-36011,-35969,-35927,-35885,-35843,-35801,-35759,-35717,-35675,-35633,-35590,-35548,
-35506,-35464,-35421,-35379,-35337,-35294,-35252,-35210,-35167,-35125,-35082,-35040,-34997,-34955,-34912,-34870,-34827,-
34785,-34742,-34699,-34657,-34614,-34571,-34529,-34486,-34443,-34400,-34358,-34315,-34272,-34229,-34186,-34143,-34100,-34057,
-34015,-33972,-33929,-33886,-33843,-33799,-33756,-33713,-33670,-33627,-33584,-33541,-33498,-33454,-33411,-33368,-33325,-
33281,-33238,-33195,-33151,-33108,-33065,-33021,-32978,-32934,-32891,-32847,-32804,-32760,-32717,-32673,-32630,-32586,-32542,
-32499,-32455,-32411,-32368,-32324,-32280,-32236,-32193,-32149,-32105,-32061,-32017,-31974,-31930,-31886,-31842,-31798,-
31754,-31710,-31666,-31622,-31578,-31534,-31490,-31446,-31402,-31357,-31313,-31269,-31225,-31181,-31136,-31092,-31048,-31004,
-30959,-30915,-30871,-30826,-30782,-30738,-30693,-30649,-30604,-30560,-30515,-30471,-30426,-30382,-30337,-30293,-30248,-
30204,-30159,-30114,-30070,-30025,-29980,-29936,-29891,-29846,-29801,-29757,-29712,-29667,-29622,-29577,-29533,-29488,-29443,
-29398,-29353,-29308,-29263,-29218,-29173,-29128,-29083,-29038,-28993,-28948,-28903,-28858,-28812,-28767,-28722,-28677,-
28632,-28586,-28541,-28496,-28451,-28405,-28360,-28315,-28269,-28224,-28179,-28133,-28088,-28042,-27997,-27952,-27906,-27861,
-27815,-27770,-27724,-27678,-27633,-27587,-27542,-27496,-27450,-27405,-27359,-27313,-27268,-27222,-27176,-27131,-27085,-
27039,-26993,-26947,-26902,-26856,-26810,-26764,-26718,-26672,-26626,-26580,-26534,-26488,-26442,-26396,-26350,-26304,-26258,
-26212,-26166,-26120,-26074,-26028,-25982,-25936,-25889,-25843,-25797,-25751,-25705,-25658,-25612,-25566,-25520,-25473,-
25427,-25381,-25334,-25288,-25241,-25195,-25149,-25102,-25056,-25009,-24963,-24916,-24870,-24823,-24777,-24730,-24684,-24637,
-24591,-24544,-24497,-24451,-24404,-24357,-24311,-24264,-24217,-24171,-24124,-24077,-24030,-23984,-23937,-23890,-23843,-
23796,-23750,-23703,-23656,-23609,-23562,-23515,-23468,-23421,-23374,-23327,-23280,-23233,-23186,-23139,-23092,-23045,-22998,
-22951,-22904,-22857,-22810,-22763,-22716,-22668,-22621,-22574,-22527,-22480,-22432,-22385,-22338,-22291,-22243,-22196,-
22149,-22102,-22054,-22007,-21960,-21912,-21865,-21817,-21770,-21723,-21675,-21628,-21580,-21533,-21485,-21438,-21390,-21343,
-21295,-21248,-21200,-21153,-21105,-21057,-21010,-20962,-20915,-20867,-20819,-20772,-20724,-20676,-20629,-20581,-20533,-
20485,-20438,-20390,-20342,-20294,-20246,-20199,-20151,-20103,-20055,-20007,-19959,-19912,-19864,-19816,-19768,-19720,-19672,
-19624,-19576,-19528,-19480,-19432,-19384,-19336,-19288,-19240,-19192,-19144,-19096,-19048,-19000,-18951,-18903,-18855,-
18807,-18759,-18711,-18663,-18614,-18566,-18518,-18470,-18421,-18373,-18325,-18277,-18228,-18180,-18132,-18084,-18035,-17987,
-17939,-17890,-17842,-17793,-17745,-17697,-17648,-17600,-17551,-17503,-17455,-17406,-17358,-17309,-17261,-17212,-17164,-
17115,-17067,-17018,-16970,-16921,-16872,-16824,-16775,-16727,-16678,-16629,-16581,-16532,-16484,-16435,-16386,-16338,-16289,
-16240,-16191,-16143,-16094,-16045,-15997,-15948,-15899,-15850,-15802,-15753,-15704,-15655,-15606,-15557,-15509,-15460,-
15411,-15362,-15313,-15264,-15215,-15167,-15118,-15069,-15020,-14971,-14922,-14873,-14824,-14775,-14726,-14677,-14628,-14579,
-14530,-14481,-14432,-14383,-14334,-14285,-14236,-14187,-14138,-14089,-14040,-13990,-13941,-13892,-13843,-13794,-13745,-
13696,-13647,-13597,-13548,-13499,-13450,-13401,-13351,-13302,-13253,-13204,-13154,-13105,-13056,-13007,-12957,-12908,-12859,
-12810,-12760,-12711,-12662,-12612,-12563,-12514,-12464,-12415,-12366,-12316,-12267,-12217,-12168,-12119,-12069,-12020,-
11970,-11921,-11872,-11822,-11773,-11723,-11674,-11624,-11575,-11525,-11476,-11426,-11377,-11327,-11278,-11228,-11179,-11129,
-11080,-11030,-10981,-10931,-10882,-10832,-10782,-10733,-10683,-10634,-10584,-10534,-10485,-10435,-10386,-10336,-10286,-
10237,-10187,-10137,-10088,-10038,-9988,-9939,-9889,-9839,-9790,-9740,-9690,-9640,-9591,-9541,-9491,-9442,-9392,-9342,-9292,-
9243,-9193,-9143,-9093,-9043,-8994,-8944,-8894,-8844,-8794,-8745,-8695,-8645,-8595,-8545,-8496,-8446,-8396,-8346,-8296,-8246
,-8196,-8147,-8097,-8047,-7997,-7947,-7897,-7847,-7797,-7747,-7697,-7648,-7598,-7548,-7498,-7448,-7398,-7348,-7298,-7248,
-7198,-7148,-7098,-7048,-6998,-6948,-6898,-6848,-6798,-6748,-6698,-6648,-6598,-6548,-6498,-6448,-6398,-6348,-6298,-6248,-
6198,-6148,-6098,-6048,-5998,-5948,-5898,-5848,-5798,-5747,-5697,-5647,-5597,-5547,-5497,-5447,-5397,-5347,-5297,-5247,-5197
,-5146,-5096,-5046,-4996,-4946,-4896,-4846,-4796,-4745,-4695,-4645,-4595,-4545,-4495,-4445,-4394,-4344,-4294,-4244,-4194,
-4144,-4093,-4043,-3993,-3943,-3893,-3843,-3792,-3742,-3692,-3642,-3592,-3541,-3491,-3441,-3391,-3341,-3291,-3240,-3190,-
3140,-3090,-3039,-2989,-2939,-2889,-2839,-2788,-2738,-2688,-2638,-2588,-2537,-2487,-2437,-2387,-2336,-2286,-2236,-2186,-2135
,-2085,-2035,-1985,-1934,-1884,-1834,-1784,-1733,-1683,-1633,-1583,-1532,-1482,-1432,-1382,-1331,-1281,-1231,-1181,-1130,
-1080,-1030,-980,-929,-879,-829,-779,-728,-678,-628,-578,-527,-477,-427,-376,-326,-276,-226,-175,-125,-75,-25,25,75,125,
175,226,276,326,376,427,477,527,578,628,678,728,779,829,879,929,980,1030,1080,1130,1181,1231,1281,1331,1382,1432,1482,1532,
1583,1633,1683,1733,1784,1834,1884,1934,1985,2035,2085,2135,2186,2236,2286,2336,2387,2437,2487,2537,2587,2638,2688,2738,2788
,2839,2889,2939,2989,3039,3090,3140,3190,3240,3291,3341,3391,3441,3491,3542,3592,3642,3692,3742,3792,3843,3893,3943,3993,
4043,4093,4144,4194,4244,4294,4344,4394,4445,4495,4545,4595,4645,4695,4745,4796,4846,4896,4946,4996,5046,5096,5146,5197,5247
,5297,5347,5397,5447,5497,5547,5597,5647,5697,5747,5798,5848,5898,5948,5998,6048,6098,6148,6198,6248,6298,6348,6398,6448,
6498,6548,6598,6648,6698,6748,6798,6848,6898,6948,6998,7048,7098,7148,7198,7248,7298,7348,7398,7448,7498,7548,7598,7648,7697
,7747,7797,7847,7897,7947,7997,8047,8097,8147,8196,8246,8296,8346,8396,8446,8496,8545,8595,8645,8695,8745,8794,8844,8894,
8944,8994,9043,9093,9143,9193,9243,9292,9342,9392,9442,9491,9541,9591,9640,9690,9740,9790,9839,9889,9939,9988,10038,10088,
10137,10187,10237,10286,10336,10386,10435,10485,10534,10584,10634,10683,10733,10782,10832,10882,10931,10981,11030,11080,11129
,11179,11228,11278,11327,11377,11426,11476,11525,11575,11624,11674,11723,11773,11822,11872,11921,11970,12020,12069,12119,
12168,12218,12267,12316,12366,12415,12464,12514,12563,12612,12662,12711,12760,12810,12859,12908,12957,13007,13056,13105,13154
,13204,13253,13302,13351,13401,13450,13499,13548,13597,13647,13696,13745,13794,13843,13892,13941,13990,14040,14089,14138,
14187,14236,14285,14334,14383,14432,14481,14530,14579,14628,14677,14726,14775,14824,14873,14922,14971,15020,15069,15118,15167
,15215,15264,15313,15362,15411,15460,15509,15557,15606,15655,15704,15753,15802,15850,15899,15948,15997,16045,16094,16143,
16191,16240,16289,16338,16386,16435,16484,16532,16581,16629,16678,16727,16775,16824,16872,16921,16970,17018,17067,17115,17164
,17212,17261,17309,17358,17406,17455,17503,17551,17600,17648,17697,17745,17793,17842,17890,17939,17987,18035,18084,18132,
18180,18228,18277,18325,18373,18421,18470,18518,18566,18614,18663,18711,18759,18807,18855,18903,18951,19000,19048,19096,19144
,19192,19240,19288,19336,19384,19432,19480,19528,19576,19624,19672,19720,19768,19816,19864,19912,19959,20007,20055,20103,
20151,20199,20246,20294,20342,20390,20438,20485,20533,20581,20629,20676,20724,20772,20819,20867,20915,20962,21010,21057,21105
,21153,21200,21248,21295,21343,21390,21438,21485,21533,21580,21628,21675,21723,21770,21817,21865,21912,21960,22007,22054,
22102,22149,22196,22243,22291,22338,22385,22432,22480,22527,22574,22621,22668,22716,22763,22810,22857,22904,22951,22998,23045
,23092,23139,23186,23233,23280,23327,23374,23421,23468,23515,23562,23609,23656,23703,23750,23796,23843,23890,23937,23984,
24030,24077,24124,24171,24217,24264,24311,24357,24404,24451,24497,24544,24591,24637,24684,24730,24777,24823,24870,24916,24963
,25009,25056,25102,25149,25195,25241,25288,25334,25381,25427,25473,25520,25566,25612,25658,25705,25751,25797,25843,25889,
25936,25982,26028,26074,26120,26166,26212,26258,26304,26350,26396,26442,26488,26534,26580,26626,26672,26718,26764,26810,26856
,26902,26947,26993,27039,27085,27131,27176,27222,27268,27313,27359,27405,27450,27496,27542,27587,27633,27678,27724,27770,
27815,27861,27906,27952,27997,28042,28088,28133,28179,28224,28269,28315,28360,28405,28451,28496,28541,28586,28632,28677,28722
,28767,28812,28858,28903,28948,28993,29038,29083,29128,29173,29218,29263,29308,29353,29398,29443,29488,29533,29577,29622,
29667,29712,29757,29801,29846,29891,29936,29980,30025,30070,30114,30159,30204,30248,30293,30337,30382,30427,30471,30516,30560
,30604,30649,30693,30738,30782,30826,30871,30915,30959,31004,31048,31092,31136,31181,31225,31269,31313,31357,31402,31446,
31490,31534,31578,31622,31666,31710,31754,31798,31842,31886,31930,31974,32017,32061,32105,32149,32193,32236,32280,32324,32368
,32411,32455,32499,32542,32586,32630,32673,32717,32760,32804,32847,32891,32934,32978,33021,33065,33108,33151,33195,33238,
33281,33325,33368,33411,33454,33498,33541,33584,33627,33670,33713,33756,33799,33843,33886,33929,33972,34015,34057,34100,34143
,34186,34229,34272,34315,34358,34400,34443,34486,34529,34571,34614,34657,34699,34742,34785,34827,34870,34912,34955,34997,
35040,35082,35125,35167,35210,35252,35294,35337,35379,35421,35464,35506,35548,35590,35633,35675,35717,35759,35801,35843,35885
,35927,35969,36011,36053,36095,36137,36179,36221,36263,36305,36347,36388,36430,36472,36514,36556,36597,36639,36681,36722,
36764,36805,36847,36889,36930,36972,37013,37055,37096,37137,37179,37220,37262,37303,37344,37386,37427,37468,37509,37551,37592
,37633,37674,37715,37756,37797,37838,37879,37920,37961,38002,38043,38084,38125,38166,38207,38248,38288,38329,38370,38411,
38451,38492,38533,38573,38614,38655,38695,38736,38776,38817,38857,38898,38938,38979,39019,39059,39100,39140,39180,39221,39261
,39301,39341,39382,39422,39462,39502,39542,39582,39622,39662,39702,39742,39782,39822,39862,39902,39942,39982,40021,40061,
40101,40141,40180,40220,40260,40299,40339,40379,40418,40458,40497,40537,40576,40616,40655,40695,40734,40773,40813,40852,40891
,40931,40970,41009,41048,41087,41127,41166,41205,41244,41283,41322,41361,41400,41439,41478,41517,41556,41595,41633,41672,
41711,41750,41788,41827,41866,41904,41943,41982,42020,42059,42097,42136,42174,42213,42251,42290,42328,42366,42405,42443,42481
,42520,42558,42596,42634,42672,42711,42749,42787,42825,42863,42901,42939,42977,43015,43053,43091,43128,43166,43204,43242,
43280,43317,43355,43393,43430,43468,43506,43543,43581,43618,43656,43693,43731,43768,43806,43843,43880,43918,43955,43992,44029
,44067,44104,44141,44178,44215,44252,44289,44326,44363,44400,44437,44474,44511,44548,44585,44622,44659,44695,44732,44769,
44806,44842,44879,44915,44952,44989,45025,45062,45098,45135,45171,45207,45244,45280,45316,45353,45389,45425,45462,45498,45534
,45570,45606,45642,45678,45714,45750,45786,45822,45858,45894,45930,45966,46002,46037,46073,46109,46145,46180,46216,46252,
46287,46323,46358,46394,46429,46465,46500,46536,46571,46606,46642,46677,46712,46747,46783,46818,46853,46888,46923,46958,46993
,47028,47063,47098,47133,47168,47203,47238,47273,47308,47342,47377,47412,47446,47481,47516,47550,47585,47619,47654,47688,
47723,47757,47792,47826,47861,47895,47929,47963,47998,48032,48066,48100,48134,48168,48202,48237,48271,48305,48338,48372,48406
,48440,48474,48508,48542,48575,48609,48643,48676,48710,48744,48777,48811,48844,48878,48911,48945,48978,49012,49045,49078,
49112,49145,49178,49211,49244,49278,49311,49344,49377,49410,49443,49476,49509,49542,49575,49608,49640,49673,49706,49739,49771
,49804,49837,49869,49902,49935,49967,50000,50032,50064,50097,50129,50162,50194,50226,50259,50291,50323,50355,50387,50420,
50452,50484,50516,50548,50580,50612,50644,50675,50707,50739,50771,50803,50834,50866,50898,50929,50961,50993,51024,51056,51087
,51119,51150,51182,51213,51244,51276,51307,51338,51369,51401,51432,51463,51494,51525,51556,51587,51618,51649,51680,51711,
51742,51773,51803,51834,51865,51896,51926,51957,51988,52018,52049,52079,52110,52140,52171,52201,52231,52262,52292,52322,52353
,52383,52413,52443,52473,52503,52534,52564,52594,52624,52653,52683,52713,52743,52773,52803,52832,52862,52892,52922,52951,
52981,53010,53040,53069,53099,53128,53158,53187,53216,53246,53275,53304,53334,53363,53392,53421,53450,53479,53508,53537,53566
,53595,53624,53653,53682,53711,53739,53768,53797,53826,53854,53883,53912,53940,53969,53997,54026,54054,54082,54111,54139,
54167,54196,54224,54252,54280,54309,54337,54365,54393,54421,54449,54477,54505,54533,54560,54588,54616,54644,54672,54699,54727
,54755,54782,54810,54837,54865,54892,54920,54947,54974,55002,55029,55056,55084,55111,55138,55165,55192,55219,55246,55274,
55300,55327,55354,55381,55408,55435,55462,55489,55515,55542,55569,55595,55622,55648,55675,55701,55728,55754,55781,55807,55833
,55860,55886,55912,55938,55965,55991,56017,56043,56069,56095,56121,56147,56173,56199,56225,56250,56276,56302,56328,56353,
56379,56404,56430,56456,56481,56507,56532,56557,56583,56608,56633,56659,56684,56709,56734,56760,56785,56810,56835,56860,56885
,56910,56935,56959,56984,57009,57034,57059,57083,57108,57133,57157,57182,57206,57231,57255,57280,57304,57329,57353,57377,
57402,57426,57450,57474,57498,57522,57546,57570,57594,57618,57642,57666,57690,57714,57738,57762,57785,57809,57833,57856,57880
,57903,57927,57950,57974,57997,58021,58044,58067,58091,58114,58137,58160,58183,58207,58230,58253,58276,58299,58322,58345,
58367,58390,58413,58436,58459,58481,58504,58527,58549,58572,58594,58617,58639,58662,58684,58706,58729,58751,58773,58795,58818
,58840,58862,58884,58906,58928,58950,58972,58994,59016,59038,59059,59081,59103,59125,59146,59168,59190,59211,59233,59254,
59276,59297,59318,59340,59361,59382,59404,59425,59446,59467,59488,59509,59530,59551,59572,59593,59614,59635,59656,59677,59697
,59718,59739,59759,59780,59801,59821,59842,59862,59883,59903,59923,59944,59964,59984,60004,60025,60045,60065,60085,60105,
60125,60145,60165,60185,60205,60225,60244,60264,60284,60304,60323,60343,60363,60382,60402,60421,60441,60460,60479,60499,60518
,60537,60556,60576,60595,60614,60633,60652,60671,60690,60709,60728,60747,60766,60785,60803,60822,60841,60859,60878,60897,
60915,60934,60952,60971,60989,61007,61026,61044,61062,61081,61099,61117,61135,61153,61171,61189,61207,61225,61243,61261,61279
,61297,61314,61332,61350,61367,61385,61403,61420,61438,61455,61473,61490,61507,61525,61542,61559,61577,61594,61611,61628,
61645,61662,61679,61696,61713,61730,61747,61764,61780,61797,61814,61831,61847,61864,61880,61897,61913,61930,61946,61963,61979
,61995,62012,62028,62044,62060,62076,62092,62108,62125,62141,62156,62172,62188,62204,62220,62236,62251,62267,62283,62298,
62314,62329,62345,62360,62376,62391,62407,62422,62437,62453,62468,62483,62498,62513,62528,62543,62558,62573,62588,62603,62618
,62633,62648,62662,62677,62692,62706,62721,62735,62750,62764,62779,62793,62808,62822,62836,62850,62865,62879,62893,62907,
62921,62935,62949,62963,62977,62991,63005,63019,63032,63046,63060,63074,63087,63101,63114,63128,63141,63155,63168,63182,63195
,63208,63221,63235,63248,63261,63274,63287,63300,63313,63326,63339,63352,63365,63378,63390,63403,63416,63429,63441,63454,
63466,63479,63491,63504,63516,63528,63541,63553,63565,63578,63590,63602,63614,63626,63638,63650,63662,63674,63686,63698,63709
,63721,63733,63745,63756,63768,63779,63791,63803,63814,63825,63837,63848,63859,63871,63882,63893,63904,63915,63927,63938,
63949,63960,63971,63981,63992,64003,64014,64025,64035,64046,64057,64067,64078,64088,64099,64109,64120,64130,64140,64151,64161
,64171,64181,64192,64202,64212,64222,64232,64242,64252,64261,64271,64281,64291,64301,64310,64320,64330,64339,64349,64358,
64368,64377,64387,64396,64405,64414,64424,64433,64442,64451,64460,64469,64478,64487,64496,64505,64514,64523,64532,64540,64549
,64558,64566,64575,64584,64592,64600,64609,64617,64626,64634,64642,64651,64659,64667,64675,64683,64691,64699,64707,64715,
64723,64731,64739,64747,64754,64762,64770,64777,64785,64793,64800,64808,64815,64822,64830,64837,64844,64852,64859,64866,64873
,64880,64887,64895,64902,64908,64915,64922,64929,64936,64943,64949,64956,64963,64969,64976,64982,64989,64995,65002,65008,
65015,65021,65027,65033,65040,65046,65052,65058,65064,65070,65076,65082,65088,65094,65099,65105,65111,65117,65122,65128,65133
,65139,65144,65150,65155,65161,65166,65171,65177,65182,65187,65192,65197,65202,65207,65212,65217,65222,65227,65232,65237,
65242,65246,65251,65256,65260,65265,65270,65274,65279,65283,65287,65292,65296,65300,65305,65309,65313,65317,65321,65325,65329
,65333,65337,65341,65345,65349,65352,65356,65360,65363,65367,65371,65374,65378,65381,65385,65388,65391,65395,65398,65401,
65404,65408,65411,65414,65417,65420,65423,65426,65429,65431,65434,65437,65440,65442,65445,65448,65450,65453,65455,65458,65460
,65463,65465,65467,65470,65472,65474,65476,65478,65480,65482,65484,65486,65488,65490,65492,65494,65496,65497,65499,65501,
65502,65504,65505,65507,65508,65510,65511,65513,65514,65515,65516,65518,65519,65520,65521,65522,65523,65524,65525,65526,65527
,65527,65528,65529,65530,65530,65531,65531,65532,65532,65533,65533,65534,65534,65534,65535,65535,65535,65535,65535,65535,
65535};private static uint[]ப={0,333772,667544,1001315,1335086,1668857,2002626,2336395,2670163,3003929,3337694,3671457,
4005219,4338979,4672736,5006492,5340245,5673995,6007743,6341488,6675230,7008968,7342704,7676435,8010164,8343888,8677609,9011325
,9345037,9678744,10012447,10346145,10679838,11013526,11347209,11680887,12014558,12348225,12681885,13015539,13349187,
13682829,14016464,14350092,14683714,15017328,15350936,15684536,16018129,16351714,16685291,17018860,17352422,17685974,18019518,
18353054,18686582,19020100,19353610,19687110,20020600,20354080,20687552,21021014,21354466,21687906,22021338,22354758,22688168,
23021568,23354956,23688332,24021698,24355052,24688396,25021726,25355046,25688352,26021648,26354930,26688200,27021456,27354702,
27687932,28021150,28354356,28687548,29020724,29353888,29687038,30020174,30353296,30686404,31019496,31352574,31685636,32018684,
32351718,32684734,33017736,33350722,33683692,34016648,34349584,34682508,35015412,35348300,35681172,36014028,36346868,36679688,
37012492,37345276,37678044,38010792,38343524,38676240,39008936,39341612,39674272,40006912,40339532,40672132,41004716,41337276,
41669820,42002344,42334848,42667332,42999796,43332236,43664660,43997060,44329444,44661800,44994140,45326456,45658752,45991028,
46323280,46655512,46987720,47319908,47652072,47984212,48316332,48648428,48980500,49312548,49644576,49976580,50308556,50640512,
50972444,51304352,51636236,51968096,52299928,52631740,52963524,53295284,53627020,53958728,54290412,54622068,54953704,55285308,
55616888,55948444,56279972,56611472,56942948,57274396,57605816,57937212,58268576,58599916,58931228,59262512,59593768,59924992,
60256192,60587364,60918508,61249620,61580704,61911760,62242788,62573788,62904756,63235692,63566604,63897480,64228332,64559148,
64889940,65220696,65551424,65882120,66212788,66543420,66874024,67204600,67535136,67865648,68196120,68526568,68856984,69187360,
69517712,69848024,70178304,70508560,70838776,71168960,71499112,71829224,72159312,72489360,72819376,73149360,73479304,73809216,
74139096,74468936,74798744,75128520,75458264,75787968,76117632,76447264,76776864,77106424,77435952,77765440,78094888,78424304,
78753688,79083032,79412336,79741608,80070840,80400032,80729192,81058312,81387392,81716432,82045440,82374408,82703336,83032224,
83361080,83689896,84018664,84347400,84676096,85004760,85333376,85661952,85990488,86318984,86647448,86975864,87304240,87632576,
87960872,88289128,88617344,88945520,89273648,89601736,89929792,90257792,90585760,90913688,91241568,91569408,91897200,92224960,
92552672,92880336,93207968,93535552,93863088,94190584,94518040,94845448,95172816,95500136,95827416,96154648,96481832,96808976,
97136080,97463136,97790144,98117112,98444032,98770904,99097736,99424520,99751256,100077944,100404592,100731192,101057744,
101384248,101710712,102037128,102363488,102689808,103016080,103342312,103668488,103994616,104320696,104646736,104972720,105298656
,105624552,105950392,106276184,106601928,106927624,107253272,107578872,107904416,108229920,108555368,108880768,109206120,
109531416,109856664,110181872,110507016,110832120,111157168,111482168,111807112,112132008,112456856,112781648,113106392,113431080
,113755720,114080312,114404848,114729328,115053760,115378136,115702464,116026744,116350960,116675128,116999248,117323312,
117647320,117971272,118295176,118619024,118942816,119266560,119590248,119913880,120237456,120560984,120884456,121207864,121531224
,121854528,122177784,122500976,122824112,123147200,123470224,123793200,124116120,124438976,124761784,125084528,125407224,
125729856,126052432,126374960,126697424,127019832,127342184,127664472,127986712,128308888,128631008,128953072,129275080,129597024
,129918912,130240744,130562520,130884232,131205888,131527480,131849016,132170496,132491912,132813272,133134576,133455816,
133776992,134098120,134419184,134740176,135061120,135382000,135702816,136023584,136344272,136664912,136985488,137306016,137626464
,137946864,138267184,138587456,138907664,139227808,139547904,139867920,140187888,140507776,140827616,141147392,141467104,
141786752,142106336,142425856,142745312,143064720,143384048,143703312,144022512,144341664,144660736,144979744,145298704,145617584
,145936400,146255168,146573856,146892480,147211040,147529536,147847968,148166336,148484640,148802880,149121056,149439152,
149757200,150075168,150393072,150710912,151028688,151346400,151664048,151981616,152299136,152616576,152933952,153251264,153568496
,153885680,154202784,154519824,154836784,155153696,155470528,155787296,156104000,156420624,156737200,157053696,157370112,
157686480,158002768,158318976,158635136,158951216,159267232,159583168,159899040,160214848,160530592,160846256,161161840,161477376
,161792832,162108208,162423520,162738768,163053952,163369040,163684080,163999040,164313936,164628752,164943504,165258176,
165572784,165887312,166201776,166516160,166830480,167144736,167458912,167773008,168087040,168400992,168714880,169028688,169342432
,169656096,169969696,170283216,170596672,170910032,171223344,171536576,171849728,172162800,172475808,172788736,173101600,
173414384,173727104,174039728,174352288,174664784,174977200,175289536,175601792,175913984,176226096,176538144,176850096,177161984
,177473792,177785536,178097200,178408784,178720288,179031728,179343088,179654368,179965568,180276704,180587744,180898720,
181209616,181520448,181831184,182141856,182452448,182762960,183073408,183383760,183694048,184004240,184314368,184624416,184934400
,185244288,185554096,185863840,186173504,186483072,186792576,187102000,187411344,187720608,188029808,188338912,188647936,
188956896,189265760,189574560,189883264,190191904,190500448,190808928,191117312,191425632,191733872,192042016,192350096,192658096
,192966000,193273840,193581584,193889264,194196848,194504352,194811792,195119136,195426400,195733584,196040688,196347712,
196654656,196961520,197268304,197574992,197881616,198188144,198494592,198800960,199107248,199413456,199719584,200025616,200331584
,200637456,200943248,201248960,201554576,201860128,202165584,202470960,202776256,203081456,203386592,203691632,203996592,
204301472,204606256,204910976,205215600,205520144,205824592,206128960,206433248,206737456,207041584,207345616,207649568,207953424
,208257216,208560912,208864512,209168048,209471488,209774832,210078112,210381296,210684384,210987408,211290336,211593184,
211895936,212198608,212501184,212803680,213106096,213408432,213710672,214012816,214314880,214616864,214918768,215220576,215522288
,215823920,216125472,216426928,216728304,217029584,217330784,217631904,217932928,218233856,218534704,218835472,219136144,
219436720,219737216,220037632,220337952,220638192,220938336,221238384,221538352,221838240,222138032,222437728,222737344,223036880
,223336304,223635664,223934912,224234096,224533168,224832160,225131072,225429872,225728608,226027232,226325776,226624240,
226922608,227220880,227519056,227817152,228115168,228413088,228710912,229008640,229306288,229603840,229901312,230198688,230495968
,230793152,231090256,231387280,231684192,231981024,232277760,232574416,232870960,233167440,233463808,233760096,234056288,
234352384,234648384,234944304,235240128,235535872,235831504,236127056,236422512,236717888,237013152,237308336,237603424,237898416
,238193328,238488144,238782864,239077488,239372016,239666464,239960816,240255072,240549232,240843312,241137280,241431168,
241724960,242018656,242312256,242605776,242899200,243192512,243485744,243778896,244071936,244364880,244657744,244950496,245243168
,245535744,245828224,246120608,246412912,246705104,246997216,247289216,247581136,247872960,248164688,248456320,248747856,
249039296,249330640,249621904,249913056,250204128,250495088,250785968,251076736,251367424,251658016,251948512,252238912,252529200
,252819408,253109520,253399536,253689456,253979280,254269008,254558640,254848176,255137632,255426976,255716224,256005376,
256294432,256583392,256872256,257161024,257449696,257738272,258026752,258315136,258603424,258891600,259179696,259467696,259755600
,260043392,260331104,260618704,260906224,261193632,261480960,261768176,262055296,262342320,262629248,262916080,263202816,
263489456,263776000,264062432,264348784,264635024,264921168,265207216,265493168,265779024,266064784,266350448,266636000,266921472
,267206832,267492096,267777264,268062336,268347312,268632192,268916960,269201632,269486208,269770688,270055072,270339360,
270623552,270907616,271191616,271475488,271759296,272042976,272326560,272610048,272893440,273176736,273459936,273743040,274026048
,274308928,274591744,274874432,275157024,275439520,275721920,276004224,276286432,276568512,276850528,277132416,277414240,
277695936,277977536,278259040,278540448,278821728,279102944,279384032,279665056,279945952,280226752,280507456,280788064,281068544
,281348960,281629248,281909472,282189568,282469568,282749440,283029248,283308960,283588544,283868032,284147424,284426720,
284705920,284985024,285264000,285542912,285821696,286100384,286378976,286657440,286935840,287214112,287492320,287770400,288048384
,288326240,288604032,288881696,289159264,289436768,289714112,289991392,290268576,290545632,290822592,291099456,291376224,
291652896,291929440,292205888,292482272,292758528,293034656,293310720,293586656,293862496,294138240,294413888,294689440,294964864
,295240192,295515424,295790560,296065600,296340512,296615360,296890080,297164704,297439200,297713632,297987936,298262144,
298536256,298810240,299084160,299357952,299631648,299905248,300178720,300452128,300725408,300998592,301271680,301544640,301817536
,302090304,302362976,302635520,302908000,303180352,303452608,303724768,303996800,304268768,304540608,304812320,305083968,
305355520,305626944,305898272,306169472,306440608,306711616,306982528,307253344,307524064,307794656,308065152,308335552,308605856
,308876032,309146112,309416096,309685984,309955744,310225408,310494976,310764448,311033824,311303072,311572224,311841280,
312110208,312379040,312647776,312916416,313184960,313453376,313721696,313989920,314258016,314526016,314793920,315061728,315329408
,315597024,315864512,316131872,316399168,316666336,316933408,317200384,317467232,317733984,318000640,318267200,318533632,
318799968,319066208,319332352,319598368,319864288,320130112,320395808,320661408,320926912,321192320,321457632,321722816,321987904
,322252864,322517760,322782528,323047200,323311744,323576192,323840544,324104800,324368928,324632992,324896928,325160736,
325424448,325688096,325951584,326215008,326478304,326741504,327004608,327267584,327530464,327793248,328055904,328318496,328580960
,328843296,329105568,329367712,329629760,329891680,330153536,330415264,330676864,330938400,331199808,331461120,331722304,
331983392,332244384,332505280,332766048,333026752,333287296,333547776,333808128,334068384,334328544,334588576,334848512,335108352
,335368064,335627712,335887200,336146624,336405920,336665120,336924224,337183200,337442112,337700864,337959552,338218112,
338476576,338734944,338993184,339251328,339509376,339767296,340025120,340282848,340540480,340797984,341055392,341312704,341569888
,341826976,342083968,342340832,342597600,342854272,343110848,343367296,343623648,343879904,344136032,344392064,344648000,
344903808,345159520,345415136,345670656,345926048,346181344,346436512,346691616,346946592,347201440,347456224,347710880,347965440
,348219872,348474208,348728448,348982592,349236608,349490528,349744320,349998048,350251648,350505152,350758528,351011808,
351264992,351518048,351771040,352023872,352276640,352529280,352781824,353034272,353286592,353538816,353790944,354042944,354294880
,354546656,354798368,355049952,355301440,355552800,355804096,356055264,356306304,356557280,356808128,357058848,357309504,
357560032,357810464,358060768,358311008,358561088,358811104,359060992,359310784,359560480,359810048,360059520,360308896,360558144
,360807296,361056352,361305312,361554144,361802880,362051488,362300032,362548448,362796736,363044960,363293056,363541024,
363788928,364036704,364284384,364531936,364779392,365026752,365274016,365521152,365768192,366015136,366261952,366508672,366755296
,367001792,367248192,367494496,367740704,367986784,368232768,368478656,368724416,368970080,369215648,369461088,369706432,
369951680,370196800,370441824,370686752,370931584,371176288,371420896,371665408,371909792,372154080,372398272,372642336,372886304
,373130176,373373952,373617600,373861152,374104608,374347936,374591168,374834304,375077312,375320224,375563040,375805760,
376048352,376290848,376533248,376775520,377017696,377259776,377501728,377743584,377985344,378227008,378468544,378709984,378951328
,379192544,379433664,379674688,379915584,380156416,380397088,380637696,380878176,381118560,381358848,381599040,381839104,
382079072,382318912,382558656,382798304,383037856,383277280,383516640,383755840,383994976,384233984,384472896,384711712,384950400
,385188992,385427488,385665888,385904160,386142336,386380384,386618368,386856224,387093984,387331616,387569152,387806592,
388043936,388281152,388518272,388755296,388992224,389229024,389465728,389702336,389938816,390175200,390411488,390647680,390883744
,391119712,391355584,391591328,391826976,392062528,392297984,392533312,392768544,393003680,393238720,393473632,393708448,
393943168,394177760,394412256,394646656,394880960,395115136,395349216,395583200,395817088,396050848,396284512,396518080,396751520
,396984864,397218112,397451264,397684288,397917248,398150080,398382784,398615424,398847936,399080320,399312640,399544832,
399776928,400008928,400240832,400472608,400704288,400935872,401167328,401398720,401629984,401861120,402092192,402323136,402553984
,402784736,403015360,403245888,403476320,403706656,403936896,404167008,404397024,404626944,404856736,405086432,405316032,
405545536,405774912,406004224,406233408,406462464,406691456,406920320,407149088,407377760,407606336,407834784,408063136,408291392
,408519520,408747584,408975520,409203360,409431072,409658720,409886240,410113664,410340992,410568192,410795296,411022304,
411249216,411476032,411702720,411929312,412155808,412382176,412608480,412834656,413060736,413286720,413512576,413738336,413964000
,414189568,414415040,414640384,414865632,415090784,415315840,415540800,415765632,415990368,416215008,416439552,416663968,
416888288,417112512,417336640,417560672,417784576,418008384,418232096,418455712,418679200,418902624,419125920,419349120,419572192
,419795200,420018080,420240864,420463552,420686144,420908608,421130976,421353280,421575424,421797504,422019488,422241344,
422463104,422684768,422906336,423127776,423349120,423570400,423791520,424012576,424233536,424454368,424675104,424895744,425116288
,425336736,425557056,425777280,425997408,426217440,426437376,426657184,426876928,427096544,427316064,427535488,427754784,
427974016,428193120,428412128,428631040,428849856,429068544,429287168,429505664,429724064,429942368,430160576,430378656,430596672
,430814560,431032352,431250048,431467616,431685120,431902496,432119808,432336992,432554080,432771040,432987936,433204736,
433421408,433637984,433854464,434070848,434287104,434503296,434719360,434935360,435151232,435367008,435582656,435798240,436013696
,436229088,436444352,436659520,436874592,437089568,437304416,437519200,437733856,437948416,438162880,438377248,438591520,
438805696,439019744,439233728,439447584,439661344,439875008,440088576,440302048,440515392,440728672,440941824,441154880,441367872
,441580736,441793472,442006144,442218720,442431168,442643552,442855808,443067968,443280032,443492000,443703872,443915648,
444127296,444338880,444550336,444761696,444972992,445184160,445395232,445606176,445817056,446027840,446238496,446449088,446659552
,446869920,447080192,447290400,447500448,447710432,447920320,448130112,448339776,448549376,448758848,448968224,449177536,
449386720,449595808,449804800,450013664,450222464,450431168,450639776,450848256,451056640,451264960,451473152,451681248,451889248
,452097152,452304960,452512672,452720288,452927808,453135232,453342528,453549760,453756864,453963904,454170816,454377632,
454584384,454791008,454997536,455203968,455410304,455616544,455822688,456028704,456234656,456440512,456646240,456851904,457057472
,457262912,457468256,457673536,457878688,458083744,458288736,458493600,458698368,458903040,459107616,459312096,459516480,
459720768,459924960,460129056,460333056,460536960,460740736,460944448,461148064,461351584,461554976,461758304,461961536,462164640
,462367680,462570592,462773440,462976160,463178816,463381344,463583776,463786144,463988384,464190560,464392608,464594560,
464796448,464998208,465199872,465401472,465602944,465804320,466005600,466206816,466407904,466608896,466809824,467010624,467211328
,467411936,467612480,467812896,468013216,468213440,468413600,468613632,468813568,469013440,469213184,469412832,469612416,
469811872,470011232,470210528,470409696,470608800,470807776,471006688,471205472,471404192,471602784,471801312,471999712,472198048
,472396288,472594400,472792448,472990400,473188256,473385984,473583648,473781216,473978688,474176064,474373344,474570528,
474767616,474964608,475161504,475358336,475555040,475751648,475948192,476144608,476340928,476537184,476733312,476929376,477125344
,477321184,477516960,477712640,477908224,478103712,478299104,478494400,478689600,478884704,479079744,479274656,479469504,
479664224,479858880,480053408,480247872,480442240,480636512,480830656,481024736,481218752,481412640,481606432,481800128,481993760
,482187264,482380704,482574016,482767264,482960416,483153472,483346432,483539296,483732064,483924768,484117344,484309856,
484502240,484694560,484886784,485078912,485270944,485462880,485654720,485846464,486038144,486229696,486421184,486612576,486803840
,486995040,487186176,487377184,487568096,487758912,487949664,488140320,488330880,488521312,488711712,488901984,489092160,
489282240,489472256,489662176,489851968,490041696,490231328,490420896,490610336,490799712,490988960,491178144,491367232,491556224
,491745120,491933920,492122656,492311264,492499808,492688256,492876608,493064864,493253056,493441120,493629120,493817024,
494004832,494192544,494380160,494567712,494755136,494942496,495129760,495316928,495504000,495691008,495877888,496064704,496251424
,496438048,496624608,496811040,496997408,497183680,497369856,497555936,497741920,497927840,498113632,498299360,498484992,
498670560,498856000,499041376,499226656,499411840,499596928,499781920,499966848,500151680,500336416,500521056,500705600,500890080
,501074464,501258752,501442944,501627040,501811072,501995008,502178848,502362592,502546240,502729824,502913312,503096704,
503280000,503463232,503646368,503829408,504012352,504195200,504377984,504560672,504743264,504925760,505108192,505290496,505472736
,505654912,505836960,506018944,506200832,506382624,506564320,506745952,506927488,507108928,507290272,507471552,507652736,
507833824,508014816,508195744,508376576,508557312,508737952,508918528,509099008,509279392,509459680,509639904,509820032,510000064
,510180000,510359872,510539648,510719328,510898944,511078432,511257856,511437216,511616448,511795616,511974688,512153664,
512332576,512511392,512690112,512868768,513047296,513225792,513404160,513582432,513760640,513938784,514116800,514294752,514472608
,514650368,514828064,515005664,515183168,515360608,515537952,515715200,515892352,516069440,516246432,516423328,516600160,
516776896,516953536,517130112,517306592,517482976,517659264,517835488,518011616,518187680,518363648,518539520,518715296,518891008
,519066624,519242144,519417600,519592960,519768256,519943424,520118528,520293568,520468480,520643328,520818112,520992800,
521167392,521341888,521516320,521690656,521864896,522039072,522213152,522387168,522561056,522734912,522908640,523082304,523255872
,523429376,523602784,523776096,523949312,524122464,524295552,524468512,524641440,524814240,524986976,525159616,525332192,
525504640,525677056,525849344,526021568,526193728,526365792,526537760,526709632,526881440,527053152,527224800,527396352,527567840
,527739200,527910528,528081728,528252864,528423936,528594880,528765760,528936576,529107296,529277920,529448480,529618944,
529789344,529959648,530129856,530300000,530470048,530640000,530809888,530979712,531149440,531319072,531488608,531658080,531827488
,531996800,532166016,532335168,532504224,532673184,532842080,533010912,533179616,533348288,533516832,533685312,533853728,
534022048,534190272,534358432,534526496,534694496,534862400,535030240,535197984,535365632,535533216,535700704,535868128,536035456
,536202720,536369888,536536992,536704000,536870912};}public enum ன{ந,த,ண}sealed class ட{private static int ľ=4;private Ꮖ
ǃ;private Ꮖ ǂ;public ட(Ꮖ ǃ,Ꮖ ǂ){this.ǃ=ǃ;this.ǂ=ǂ;}public static ட ċ(byte[]f,int ù){var ǃ=BitConverter.ToInt16(f,ù);var ǂ
=BitConverter.ToInt16(f,ù+2);return new ட(Ꮖ.Ꭸ(ǃ),Ꮖ.Ꭸ(ǂ));}public static ட[]ÿ(ಆ þ,int ý){var û=þ.ಡ(ý);if(û%ľ!=0){throw new
Exception();}var f=þ.ಠ(ý);var ú=û/ľ;var ι=new ட[ú];;for(var Ä=0;Ä<ú;Ä++){var ù=ľ*Ä;ι[Ä]=ċ(f,ù);}return ι;}public Ꮖ ޚ=>ǃ;public Ꮖ
ޙ=>ǂ;}class ಪ:Ⴞ{private ଦ l;private ಫ ˌ;private Ŀ ô;private Ꮖ ಧ;private Ꮖ Ζ;private int Β;private int ದ;private int ಥ;
public ಪ(ଦ l){this.l=l;}public override void Ⴛ(){var Ü=l.Đ;ί ŀ;switch(Β){case 0:if(--ಥ==0){switch(ˌ){case ಫ.ಲ:Β=-1;l.ૐ(ô.ĕ,ɴ.ʰ
,ʡ.ʝ);break;case ಫ.Ϊ:Β=-1;l.ૐ(ô.ĕ,ɴ.ȫ,ʡ.ʝ);break;case ಫ.ಮ:Β=1;l.ૐ(ô.ĕ,ɴ.ȩ,ʡ.ʝ);break;default:break;}}break;case 2:if(--ಥ
==0){switch(ˌ){case ಫ.ಱ:Β=1;ˌ=ಫ.Ϊ;l.ૐ(ô.ĕ,ɴ.ȩ,ʡ.ʝ);break;default:break;}}break;case-1:ŀ=Ü.Ξ(ô,Ζ,ô.Ģ,false,1,Β);if(ŀ==ί.ά){
switch(ˌ){case ಫ.ಲ:case ಫ.ವ:ô.Ē=null;l.વ.ᄪ(this);ô.Ĝ();l.ૐ(ô.ĕ,ɴ.ʰ,ʡ.ʝ);break;case ಫ.Ϊ:case ಫ.ಯ:ô.Ē=null;l.વ.ᄪ(this);ô.Ĝ();
break;case ಫ.ಮ:Β=0;ಥ=35*30;break;default:break;}}else if(ŀ==ί.έ){switch(ˌ){case ಫ.ವ:case ಫ.ಯ:break;default:Β=1;l.ૐ(ô.ĕ,ɴ.ȩ,ʡ.
ʝ);break;}}break;case 1:ŀ=Ü.Ξ(ô,Ζ,ಧ,false,1,Β);if(ŀ==ί.ά){switch(ˌ){case ಫ.ಲ:case ಫ.Ϊ:Β=0;ಥ=ದ;break;case ಫ.ಮ:case ಫ.ಳ:
case ಫ.ರ:ô.Ē=null;l.વ.ᄪ(this);ô.Ĝ();break;default:break;}}break;}}public ಫ ܫ{get{return ˌ;}set{ˌ=value;}}public Ŀ Ŀ{get{
return ô;}set{ô=value;}}public Ꮖ ತ{get{return ಧ;}set{ಧ=value;}}public Ꮖ ݏ{get{return Ζ;}set{Ζ=value;}}public int ಣ{get{return
Β;}set{Β=value;}}public int ಢ{get{return ದ;}set{ದ=value;}}public int ನ{get{return ಥ;}set{ಥ=value;}}}public enum ಫ{Ϊ,ಮ,ಯ,ರ
,ಱ,ಲ,ಳ,ವ}sealed class બ{private ଦ l;private Ꮖ ಶ;private Ꮖ ಷ;private Ꮖ ಸ;private ᗕ ڒ;private Ꮖ ಹ;private Ꮖ ಽ;private ᗕ ೞ;
public બ(ଦ l){this.l=l;ڒ=new ᗕ();ೞ=new ᗕ();}private Ꮖ گ(ᗕ ھ,ᗕ ܡ){var ܢ=(ܡ.ޗ>>8)*ھ.ޘ-(ܡ.ޘ>>8)*ھ.ޗ;if(ܢ==Ꮖ.Ꮓ){return Ꮖ.Ꮓ;}var ܣ=
((ܡ.ޚ-ھ.ޚ)>>8)*ܡ.ޗ+((ھ.ޙ-ܡ.ޙ)>>8)*ܡ.ޘ;var ڂ=ܣ/ܢ;return ڂ;}private bool ಭ(int ಬ,int Ĺ){var ࠎ=l.શ;var त=ࠎ.ᜂ[ಬ];var ú=त.ต;
for(var Ä=0;Ä<ú;Ä++){var ࡉ=ࠎ.ᜀ[त.ถ+Ä];var ò=ࡉ.ɢ;if(ò.Ĕ==Ĺ){continue;}ò.Ĕ=Ĺ;var ܡ=ò.ɝ;var ھ=ò.ɞ;var Ȅ=ᵎ.ᴬ(ܡ.ޚ,ܡ.ޙ,ڒ);var ȅ=ᵎ
.ᴬ(ھ.ޚ,ھ.ޙ,ڒ);if(Ȅ==ȅ){continue;}ೞ.ᗗ(ò);Ȅ=ᵎ.ᴬ(ڒ.ޚ,ڒ.ޙ,ೞ);ȅ=ᵎ.ᴬ(ಹ,ಽ,ೞ);if(Ȅ==ȅ){continue;}if(ò.ɤ==null){return false;}if((
ò.ᘾ&ᶘ.ᶔ)==0){return false;}var ಇ=ࡉ.ɣ;var ಈ=ࡉ.ɤ;if(ಇ.Ģ==ಈ.Ģ&&ಇ.ģ==ಈ.ģ){continue;}Ꮖ ಉ;if(ಇ.ģ<ಈ.ģ){ಉ=ಇ.ģ;}else{ಉ=ಈ.ģ;}Ꮖ ಊ;if
(ಇ.Ģ>ಈ.Ģ){ಊ=ಇ.Ģ;}else{ಊ=ಈ.Ģ;}if(ಊ>=ಉ){return false;}var ڂ=گ(ڒ,ೞ);if(ಇ.Ģ!=ಈ.Ģ){var ѭ=(ಊ-ಶ)/ڂ;if(ѭ>ಷ){ಷ=ѭ;}}if(ಇ.ģ!=ಈ.ģ){
var ѭ=(ಉ-ಶ)/ڂ;if(ѭ<ಸ){ಸ=ѭ;}}if(ಸ<=ಷ){return false;}}return true;}private bool ಌ(int ಎ,int Ĺ){if(ߵ.ޝ(ಎ)){if(ಎ==-1){return ಭ(
0,Ĺ);}else{return ಭ(ߵ.ޜ(ಎ),Ĺ);}}var ޛ=l.શ.ᜆ[ಎ];var ñ=ᵎ.ᴬ(ڒ.ޚ,ڒ.ޙ,ޛ);if(ñ==2){ñ=0;}if(!ಌ(ޛ.ޕ[ñ],Ĺ)){return false;}if(ñ==ᵎ.
ᴬ(ಹ,ಽ,ޛ)){return true;}return ಌ(ޛ.ޕ[ñ^1],Ĺ);}public bool ಏ(Ɠ ಐ,Ɠ Ψ){var ࠎ=l.શ;if(ࠎ.Ǭ.ǲ(ಐ.ฃ.Ŀ,Ψ.ฃ.Ŀ)){return false;}ಶ=ಐ.ኴ+
ಐ.ǡ-(ಐ.ǡ>>2);ಸ=(Ψ.ኴ+Ψ.ǡ)-ಶ;ಷ=(Ψ.ኴ)-ಶ;ڒ.ޚ=ಐ.ޚ;ڒ.ޙ=ಐ.ޙ;ڒ.ޘ=Ψ.ޚ-ಐ.ޚ;ڒ.ޗ=Ψ.ޙ-ಐ.ޙ;ಹ=Ψ.ޚ;ಽ=Ψ.ޙ;return ಌ(ࠎ.ᜆ.Length-1,l.ૡ());}}
sealed class ಆ{private List<string>ಅ;private byte[]ౡ;private List<ᶞ>ౠ;private ಗ ౙ;private ಖ ౘ;private ಕ ఽ;public ಆ(params
string[]హ){try{ᕣ.ᔵ.འ("Open WAD files: ");ಅ=new List<string>();ౠ=new List<ᶞ>();foreach(var ష in హ){స(ష);}ౘ=ಚ(ಅ);ఽ=ಙ(ಅ);ౙ=ಛ(ಅ);ᕣ
.ᔵ.འ("OK ("+string.Join(", ",హ)+")");}catch(Exception e){ᕣ.ᔵ.འ("Failed");throw e;}}private void స(string ష){String శ=ష.
Split('.')[0].ToLower();ಅ.Add(శ);byte[]వ=ᕣ.ᔵ.ཡ(శ);string ಋ;int ಒ;int ಟ;{var f=new byte[12];Array.Copy(వ,0,f,0,12);ಋ=ን.ቦ(f,0,4
);ಒ=BitConverter.ToInt32(f,4);ಟ=BitConverter.ToInt32(f,8);if(ಋ!="IWAD"&&ಋ!="PWAD"){throw new Exception(
"The file is not a WAD file.");}}{var f=new byte[ᶞ.ᅁ*ಒ];Array.Copy(వ,ಟ,f,0,ᶞ.ᅁ*ಒ);for(var Ä=0;Ä<ಒ;Ä++){var ù=ᶞ.ᅁ*Ä;var ಝ=new ᶞ(ን.ቦ(f,ù+8,8),వ,
BitConverter.ToInt32(f,ù),BitConverter.ToInt32(f,ù+4));ౠ.Add(ಝ);}}}public int ಞ(string Ĭ){for(var Ä=ౠ.Count-1;Ä>=0;Ä--){if(ౠ[Ä].Ń==Ĭ
){return Ä;}}return-1;}public int ಡ(int Ć){return ౠ[Ć].ᶛ;}public byte[]ಠ(int Ć){var ಝ=ౠ[Ć];var f=new byte[ಝ.ᶛ];Array.Copy
(ಝ.ᶜ,ಝ.ᒪ,f,0,ಝ.ᶛ);return f;}public byte[]ಠ(string Ĭ){var ಜ=ಞ(Ĭ);if(ಜ==-1){throw new Exception("The lump '"+Ĭ+
"' was not found.");}return ಠ(ಜ);}private static ಗ ಛ(IReadOnlyList<string>ಅ){foreach(var Ĭ in ಅ){switch(Ĭ.ToLower()){case"doom2":case
"freedoom2":return ಗ.ᵊ;case"doom":case"doom1":case"freedoom1":return ಗ.ᵋ;case"plutonia":case"tnt":return ಗ.ᵌ;}}return ಗ.ᵊ;}private
static ಖ ಚ(IReadOnlyList<string>ಅ){foreach(var Ĭ in ಅ){switch(Ĭ.ToLower()){case"doom2":case"plutonia":case"tnt":case
"freedoom2":return ಖ.ᴉ;case"doom":case"freedoom1":return ಖ.ᴈ;case"doom1":return ಖ.ᴋ;}}return ಖ.ᴇ;}private static ಕ ಙ(IReadOnlyList<
string>ಅ){foreach(var Ĭ in ಅ){switch(Ĭ.ToLower()){case"plutonia":return ಕ.ᕩ;case"tnt":return ಕ.ᕹ;}}return ಕ.ኾ;}public
IReadOnlyList<ᶞ>ಘ=>ౠ;public ಗ ಗ=>ౙ;public ಖ ಖ=>ౘ;public ಕ ಕ=>ఽ;}sealed class ଊ{public static Ꮖ ಔ=Ꮖ.Ꭸ(64);public static Ꮖ ಓ=Ꮖ.Ꭸ(32*64)
;public static Ꮖ ஞ=Ꮖ.Ꭸ(32);public static Ꮖ இ=Ꮖ.Ꭸ(128);private static Ꮖ ਸ਼=Ꮖ.Ꭸ(6);private static Ꮖ ઘ=Ꮖ.Ꭸ(6);private ଦ l;
private Ꮖ ઙ;public ଊ(ଦ l){this.l=l;}public void ચ(ђ Ð){Ð.ƪ=0;}public void ઝ(ђ Ð,Ɓ Ś){var છ=l.Ư;if(Ð.Ɠ.Ŷ==ኑ.ጔ[(int)ᚏ.ᨎ]||Ð.Ɠ.Ŷ==
ኑ.ጔ[(int)ᚏ.ᨏ]){Ð.Ɠ.ᘸ(ᚏ.ᨊ);}if(Ð.Ə==ਖ਼.ନ&&Ś.Ŷ==ኑ.ጔ[(int)ᚏ.ન]){l.ૐ(Ð.Ɠ,ɴ.ɨ,ʡ.Ƅ);}if(Ð.Ǝ!=ਖ਼.ଫ||Ð.ƙ==0){var ţ=ኑ.ኔ[(int)Ð.Ə].ਫ਼;
છ.Ť(Ð,ſ.Ƅ,ţ);return;}if((Ð.ƕ.ఠ&ట.ఞ)!=0){if(!Ð.Ɗ||(Ð.Ə!=ਖ਼.પ&&Ð.Ə!=ਖ਼.Ϻ)){Ð.Ɗ=true;ઍ(Ð);return;}}else{Ð.Ɗ=false;}var Ş=(128*
Ð.Ɠ.ଦ.ଖ)&ஜ.ற;Ś.Ŵ=Ꮖ.Ꮒ+Ð.Ƙ*ஜ.ௐ(Ş);Ş&=ஜ.ஷ/2-1;Ś.ų=ஞ+Ð.Ƙ*ஜ.ஸ(Ş);}private bool જ(ђ Ð){var ۅ=ኑ.ኔ[(int)Ð.Ə].ƌ;int ú;if(Ð.Ə==ਖ਼.Ϻ)
{ú=ኑ.ᕺ.ᕬ;}else if(Ð.Ə==ਖ਼.ପ){ú=2;}else{ú=1;}if(ۅ==ᓩ.ᓫ||Ð.ƌ[(int)ۅ]>=ú){return true;}do{if(Ð.ƍ[(int)ਖ਼.Ϲ]&&Ð.ƌ[(int)ᓩ.ظ]>0&&
l.હ.ಖ!=ಖ.ᴋ){Ð.Ǝ=ਖ਼.Ϲ;}else if(Ð.ƍ[(int)ਖ਼.ପ]&&Ð.ƌ[(int)ᓩ.ᓪ]>2&&l.હ.ಖ==ಖ.ᴉ){Ð.Ǝ=ਖ਼.ପ;}else if(Ð.ƍ[(int)ਖ਼.Ѝ]&&Ð.ƌ[(int)ᓩ.З]>0)
{Ð.Ǝ=ਖ਼.Ѝ;}else if(Ð.ƍ[(int)ਖ਼.Љ]&&Ð.ƌ[(int)ᓩ.ᓪ]>0){Ð.Ǝ=ਖ਼.Љ;}else if(Ð.ƌ[(int)ᓩ.З]>0){Ð.Ǝ=ਖ਼.ਸ;}else if(Ð.ƍ[(int)ਖ਼.ନ]){Ð.Ǝ=ਖ਼
.ନ;}else if(Ð.ƍ[(int)ਖ਼.પ]&&Ð.ƌ[(int)ᓩ.પ]>0){Ð.Ǝ=ਖ਼.પ;}else if(Ð.ƍ[(int)ਖ਼.Ϻ]&&Ð.ƌ[(int)ᓩ.ظ]>ኑ.ᕺ.ᕬ&&l.હ.ಖ!=ಖ.ᴋ){Ð.Ǝ=ਖ਼.Ϻ;}
else{Ð.Ǝ=ਖ਼.ਹ;}}while(Ð.Ǝ==ਖ਼.ଫ);l.Ư.Ť(Ð,ſ.Ƅ,ኑ.ኔ[(int)Ð.Ə].ਫ਼);return false;}private void ખ(Ŀ ક,int ઔ,Ɠ ઓ,int Ĺ){if(ક.Ĕ==Ĺ&&ક.Ę
<=ઔ+1){return;}ક.Ĕ=Ĺ;ક.Ę=ઔ+1;ક.ė=ઓ;var ઑ=l.ય;for(var Ä=0;Ä<ક.đ.Length;Ä++){var ˠ=ક.đ[Ä];if((ˠ.ᘾ&ᶘ.ᶔ)==0){continue;}ઑ.ᛣ(ˠ);
if(ઑ.ᛖ<=Ꮖ.Ꮓ){continue;}Ŀ ˑ;if(ˠ.ᶖ.Ŀ==ક){ˑ=ˠ.ᶗ.Ŀ;}else{ˑ=ˠ.ᶖ.Ŀ;}if((ˠ.ᘾ&ᶘ.ᶐ)!=0){if(ઔ==0){ખ(ˑ,1,ઓ,Ĺ);}}else{ખ(ˑ,ઔ,ઓ,Ĺ);}}}
private void ઐ(Ɠ Ψ,Ɠ એ){ખ(એ.ฃ.Ŀ,0,Ψ,l.ૡ());}private void ઍ(ђ Ð){if(!જ(Ð)){return;}Ð.Ɠ.ᘸ(ᚏ.ᨎ);var ţ=ኑ.ኔ[(int)Ð.Ə].ਜ਼;l.Ư.Ť(Ð,ſ.Ƅ,
ţ);ઐ(Ð.Ɠ,Ð.Ɠ);}public void ઞ(ђ Ð,Ɓ Ś){Ś.ų+=ઘ;if(Ś.ų<இ){return;}if(Ð.Ų==Ų.Ű){Ś.ų=இ;return;}var છ=l.Ư;if(Ð.ƙ==0){છ.Ť(Ð,ſ.Ƅ,
ᚏ.ᚠ);return;}Ð.Ə=Ð.Ǝ;છ.Ţ(Ð);}public void દ(ђ Ð,Ɓ Ś){Ś.ų-=ਸ਼;if(Ś.ų>ஞ){return;}Ś.ų=ஞ;var ţ=ኑ.ኔ[(int)Ð.Ə].ੜ;l.Ư.Ť(Ð,ſ.Ƅ,ţ);}
public void ધ(ђ Ð){var ǵ=l.ષ;var Ь=(ǵ.ѐ()%10+1)<<1;if(Ð.Ɲ[(int)Ů.Ŭ]!=0){Ь*=10;}var ਲ਼=l.ભ;var Ş=Ð.Ɠ.ɡ;Ş+=new ɡ((ǵ.ѐ()-ǵ.ѐ())<<
18);var ѭ=ਲ਼.ᮼ(Ð.Ɠ,Ş,ಔ);ਲ਼.ᮿ(Ð.Ɠ,Ş,ಔ,ѭ,Ь);if(ਲ਼.ᮯ!=null){l.ૐ(Ð.Ɠ,ɴ.ʃ,ʡ.Ƅ);Ð.Ɠ.ɡ=ᵎ.ᴥ(Ð.Ɠ.ޚ,Ð.Ɠ.ޙ,ਲ਼.ᮯ.ޚ,ਲ਼.ᮯ.ޙ);}}public void ન(
ђ Ð){var Ь=2*(l.ષ.ѐ()%10+1);var ǵ=l.ષ;var થ=Ð.Ɠ.ɡ;થ+=new ɡ((ǵ.ѐ()-ǵ.ѐ())<<18);var ਲ਼=l.ભ;var ѭ=ਲ਼.ᮼ(Ð.Ɠ,થ,ಔ+Ꮖ.Ꮏ);ਲ਼.ᮿ(Ð.Ɠ,થ,
ಔ+Ꮖ.Ꮏ,ѭ,Ь);if(ਲ਼.ᮯ==null){l.ૐ(Ð.Ɠ,ɴ.ɗ,ʡ.Ƅ);return;}l.ૐ(Ð.Ɠ,ɴ.ɖ,ʡ.Ƅ);var ત=ᵎ.ᴥ(Ð.Ɠ.ޚ,Ð.Ɠ.ޙ,ਲ਼.ᮯ.ޚ,ਲ਼.ᮯ.ޙ);if(ત-Ð.Ɠ.ɡ>ɡ.ᓣ){if(
(int)(ત-Ð.Ɠ.ɡ).Ꮁ<-ɡ.ᓮ.Ꮁ/20){Ð.Ɠ.ɡ=ત+ɡ.ᓮ/21;}else{Ð.Ɠ.ɡ-=ɡ.ᓮ/20;}}else{if(ત-Ð.Ɠ.ɡ>ɡ.ᓮ/20){Ð.Ɠ.ɡ=ત-ɡ.ᓮ/21;}else{Ð.Ɠ.ɡ+=ɡ.ᓮ/
20;}}Ð.Ɠ.ᘾ|=ళ.ᘔ;}public void ણ(ђ Ð){if((Ð.ƕ.ఠ&ట.ఞ)!=0&&Ð.Ǝ==ਖ਼.ଫ&&Ð.ƙ!=0){Ð.Ƈ++;ઍ(Ð);}else{Ð.Ƈ=0;જ(Ð);}}private void ઢ(Ɠ ઠ)
{var ਲ਼=l.ભ;var Ş=ઠ.ɡ;ઙ=ਲ਼.ᮼ(ઠ,Ş,Ꮖ.Ꭸ(1024));if(ਲ਼.ᮯ==null){Ş+=new ɡ(1<<26);ઙ=ਲ਼.ᮼ(ઠ,Ş,Ꮖ.Ꭸ(1024));if(ਲ਼.ᮯ==null){Ş-=new ɡ(2<<26
);ઙ=ਲ਼.ᮼ(ઠ,Ş,Ꮖ.Ꭸ(1024));}}}private void ડ(Ɠ ઠ,bool ટ){var ǵ=l.ષ;var Ь=5*(ǵ.ѐ()%3+1);var Ş=ઠ.ɡ;if(!ટ){Ş+=new ɡ((ǵ.ѐ()-ǵ.ѐ()
)<<18);}l.ભ.ᮿ(ઠ,Ş,ಓ,ઙ,Ь);}public void ઌ(ђ Ð){l.ૐ(Ð.Ɠ,ɴ.ɲ,ʡ.Ƅ);Ð.Ɠ.ᘸ(ᚏ.ᨏ);Ð.ƌ[(int)ኑ.ኔ[(int)Ð.Ə].ƌ]--;l.Ư.Ť(Ð,ſ.ƀ,ኑ.ኔ[(int
)Ð.Ə].ਗ਼);ઢ(Ð.Ɠ);ડ(Ð.Ɠ,Ð.Ƈ==0);}public void ਤ(ђ Ð){Ð.ƪ=1;}public void ਮ(ђ Ð){l.ૐ(Ð.Ɠ,ɴ.ɱ,ʡ.Ƅ);Ð.Ɠ.ᘸ(ᚏ.ᨏ);Ð.ƌ[(int)ኑ.ኔ[(int
)Ð.Ə].ƌ]--;l.Ư.Ť(Ð,ſ.ƀ,ኑ.ኔ[(int)Ð.Ə].ਗ਼);ઢ(Ð.Ɠ);for(var Ä=0;Ä<7;Ä++){ડ(Ð.Ɠ,false);}}public void ਯ(ђ Ð){Ð.ƪ=2;}public void
ਰ(ђ Ð,Ɓ Ś){l.ૐ(Ð.Ɠ,ɴ.ɲ,ʡ.Ƅ);if(Ð.ƌ[(int)ኑ.ኔ[(int)Ð.Ə].ƌ]==0){return;}Ð.Ɠ.ᘸ(ᚏ.ᨏ);Ð.ƌ[(int)ኑ.ኔ[(int)Ð.Ə].ƌ]--;l.Ư.Ť(Ð,ſ.ƀ,ኑ
.ኔ[(int)Ð.Ə].ਗ਼+Ś.Ŷ.ġ-ኑ.ጔ[(int)ᚏ.ᙧ].ġ);ઢ(Ð.Ɠ);ડ(Ð.Ɠ,Ð.Ƈ==0);}public void ਲ(ђ Ð){l.ૐ(Ð.Ɠ,ɴ.ɯ,ʡ.Ƅ);Ð.Ɠ.ᘸ(ᚏ.ᨏ);Ð.ƌ[(int)ኑ.ኔ[(
int)Ð.Ə].ƌ]-=2;l.Ư.Ť(Ð,ſ.ƀ,ኑ.ኔ[(int)Ð.Ə].ਗ਼);ઢ(Ð.Ɠ);var ǵ=l.ષ;var ਲ਼=l.ભ;for(var Ä=0;Ä<20;Ä++){var Ь=5*(ǵ.ѐ()%3+1);var Ş=Ð.Ɠ.
ɡ;Ş+=new ɡ((ǵ.ѐ()-ǵ.ѐ())<<19);ਲ਼.ᮿ(Ð.Ɠ,Ş,ಓ,ઙ+new Ꮖ((ǵ.ѐ()-ǵ.ѐ())<<5),Ь);}}public void ਭ(ђ Ð){જ(Ð);}public void ਬ(ђ Ð){l.ૐ(
Ð.Ɠ,ɴ.ɮ,ʡ.Ƅ);}public void ਫ(ђ Ð){l.ૐ(Ð.Ɠ,ɴ.ɬ,ʡ.Ƅ);}public void ਪ(ђ Ð){l.ૐ(Ð.Ɠ,ɴ.ɭ,ʡ.Ƅ);ણ(Ð);}public void ਨ(ђ Ð){Ð.Ɠ.ᘸ(ᚏ.ᨏ
);l.Ư.Ť(Ð,ſ.ƀ,ኑ.ኔ[(int)Ð.Ə].ਗ਼);}public void ਧ(ђ Ð){Ð.ƌ[(int)ኑ.ኔ[(int)Ð.Ə].ƌ]--;l.ળ.ᆒ(Ð.Ɠ,ё.ϸ);}public void ਦ(ђ Ð){Ð.ƌ[(
int)ኑ.ኔ[(int)Ð.Ə].ƌ]--;l.Ư.Ť(Ð,ſ.ƀ,ኑ.ኔ[(int)Ð.Ə].ਗ਼+(l.ષ.ѐ()&1));l.ળ.ᆒ(Ð.Ɠ,ё.Ϲ);}public void ਥ(ђ Ð){l.ૐ(Ð.Ɠ,ɴ.ɪ,ʡ.Ƅ);}public
void ਵ(ђ Ð){Ð.ƌ[(int)ኑ.ኔ[(int)Ð.Ə].ƌ]-=ኑ.ᕺ.ᕬ;l.ળ.ᆒ(Ð.Ɠ,ё.Ϻ);}public void ઈ(Ɠ ઉ){var ਲ਼=l.ભ;var ǵ=l.ષ;for(var Ä=0;Ä<40;Ä++){
var ઊ=ઉ.ɡ-ɡ.ᓮ/2+ɡ.ᓮ/40*(uint)Ä;ਲ਼.ᮼ(ઉ.ᘻ,ઊ,Ꮖ.Ꭸ(16*64));if(ਲ਼.ᮯ==null){continue;}l.ળ.ᆕ(ਲ਼.ᮯ.ޚ,ਲ਼.ᮯ.ޙ,ਲ਼.ᮯ.ኴ+(ਲ਼.ᮯ.ǡ>>2),ё.ϥ);var Ь=
0;for(var ͼ=0;ͼ<15;ͼ++){Ь+=(ǵ.ѐ()&7)+1;}l.ર.ᅹ(ਲ਼.ᮯ,ઉ.ᘻ,ઉ.ᘻ,Ь);}}}sealed class ઋ{private ᓩ ۅ;private ᚏ ઇ;private ᚏ આ;
private ᚏ અ;private ᚏ ੴ;private ᚏ ੳ;public ઋ(ᓩ ۅ,ᚏ ઇ,ᚏ આ,ᚏ અ,ᚏ ੴ,ᚏ ੳ){this.ۅ=ۅ;this.ઇ=ઇ;this.આ=આ;this.અ=અ;this.ੴ=ੴ;this.ੳ=ੳ;}
public ᓩ ƌ{get{return ۅ;}set{ۅ=value;}}public ᚏ ੲ{get{return ઇ;}set{ઇ=value;}}public ᚏ ਫ਼{get{return આ;}set{આ=value;}}public ᚏ
ੜ{get{return અ;}set{અ=value;}}public ᚏ ਜ਼{get{return ੴ;}set{ੴ=value;}}public ᚏ ਗ਼{get{return ੳ;}set{ੳ=value;}}}public enum
ਖ਼{ਹ,ਸ,Љ,Ѝ,પ,Ϲ,Ϻ,ନ,ପ,ŏ,ଫ}sealed class ବ{private short[]ǂ;private int ˢ;private Ᏺ ǵ;public ବ(int ځ,int ˢ){ǂ=new short[ځ];
this.ˢ=ˢ;ǵ=new Ᏺ(DateTime.Now.Millisecond);}public void ମ(){ǂ[0]=(short)(-(ǵ.ѐ()%16));for(var Ä=1;Ä<ǂ.Length;Ä++){var ڬ=(ǵ.ѐ
()%3)-1;ǂ[Ä]=(short)(ǂ[Ä-1]+ڬ);if(ǂ[Ä]>0){ǂ[Ä]=0;}else if(ǂ[Ä]==-16){ǂ[Ä]=-15;}}}public ன ڞ(){var ଧ=true;for(var Ä=0;Ä<ǂ.
Length;Ä++){if(ǂ[Ä]<0){ǂ[Ä]++;ଧ=false;}else if(ǂ[Ä]<ˢ){var Ǆ=(ǂ[Ä]<16)?ǂ[Ä]+1:8;if(ǂ[Ä]+Ǆ>=ˢ){Ǆ=ˢ-ǂ[Ä];}ǂ[Ä]+=(short)Ǆ;ଧ=false
;}}if(ଧ){return ன.த;}else{return ன.ந;}}public short[]ޙ=>ǂ;}sealed partial class ଦ{private ᴆ Å;private ᕣ Æ;private Ᏺ ǵ;
private શ ࠎ;private વ Ý;private ƒ ଥ;private ળ ତ;private લ ଣ;private ર ଢ;private ય ଡ;private મ ଠ;private ڏ ଭ;private ભ ଯ;private
બ ଢ଼;private Đ ଶ;private Ư ଷ;private ଔ ସ;private ଊ ହ;private Ҳ ଽ;private ଋ ଡ଼;private ଌ ȍ;private ଏ Ȑ;private ଐ அ;private
int ୟ;private int ୠ;private int ୡ;private int À;private bool ୱ;private bool ஃ;private bool ஆ;private int Ĺ;private int ǈ;
private Ɠ ଵ;public ଦ(ᴍ ଳ,ᴆ Å,ᕣ Æ){this.Å=Å;this.Æ=Æ;this.ǵ=Å.ષ;ࠎ=new શ(ଳ,this);Ý=new વ(this);ଥ=new ƒ(this);ତ=new ળ(this);ଣ=new
લ(this);ଢ=new ર(this);ଡ=new ય(this);ଠ=new મ(this);ଭ=new ڏ(this);ଯ=new ભ(this);ଢ଼=new બ(this);ଶ=new Đ(this);ଷ=new Ư(this);ସ
=new ଔ(this);ହ=new ଊ(this);ଽ=new Ҳ(this);ଡ଼=new ଋ(this);ȍ=new ଌ(this);Ȑ=new ଏ(this);அ=new ଐ(this);Å.ᰋ.ᰃ=0;Å.ᰋ.ᰂ=180;for(
var Ä=0;Ä<ђ.ۇ;Ä++){Å.ᰁ[Ä].Ź=0;Å.ᰁ[Ä].Ż=0;Å.ᰁ[Ä].ź=0;}Å.ᰁ[Å.ଙ].Ɯ=Ꮖ.Ꮏ;ୟ=0;ୠ=0;ୡ=0;ଲ();if(Å.ᴵ!=0){for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Å.
ᰁ[Ä].Ÿ){Å.ᰁ[Ä].Ɠ=null;ତ.ᆝ(Ä);}}}ଥ.ཆ();À=0;ୱ=false;ஃ=false;ஆ=false;Ĺ=0;ǈ=Å.ଙ;ଵ=new Ɠ(this);Å.ᴾ.ᯉ(શ.ᛦ(Å),true);}public ன ڞ(
){var Ë=Å.ᰁ;for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Ë[Ä].Ÿ){Ë[Ä].Ċ();}}Ý.Ċ();foreach(var ô in ࠎ.ᛩ){ô.Ċ();}for(var Ä=0;Ä<ђ.ۇ;Ä++){if(Ë[Ä
].Ÿ){ଷ.ƶ(Ë[Ä]);}}Ý.Ⴛ();ଥ.ڞ();ତ.ᆢ();ȍ.ڞ();Ȑ.ڞ();À++;if(ஆ){return ன.த;}else{if(ୱ){return ன.ந;}else{ୱ=true;return ன.ண;}}}
private void ଲ(){for(var Ä=0;Ä<ࠎ.ᜃ.Length;Ä++){var ର=ࠎ.ᜃ[Ä];var ଟ=true;if(Å.ಖ!=ಖ.ᴉ){switch(ର.ܫ){case 68:case 64:case 88:case 89
:case 69:case 67:case 71:case 65:case 66:case 84:ଟ=false;break;}}if(!ଟ){break;}ତ.ᅑ(ର);}}public void ଞ(){ஃ=false;ஆ=true;}
public void ફ(){ஃ=true;ஆ=true;}public void ૐ(Ɠ ã,ɴ Ǯ,ʡ ˌ){Å.ᴽ.ૐ(ã,Ǯ,ˌ);}public void ૐ(Ɠ ã,ɴ Ǯ,ʡ ˌ,int ߛ){Å.ᴽ.ૐ(ã,Ǯ,ˌ,ߛ);}
public void ૠ(Ɠ ã){Å.ᴽ.ૠ(ã);}public int ૡ(){Ĺ++;return Ĺ;}public bool ଅ(ᕨ Ň){if(!Å.ᴶ){அ.ଅ(Ň);}if(Ȑ.ᔢ){if(Ȑ.ଅ(Ň)){return true;}
}if(Ň.ᕤ==ኈ.ቖ&&Ň.ܫ==ፎ.ፏ){if(Ȑ.ᔢ){Ȑ.ಯ();}else{Ȑ.ರ();}return true;}if(Ň.ᕤ==ኈ.ቷ&&Ň.ܫ==ፎ.ፏ){if(Å.ᴵ==0){ଆ();}return true;}
return false;}public void ଆ(){ǈ++;if(ǈ==ђ.ۇ||!Å.ᰁ[ǈ].Ÿ){ǈ=0;}}public Ɠ ઽ(Ɠ ã){if(ã==null){ଵ.ޚ=Ꮖ.Ꮓ;ଵ.ޙ=Ꮖ.Ꮓ;ଵ.ኴ=Ꮖ.Ꮓ;ଵ.ᘾ=0;return
ଵ;}else{return ã;}}public ᴆ હ=>Å;public ᕣ સ=>Æ;public Ᏺ ષ=>ǵ;public શ શ=>ࠎ;public વ વ=>Ý;public ƒ ƒ=>ଥ;public ળ ળ=>ତ;
public લ લ=>ଣ;public ર ર=>ଢ;public ય ય=>ଡ;public મ મ=>ଠ;public ڏ ڏ=>ଭ;public ભ ભ=>ଯ;public બ બ=>ଢ଼;public Đ Đ=>ଶ;public Ư Ư=>ଷ;
public ଔ ଔ=>ସ;public ଊ ଊ=>ହ;public Ҳ Ҳ=>ଽ;public ଋ ଋ=>ଡ଼;public ଌ ଌ=>ȍ;public ଏ ଏ=>Ȑ;public ଐ ଐ=>அ;public int ଓ{get{return ୟ;}
set{ୟ=value;}}public int କ{get{return ୠ;}set{ୠ=value;}}public int ଜ{get{return ୡ;}set{ୡ=value;}}public int ଖ{get{return À;}
set{À=value;}}public int ଗ=>Æ.ଗ;public bool ଘ=>ஃ;public ђ ଙ=>Å.ᰁ[Å.ଙ];public ђ ଚ=>Å.ᰁ[ǈ];public bool ଛ=>ଙ.Ɯ==Ꮖ.Ꮏ;}static
class ଝ{public static IReadOnlyList<IReadOnlyList<ଈ>>ଉ=new ଈ[][]{new ଈ[]{new ଈ(185,164),new ଈ(148,143),new ଈ(69,122),new ଈ(
209,102),new ଈ(116,89),new ଈ(166,55),new ଈ(71,56),new ଈ(135,29),new ଈ(71,24)},new ଈ[]{new ଈ(254,25),new ଈ(97,50),new ଈ(188,
64),new ଈ(128,78),new ଈ(214,92),new ଈ(133,130),new ଈ(208,136),new ଈ(148,140),new ଈ(235,158)},new ଈ[]{new ଈ(156,168),new ଈ(
48,154),new ଈ(174,95),new ଈ(265,75),new ଈ(130,48),new ଈ(279,23),new ଈ(198,48),new ଈ(140,25),new ଈ(281,136)}};public class
ଈ{private int ǃ;private int ǂ;public ଈ(int ǃ,int ǂ){this.ǃ=ǃ;this.ǂ=ǂ;}public int ޚ=>ǃ;public int ޙ=>ǂ;}}sealed class ଇ:ᜮ
{private string[]Ǉ;private Action Ǳ;public ଇ(ባ ĭ,string Ǉ,Action Ǳ):base(ĭ){this.Ǉ=Ǉ.Split('\n');this.Ǳ=Ǳ;}public
override bool ଅ(ᕨ Ň){if(Ň.ܫ!=ፎ.ፏ){return true;}if(Ň.ᕤ==ኈ.ޙ||Ň.ᕤ==ኈ.ቔ||Ň.ᕤ==ኈ.ቓ){Ǳ();ኘ.ಯ();ኘ.ૐ(ɴ.ɲ);}if(Ň.ᕤ==ኈ.ኩ||Ň.ᕤ==ኈ.ኡ){ኘ.ಯ()
;ኘ.ૐ(ɴ.ȭ);}return true;}public IReadOnlyList<string>ǭ=>Ǉ;}