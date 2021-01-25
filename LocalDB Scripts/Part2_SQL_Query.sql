WITH test as (
	SELECT p.uniqueName as platformName, w.*, ROW_NUMBER() OVER (PARTITION BY w.platformId ORDER BY w.updatedAt DESC) AS rn
	FROM Well as w, platform as p
	where w.platformId = p.id
)
SELECT platformName, id, platformId, uniqueName, latitude, longitude, createdAt, updatedAt FROM test WHERE rn=1