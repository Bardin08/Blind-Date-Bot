﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BlindDateBot {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BlindDateBot.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
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
        ///   Ищет локализованную строку, похожую на Ты уже зарегистрирован!.
        /// </summary>
        internal static string AlreadyRegisteredUser {
            get {
                return ResourceManager.GetString("AlreadyRegisteredUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Ошибка распознавание команды.
        /// </summary>
        internal static string CommandNotFoundMessage {
            get {
                return ResourceManager.GetString("CommandNotFoundMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Твой пол: {0},
        ///Пол собеседника: {1}.
        /// </summary>
        internal static string ConfirmData {
            get {
                return ResourceManager.GetString("ConfirmData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Свидание завершено. Спасибо. 
        ///
        ///Хочешь пообщаться ещё с кем-нибудь? Испльзуй команду /next_date..
        /// </summary>
        internal static string DateEnd {
            get {
                return ResourceManager.GetString("DateEnd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Свидание найдено!🥰🤩.
        /// </summary>
        internal static string DateHasBegan {
            get {
                return ResourceManager.GetString("DateHasBegan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Начинаю поиск свидания…⏱🤔.
        /// </summary>
        internal static string DateSearchText {
            get {
                return ResourceManager.GetString("DateSearchText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Напиши, что ты думаешь об опыте использования бота. Спасибо!
        ///Твое следующее сообщение будет переслано студсовету..
        /// </summary>
        internal static string FeedbackInitiated {
            get {
                return ResourceManager.GetString("FeedbackInitiated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Найти свидание.
        /// </summary>
        internal static string FindDate {
            get {
                return ResourceManager.GetString("FindDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на В этом боте тебе доступны такие команды:
        ////next_date – переключит тебя на следующего собеседника;
        ////end_date – позволяет закончить свидание;
        ////feedback – тут можно оставить свои впечатления после использования бота..
        /// </summary>
        internal static string HelpMessage {
            get {
                return ResourceManager.GetString("HelpMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Я девушка 👩.
        /// </summary>
        internal static string IFemale {
            get {
                return ResourceManager.GetString("IFemale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Я парень🧑.
        /// </summary>
        internal static string IMale {
            get {
                return ResourceManager.GetString("IMale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Девушки 🙍‍♀️.
        /// </summary>
        internal static string InterlocutorFemale {
            get {
                return ResourceManager.GetString("InterlocutorFemale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Парни🙍‍♂️.
        /// </summary>
        internal static string InterlocutorMale {
            get {
                return ResourceManager.GetString("InterlocutorMale", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Sorry, smth went wrong. Try to use /start command again..
        /// </summary>
        internal static string InternalErrorUserNotFound {
            get {
                return ResourceManager.GetString("InternalErrorUserNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Нет❌.
        /// </summary>
        internal static string No {
            get {
                return ResourceManager.GetString("No", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на У тебя сейчас нет активных свиданий, нельзя отправить жалобу просто так!.
        /// </summary>
        internal static string NoActiveDate {
            get {
                return ResourceManager.GetString("NoActiveDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Регистрация прошла успешно. Скоро ты сможешь познакомиться с человеком..
        /// </summary>
        internal static string RegistrationComplete {
            get {
                return ResourceManager.GetString("RegistrationComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Привет! Этот бот создан для анонимного общения в День Святого Валентина. Укажи свой пол и пол человека, с которым хочешь поговорить..
        /// </summary>
        internal static string RegistrationInitMessage {
            get {
                return ResourceManager.GetString("RegistrationInitMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Опиши, пожалуйста, причину жалобы *одним сообщением*..
        /// </summary>
        internal static string ReportInitiated {
            get {
                return ResourceManager.GetString("ReportInitiated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Кто тебе интересен?.
        /// </summary>
        internal static string SelectInterlocuterGender {
            get {
                return ResourceManager.GetString("SelectInterlocuterGender", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Кто-то на тебя пожаловался!.
        /// </summary>
        internal static string SomebodyComplainedAboutYou {
            get {
                return ResourceManager.GetString("SomebodyComplainedAboutYou", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Smth went wrong. Try again.
        /// </summary>
        internal static string SomethingWentWrong {
            get {
                return ResourceManager.GetString("SomethingWentWrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Да✅.
        /// </summary>
        internal static string Yes {
            get {
                return ResourceManager.GetString("Yes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на У тебя уже идет свидание. Если хочешь найти новое сначала заверши это командой /end_date..
        /// </summary>
        internal static string YouHaveAnActiveDate {
            get {
                return ResourceManager.GetString("YouHaveAnActiveDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на У тебя сейчас нет активного свидания, используй команду /next_date, чтобы найти его..
        /// </summary>
        internal static string YouHaventAnActiveDate {
            get {
                return ResourceManager.GetString("YouHaventAnActiveDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на На тебя поступило слишком много жалоб, поэтому мы решили временно заблокировать твой аккаунт..
        /// </summary>
        internal static string YourAccountBlocked {
            get {
                return ResourceManager.GetString("YourAccountBlocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Твой аккаунт заблокирован. Ожидай снятия бана..
        /// </summary>
        internal static string YourAccountIsBlocked {
            get {
                return ResourceManager.GetString("YourAccountIsBlocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Жалоба отправленна.  .
        /// </summary>
        internal static string YourReportSent {
            get {
                return ResourceManager.GetString("YourReportSent", resourceCulture);
            }
        }
    }
}
