using System.Linq.Expressions;
using System.Reflection;

namespace HenBot.Core.Extensions;

public static class ObjectExtensions
{
	public static void SetPropertyValue<TObject, TProperty>(
		this TObject @object,
		Expression<Func<TObject, TProperty>> propertyExpression,
		TProperty propertyValue)
	{
		if (propertyExpression.Body is MemberExpression { Member: PropertyInfo property })
		{
			property.SetValue(@object, propertyValue);
		}
	}

	public static TObject FluentAction<TObject>(this TObject @object, Action<TObject> action)
	{
		action(@object);
		return @object;
	}

	public static Task<TObject> AsTask<TObject>(this TObject @object)
		=> Task.FromResult(@object);
}