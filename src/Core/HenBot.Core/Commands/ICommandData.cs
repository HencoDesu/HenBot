using JetBrains.Annotations;

namespace HenBot.Core.Commands;

/// <summary>
/// Интерфейс-маркер показывающий что объект хранит данные для <see cref="BaseCommand{TData}"/>
/// </summary>
[PublicAPI]
public interface ICommandData
{
}