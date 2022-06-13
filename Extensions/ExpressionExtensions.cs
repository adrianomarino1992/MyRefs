using System.Linq.Expressions;
using System.Reflection;

namespace MyRefs.Extensions
{
    public static class ExpressionExtensions
    {
        public static ExpressionType GetExpressionType(this Expression exp)
        {
            return exp.NodeType;
        }

        public static bool IsPredicate(this Expression exp)
        {
            if (exp.NodeType == ExpressionType.MemberAccess && exp is MemberExpression mExp)
            {
                return (mExp.Member as PropertyInfo)?.PropertyType == typeof(bool);
            }

            if (exp.NodeType == ExpressionType.Call && exp is MethodCallExpression mtExp)
            {
                return mtExp.Method.ReturnType == typeof(bool);
            }

            if (exp.NodeType == ExpressionType.Lambda && exp is LambdaExpression lExp)
            {
                return lExp.ReturnType == typeof(bool);
            }

            return false;
        }

        public static bool IsMemberAcess(this Expression exp)
        {
            return ((exp as LambdaExpression)?.Body as MemberExpression) != null;
        }

        public static PropertyInfo? GetPropertyInfoOfExpression(this Expression expression)
        {
            MemberExpression? m = ((expression as LambdaExpression)?.Body as MemberExpression);

            if (m == null)
                return null;

            return m.Member as PropertyInfo;

        }
    }
}
