namespace SelectFile
{
    public class Rule
    {
        /// <summary>
        /// 修改类型，编辑或删除
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// 正则匹配
        /// </summary>
        public string RegexRule { get; set; }
        /// <summary>
        /// 固定字段匹配
        /// </summary>
        public string Str { get; set; }
        /// <summary>
        /// 替换字段
        /// </summary>
        public string RepalceStr { get; set; }
    }

    public enum Type
    {
        Repalce = 1,
        Delete = 2
    }
}
