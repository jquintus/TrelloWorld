namespace TrelloWorld.Server.Models
{
    public class Commit
    {
        public string Branch { get; set; }

        public string CardId { get; set; }

        public string CommitUrl { get; set; }

        public string Message { get; set; }

        public bool Equals(Commit other)
        {
            return string.Equals(Branch, other.Branch) && string.Equals(CommitUrl, other.CommitUrl) && string.Equals(CardId, other.CardId) && string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Commit)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Branch != null ? Branch.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CommitUrl != null ? CommitUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CardId != null ? CardId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("Branch: {0}, CommitUrl: {1}, Message: {2}, CardId: {3}", Branch, CommitUrl, Message, CardId);
        }
    }
}