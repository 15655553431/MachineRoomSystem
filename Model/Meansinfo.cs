using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    //用来存储菜单信息的,管理员类型如果比较多，可以直接在数据库中建表
    public class Meansinfo
    {
        public Meansinfo(int id, string name, string url, int layer, int pid, int type,string style)
        {
            this.Id = id;
            this.Name = name;
            this.Url = url;
            this.Layer = layer;
            this.PId = pid;
            this.MType = type;
            this.Style = style;
        }

        public int Id;
        public string Name;
        public string Url;
        public int Layer;
        public int PId;
        public int MType;
        public string Style;

    }



}
