using AutoMapper;

namespace WebTrackr
{
    /// <summary>
    /// Project Mapping Config.
    /// </summary>
    public static class MappingConfig
    {
        /// <summary>
        /// Registers the AutoMapper Mappings.
        /// </summary>
        public static void RegisterMaps()
        {
            Mapper.Initialize(config =>
            {

            });
        }
    }
}
