using System.ComponentModel.DataAnnotations;

namespace Arrba.Domain.Models
{
    public enum ActiveStatus
    {
        /// <summary>
        ///     Inactive if you do not want to show or be able
        /// </summary>
        inactive,

        /// <summary>
        ///     Active
        /// </summary>
        active
    }

    /// <summary>
    /// For phone numbers
    /// </summary>
    public enum PriorityStatus
    {
        @default,
        main,
        secondary
    }

    public enum WatchWeightStatus
    {
        [Display(Name = "Не следить")]
        inactive,
        [Display(Name = "Следить за кликами")]
        active,
        [Display(Name = "Постоянно в топе")]
        solid
    }

    /// <summary>
    /// "Добавить в фильтр"
    /// </summary>
    public enum AddToFilter
    {
        [Display(Name = "Убрать отовсюду")]
        RemovedFromEveryWhere,
        [Display(Name = "Отображать только в фильтре")]
        AddedToFilterOnly,
        [Display(Name = "Отображать только при подаче объявления")]
        AddedToAdOnly,
        [Display(Name = "Отображать везде")]
        AddedToEveryWhere
    }

    /// <summary>
    /// "Добавить в объявление"
    /// </summary>
    public enum AddToCard
    {
        [Display(Name = "Не добавлять в карточку объявления")]
        RemoveFromCard,
        [Display(Name = "Добавить в карточку объявления (вверх и вниз)")]
        AddToCardBoth,
        [Display(Name = "Добавить в карточку объявления (только вверх)")]
        AddToTopOnly,
        [Display(Name = "Добавить в карточку объявления (только вниз)")]
        AddToBottomOnly
    }

    public enum ControlTypeEnum
    {
        [Display(Name = "Одно текстовое значение")]
        Value,
        [Display(Name = "Значение от и до")]
        ValueFromTo,
        [Display(Name = "Списком")]
        Select,
        [Display(Name = "Чекбокс")]
        CheckBox
    }

    public enum CurrencyBaseStatus
    {
        [Display(Name = "Не основная валюта")]
        NotBase,
        [Display(Name = "Основная валюта")]
        IsBase
    }


    public enum VehicleCondition
    {
        // TODO change Undefined to null later
        Undefined = 0,
        New = 1,
        Used = 2,
        Crashed = 3
        //Refobished
    }
}