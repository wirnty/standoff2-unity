namespace Axlebolt.Standoff.Settings
{
	public interface IQualityDataStore
	{
		QualityData Load();

		void Save(QualityData qualityData);
	}
}
