﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Indextrious.Models
{
    public class CardFile
    {
        /// <summary>
        /// Unique integer identifier for each file
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Label of file
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// List of files in current file
        /// </summary>
        public List<CardFile> SubFiles { get; set; } = new List<CardFile>();
        /// <summary>
        /// List of cards in current file
        /// </summary>
        [NotMapped]
        public List<ICard> Cards { get; set; } = new List<ICard>();

        /// <summary>
        /// Reference to parent CardFile
        /// Null if no parent
        /// </summary>
        public CardFile? ParentCardFile { get; set; }
        public int? ParentCardFileId { get; set; }

        /// <summary>
        /// Reference to parent CardCollection
        /// </summary>
        public CardCollection ParentCollection { get; set; }
        [ForeignKey("ParentCollection")]
        public int ParentCollectionId { get; set; }
    }   
}
