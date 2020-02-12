JobEntity = { Id = 0, Name = "", Headpic = "", PrefabName = "", Des = "" }

--这句是重定义元表的索引，就是说有了这句，这个才是一个类
JobEntity.__index = JobEntity;

function JobEntity.New(Id, Name, Headpic, PrefabName, Des)
    local self = { }; --初始化self
    setmetatable(self, JobEntity); --将self的元表设定为Class

    self.Id = Id;
    self.Name = Name;
    self.Headpic = Headpic;
    self.PrefabName = PrefabName;
    self.Des = Des;

    return self;
end