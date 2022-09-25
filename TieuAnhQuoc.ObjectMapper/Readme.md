# How do I get started?
#### Install
    Install-Package TieuAnhQuoc.ObjectMapper

#### Mapping Object/List
    var dto = entity.ProjectTo<TEntity, TDto>()
    var dtos = entities.ProjectTo<TEntity, TDto>()