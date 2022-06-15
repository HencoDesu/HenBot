using JetBrains.Annotations;

namespace HenBot.Core.Commands.Parsing;

/// <summary>
/// Конфигуратор разбора данных для <see cref="BaseCommand{TData}"/>
/// </summary>
/// <typeparam name="TData">Тип данных команды</typeparam>
public interface IDataPropertyBuilder<in TData>
{
	/// <summary>
	/// Заполнить данными
	/// </summary>
	/// <param name="data">Текущий объект данных</param>
	/// <param name="args">Аргументы команды</param>
	/// <returns></returns>
	IEnumerable<string> FillData(TData data, string[] args);
}

/// <summary>
/// Конфигуратор разбора данных для <see cref="BaseCommand{TData}"/>
/// </summary>
/// <typeparam name="TData">Тип данных команды</typeparam>
/// <typeparam name="TProperty">Тип свойства для которого задается конфигурация</typeparam>
[PublicAPI]
public interface IDataPropertyBuilder<in TData, TProperty> : IDataPropertyBuilder<TData>
{
	/// <summary>
	/// Задает для свойства значение по умолчанию
	/// </summary>
	/// <param name="defaultValue"></param>
	/// <returns>Текущий экземпляр <see cref="IDataPropertyBuilder{TData,TProperty}"/></returns>
	IDataPropertyBuilder<TData, TProperty> HasDefaultValue(TProperty defaultValue);

	/// <summary>
	/// Добавляет функцию валидации для свойства
	/// </summary>
	/// <param name="validationFunc">Функция валидации</param>
	/// <param name="failMessage">Сообщение об ошибке, если валидация не была пройдена</param>
	/// <returns>Текущий экземпляр <see cref="IDataPropertyBuilder{TData,TProperty}"/></returns>
	IDataPropertyBuilder<TData, TProperty> Validate(Func<TProperty, bool> validationFunc, string failMessage);

	/// <summary>
	/// Обработка исключений при задании свойства
	/// </summary>
	/// <param name="message">Сообщение об ошибке, если произошло исключение</param>
	/// <typeparam name="TException">Тип исключения</typeparam>
	/// <returns>Текущий экземпляр <see cref="IDataPropertyBuilder{TData,TProperty}"/></returns>
	IDataPropertyBuilder<TData, TProperty> HandleException<TException>(string message) 
		where TException : Exception;

	/// <summary>
	/// Задает функцию для парсинга свойства
	/// </summary>
	/// <param name="mapFunc">Функция для парсинга свойства</param>
	/// <returns>Текущий экземпляр <see cref="IDataPropertyBuilder{TData,TProperty}"/></returns>
	IDataPropertyBuilder<TData, TProperty> MapFrom(Func<string[], TProperty> mapFunc);
}