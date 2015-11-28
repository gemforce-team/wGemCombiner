﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WGemCombiner.Resources {
    using System;
    
    
    /// <summary>
    ///   Classe di risorse fortemente tipizzata per la ricerca di stringhe localizzate e così via.
    /// </summary>
    // Questa classe è stata generata automaticamente dalla classe StronglyTypedResourceBuilder.
    // tramite uno strumento quale ResGen o Visual Studio.
    // Per aggiungere o rimuovere un membro, modificare il file con estensione ResX ed eseguire nuovamente ResGen
    // con l'opzione /str oppure ricompilare il progetto VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Restituisce l'istanza di ResourceManager nella cache utilizzata da questa classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WGemCombiner.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Esegue l'override della proprietà CurrentUICulture del thread corrente per tutte le
        ///   ricerche di risorse eseguite utilizzando questa classe di risorse fortemente tipizzata.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Paste gem combining equations or parenthesis formula into the text box, then click the &apos;Parse custom recipe&apos; button.
        ///
        ///Set the &apos;delay&apos; to at LEAST as many milliseconds as a frame on GC2 is taking. I recommend at least 45-50 for no lag (it will usually work with much lower, but going any lower than the frame time will not actually speed up the process, as the game will only do one step per frame).
        ///
        ///Game gem info tooltips have to be hidden during the combine.
        ///The combiner will automatically hide them and  [stringa troncata]&quot;;.
        /// </summary>
        internal static string HelpCombinerMessage {
            get {
                return ResourceManager.GetString("HelpCombinerMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Working with the combiner - custom recipes.
        /// </summary>
        internal static string HelpCombinerTitle {
            get {
                return ResourceManager.GetString("HelpCombinerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Contributors (alphabetic order):
        ///- 12345ieee: some spec parsing, preset recipes
        ///- CooLTanG: automate the &apos;Get Instructions&apos; &amp; &apos;Combine&apos; steps, fix window size/resolution issues, customizable hotkey
        ///- Hellrage: some GUI tweaks, skin, small bug fixes, preset and resource management
        ///- Suuper: original author, initial idea, main program logic
        ///- RobinHood70: code cleaning and revamping; partial localization support
        ///
        ///
        ///Release version: {0}
        ///
        ///To report a problem, see
        ///https://github.com/gemforce-team/wGemC [stringa troncata]&quot;;.
        /// </summary>
        internal static string HelpCreditsMessage {
            get {
                return ResourceManager.GetString("HelpCreditsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Credits.
        /// </summary>
        internal static string HelpCreditsTitle {
            get {
                return ResourceManager.GetString("HelpCreditsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a In debug mode you can import new presets from a .txt file with recipes.
        ///
        ///Your txt must contain one recipe per line. The recipes have to be valid parenthesis formulas (not equations).
        ///The parser is somewhat robust but i wouldn&apos;t test it too hard :)
        ///
        ///To start importing, choose the &apos;Import...&apos; option from the Preset drop-down, then choose the .txt file with recipes..
        /// </summary>
        internal static string HelpImportingPresetsMessage {
            get {
                return ResourceManager.GetString("HelpImportingPresetsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Importing New Presets.
        /// </summary>
        internal static string HelpImportingPresetsTitle {
            get {
                return ResourceManager.GetString("HelpImportingPresetsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Example of combining equations:
        ///(val = 1)	0 = g1 orange
        ///(val = 2)	1 = 0 + 0
        ///(val = 3)	2 = 1 + 0
        ///Works if you have first gem as &apos;1&apos; and second as &apos;2&apos;, etc, as well.
        ///
        ///Example of parenthesis formula:
        ///(2+1)+1
        ///or
        ///(1+0)+0
        ///or
        ///(2m+m)+m
        ///If zeros are present they are treated as 1s and 1s as 2s..
        /// </summary>
        internal static string HelpInputFormatMessage {
            get {
                return ResourceManager.GetString("HelpInputFormatMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Input Formats.
        /// </summary>
        internal static string HelpInputFormatTitle {
            get {
                return ResourceManager.GetString("HelpInputFormatTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Preset schemes are supported.
        ///To use one, first select a color + combine/spec from the top drop-down list.
        ///
        ///After selecting a color + combine/spec, the second drop-down list will display the preset options. First number is number of base gems, second number is the growth rate. Ones marked with a &apos;-&apos; are 2^n combines [over 8], and are rarely the best for growth rate up to that cost, they are included for convenience if you want easily comparable gem costs.
        ///Selecting a preset will change the textbox&apos;s tex [stringa troncata]&quot;;.
        /// </summary>
        internal static string HelpPresetsMessage {
            get {
                return ResourceManager.GetString("HelpPresetsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Presets.
        /// </summary>
        internal static string HelpPresetsTitle {
            get {
                return ResourceManager.GetString("HelpPresetsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Gem Combiner now supports speccing.
        ///To spec, place base gems of different color in 1A, 1B, and 1C.
        ///Order of colors, starting at 1A, should be: orange/yellow, black, red.
        ///Speccing provides another way of squeezing red out of a gem. Simply select your mana/kill gem combine and replace one of the &apos;k&apos; or &apos;m&apos; with another (valid) color&apos;s letter. Be sure you have your two gems in the proper slots, though!
        ///
        ///EXAMPLE:
        ///Pick Mana Gem Spec
        ///Choose the default 8 pattern
        ///Put orange grade 1 in 1A (bottom right slot [stringa troncata]&quot;;.
        /// </summary>
        internal static string HelpSpeccingMessage {
            get {
                return ResourceManager.GetString("HelpSpeccingMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Speccing.
        /// </summary>
        internal static string HelpSpeccingTitle {
            get {
                return ResourceManager.GetString("HelpSpeccingTitle", resourceCulture);
            }
        }
    }
}