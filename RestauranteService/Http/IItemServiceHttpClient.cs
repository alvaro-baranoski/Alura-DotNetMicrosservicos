using RestauranteService.Dtos;

namespace RestauranteService.Http
{
    public interface IItemServiceHttpClient
    {
        public void EnviaRestauranteParaItemService(RestauranteReadDto readDto);
    }
}