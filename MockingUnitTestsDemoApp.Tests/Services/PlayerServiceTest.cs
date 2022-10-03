using FluentAssertions;
using MockingUnitTestsDemoApp.Impl.Models;
using MockingUnitTestsDemoApp.Impl.Repositories.Interfaces;
using MockingUnitTestsDemoApp.Impl.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MockingUnitTestsDemoApp.Tests.Services
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

            _playerService = new PlayerService(_mockIPlayerRepository.Object,
                                               _mockITeamRepository.Object,
                                               _mockILeagueRepository.Object);
        }

        [Fact]
        public void GetForLeague_HappyDay_ReturnPlayersListNotEmpty()
        {
            //arrange
            _mockILeagueRepository.Setup(x => x.IsValid(It.IsAny<int>()))
                .Returns(true);
            _mockITeamRepository.Setup(x => x.GetForLeague(It.IsAny<int>()))
                .Returns(GetFakeTeams());
            _mockIPlayerRepository.Setup(x => x.GetForTeam(It.IsAny<int>()))
                .Returns(GetFakePlayers());

            //act
            var playersResult = _playerService.GetForLeague(It.IsAny<int>());

            //assert
            playersResult.Should()
                .NotBeEmpty();

            _mockILeagueRepository.Verify(x => x.IsValid(It.IsAny<int>()), Times.Once);
            _mockITeamRepository.Verify(x => x.GetForLeague(It.IsAny<int>()), Times.Once);
            _mockIPlayerRepository.Verify(x => x.GetForTeam(It.IsAny<int>()), Times.Exactly(GetFakeTeams().Count()));

        }


        [Fact]
        public void GetForLeague_InvalidLeague_ReturnEmptyPlayersList()
        {
            //arrange
            _mockILeagueRepository.Setup(mock => mock.IsValid(It.IsAny<int>()))
                .Returns(false);

            //act
            var result = _playerService.GetForLeague(It.IsAny<int>());


            //assert
            result.Should()
                .BeEmpty();

            _mockILeagueRepository.Verify(x => x.IsValid(It.IsAny<int>()), Times.Once);
            _mockITeamRepository.Verify(x => x.GetForLeague(It.IsAny<int>()), Times.Never);
            _mockIPlayerRepository.Verify(x => x.GetForTeam(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void GetForLeague_LeagueWithouTeams_ReturnEmptyList()
        {
            //arrange
            _mockILeagueRepository.Setup(mock => mock.IsValid(It.IsAny<int>()))
                .Returns(true);
            _mockITeamRepository.Setup(mock => mock.GetForLeague(It.IsAny<int>()))
                .Returns(new List<Team>());

            //act
            var result = _playerService.GetForLeague(It.IsAny<int>());


            //assert
            result.Should()
                .BeEmpty();

            _mockILeagueRepository.Verify(x => x.IsValid(It.IsAny<int>()), Times.Once);
            _mockITeamRepository.Verify(x => x.GetForLeague(It.IsAny<int>()), Times.Once);
            _mockIPlayerRepository.Verify(x => x.GetForTeam(It.IsAny<int>()), Times.Never);
        }

        private List<Player> GetFakePlayers()
        {
            return new List<Player>
            {
                new Player { ID = 1, FirstName = "Bess", LastName = "Low" , TeamID = 1},
                new Player { ID = 2, FirstName = "Drusi", LastName = "Kensley", TeamID = 3},
                new Player { ID = 3, FirstName = "Aretha", LastName = "Woodlands" , TeamID = 5},
                new Player { ID = 4, FirstName = "Lezlie", LastName = "Pemberton" , TeamID = 4},
                new Player { ID = 5, FirstName = "Pauli", LastName = "Slack" , TeamID = 3},
                new Player { ID = 6, FirstName = "Erin", LastName = "Birchenhead" , TeamID = 1},
                new Player { ID = 7, FirstName = "Luca", LastName = "Scoullar" , TeamID = 4},
                new Player { ID = 8, FirstName = "Maribeth", LastName = "Fennelow" , TeamID = 3},
                new Player { ID = 9, FirstName = "Kelby", LastName = "Slocket" , TeamID = 3},
                new Player { ID = 10, FirstName = "Yank", LastName = "Amberger" , TeamID = 4},
            };
        }

        private List<League> GetFakeLeagues()
        {
            return new List<League>()
            {
            new League { ID = 1, Name = "Gold" },
            new League { ID = 2, Name = "Silver" },
            new League { ID = 3, Name = "Brass" }
            };
        }

        private List<Team> GetFakeTeams()
        {
            return new List<Team>()
            {
                new Team { ID =1 ,Name = "Wood pigeon",  LeagueID =1 },
                new Team { ID =2 ,Name = "Red falcon",  LeagueID =2 },
                new Team { ID =3 ,Name = "Greylag goose",  LeagueID =1 },
                new Team { ID =4 ,Name = "Black bear",  LeagueID =2 },
                new Team { ID =5 ,Name = "Black vulture",  LeagueID =1 },
            };
        }
    }
}
