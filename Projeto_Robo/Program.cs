using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace Projeto_Robo
{
    class Program
    {
        static SpeechRecognitionEngine engine = null;
        static SpeechSynthesizer sp = null;
        static bool skyOuvindo = true;

        static void Main(string[] args)
        {

            engine = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pt-BR")); //Instancia e idioma
            engine.SetInputToDefaultAudioDevice(); //Microfone
            sp = new SpeechSynthesizer();

            #region Comandos conversação
            string[] conversas = { "Olá", "Boa Noite", "Boa Tarde", "Bom Dia", "Tudo Bem", "SKY", "Qual o seu nome" };
            Choices c_conversas = new Choices(conversas);
            GrammarBuilder gb_conversas = new GrammarBuilder();
            gb_conversas.Append(c_conversas);
            Grammar g_conversas = new Grammar(gb_conversas);
            g_conversas.Name = "conversas";
            #endregion

            #region Comandos do sistema
            string[] comandosSistema = { "Que horas são", "Que dia é hoje", "Abrir calculadora", "Abrir chrome" };
            Choices c_comandosSistema = new Choices(comandosSistema);
            GrammarBuilder gb_comandosSistemas = new GrammarBuilder();
            gb_comandosSistemas.Append(c_comandosSistema);
            Grammar g_comandosSistema = new Grammar(gb_comandosSistemas);
            g_comandosSistema.Name = "sistema";
            #endregion

            #region Inicia Sky            
            Choices c_comandosAtivaSky = new Choices();
            c_comandosAtivaSky.Add(skyAtiva.ToArray());
            GrammarBuilder gb_comandosAtivaSky = new GrammarBuilder();
            gb_comandosAtivaSky.Append(c_comandosAtivaSky);
            Grammar g_comandosAtivaSky = new Grammar(gb_comandosAtivaSky);
            g_comandosAtivaSky.Name = "ativaSky";
            #endregion

            #region Selencia Sky            
            Choices c_comandosDesativaSky = new Choices();
            c_comandosDesativaSky.Add(skyDesativa.ToArray());
            GrammarBuilder gb_comandosDesativaSky = new GrammarBuilder();
            gb_comandosDesativaSky.Append(c_comandosDesativaSky);
            Grammar g_comandosDesativaSky = new Grammar(gb_comandosDesativaSky);
            g_comandosDesativaSky.Name = "desativaSky";
            #endregion


            Console.WriteLine("=========================================================");
            Speak("Carregando sistema");
            Thread.Sleep(5000);
            engine.LoadGrammar(g_comandosSistema);
            engine.LoadGrammar(g_conversas);
            engine.LoadGrammar(g_comandosAtivaSky);
            engine.LoadGrammar(g_comandosDesativaSky);
            Console.WriteLine("==========================================================");
            engine.SpeechRecognized += rec;
            Speak("Sistema carregado com sucesso!!! \n Em que posso te ajudar! Meu nome é SKY");
            Console.WriteLine("\nEstou ouvindo.....");

            engine.RecognizeAsync(RecognizeMode.Multiple); //Inicia reconhecimento
            //sp.SelectVoiceByHints(VoiceGender.NotSet);
            Console.ReadKey();

        }



        //Matodo que é chamado quando a voz é reconhecida
        private static void rec(object s, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence >= 0.4f)
            {
                string speech = e.Result.Text;
                Console.WriteLine("Você disse: " + speech + " |||  Confiança: " + e.Result.Confidence);
                switch (e.Result.Grammar.Name)
                {
                    case "conversas":
                        processConversa(speech);
                        break;
                    case "sistema":
                        processSistema(speech);
                        break;
                    case "ativaSky":
                        processAtivaSky(speech);
                        break;
                    case "desativaSky":
                        processDesativaSky(speech);
                        break;
                }
            }
            else
            {
                Speak("Não entedi sua voz claramente. Repita por favor");
            }
        }
        private static void Speak(string text)
        {
            sp.SpeakAsyncCancelAll();
            sp.SpeakAsync(text);
        }

        private static void processAtivaSky(string conversa)
        {
            skyOuvindo = true;
        }

        private static void processDesativaSky(string conversa)
        {
            skyOuvindo = false;
        }

        private static void processConversa(string conversa)
        {
            if (skyOuvindo == true)
            {
                switch (conversa)
                {
                    case "SKY":
                        List<string> resp = new List<string> { "Estou a disposição, em que posso lhe ajudar?", "Estou aqui", "Fala ai!" };
                        Speak(RespostasSky(resp));
                        break;
                    case "Qual o seu nome":
                        Speak("Meu nome é SKY, muito prazer em lhe conhecer");
                        break;
                    case "Olá":
                        Speak("Olá, como vai você?");
                        break;
                    case "Boa Noite":
                        Speak("Boa Noite, como vai?");
                        break;
                    case "Boa Tarde":
                        Speak("Boa Tarde, como vai?");
                        break;
                    case "Bom Dia":
                        Speak("Bom Dia, como vai?");
                        break;
                    case "Tudo Bem":
                        Speak("Estou bem obrigado e você como esta?");
                        break;
                    default:
                        break;
                }
            }
        }

        private static void processSistema(string comando)
        {           
            if (skyOuvindo == true)
            {
                switch (comando)
                {
                    case "Que horas são":
                        var hora = DateTime.Now.ToShortTimeString();
                        Speak(DateTime.Now.ToShortTimeString());
                        break;
                    case "Que dia é hoje":
                        Speak(DateTime.Today.ToShortDateString());
                        break;
                    case "Abrir calculadora":
                        Speak("Irei abrir a calculadora só um momento por favor");
                        System.Diagnostics.Process.Start("Calc.exe");
                        break;
                    case "Abrir chrome":
                        Speak("Irei abrir o chrome só um momento por favor");
                        System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
                        break;
                    default:
                        break;
                }
            }
        }

        #region 
        public static IList<string> skyDesativa = new List<string>()
        {
            "Pare de ouvir",
            "Pare de me ouvir",
            "Fique queta"
        };

        public static IList<string> skyAtiva = new List<string>()
        {
            "SKY você esta ai?",
            "Olá SKY",
            "Oi SKY"
        };

        #endregion

        public static string RespostasSky(List<string> Resposta)
        {
            //IList<string> Resposta = new List<string>() {resposta};
            Random rnd = new Random();
            var valorAleatorio = Resposta[rnd.Next(Resposta.Count)];

            return valorAleatorio;
        }
    }
}
