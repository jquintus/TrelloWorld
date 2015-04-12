namespace TrelloWorld.Server.Services
{
    using Models;
    using System.Collections.Generic;

    public interface ICommitParser
    {
        IEnumerable<Commit> Parse(dynamic push);
    }
}