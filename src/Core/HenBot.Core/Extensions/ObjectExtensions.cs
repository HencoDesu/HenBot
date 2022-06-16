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
}