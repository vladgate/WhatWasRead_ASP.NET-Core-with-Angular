import { Component, OnInit } from '@angular/core';
import { Repository } from '../../models/repository';
import { Tag } from '../../models/tag.model';

@Component({
  selector: 'app-tag-list',
  templateUrl: './tag-list.component.html',
  styleUrls: ['./tag-list.component.css']
})
export class TagListComponent implements OnInit {
  editedTag: Tag;
  newNameForLabels: string;
  newNameForLinks: string;

  constructor(private repo: Repository) { }

  ngOnInit() {
    this.repo.getTags();
  }
  get tags(): Tag[] {
    return this.repo.tags;
  }

  saveNewTag() {
    this.validateTag(this.newNameForLabels, this.newNameForLinks);
    if (this.repo.tagSaveErrors) {
      return;
    }
    this.repo.saveNewTag(new Tag(this.newNameForLabels, this.newNameForLinks));
    this.newNameForLabels = "";
    this.newNameForLinks = "";
  }

  validateTag(nameForLabels, nameForLinks): void {
    this.repo.tagSaveErrors = "";
    if (nameForLabels.trim().length < 1 || nameForLabels.trim().length > 50) {
      this.repo.tagSaveErrors += "Текст представления тега должно состоять от 1 до 50 символов. "
    }
    if (nameForLinks.trim().length < 1 || nameForLinks.trim().length > 50) {
      this.repo.tagSaveErrors += "Текст ссылки тега должен состоять от 1 до 50 символов. "
    }
  }

  editTag(tagId: number) {
    this.editedTag = this.repo.tags.find(t => t.tagId === tagId);
  }

  saveEditedTag(tagId: number) {
    this.validateTag(this.editedTag.nameForLabels, this.editedTag.nameForLinks);
    if (this.repo.tagSaveErrors) {
      return;
    }
    this.repo.updateTag(this.editedTag);
    this.editedTag = null;
  }

  deleteTag(tagId: number) {
    this.repo.deleteTag(tagId);
  }
}
