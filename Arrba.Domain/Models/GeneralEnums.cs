using System;
using System.ComponentModel.DataAnnotations;

namespace Arrba.Domain.Models
{
    public enum UserAuthStatus
    {
        [Display(Name = "Анонимный пользователь подавший объявление")] Anonymous,
        [Display(Name = "Зарегистрированный пользователь")] Authorized
    }

    public enum CommentRestriction
    {
        [Display(Name = "Все могут комментировать")] EveryBody = 0,
        [Display(Name = "Могут комментировать только зарегистрированные пользователи")] RegisterdUserOnly = 1,
        [Display(Name = "Запретить комментарии к объявлению")] NoBody = 3
    }

    /// <summary>
    ///     Использутеся для того чтобы понять указал ли пользователь свою модель или выбрал из списка
    /// </summary>
    public enum AdNeedModelVerification
    {
        [Display(Name = "Проверка модели/марки не обязательна, пользователь всё указал")] Ok,
        [Display(Name = "Пользователь указал свой вариант модели/марки, нужно проверить")] Required
    }

    public enum ModirationStatus
    {
        /// <summary>
        ///     Everything is good no need modiraiton varification, already verified
        /// </summary>
        [Display(Name = "Всё хорошо, проверен или не требует модерации")] Ok,

        /// <summary>
        ///     Requred varification
        /// </summary>
        [Display(Name = "Нужно проверить")] Required
    }

    //public enum AdImageStatus
    //{
    //    [Display(Name = "Не основная фотография объявления")]
    //    NotMain,
    //    [Display(Name = "Основная фотография объявления")]
    //    Main
    //}

    public enum ServiceEnum
    {
        /// <summary>
        ///     Неактивный
        /// </summary>
        [Display(Name = "Неактивный")] Inactive = 0,

        /// <summary>
        ///     Поднять
        /// </summary>
        [Display(Name = "Поднять")] ToGetUpper = 1,

        /// <summary>
        ///     В горячие
        /// </summary>
        [Display(Name = "В горячие")] IsHot = 2,

        /// <summary>
        ///     Срочно, торг
        /// </summary>
        [Display(Name = "Срочно, торг")] HurryAuction = 3,

        /// <summary>
        ///     Покрасить
        /// </summary>
        [Display(Name = "Покрасить")] ToColorize = 4,

        /// <summary>
        ///     Продление объявления
        /// </summary>
        [Display(Name = "Продлить объявление")] AdToLong = 5,

        /// <summary>
        ///     Больше фотографий
        /// </summary>
        [Display(Name = "Больше фотографий")] MorePhotos = 6
    }

    public enum AdStatus
    {
        /// <summary>
        ///     User doesn't touch it everything ok
        /// </summary>
        [Display(Name = "Действует")] IsOk = 0,
        //InArchive = 1,
        /// <summary>
        ///     Deleted by user
        /// </summary>
        [Display(Name = "Удален")] Deleted = 2,

        /// <summary>
        ///     Deleted by user and removed into history store database
        /// </summary>
        [Display(Name = "Удалить невсегда")] DeleteForever = 3
    }

    [Flags]
    public enum SuperCategType
    {
        [Display(Name = "Автомобили")] Car = 2,
        [Display(Name = "Рекреационная техника")] Vehicle = 4,
        [Display(Name = "Услуги")] Service = 8,
        [Display(Name = "АКСЕССУАРЫ И АВТОХИМИЯ")] Chemistry = 16,
        [Display(Name = "Запчасти")] RepairParts = 32,
        [Display(Name = "Магазины")] Shop = 64
    }

    public enum BalanceTransactionType
    {
        /// <summary>
        ///     Payment get in. Addition
        ///     |Прибавить
        /// </summary>
        [Display(Name = "Прибавить сумму")] Add = 1,

        /// <summary>
        ///     Payment used or get out, substract |
        ///     Отнять
        /// </summary>
        [Display(Name = "Отнять сумму")] Substract = 2
    }
}