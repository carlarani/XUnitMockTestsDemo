using FluentAssertions;
using MockingUnitTestsDemoApp.Impl.Models;
using MockingUnitTestsDemoApp.Impl.Repositories.Interfaces;
using MockingUnitTestsDemoApp.Impl.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MockingUnitTestsDemoApp.Tests.Service
{
    public class PlayerServiceTest
    {
        private readonly PlayerService _playerService;
        private readonly Mock<IPlayerRepository> _mockIPlayerRepository;
        private readonly Mock<ILeagueRepository> _mockILeagueRepository;
        private readonly Mock<ITeamRepository> _mockITeamRepository;

        public PlayerServiceTest()
        {
            _mockILeagueRepository = new Mock<ILeagueRepository>();
            _mockIPlayerRepository = new Mock<IPlayerRepository>();
            _mockITeamRepository = new Mock<ITeamRepository>();

            _playerService = new PlayerService(_mockIPlayerRepository.Object, _mockITeamRepository.Object, _mockILeagueRepository.Object);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        public void GetForLeague_InvalidId_ReturnEmptyList(int id)
        {
            //arrange
            _mockILeagueRepository.Setup(mock => mock.IsValid(id))
                .Returns(false);

            //act
            var result = _playerService.GetForLeague(id);


            //assert
            result.Should()
                .BeEmpty();
        }

        [Theory]
        [InlineData(3)]
        //[InlineData(1)]
        public void GetForLeague_ValidIdNoPlayers_ReturnEmptyList(int id)
        {
            //arrange
            _mockILeagueRepository.Setup(mock => mock.IsValid(id))
                .Returns(true);
            _mockITeamRepository.Setup(mock => mock.GetForLeague(id))
                .Returns(GetFakeTeams().Where(p => p.LeagueID == id).ToList());

            //act
            var result = _playerService.GetForLeague(id);


            //assert
            result.Should()
                .BeEmpty();
        }

        private List<Player> GetFakePlayers()
        {
            return new List<Player>
            {
                new Player { ID = 1, FirstName = "Bess", LastName = "Low", DateOfBirth = DateTime.Parse("2/14/1998") , TeamID = 1},
                new Player { ID = 2, FirstName = "Drusi", LastName = "Kensley", DateOfBirth = DateTime.Parse("1/10/2004") , TeamID = 3},
                new Player { ID = 3, FirstName = "Aretha", LastName = "Woodlands", DateOfBirth = DateTime.Parse("1/15/2004") , TeamID = 5},
                new Player { ID = 4, FirstName = "Lezlie", LastName = "Pemberton", DateOfBirth = DateTime.Parse("12/2/1995") , TeamID = 4},
                new Player { ID = 5, FirstName = "Pauli", LastName = "Slack", DateOfBirth = DateTime.Parse("11/11/1993") , TeamID = 3},
                new Player { ID = 6, FirstName = "Erin", LastName = "Birchenhead", DateOfBirth = DateTime.Parse("11/19/1999") , TeamID = 1},
                new Player { ID = 7, FirstName = "Luca", LastName = "Scoullar", DateOfBirth = DateTime.Parse("6/14/1995") , TeamID = 4},
                new Player { ID = 8, FirstName = "Maribeth", LastName = "Fennelow", DateOfBirth = DateTime.Parse("5/6/1995") , TeamID = 3},
                new Player { ID = 9, FirstName = "Kelby", LastName = "Slocket", DateOfBirth = DateTime.Parse("2/6/2003") , TeamID = 3},
                new Player { ID = 10, FirstName = "Yank", LastName = "Amberger", DateOfBirth = DateTime.Parse("6/13/1999") , TeamID = 4},
            };
        }

        private List<League> GetFakeLeagues()
        {
            return new List<League>()
            {
            new League { ID = 1, Name = "Gold", FoundingDate = DateTime.Parse("10/8/1961") },
            new League { ID = 2, Name = "Silver", FoundingDate = DateTime.Parse("3/20/1964") },
            new League { ID = 3, Name = "Brass", FoundingDate = DateTime.Parse("1/25/1957") }
            };
        }

        private List<Team> GetFakeTeams()
        {
            return new List<Team>()
            {
                new Team { ID =1 ,Name = "Wood pigeon", FoundingDate = DateTime.Parse("1/10/1970"), LeagueID =1 },
                new Team { ID =2 ,Name = "Red falcon", FoundingDate = DateTime.Parse("1/10/1980"), LeagueID =2 },
                new Team { ID =3 ,Name = "Greylag goose", FoundingDate = DateTime.Parse("1/10/1990"), LeagueID =1 },
                new Team { ID =4 ,Name = "Black bear", FoundingDate = DateTime.Parse("1/10/2000"), LeagueID =2 },
                new Team { ID =5 ,Name = "Black vulture", FoundingDate = DateTime.Parse("1/10/2010"), LeagueID =1 },
            };
        }
    }
}
