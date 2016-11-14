using System.Collections.Generic;
using Data.Fluent.Enums;
using Data.Fluent.Model;
using Data.Fluent.Model.Schema;
using Newtonsoft.Json;

namespace Data.Fluent.Interfaces
{
    public interface IQueryObject
    {
        #region *** Properties ***

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        SqlLanguageType LanguageType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<SelectColumn> SelectColumns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        int? TopValue { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        FromTable FromTable { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<Join> Joins { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<WhereFilter> WhereFilters { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<GroupByColumn> GroupByColumns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<HavingFilter> HavingFilters { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        List<OrderBy> OrderByClauses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        int? SkipValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        int? TakeValue { get; set; }

        #endregion

        #region *** SELECT Fluent Methods ***

        IQueryObject SelectAll();
        IQueryObject Top(int quantity);
        IQueryObject Select(params string[] columns);
        IQueryObject Select(string columnsString, SchemaValueType valueType = SchemaValueType.Preformatted, bool clearExisting = false);
        IQueryObject Select(IEnumerable<string> columns, SchemaValueType valueType = SchemaValueType.Preformatted, bool clearExisting = false);
        IQueryObject Select(IEnumerable<SelectColumn> columns, bool clearExisting = false);

        #endregion

        #region *** FROM Fluent Methods ***

        IQueryObject From(FromTable table);

        #endregion

        #region *** Join Fluent Methods ***

        IQueryObject Join(Join join);
        IQueryObject Join(JoinType joinType, JoinTable toTableName, JoinColumn toColumnName, ComparisonOperatorType comparisonOperator, JoinTable fromTableName, JoinColumn fromColumnName);

        #endregion

        #region *** Where Fluent Methods ***

        IQueryObject Where(FilterColumn column, ComparisonOperatorType comparisonOperator, object value, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or);
        IQueryObject WhereAnd(FilterColumn column, ComparisonOperatorType comparisonOperator, object value);
        IQueryObject WhereOr(FilterColumn column, ComparisonOperatorType comparisonOperator, object value);
        IQueryObject Where(WhereFilter whereFilter);

        #endregion

        #region *** GroupBy Fluent Methods ***

        IQueryObject GroupBy(params string[] columns);
        IQueryObject GroupBy(string columnsString, SchemaValueType valueType = SchemaValueType.Preformatted, bool clearExisting = false);
        IQueryObject GroupBy(IEnumerable<string> columns, SchemaValueType valueType = SchemaValueType.Preformatted, bool clearExisting = false);
        IQueryObject GroupBy(IEnumerable<GroupByColumn> columns, bool clearExisting = false);

        #endregion

        #region *** Having Fluent Methods ***

        IQueryObject Having(FilterColumn column, ComparisonOperatorType comparisonOperator, object value, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or);
        IQueryObject HavingAnd(FilterColumn column, ComparisonOperatorType comparisonOperator, object value);
        IQueryObject HavingOr(FilterColumn column, ComparisonOperatorType comparisonOperator, object value);
        IQueryObject Having(HavingFilter havingFilter);

        #endregion

        #region *** SKIP/TAKE (LIMIT/OFFSET) Fluent Methods ***

        IQueryObject Take(int take);
        IQueryObject Skip(int skip);

        #endregion

        #region *** Utility Methods ***

        string GetQuery(IQueryBuilder builder);

        string ToJson(bool indented = false);

        #region *** ObjectBase Methods ***

        void PopulateFromJson(string value);

        void PopulateFromJson(string value, JsonSerializerSettings settings);

        #endregion

        #endregion
    }
}