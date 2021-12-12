namespace Resident {
	public enum Type { Passive, RandomClash, RandomTime, Hunter }
	public static class ResidentType {
		public static int TypeToInt(Type type) { return (int)type; }
	}
}