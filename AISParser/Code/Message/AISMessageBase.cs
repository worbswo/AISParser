using AISParser.Code.Parser;
using AISParser.Code.Parser.Interface;
using System;
using System.Collections.Generic;

namespace AISParser.Code.Message
{

    public class AISMessageBase
    {
        #region Property

        #region Static
        static public bool DebugMessage { get; set; } = false;
        static public float LongitudeLatitudeRatio { get; set; } = 1f / 600000f;
        static public char[] SixBitASCII { get; set; } = new char[] { '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
           '[','\\',']','^','_',' ','!','"','#','$','%','&','"','(',')','*','+',',','-','.','/',
           '0','1','2','3','4','5','6','7','8','9',':',';','<','=','>','?'};
        static public List<string> NaviSatus { get; set; } = new List<string>() { "under way using engine","at anchor","not under command",
                                                                                  "restricted maneuverability","constrained by her draught",
                                                                                  "moored","aground","engaged in fishing","under way sailing",
                                                                                  "reserved forfuture amendment of navigational status for ships carrying DG, HS, or MP,or IMO hazard or pollutant category C, high speed craft (HSC)",
                                                                                  "reserved for future amendment of navigational status for ships carrying dangerous goods (DG), harmful substances (HS) or marine pollutants (MP),or IMO hazard or pollutant category A, wing in ground (WIG)",
                                                                                  "power-driven vessel towing astern (regional use)","power-driven vessel pushing ahead or towing alongside (regional use)",
                                                                                  "reserved for future use","AIS-SART (active), MOB-AIS, EPIRB-AIS ",
                                                                                  "undefined = default (also used by AIS-SART, MOB-AIS and EPIRB-AIS under test)"};
        static public List<string> PositionFixingDevice { get; set; } = new List<string>() { "undefined","GPS","GNSS","combined GPS/GLONASS","Loran-C",
                                                                                            "Chayka","intergrated navigation system","surveyed",
                                                                                            "Galile","not used","not used","not used","not used","not used","not used",
                                                                                            "internal GNSS"};
        static public List<string> CagoTypeSpecial { get; set; } = new List<string>() { "Pilot vessel", "Search and rescue vessels", "Tugs", "Port tenders",
                                                                                          "Vessels with anti-pollution facilities or equipment",
                                                                                          "Law enforcement vessels","Spare-for assignments to local vessels",
                                                                                          "Spare-for assignments to local vessels","Medical transports",
                                                                                          "Ships and aircraft of States not parties to an armed conflict"};
        static public List<string> CargoTypeFirst { get; set; } = new List<string>() { "","Reseved for future use","WIG","Vessel","HSC","","Passenger ships",
          "Cargo Ships","Tanker(s)","Other types of ship"};
        static public List<string> CargoTypeSecond { get; set; } = new List<string>() { "All ships of this type", "Carrying DG,HS, or MP,IMO hazard or pollutant category X",
                                                                                       "Carrying DG,HS, or MP,IMO hazard or pollutant category Y",
                                                                                       "Carrying DG,HS, or MP,IMO hazard or pollutant category Z",
                                                                                       "Carrying DG,HS, or MP,IMO hazard or pollutant category OS",
                                                                                       "Reserved for future use","Reserved for future use","Reserved for future use",
                                                                                       "Reserved for future use","No additional information"};
        static public List<string> CargoTypeSecondVessel { get; set; } = new List<string>() { "Fishing", "Towing", "Towing and length of the two exceeds 200 m or breadth exceeds 25m",
                                                                                                "Engaged in dredging or underwater operations"," Engaged in diving operations",
                                                                                                "Sailing","Pleasure craft","Reserved for future use","Reserved for future use"};
        static public Dictionary<int, List<string>> NationNumber { get; set; } = new Dictionary<int, List<string>>()
        {
            {201 ,new List<string>(){"Albania","al"}},                          {202 ,new List<string>(){"Andorra","ad"}},                          {203 ,new List<string>(){"Austria","aq"}},                          {204 ,new List<string>(){"Azores",""}},                     {205 ,new List<string>(){"Belgium","be"}},
            {206 ,new List<string>(){"Belarus","by"}},                          {207 ,new List<string>(){"Bulgaria","bg"}},                         {208 ,new List<string>(){"Vatican City","va"}},                     {209 ,new List<string>(){"Cyprus","cy"}},                   {210 ,new List<string>(){"Cyprus","cy"}},
            {211 ,new List<string>(){"Germany","de"}},                          {212 ,new List<string>(){"Cyprus","cy"}},                           {213 ,new List<string>(){"Georgia","ge"}},                          {214 ,new List<string>(){"Moldova","md"}},                  {215 ,new List<string>(){"Malta","mt"}},
            {216 ,new List<string>(){"Armenia","am"}},                          {218 ,new List<string>(){"Germany","de"}},                          {219 ,new List<string>(){"Denmark","dk"}},                          {220 ,new List<string>(){"Denmark","dk"}},                  {224 ,new List<string>(){"Spain","es"}},
            {225 ,new List<string>(){"Spain","es"}},                            {226 ,new List<string>(){"France","fr"}},                           {227 ,new List<string>(){"France","fr"}},                           {228 ,new List<string>(){"France","fr"}},                   {230 ,new List<string>(){"Finland","fi"}},
            {231 ,new List<string>(){"Faroe Islands","fo"}},                    {232 ,new List<string>(){"United Kingdom","gb"}},                   {233 ,new List<string>(){"United Kingdom","gb"}},                   {234 ,new List<string>(){"United Kingdom","gb"}},           {235 ,new List<string>(){"United Kingdom","gb"}},
            {236 ,new List<string>(){"Gibraltar","gi"}},                        {237 ,new List<string>(){"Greece","gr"}},                           {238 ,new List<string>(){"Croatia","hr"}},                          {239 ,new List<string>(){"Greece","gr"}},                   {240 ,new List<string>(){"Greece","gr"}},
            {241 ,new List<string>(){"Greece","gr"}},                           {242 ,new List<string>(){"Morocco","ma"}},                          {243 ,new List<string>(){"Hungary","hu"}},                          {244 ,new List<string>(){"Netherlands","nl"}},              {245 ,new List<string>(){"Netherlands","nl"}},
            {246 ,new List<string>(){"Netherlands","nl"}},                      {247 ,new List<string>(){"Italy","it"}},                            {248 ,new List<string>(){"Malta","mt"}},                            {249 ,new List<string>(){"Malta","mt"}},                    {250 ,new List<string>(){"Ireland","ie"}},
            {251 ,new List<string>(){"Iceland","is"}},                          {252 ,new List<string>(){"Liechtenstein","li"}},                    {253 ,new List<string>(){"Luxembourg","lu"}},                       {254 ,new List<string>(){"Monaco","mc"}},                   {255 ,new List<string>(){"Madeira",""}},
            {256 ,new List<string>(){"Malta","mt"}},                            {257 ,new List<string>(){"Norway","no"}},                           {258 ,new List<string>(){"Norway","no"}},                           {259 ,new List<string>(){"Norway","no"}},                   {261 ,new List<string>(){"Poland","pl"}},
            {262 ,new List<string>(){"Montenegro","me"}},                       {263 ,new List<string>(){"Portugal","pt"}},                         {264 ,new List<string>(){"Romania","ro"}},                          {265 ,new List<string>(){"Sweden","se"}},                   {266 ,new List<string>(){"Sweden","se"}},
            {267 ,new List<string>(){"Slovakia","sk"}},                         {268 ,new List<string>(){"San Marino","sm"}},                       {269 ,new List<string>(){"Switzerland","ch"}},                      {270 ,new List<string>(){"Czech","cz"}},                    {271 ,new List<string>(){"Turkey","tr"}},
            {272 ,new List<string>(){"Ukraine","ua"}},                          {273 ,new List<string>(){"Russian Federation","ru"}},               {274 ,new List<string>(){"Makedonia","mk"}},                        {275 ,new List<string>(){"Latvia","lv"}},                   {276 ,new List<string>(){"Estonia","ee"}},
            {277 ,new List<string>(){"Lithuania","lt"}},                        {278 ,new List<string>(){"Slovenia","si"}},                         {279 ,new List<string>(){"Serbia","rs"}},                           {301 ,new List<string>(){"Anguilla","ai"}},                 {303 ,new List<string>(){"Alaska",""}},
            {304 ,new List<string>(){"Antigua and Barbuda","ag"}},              {305 ,new List<string>(){"Antigua and Barbuda","ag"}},              {306 ,new List<string>(){"Netherlands Antilles","an"}},             {307 ,new List<string>(){"Aruba","aw"}},                    {308 ,new List<string>(){"Bahamas","bs"}},
            {309 ,new List<string>(){"Bahamas","bs"}},                          {310 ,new List<string>(){"Bermuda","bm"}},                          {311 ,new List<string>(){"Bahamas","bs"}},                          {312 ,new List<string>(){"Belize","bz"}},                   {314 ,new List<string>(){"Barbados","bb"}},
            {316 ,new List<string>(){"Canada","ca"}},                           {319 ,new List<string>(){"Cayman Islands","ky"}},                   {321 ,new List<string>(){"Costa Rica","cr"}},                       {323 ,new List<string>(){"Cuba","cu"}},                     {325 ,new List<string>(){"Dominica","dm"}},
            {327 ,new List<string>(){"Dominican","do"}},                        {329 ,new List<string>(){"Guadeloupe","gp"}},                       {330 ,new List<string>(){"Grenada","gd"}},                          {331 ,new List<string>(){"Greenland","gl"}},                {332 ,new List<string>(){"Guatemala","gt"}},
            {334 ,new List<string>(){"Honduras","hn"}},                         {336 ,new List<string>(){"Haiti","ht"}},                            {338 ,new List<string>(){"United States of America","us"}},         {339 ,new List<string>(){"Jamaica","jm"}},                  {341 ,new List<string>(){"Saint Kitts and Nevis","kn"}},
            {343 ,new List<string>(){"Saint Lucia","lc"}},                      {345 ,new List<string>(){"Mexico","mx"}},                           {347 ,new List<string>(){"Martinique","mq"}},                       {348 ,new List<string>(){"Montserrat","ms"}},               {350 ,new List<string>(){"Nicaragua","nl"}},
            {351 ,new List<string>(){"Panama","pa"}},                           {352 ,new List<string>(){"Panama","pa"}},                           {353 ,new List<string>(){"Panama","pa"}},                           {373 ,new List<string>(){"Panama","pa"}},                   {355 ,new List<string>(){"Panama","pa"}},
            {357 ,new List<string>(){"Panama","pa"}},                           {356 ,new List<string>(){"Panama","pa"}},                           {354 ,new List<string>(){"Panama","pa"}},                           {358 ,new List<string>(){"Puerto Rico","pr"}},              {359 ,new List<string>(){"El Salvador","sv"}},
            {361 ,new List<string>(){"Saint Pierre and Miquelon","pm"}},        {362 ,new List<string>(){"Trinidad and Tobago","tt"}},              {364 ,new List<string>(){"Turks and Caicos Islands","tc"}},         {366 ,new List<string>(){"United States of America","us"}}, {367 ,new List<string>(){"United States of America","us"}},
            {368 ,new List<string>(){"United States of America","us"}},         {369 ,new List<string>(){"United States of America","us"}},         {370 ,new List<string>(){"Panama","pa"}},                           {371 ,new List<string>(){"Panama","pa"}},                   {372 ,new List<string>(){"Panama","pa"}},
            {375 ,new List<string>(){"Saint Vincent and the Grenadines","vc"}}, {376 ,new List<string>(){"Saint Vincent and the Grenadines","vc"}}, {377 ,new List<string>(){"Saint Vincent and the Grenadines","vc"}}, {378 ,new List<string>(){"British Virgin Islands","vg"}},   {379 ,new List<string>(){"United States Virgin Islands","vg"}},
            {401 ,new List<string>(){"Afghanistan","af"}},                      {403 ,new List<string>(){"Saudi Arabia","sa"}},                     {405 ,new List<string>(){"Bangladesh","bd"}},                       {408 ,new List<string>(){"Bahrain","bh"}},                  {410 ,new List<string>(){"Bhutan","bt"}},
            {412 ,new List<string>(){"China","cn"}},                            {413 ,new List<string>(){"China","cn"}},                            {416 ,new List<string>(){"Taiwan","tw"}},                           {417 ,new List<string>(){"Sri Lanka","lk"}},                {419 ,new List<string>(){"India","in"}},
            {422 ,new List<string>(){"Iran","ir"}},                             {423 ,new List<string>(){"Azerbaijani",""}},                        {425 ,new List<string>(){"Iraq","iq"}},                             {428 ,new List<string>(){"Israel","il"}},                   {431 ,new List<string>(){"Japan","jp"}},
            {432 ,new List<string>(){"Japan","jp"}},                            {434 ,new List<string>(){"Turkmenistan","tm"}},                     {436 ,new List<string>(){"Kazakhstan","kz"}},                       {437 ,new List<string>(){"Uzbekistan","uz"}},               {438 ,new List<string>(){"Jordan","jo"}},
            {440 ,new List<string>(){"South Korea","kr"}},                      {441 ,new List<string>(){"South Korea","kr"}},                      {443 ,new List<string>(){"Palestine","ps"}},                        {445 ,new List<string>(){"North Korea","kp"}},              {447 ,new List<string>(){"Kuwait","kw"}},
            {450 ,new List<string>(){"Lebanon","lb"}},                          {451 ,new List<string>(){"Kyrgyzstan","kg"}},                       {453 ,new List<string>(){"Macao","mo"}},                            {455 ,new List<string>(){"Maldives","mv"}},                 {457 ,new List<string>(){"Mongolia","mn"}},
            {459 ,new List<string>(){"Nepal","np"}},                            {461 ,new List<string>(){"Oman","om"}},                             {463 ,new List<string>(){"Pakistan","pk"}},                         {466 ,new List<string>(){"Qatar","qa"}},                    {468 ,new List<string>(){"Syrian Arab","sy"}},
            {470 ,new List<string>(){"United Arab Emirates","ae"}},             {473 ,new List<string>(){"Yemen","ye"}},                            {475 ,new List<string>(){"Yemen","ye"}},                            {477 ,new List<string>(){"Hong Kong","hk"}},                {478 ,new List<string>(){"Bosnia and Herzegovina","ba"}},
            {501 ,new List<string>(){"Adelie Land","tf"}},                      {503 ,new List<string>(){"Australia","au"}},                        {506 ,new List<string>(){"Myanmar","mm"}},                          {508 ,new List<string>(){"Brunei Darussalam","bn"}},        {510 ,new List<string>(){"Micronesia","fm"}},
            {511 ,new List<string>(){"Palau","pw"}},                            {512 ,new List<string>(){"New Zealand","nz"}},                      {514 ,new List<string>(){"Cambodia","kh"}},                         {515 ,new List<string>(){"Cambodia","kh"}},                 {516 ,new List<string>(){"Christmas Island","cx"}},
            {518 ,new List<string>(){"Cook Islands","ck"}},                     {520 ,new List<string>(){"Fiji","fj"}},                             {523 ,new List<string>(){"Cocos Islands","cc"}},                    {525 ,new List<string>(){"Indonesia","id"}},                {529 ,new List<string>(){"Kiribati","ki"}},
            {531 ,new List<string>(){"Lao","la"}},                              {533 ,new List<string>(){"Malaysia","my"}},                         {536 ,new List<string>(){"Northern Mariana Islands","mp"}},         {538 ,new List<string>(){"Marshall Islands","mh"}},         {540 ,new List<string>(){"New Caledonia","nc"}},
            {542 ,new List<string>(){"Niue","nu"}},                             {544 ,new List<string>(){"Nauru","nr"}},                            {546 ,new List<string>(){"French Polynesia","pf"}},                 {548 ,new List<string>(){"Philippines","ph"}},              {553 ,new List<string>(){"Papua New Guinea","pg"}},
            {555 ,new List<string>(){"Pitcairn Island","pn"}},                  {557 ,new List<string>(){"Solomon Islands","sb"}},                  {559 ,new List<string>(){"American Samoa","as"}},                   {561 ,new List<string>(){"Samoa","ws"}},                    {563 ,new List<string>(){"Singapore","sg"}},
            {564 ,new List<string>(){"Singapore","sg"}},                        {565 ,new List<string>(){"Singapore","sg"}},                        {567 ,new List<string>(){"Thailand","th"}},                         {570 ,new List<string>(){"Tonga","yo"}},                    {572 ,new List<string>(){"Tuvalu","tv"}},
            {574 ,new List<string>(){"VietNam","vn"}},                          {576 ,new List<string>(){"Vanuatu","vu"}},                          {578 ,new List<string>(){"Wallis and Futuna Islands","wf"}},        {601 ,new List<string>(){"South Africa","za"}},             {603 ,new List<string>(){"Angola","ao"}},
            {605 ,new List<string>(){"Algeria","dz"}},                          {607 ,new List<string>(){"Saint Paul and Amsterdam Islands",""}},   {608 ,new List<string>(){"Ascension Island","ac"}},                 {609 ,new List<string>(){"Burundi","bi"}},                  {610 ,new List<string>(){"Benin","bj"}},
            {611 ,new List<string>(){"Botswana","bw"}},                         {612 ,new List<string>(){"Central African","cf"}},                  {613 ,new List<string>(){"Cameroon","cm"}},                         {615 ,new List<string>(){"Congo","cg"}},                    {616 ,new List<string>(){"Comoros","km"}},
            {617 ,new List<string>(){"Cape Verde","cv"}},                       {618 ,new List<string>(){"Crozet Archipelago",""}},                 {619 ,new List<string>(){"Côte d'Ivoire","ci"}},                    {621 ,new List<string>(){"Djibouti","dj"}},                 {622 ,new List<string>(){"Egypt","eg"}},
            {624 ,new List<string>(){"Ethiopia","et"}},                         {625 ,new List<string>(){"Eritrea","er"}},                          {626 ,new List<string>(){"Gabonese","ga"}},                         {627 ,new List<string>(){"Ghana","gh"}},                    {629 ,new List<string>(){"Gambia","gm"}},
            {630 ,new List<string>(){"Guinea-Bissau","gw"}},                    {631 ,new List<string>(){"Equatorial Guinea","gq"}},                {632 ,new List<string>(){"Guinea","gn"}},                           {633 ,new List<string>(){"Burkina Faso","bf"}},             {634 ,new List<string>(){"Kenya","ke"}},
            {635 ,new List<string>(){"Kerguelen Islands",""}},                  {636 ,new List<string>(){"Liberia","lr"}},                          {637 ,new List<string>(){"Liberia","lr"}},                          {642 ,new List<string>(){"Libya","ly"}},                    {644 ,new List<string>(){"Lesotho","ls"}},
            {645 ,new List<string>(){"Mauritius","mu"}},                        {647 ,new List<string>(){"Madagascar","mg"}},                       {649 ,new List<string>(){"Mali","ml"}},                             {650 ,new List<string>(){"Mozambique","mz"}},               {654 ,new List<string>(){"Mauritania","mr"}},
            {655 ,new List<string>(){"Malawi","mw"}},                           {656 ,new List<string>(){"Niger","ne"}},                            {657 ,new List<string>(){"Nigeria","ng"}},                          {659 ,new List<string>(){"Namibia","na"}},                  {660 ,new List<string>(){"Reunion","re"}},
            {661 ,new List<string>(){"Rwanda","rw"}},                           {662 ,new List<string>(){"Sudan","sd"}},                            {663 ,new List<string>(){"Senegal","sn"}},                          {664 ,new List<string>(){"Seychelles","sc"}},               {665 ,new List<string>(){"Saint Helena","sh"}},
            {666 ,new List<string>(){"Somalia","so"}},                          {667 ,new List<string>(){"Sierra Leone","sl"}},                     {668 ,new List<string>(){"Sao Tome and Principe","st"}},            {669 ,new List<string>(){"Swaziland","xz"}},                {670 ,new List<string>(){"Chad","td"}},
            {671 ,new List<string>(){"Togolese","tg"}},                         {672 ,new List<string>(){"Tunisia","tn"}},                          {674 ,new List<string>(){"Tanzania","tz"}},                         {675 ,new List<string>(){"Uganda","ug"}},                   {676 ,new List<string>(){"DR Congo","cd"}},
            {677 ,new List<string>(){"Tanzania","tz"}},                         {678 ,new List<string>(){"Zambia","zm"}},                           {679 ,new List<string>(){"Zimbabwe","zw"}},                         {701 ,new List<string>(){"Argentina","ar"}},                {710 ,new List<string>(){"Brazil","br"}},
            {720 ,new List<string>(){"Bolivia","bo"}},                          {725 ,new List<string>(){"Chile","cl"}},                            {730 ,new List<string>(){"Colombia","co"}},                         {735 ,new List<string>(){"Ecuador","ec"}},                  {740 ,new List<string>(){"Falkland Islands","fk"}},
            {745 ,new List<string>(){"Guiana","gf"}},                           {750 ,new List<string>(){"Guyana","gy"}},                           {755 ,new List<string>(){"Paraguay","py"}},                         {760 ,new List<string>(){"Peru","pe"}},                     {765 ,new List<string>(){"Suriname","sr"}},
            {770 ,new List<string>(){"Uruguay","uy"}},                          {775 ,new List<string>(){"Venezuela","ve"}},                        {566 ,new List<string>(){"Singapore","sg"}},                        {577 ,new List<string>(){"Vanuatu","vu"}},                  {999 ,new List<string>(){"",""}},
            {414,new List<string>(){"China","cn"}}
        };
        #endregion

