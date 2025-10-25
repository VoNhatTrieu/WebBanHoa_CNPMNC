//using System.Data.Entity;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace giadinhthoxinh.Models
//{
//    public class ChatDbContext : DbContext
//    {
//        public ChatDbContext() : base("name=ChatDbConnection")
//        {
//            // TẮT tự động phát hiện bảng - chỉ dùng những gì khai báo
//            Configuration.LazyLoadingEnabled = false;
//            Configuration.ProxyCreationEnabled = false;
//        }

//        public virtual DbSet<tblChatConversation> tblChatConversations { get; set; }
//        public virtual DbSet<tblChatConversationMessage> tblChatConversationMessages { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            // BỎ QUA tất cả các bảng khác trong database
//            modelBuilder.Ignore<tblUser>();
//            modelBuilder.Ignore<tblImportOrder>();
//            modelBuilder.Ignore<tblCheckinDetail>();
//            modelBuilder.Ignore<tblProduct>();
//            modelBuilder.Ignore<tblCategory>();
//            modelBuilder.Ignore<tblCheckoutDetail>();
//            modelBuilder.Ignore<tblOrder>();
//            modelBuilder.Ignore<tblImage>();
//            modelBuilder.Ignore<tblProductColor>();
//            modelBuilder.Ignore<tblProductPrice>();
//            modelBuilder.Ignore<tblProductSize>();
//            modelBuilder.Ignore<tblPromote>();
//            modelBuilder.Ignore<tblReview>();
//            modelBuilder.Ignore<tblImportMaterial>();
//            modelBuilder.Ignore<tblMaterial>();
//            modelBuilder.Ignore<tblMaterColor>();
//            modelBuilder.Ignore<tblMaterPriceImport>();
//            modelBuilder.Ignore<tblMaterSize>();
//            modelBuilder.Ignore<tblSupplier>();
//            modelBuilder.Ignore<tblPermission>();

//            // CHỈ configure 2 bảng chat
//            modelBuilder.Entity<tblChatConversation>()
//                .ToTable("tblChatConversation")
//                .HasKey(c => c.PK_iConversationID);

//            modelBuilder.Entity<tblChatConversation>()
//                .Property(c => c.PK_iConversationID)
//                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

//            // Bỏ qua navigation properties tới tblUser
//            modelBuilder.Entity<tblChatConversation>()
//                .Ignore(c => c.CustomerUser)
//                .Ignore(c => c.AdminUser);

//            // Configure tblChatConversationMessage
//            modelBuilder.Entity<tblChatConversationMessage>()
//                .ToTable("tblChatConversationMessage")
//                .HasKey(m => m.PK_iMessageID);

//            modelBuilder.Entity<tblChatConversationMessage>()
//                .Property(m => m.PK_iMessageID)
//                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

//            // Bỏ qua navigation property tới tblUser
//            modelBuilder.Entity<tblChatConversationMessage>()
//                .Ignore(m => m.tblUser);

//            // Configure relationships CHỈ giữa 2 bảng chat
//            modelBuilder.Entity<tblChatConversationMessage>()
//                .HasRequired(m => m.tblChatConversation)
//                .WithMany(c => c.tblChatConversationMessages)
//                .HasForeignKey(m => m.FK_iConversationID)
//                .WillCascadeOnDelete(true);

//            base.OnModelCreating(modelBuilder);
//        }
//    }
//}
