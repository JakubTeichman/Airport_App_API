using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using AutoMapper;
using Domain.Entities;
using Domain.EntitiesDetails;

namespace Application.Mappings
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize() 
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Reservation, ReservationDTO>();
                cfg.CreateMap<CreateReservationDTO, Reservation>();
                cfg.CreateMap<UpdateReservationDTO, Reservation>();
                cfg.CreateMap<Connection,  ConnectionDTO>(); 
                cfg.CreateMap<ConnectionDTO, ConnectionDTO>();
                cfg.CreateMap<ConnectionDTO, Connection>();
                cfg.CreateMap<UpdateFlightDTO, FlightDetails>();
                cfg.CreateMap<FlightDetails, FlightDetails>();
                cfg.CreateMap<Reservation, Reservation>();
                cfg.CreateMap<FlightDetails, FlightDTO>();
                cfg.CreateMap<CreateFlightDTO, FlightDetails>();
                cfg.CreateMap<ReservationDTO, CreateReservationDTO>();
                cfg.CreateMap<FlightDTO, UpdateFlightDTO>();
            })
            .CreateMapper();
    }
}
