import { Component, OnInit } from '@angular/core';
import { Repository } from '../../models/repository';
import { Tag } from '../../models/tag.model';

@Component({
  selector: 'app-tag-list',
  templateUrl: './tag-list.component.html',
  styleUrls: ['./tag-list.component.css', '../admin.css']
})
export class TagListComponent implements OnInit {
  editedTag: Tag;
  newNameForLabels: string = "";
  newNameForLinks: string = "";

  constructor(private repo: Repository) {}

  ngOnInit() {
    this.repo.getTags();
  }
  get tags(): Tag[] {
    return this.repo.tags;
  }

  saveNewTag() {
    const newTag = new Tag(this.newNameForLabels, this.newNameForLinks);
    const errors = newTag.validate();
    if (errors) {
      this.repo.tagSaveErrors = errors;
      return;
    }
    this.repo.saveNewTag(newTag);
    this.newNameForLabels = "";
    this.newNameForLinks = "";
  }

  editTag(tagId: number) {
    this.editedTag = this.repo.tags.find(t => t.tagId === tagId);
  }

  saveEditedTag() {
    const errors = this.editedTag.validate();
    if (errors) {
      this.repo.tagSaveErrors = errors;
      return;
    }
    this.repo.updateTag(this.editedTag);
    this.editedTag = null;
  }

  deleteTag(tagId: number) {
    this.repo.deleteTag(tagId);
  }
}
