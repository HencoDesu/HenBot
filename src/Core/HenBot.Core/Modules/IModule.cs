using JetBrains.Annotations;

namespace HenBot.Core.Modules;

/// <summary>
/// Абстракция для модулей бота
/// </summary>
[PublicAPI]
public interface IModule
{
	/// <summary>
	/// Метод вызывается при инициализации бота.
	/// Позволяет зарегистрировать все сервисы и команды используемые модулем.
	/// </summary>
	/// <param name="moduleBuilder">Экземпляр <see cref="IModuleBuilder"/> созданный для инициализации модуля</param>
	static abstract void Init(IModuleBuilder moduleBuilder);
}