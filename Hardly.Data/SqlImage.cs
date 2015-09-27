using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardly {
	public class SqlImage {
		public readonly SqlImageMetadata imageMetadata;
		public readonly SqlImageFile originalImageFile;
		public readonly SqlDomainsFile domainsFile;

		public SqlImage(SqlImageMetadata imageMetadata, SqlImageFile originalImageFile, SqlDomainsFile domainsFile) {
			this.imageMetadata = imageMetadata;
			this.originalImageFile = originalImageFile;
			this.domainsFile = domainsFile;
		}

		public static SqlImage GetImage(SqlDomain domain, ulong imageId) {
			object[] results = SqlImageMetadata._table.Select(
				"join image_files on image_files.ContentId=OriginalContentId "
				+ "join domains_files on domains_files.ContentId=OriginalContentId "
				+ "join authors on authors.Id=AuthorId "
				+ "join domains_collections on domains_collections.DomainId=domains_files.DomainId",
				"OriginalContentId,Alt,CreatedDate,Width,Height,FileName,AuthorId,authors.Name",
				"domains_files.DomainId=?a and OriginalContentId=?b",
				new object[] { domain.id, imageId }, null
				);

			if(results != null && results.Length > 0) {
				return FromSql(domain, results);
			} else {
				return null;
			}
		}

		public static SqlImage[] GetMetadataList(SqlDomain domain, SqlCollection collection) {
			object[][] results = SqlImageMetadata._table.Select(
				"join image_files on image_files.ContentId=OriginalContentId "
				+ "join domains_files on domains_files.ContentId=OriginalContentId "
				+ "join authors on authors.Id=AuthorId "
				+ "join domains_collections on domains_collections.DomainId=domains_files.DomainId "
				+ "join static_content_collections on static_content_collections.CollectionId=domains_collections.CollectionId and static_content_collections.ContentId=OriginalContentId",
				"OriginalContentId,Alt,CreatedDate,Width,Height,FileName,AuthorId,authors.Name",
				"domains_files.DomainId=?a and static_content_collections.CollectionId=?b",
				new object[] { domain.id, collection.id }, null, 0
				);

			if(results != null && results.Length > 0) {
				SqlImage[] images = new SqlImage[results.Length];

				for(int i = 0; i < results.Length; i++) {
					images[i] = FromSql(domain, results[i]);
				}

				return images;
			}

			return null;
		}

		static SqlImage FromSql(SqlDomain domain, object[] results) {
			SqlStaticContent file = new SqlStaticContent(results[0].FromSql<ulong>());
			return new SqlImage(
						new SqlImageMetadata(file,
							results[1].FromSql<string>(),
							new SqlAuthor(results[6].FromSql<ulong>(), results[7].FromSql<string>()),
							results[2].FromSql<DateTime>()),
						new SqlImageFile(file, results[3].FromSql<uint>(), results[4].FromSql<uint>()),
						new SqlDomainsFile(domain, results[5].FromSql<string>(), file)
						);
		}
	}
}
