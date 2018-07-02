public class Score {
	public int LinesCleared { get; set; }
	public int SoftDropTiles { get { return softDropTiles; } }

	private int softDropTiles = 0;

	public void IncrementSoftDropTiles() {
		softDropTiles += 1;
	}
}