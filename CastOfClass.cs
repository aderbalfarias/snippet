public static class Extensions 
{
    public static T CastT(this object objCast) where T  new()
    {
        if (objCast == null)
            return new T();

        Type target = typeof(T);
        var instance = Activator.CreateInstance(target, false);

        var memberInfos = target
            .GetMembers()
            .Where(w = w.MemberType == MemberTypes.Property)
            .Select(s = s)
            .ToList();

        IListMemberInfo members = memberInfos
            .Where(memberInfo = memberInfos.Select(c = c.Name).Contains(memberInfo.Name))
            .ToList();

        var membersName = objCast
            .GetType()
            .GetMembers()
            .Where(memberInfo = memberInfos.Select(c = c.Name).Contains(memberInfo.Name))
            .Select(s = s.Name)
            .ToList();

        object value;
        PropertyInfo propertyInfo;
        foreach (var member in members.Where(member = membersName.Contains(member.Name)))
        {
            propertyInfo = typeof(T).GetProperty(member.Name);
            value = objCast.GetType()
                .GetProperty(member.Name).GetValue(objCast, null);

            propertyInfo.SetValue(instance, value, null);
        }

        return (T)instance;
    }
}