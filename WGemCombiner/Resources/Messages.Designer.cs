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
        ///   Cerca una stringa localizzata simile a You can cancel the combine by pressing the &apos;Escape&apos; key (you may have to hold it for a second with lag). Using a delay lower than GC2&apos;s frame time will cause input to be ahead of what you see happening, and so canceling may appear to not work. If your cursor has stopped moving, the program has stopped.
        ///
        ///If you need to save a specific amount of space on the top, lower the Slot Limit box, though this will require more moves to complete the same combine, sometimes significantly.
        ///
        ///Game gem info tooltips hav [stringa troncata]&quot;;.
        /// </summary>
        internal static string HelpCombiner2Message {
            get {
                return ResourceManager.GetString("HelpCombiner2Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Working with the combiner (2).
        /// </summary>
        internal static string HelpCombiner2Title {
            get {
                return ResourceManager.GetString("HelpCombiner2Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Select the desired kind of recipe (&quot;Mana Spec&quot; for a managem, etc...) and an affordable combine - the first number is the number of basegems used for the combine.
        ///After performing a Spec and an Amp recipe, you don&apos;t need to use that anymore, after this first step use only &quot;Combine&quot; recipes on these initially created gems. Again, the first number indicates the amount of basegems needed. You can use that to calculate the manacost.
        ///
        ///After choosing the recipe, you switch to the game and perform the following [stringa troncata]&quot;;.
        /// </summary>
        internal static string HelpCombinerMessage {
            get {
                return ResourceManager.GetString("HelpCombinerMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Working with the combiner (1).
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
        ///- RobinHood70: code cleaning and revamping; partial localization support
        ///- Suuper: original author, initial idea, main program logic
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
        ///   Cerca una stringa localizzata simile a To import new recipes, open or create the &quot;recipes.txt&quot; file in the same folder as your executable. Either parenthetical or equation recipes can be used, with equation recipes requiring a blank line between them.
        ///Comments can also be inserted on separate lines beginning with # or //..
        /// </summary>
        internal static string HelpImportingPresetsMessage {
            get {
                return ResourceManager.GetString("HelpImportingPresetsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Adding New Recipes.
        /// </summary>
        internal static string HelpImportingPresetsTitle {
            get {
                return ResourceManager.GetString("HelpImportingPresetsTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a You can also paste gem combining equations or a parenthesis formula into the text box, then click one of the &apos;Parse recipe&apos; buttons.
        ///
        ///Example of equation format:
        ///0 = o
        ///1 = 0 + 0
        ///2 = 1 + 0
        ///3 = 2 + 0
        ///
        ///Example of parenthesis format (same recipe as above):
        ///(2o+o)+o
        ///
        ///Equations are faster to process, though this will only be noticeable with large recipes or large numbers of recipes in the recipes.txt file..
        /// </summary>
        internal static string HelpInputFormatMessage {
            get {
                return ResourceManager.GetString("HelpInputFormatMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a Input recipes directly.
        /// </summary>
        internal static string HelpInputFormatTitle {
            get {
                return ResourceManager.GetString("HelpInputFormatTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Cerca una stringa localizzata simile a To use one of the built-in recipes, first select a color + combine/spec from the top drop-down list.
        ///
        ///After selecting a color + combine/spec, the second drop-down list will display the preset options. The first number is number of base gems; the second number is the growth rate. Those marked with a &apos;*&apos; are 2^n combines, and are rarely the best for growth rate up to that cost. They are mostly included for convenience if you want easily comparable gem costs.
        ///
        ///Selecting a preset will display the recipe in  [stringa troncata]&quot;;.
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
    }
}
