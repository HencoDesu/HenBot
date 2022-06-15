using HenBot.Core.Modules;
using JetBrains.Annotations;

namespace HenBot.Core;

/// <summary>
/// Билдер, позволяющий настроить бота
/// </summary>
[PublicAPI]
public interface IBotBuilder
{
	/// <summary>
	/// Подключает к боту модуль
	/// </summary>
	/// <typeparam name="TModule">Тип модуля</typeparam>
	/// <returns>Текущий экземпляр <see cref="IBotBuilder"/></returns>
	IBotBuilder UseModule<TModule>()
		where TModule : class, IModule;
}