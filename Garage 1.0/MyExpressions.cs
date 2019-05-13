using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Garage_1._0
{
    class MyExpressions : IFilter<TClass> where TClass : class
    {
        public List<IFilterStatement> Statements { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Expression<Func<TClass, bool>> BuildExpression()
        {
            throw new NotImplementedException();
        }
    }

    class MyExpressionsStatements : IFilterStatement
    {
        public string PropertyName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Operation Operation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    /// <summary>
    /// Defines a filter from which a expression will be built.
    /// </summary>
    public interface IFilter<TClass> where TClass : class
    {
        /// <summary>
        /// Group of statements that compose this filter.
        /// </summary>
        IEnumerable<IFilterStatement> Statements { get; }
        /// <summary>
        /// Adds another statement to this filter.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property that will be filtered.</param>
        /// <param name="operation">
        /// Express the interaction between the property and the constant value.</param>
        /// <param name="value">
        /// Constant value that will interact with the property.</param>
        /// <param name="connector">
        /// Establishes how this filter statement will connect to the next one.</param>
        /// <returns>A FilterStatementConnection object that 
        /// defines how this statement will be connected to the next one.</returns>
        IFilterStatementConnection<TClass> By<TPropertyType>
            (string propertyName, Operation operation, TPropertyType value,
            FilterStatementConnector connector = FilterStatementConnector.And);
        /// <summary>
        /// Removes all statements from this filter.
        /// </summary>
        void Clear();
        /// <summary>
        /// Builds a LINQ expression based upon the statements included in this filter.
        /// </summary>
        /// <returns></returns>
        Expression<Func<TClass, bool>> BuildExpression();
    }

    public interface IFilterStatementConnection<TClass> where TClass : class
    {
        /// <summary>
        /// Defines that the last filter statement will 
        /// connect to the next one using the 'AND' logical operator.
        /// </summary>
        IFilter<TClass> And { get; }
        /// <summary>
        /// Defines that the last filter statement will connect 
        /// to the next one using the 'OR' logical operator.
        /// </summary>
        IFilter<TClass> Or { get; }
    }

    public enum FilterStatementConnector { And, Or }

    /// <summary>
    /// Defines how a property should be filtered.
    /// </summary>
    public interface IFilterStatement
    {
        /// <summary>
        /// Establishes how this filter statement will connect to the next one.
        /// </summary>
        FilterStatementConnector Connector { get; set; }
        /// <summary>
        /// Name of the property (or property chain).
        /// </summary>
        string PropertyName { get; set; }
        /// <summary>
        /// Express the interaction between the property and
        /// the constant value defined in this filter statement.
        /// </summary>
        Operation Operation { get; set; }
        /// <summary>
        /// Constant value that will interact with
        /// the property defined in this filter statement.
        /// </summary>
        object Value { get; set; }
    }
    public enum Operation
    {
        EqualTo,
        Contains,
        StartsWith,
        EndsWith,
        NotEqualTo,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo
    }
}