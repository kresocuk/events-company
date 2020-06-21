using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TestWebApp.Models
{
    public partial class PI03Context : DbContext
    {
        public PI03Context()
        {
        }

        public PI03Context(DbContextOptions<PI03Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Certifikat> Certifikat { get; set; }
        public virtual DbSet<Dobavljac> Dobavljac { get; set; }
        public virtual DbSet<IznajmljivanjeOprema> IznajmljivanjeOprema { get; set; }
        public virtual DbSet<JavniNatjecaji> JavniNatjecaji { get; set; }
        public virtual DbSet<Korisnik> Korisnik { get; set; }
        public virtual DbSet<Natjecaj> Natjecaj { get; set; }
        public virtual DbSet<Oprema> Oprema { get; set; }
        public virtual DbSet<OpremaDobavljac> OpremaDobavljac { get; set; }
        public virtual DbSet<OpremaTroskovi> OpremaTroskovi { get; set; }
        public virtual DbSet<Osoba> Osoba { get; set; }
        public virtual DbSet<OsobaCertifikat> OsobaCertifikat { get; set; }
        public virtual DbSet<OsobaPosao> OsobaPosao { get; set; }
        public virtual DbSet<Ponuda> Ponuda { get; set; }
        public virtual DbSet<PonudaPosao> PonudaPosao { get; set; }
        public virtual DbSet<PonudaReferentniTip> PonudaReferentniTip { get; set; }
        public virtual DbSet<Posao> Posao { get; set; }
        public virtual DbSet<ReferentniTip> ReferentniTip { get; set; }
        public virtual DbSet<Skladiste> Skladiste { get; set; }
        public virtual DbSet<SklopljeniPosao> SklopljeniPosao { get; set; }
        public virtual DbSet<SklopljeniPosaoOprema> SklopljeniPosaoOprema { get; set; }
        public virtual DbSet<SklopljeniPosaoOsoba> SklopljeniPosaoOsoba { get; set; }
        public virtual DbSet<Tip> Tip { get; set; }
        public virtual DbSet<UnajmljivanjeOprema> UnajmljivanjeOprema { get; set; }
        public virtual DbSet<Usluga> Usluga { get; set; }
        public virtual DbSet<UslugaReferentniTip> UslugaReferentniTip { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certifikat>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID");
                    

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).IsUnicode(false);
            });

            modelBuilder.Entity<Dobavljac>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Detalji)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Lokacija)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<IznajmljivanjeOprema>(entity =>
            {
                entity.ToTable("Iznajmljivanje_Oprema");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Detalji)
                    .IsRequired()
                    .HasColumnName("detalji")
                    .IsUnicode(false);

                entity.Property(e => e.Kontakt)
                    .IsRequired()
                    .HasColumnName("kontakt")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OpremaId).HasColumnName("OpremaID");

                entity.Property(e => e.Trajanje).HasColumnName("trajanje");

                entity.Property(e => e.Vrijeme)
                    .HasColumnName("vrijeme")
                    .HasColumnType("date");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.IznajmljivanjeOprema)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Iznajmljivanje_Oprema_Oprema");
            });

            modelBuilder.Entity<JavniNatjecaji>(entity =>
            {
                entity.ToTable("Javni_Natjecaji");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Detalji)
                    .IsRequired()
                    .HasColumnName("detalji")
                    .IsUnicode(false);

                entity.Property(e => e.DobitnaPonuda).HasColumnName("dobitna_ponuda");

                entity.Property(e => e.NasaPonuda).HasColumnName("nasa_ponuda");

                entity.Property(e => e.Vrijeme)
                    .HasColumnName("vrijeme")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<Korisnik>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kontakt)
                    .HasColumnName("kontakt")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Lozinka)
                    .IsRequired()
                    .HasColumnName("lozinka")
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Natjecaj>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Detalji)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.PonudaId).HasColumnName("PonudaID");

                entity.HasOne(d => d.Ponuda)
                    .WithMany(p => p.Natjecaj)
                    .HasForeignKey(d => d.PonudaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Natjecaj_Ponuda");
            });

            modelBuilder.Entity<Oprema>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InventarniBroj)
                    .IsRequired()
                    .HasColumnName("Inventarni_broj")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.KnjigovodstvenaVrijednost).HasColumnName("Knjigovodstvena_vrijednost");

                entity.Property(e => e.KupovnaVrijednost).HasColumnName("Kupovna_vrijednost");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).IsUnicode(false);

                entity.Property(e => e.ReferentniTipId).HasColumnName("Referentni_tipID");

                entity.Property(e => e.SkladisteId).HasColumnName("SkladisteID");

                entity.Property(e => e.TipId).HasColumnName("TipID");

                entity.Property(e => e.VrijemeAmortizacije).HasColumnName("Vrijeme_amortizacije");

                entity.Property(e => e.VrijemeKupnje)
                    .HasColumnName("Vrijeme_kupnje")
                    .HasColumnType("date");

                entity.HasOne(d => d.ReferentniTip)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.ReferentniTipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_Referentni_tip");

                entity.HasOne(d => d.Skladiste)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.SkladisteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_Skladiste");

                entity.HasOne(d => d.Tip)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.TipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_Tip");
            });

            modelBuilder.Entity<OpremaDobavljac>(entity =>
            {
                entity.ToTable("Oprema_Dobavljac");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DobavljacId).HasColumnName("DobavljacID");

                entity.Property(e => e.OpremaId).HasColumnName("OpremaID");

                entity.HasOne(d => d.Dobavljac)
                    .WithMany(p => p.OpremaDobavljac)
                    .HasForeignKey(d => d.DobavljacId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_Dobavljac_Dobavljac");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.OpremaDobavljac)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_Dobavljac_Oprema");
            });

            modelBuilder.Entity<OpremaTroskovi>(entity =>
            {
                entity.ToTable("Oprema_Troskovi");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("opis")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OpremaId).HasColumnName("OpremaID");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.OpremaTroskovi)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Oprema_Troskovi_Oprema");
            });

            modelBuilder.Entity<Osoba>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DatumRodjenja)
                    .HasColumnName("Datum_rodjenja")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ime)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Prezime)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OsobaCertifikat>(entity =>
            {
                entity.ToTable("Osoba_Certifikat");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CertifikatId).HasColumnName("CertifikatID");

                entity.Property(e => e.OsobaId).HasColumnName("OsobaID");

                entity.HasOne(d => d.Certifikat)
                    .WithMany(p => p.OsobaCertifikat)
                    .HasForeignKey(d => d.CertifikatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Osoba_Certifikat_Certifikat");

                entity.HasOne(d => d.Osoba)
                    .WithMany(p => p.OsobaCertifikat)
                    .HasForeignKey(d => d.OsobaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Osoba_Certifikat_Osoba");
            });

            modelBuilder.Entity<OsobaPosao>(entity =>
            {
                entity.ToTable("Osoba_Posao");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OsobaId).HasColumnName("OsobaID");

                entity.Property(e => e.PosaoId).HasColumnName("PosaoID");

                entity.HasOne(d => d.Osoba)
                    .WithMany(p => p.OsobaPosao)
                    .HasForeignKey(d => d.OsobaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Osoba_Posao_Osoba");

                entity.HasOne(d => d.Posao)
                    .WithMany(p => p.OsobaPosao)
                    .HasForeignKey(d => d.PosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Osoba_Posao_Posao");
            });

            modelBuilder.Entity<Ponuda>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).IsUnicode(false);

                entity.Property(e => e.Vrijeme).HasColumnType("datetime");
            });

            modelBuilder.Entity<PonudaPosao>(entity =>
            {
                entity.ToTable("Ponuda_Posao");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.PonudaId).HasColumnName("PonudaID");

                entity.Property(e => e.PosaoId).HasColumnName("PosaoID");

                entity.HasOne(d => d.Ponuda)
                    .WithMany(p => p.PonudaPosao)
                    .HasForeignKey(d => d.PonudaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ponuda_Posao_Ponuda");

                entity.HasOne(d => d.Posao)
                    .WithMany(p => p.PonudaPosao)
                    .HasForeignKey(d => d.PosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ponuda_Posao_Posao");
            });

            modelBuilder.Entity<PonudaReferentniTip>(entity =>
            {
                entity.ToTable("Ponuda_Referentni_tip");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PonudaId).HasColumnName("PonudaID");

                entity.Property(e => e.ReferentniTipId).HasColumnName("Referentni_tipID");

                entity.HasOne(d => d.Ponuda)
                    .WithMany(p => p.PonudaReferentniTip)
                    .HasForeignKey(d => d.PonudaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ponuda_Referentni_tip_Ponuda");

                entity.HasOne(d => d.ReferentniTip)
                    .WithMany(p => p.PonudaReferentniTip)
                    .HasForeignKey(d => d.ReferentniTipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ponuda_Referentni_tip_Referentni_tip");
            });

            modelBuilder.Entity<Posao>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).IsUnicode(false);
            });

            modelBuilder.Entity<ReferentniTip>(entity =>
            {
                entity.ToTable("Referentni_tip");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Opis).IsUnicode(false);
            });

            modelBuilder.Entity<Skladiste>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Lokacija)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SklopljeniPosao>(entity =>
            {
                entity.ToTable("Sklopljeni_Posao");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Detalji)
                    .IsRequired()
                    .HasColumnName("detalji")
                    .IsUnicode(false);

                entity.Property(e => e.Kontakt)
                    .IsRequired()
                    .HasColumnName("kontakt")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Lokacija)
                    .IsRequired()
                    .HasColumnName("lokacija")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Vrijeme)
                    .HasColumnName("vrijeme")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<SklopljeniPosaoOprema>(entity =>
            {
                entity.ToTable("Sklopljeni_Posao_Oprema");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OpremaId).HasColumnName("OpremaID");

                entity.Property(e => e.SklopljeniPosaoId).HasColumnName("Sklopljeni_PosaoID");

                entity.Property(e => e.Trajanje).HasColumnName("trajanje");

                entity.HasOne(d => d.Oprema)
                    .WithMany(p => p.SklopljeniPosaoOprema)
                    .HasForeignKey(d => d.OpremaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sklopljeni_Posao_Oprema_Oprema");

                entity.HasOne(d => d.SklopljeniPosao)
                    .WithMany(p => p.SklopljeniPosaoOprema)
                    .HasForeignKey(d => d.SklopljeniPosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sklopljeni_Posao_Oprema_Sklopljeni_Posao");
            });

            modelBuilder.Entity<SklopljeniPosaoOsoba>(entity =>
            {
                entity.ToTable("Sklopljeni_Posao_Osoba");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OsobaId).HasColumnName("OsobaID");

                entity.Property(e => e.PosaoId).HasColumnName("PosaoID");

                entity.Property(e => e.SklopljeniPosaoId).HasColumnName("Sklopljeni_PosaoID");

                entity.Property(e => e.Trajanje).HasColumnName("trajanje");

                entity.HasOne(d => d.Osoba)
                    .WithMany(p => p.SklopljeniPosaoOsoba)
                    .HasForeignKey(d => d.OsobaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sklopljeni_Posao_Osoba_Osoba");

                entity.HasOne(d => d.Posao)
                    .WithMany(p => p.SklopljeniPosaoOsoba)
                    .HasForeignKey(d => d.PosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sklopljeni_Posao_Osoba_Posao");

                entity.HasOne(d => d.SklopljeniPosao)
                    .WithMany(p => p.SklopljeniPosaoOsoba)
                    .HasForeignKey(d => d.SklopljeniPosaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sklopljeni_Posao_Osoba_Sklopljeni_Posao");
            });

            modelBuilder.Entity<Tip>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UnajmljivanjeOprema>(entity =>
            {
                entity.ToTable("Unajmljivanje_Oprema");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Detalji)
                    .IsRequired()
                    .HasColumnName("detalji")
                    .IsUnicode(false);

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ReferentniTipId).HasColumnName("Referentni_tipID");

                entity.Property(e => e.Trajanje).HasColumnName("trajanje");

                entity.Property(e => e.Vrijeme)
                    .HasColumnName("vrijeme")
                    .HasColumnType("date");

                entity.HasOne(d => d.ReferentniTip)
                    .WithMany(p => p.UnajmljivanjeOprema)
                    .HasForeignKey(d => d.ReferentniTipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Unajmljivanje_Oprema_Referentni_tip");
            });

            modelBuilder.Entity<Usluga>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Detalji).IsUnicode(false);

                entity.Property(e => e.Kontakt)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Lokacija)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Vrijeme).HasColumnType("datetime");
            });

            modelBuilder.Entity<UslugaReferentniTip>(entity =>
            {
                entity.ToTable("Usluga_Referentni_tip");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ReferentniTipId).HasColumnName("Referentni_tipID");

                entity.Property(e => e.UslugaId).HasColumnName("UslugaID");

                entity.HasOne(d => d.ReferentniTip)
                    .WithMany(p => p.UslugaReferentniTip)
                    .HasForeignKey(d => d.ReferentniTipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usluga_Referentni_tip_Referentni_tip");

                entity.HasOne(d => d.Usluga)
                    .WithMany(p => p.UslugaReferentniTip)
                    .HasForeignKey(d => d.UslugaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usluga_Referentni_tip_Usluga");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