        public byte MessageId { get; set; } = new byte();
        public byte Repeatindicator { get; set; } = new byte();
        public uint UserId { get; set; } = new uint();
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string ReceivingDate { get; set; }
        public int MID { get; set; }

        #endregion

        #region Method
        /// <summary>
        /// RoT 계산
        /// </summary>
        /// <param name="rot"></param>
        /// <returns></returns>
        public static string ConvertROT(int rot)
        {
            if (rot == -128)
            {
                return "Not turning";
            }
            else if(rot == -127)
            {
                return "turning left  at more than 5deg/30s";
            }
            else if (rot == 127)
            {
                return "turning right at more than 5deg/30s";
            }
            float tmp = (float)rot/4.733f;
            int result = (int)(tmp * tmp);
            return result.ToString();

        }
        /// <summary>
        /// MMSI에서 MID 추출 
        /// </summary>
        /// <param name="mmsi"></param>
        /// <returns></returns>B
        public static int MIDParsingInMMSI(int mmsi)
        {
            int code = 0;
            if (mmsi / 100000000 == 0)
            {
                code = mmsi / 10000;
            }
            else if (mmsi / 10000000 == 99)
            {
                mmsi -= 990000000;
                code = mmsi / 10000;
            }
            else
            {
                code = mmsi / 1000000;
            }
            if (AISMessageBase.NationNumber.ContainsKey(code))
            {
                return  code;
            }
            else
            {
                return 999;
            }
        }
        /// <summary>
        /// messageID를 파싱해서 반환
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Message ID</returns>
        public static byte ParsingMessageID(byte[] message)
        {

            return BitParser<MyByte>.Parsing(message, 0, 6).val;
        }

        /// <summary>
        /// Cago type 을 문자열로 변환
        /// </summary>
        /// <param name="cafoType">카고 타입</param>
        /// <returns></returns>
        public static string ConvertCargoType(int cagoType)
        {
            string result;
            if (cagoType > 99 && cagoType < 200)
            {
                return "reserved, for regional use";
            }
            else if (cagoType >= 200)
            {
                return "reserved, for future use";
            }

            if (cagoType >= 50 && cagoType <= 59)
            {
                result = CagoTypeSpecial[cagoType - 50];
            }
            else if (cagoType == 0)
            {
                result = "not available or no ship";
            }
            else
            {
                int firstDigit = cagoType / 10;
                int secondDigit = cagoType % 10;
                if (firstDigit == 3)
                {
                    result = "Vessel=" + CargoTypeSecondVessel[secondDigit];
                }
                else
                {
                    result = CargoTypeFirst[firstDigit] + "-" + CargoTypeSecond[secondDigit];
                }
            }
            return result;
        }
        /// <summary>
        /// 6bit 아스키코드 문자열로 변환
        /// </summary>
        /// <param name="message">변환할 메시지</param>
        /// <param name="length">변환할 문자열의 길이</param>
        /// <returns>6bit 아스키 문자열</returns>
        public static string Convert6bitAscII(byte[] message, int length)
        {
            bool isStart = false;
            int nullCount = 0;
            string messageStr = "";
            bool isEmpty = true;
            for (int i = 0; i < length; i++)
            {
                int tmp = (int)message[i];
                if (nullCount >= 2)
                {
                    isStart = false;
                }
                if (AISMessageBase.SixBitASCII[tmp] == '@')
                {
                    break;
                }
                else if (AISMessageBase.SixBitASCII[tmp] == ' ')
                {
                    nullCount++;
                    if (!isStart)
                    {
                        continue;
                    }
                }
                else
                {
                    nullCount = 0;
                    isStart = true;
                    isEmpty = false;
                }
                messageStr += AISMessageBase.SixBitASCII[tmp].ToString();
            }
           
            if (isEmpty)
            {
                return "";
            }
            return messageStr;
        }
        /// <summary>
        /// 들어온 메시지를 파싱
        /// </summary>
        /// <param name="message"></param>
        virtual public void Parsing(byte[] message)
        {
            ReceivingDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            MessageId = BitParser<MyByte>.Parsing(message, 0, 6).val;
            Repeatindicator = BitParser<MyByte>.Parsing(message, 6, 2).val;
            UserId = BitParser<MyUint>.Parsing(message, 8, 30).val;
            MID = MIDParsingInMMSI((int)UserId);
        }

        #endregion
    }
}
